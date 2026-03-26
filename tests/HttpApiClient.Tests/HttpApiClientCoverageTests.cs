using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using YourSdk;

namespace HttpApiClient.Tests
{
    internal sealed class QueueHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses = new Queue<HttpResponseMessage>();

        public void Enqueue(HttpResponseMessage response)
        {
            _responses.Enqueue(response);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_responses.Count == 0)
            {
                throw new InvalidOperationException("No response queued for request.");
            }

            return Task.FromResult(_responses.Dequeue());
        }
    }

    internal sealed class TestClient : Client
    {
        public TestClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

#pragma warning disable CA1822, S2325
        public async Task<(T? Result, string Text)> ReadObjectResponseAsyncPublic<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
        {
            var objectResponse = await ReadObjectResponseAsync<T>(response, headers, cancellationToken).ConfigureAwait(false);
            return (objectResponse.Object, objectResponse.Text);
        }
#pragma warning restore CA1822, S2325
    }

    public sealed class ApiMethodCase
    {
        public ApiMethodCase(string methodName, int statusCode, string? responseTypeName, bool isSuccess, bool isUnexpectedStatus)
        {
            MethodName = methodName;
            StatusCode = statusCode;
            ResponseTypeName = responseTypeName;
            IsSuccess = isSuccess;
            IsUnexpectedStatus = isUnexpectedStatus;
        }

        public string MethodName { get; }

        public int StatusCode { get; }

        public string? ResponseTypeName { get; }

        public bool IsSuccess { get; }

        public bool IsUnexpectedStatus { get; }

        public override string ToString()
        {
            var kind = "error";
            if (IsUnexpectedStatus)
            {
                kind = "unexpected";
            }
            else if (IsSuccess)
            {
                kind = "success";
            }
            return $"{MethodName}:{StatusCode}:{kind}";
        }
    }

    internal static class ClientSourceParser
    {
        private static readonly Regex MethodRegex = new Regex(
            @"public\s+virtual\s+async\s+System\.Threading\.Tasks\.Task<.+?>\s+(?<name>\w+)\((?<params>[^)]*)\)",
            RegexOptions.Compiled);

        private static readonly Regex StatusRegex = new Regex(@"if\s*\((?<condition>[^)]*status_[^)]*)\)", RegexOptions.Compiled);

        private static readonly Regex StatusCodeRegex = new Regex(@"status_\s*==\s*(?<status>\d+)", RegexOptions.Compiled);

        private static readonly Regex ReadObjectRegex = new Regex(@"ReadObjectResponseAsync<(?<type>.+?)>\(", RegexOptions.Compiled);

        public static IReadOnlyList<ApiMethodCase> GetCases(string sourceText)
        {
            var cases = new List<ApiMethodCase>();

            foreach (Match match in MethodRegex.Matches(sourceText))
            {
                var methodName = match.Groups["name"].Value;
                var bodyStart = sourceText.IndexOf('{', match.Index);
                if (bodyStart < 0)
                {
                    continue;
                }

                var bodyEnd = FindMatchingBrace(sourceText, bodyStart);
                if (bodyEnd <= bodyStart)
                {
                    continue;
                }

                var body = sourceText.Substring(bodyStart, bodyEnd - bodyStart + 1);
                var statusMatches = StatusRegex.Matches(body).Cast<Match>().ToList();
                foreach (var statusMatch in statusMatches)
                {
                    var condition = statusMatch.Groups["condition"].Value;
                    var branch = GetConditionBranch(body, statusMatch.Index);
                    var readMatch = ReadObjectRegex.Match(branch);
                    var responseTypeName = readMatch.Success ? readMatch.Groups["type"].Value : null;
                    var isSuccess =
                        (branch.Contains("return ", StringComparison.Ordinal) &&
                         !branch.Contains("throw new ApiException", StringComparison.Ordinal)) ||
                        branch.Contains("return new SwaggerResponse", StringComparison.Ordinal) ||
                        branch.Contains("return new FileResponse", StringComparison.Ordinal);

                    foreach (Match codeMatch in StatusCodeRegex.Matches(condition))
                    {
                        var statusCode = int.Parse(codeMatch.Groups["status"].Value);
                        cases.Add(new ApiMethodCase(methodName, statusCode, responseTypeName, isSuccess, false));
                    }
                }

                if (body.Contains("The HTTP status code of the response was not expected", StringComparison.Ordinal))
                {
                    cases.Add(new ApiMethodCase(methodName, 599, null, false, true));
                }
            }

            return cases;
        }

        private static string GetConditionBranch(string methodBody, int conditionIndex)
        {
            var blockStart = methodBody.IndexOf('{', conditionIndex);
            if (blockStart < 0)
            {
                return string.Empty;
            }

            var blockEnd = FindMatchingBrace(methodBody, blockStart);
            if (blockEnd <= blockStart)
            {
                return string.Empty;
            }

            return methodBody.Substring(blockStart, blockEnd - blockStart + 1);
        }

        private static int FindMatchingBrace(string sourceText, int openBraceIndex)
        {
            var depth = 0;
            for (var i = openBraceIndex; i < sourceText.Length; i++)
            {
                var ch = sourceText[i];
                if (ch == '{')
                {
                    depth++;
                }
                else if (ch == '}')
                {
                    depth--;
                    if (depth == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }

    internal static class SampleValueFactory
    {
        public static object? CreateValue(Type type)
        {
            return CreateValue(type, new HashSet<Type>());
        }

        private static object? CreateValue(Type type, HashSet<Type> visiting)
        {
            var underlyingNullable = Nullable.GetUnderlyingType(type);
            if (underlyingNullable != null)
            {
                return CreateValue(underlyingNullable, visiting);
            }

            if (type == typeof(FileParameter))
            {
                return new FileParameter(new MemoryStream(new byte[] { 1, 2, 3 }), "sample.bin", "application/octet-stream");
            }

            if (TryCreateScalar(type, out var scalarValue))
            {
                return scalarValue;
            }

            if (type.IsEnum)
            {
                return Enum.GetValues(type).GetValue(0);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType() ?? typeof(object);
                var array = Array.CreateInstance(elementType, 1);
                array.SetValue(CreateValue(elementType, visiting), 0);
                return array;
            }

            if (IsDictionaryType(type))
            {
                return CreateDictionaryInstance(type);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                var collection = CreateCollectionInstance(type);
                if (collection != null)
                {
                    return collection;
                }

                return Array.Empty<object>();
            }

            var instance = CreateDefaultInstance(type);
            if (instance != null)
            {
                PopulateRequiredProperties(instance, visiting);
                return instance;
            }

            return new object();
        }

        private static bool TryCreateScalar(Type type, out object? value)
        {
            if (type == typeof(string))
            {
                value = "sample";
                return true;
            }

            if (type == typeof(Guid))
            {
                value = Guid.NewGuid();
                return true;
            }

            if (type == typeof(DateTime))
            {
                value = DateTime.UtcNow;
                return true;
            }

            if (type == typeof(bool))
            {
                value = true;
                return true;
            }

            if (type == typeof(int))
            {
                value = 1;
                return true;
            }

            if (type == typeof(long))
            {
                value = 1L;
                return true;
            }

            if (type == typeof(double))
            {
                value = 1.0;
                return true;
            }

            if (type == typeof(decimal))
            {
                value = 1.0m;
                return true;
            }

            if (type == typeof(byte[]))
            {
                value = new byte[] { 1, 2, 3 };
                return true;
            }

            if (type == typeof(CancellationToken))
            {
                value = CancellationToken.None;
                return true;
            }

            if (type == typeof(Uri))
            {
                value = BuildExampleUri();
                return true;
            }

            value = null;
            return false;
        }

        private static object CreateDictionaryInstance(Type type)
        {
            var instance = CreateDefaultInstance(type);
            if (instance != null)
            {
                return instance;
            }

            var dictionaryType = type.IsGenericType && type.GetGenericArguments().Length == 2
                ? type
                : type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));

            if (dictionaryType != null)
            {
                var keyValueTypes = dictionaryType.GetGenericArguments();
                var concreteDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyValueTypes[0], keyValueTypes[1]);
                return Activator.CreateInstance(concreteDictionaryType) ?? new Dictionary<string, object>();
            }

            return new Dictionary<string, object>();
        }

        private static object? CreateCollectionInstance(Type type)
        {
            var instance = CreateDefaultInstance(type);
            if (instance != null && type.IsInstanceOfType(instance))
            {
                return instance;
            }

            var enumerableType = type.IsGenericType && type.GetGenericArguments().Length == 1
                ? type
                : type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerableType == null)
            {
                return null;
            }

            var elementType = enumerableType.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            return Activator.CreateInstance(listType);
        }

        public static object? CreateDefaultInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }

                try
                {
                    return RuntimeHelpers.GetUninitializedObject(type);
                }
                catch
                {
                    return null;
                }
            }
        }

        private static void PopulateRequiredProperties(object instance, HashSet<Type> visiting)
        {
            var type = instance.GetType();
            if (visiting.Contains(type))
            {
                return;
            }

            visiting.Add(type);

            foreach (var property in GetSerializableProperties(type))
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                if (!IsRequiredProperty(property))
                {
                    continue;
                }

                var currentValue = property.GetValue(instance);
                if (currentValue != null)
                {
                    continue;
                }

                var value = CreateValue(property.PropertyType, visiting);
                property.SetValue(instance, value);
            }

            visiting.Remove(type);
        }

        public static string CreateSampleJson(Type type, bool forceNull)
        {
            if (forceNull)
            {
                return "null";
            }

            return CreateSampleJson(type, new HashSet<Type>());
        }

        private static string CreateSampleJson(Type type, HashSet<Type> visiting)
        {
            var underlyingNullable = Nullable.GetUnderlyingType(type) ?? type;

            if (underlyingNullable == typeof(string))
            {
                return JsonConvert.SerializeObject("sample");
            }

            if (underlyingNullable == typeof(Guid))
            {
                return JsonConvert.SerializeObject(Guid.NewGuid());
            }

            if (underlyingNullable == typeof(DateTime))
            {
                return JsonConvert.SerializeObject(DateTime.UtcNow);
            }

            if (underlyingNullable == typeof(bool))
            {
                return "true";
            }

            if (underlyingNullable == typeof(int) || underlyingNullable == typeof(long) || underlyingNullable == typeof(double) || underlyingNullable == typeof(decimal))
            {
                return "1";
            }

            if (underlyingNullable == typeof(Uri))
            {
                return JsonConvert.SerializeObject(BuildExampleUri());
            }

            if (underlyingNullable.IsEnum)
            {
                var enumValue = Enum.GetValues(underlyingNullable).GetValue(0);
                var memberName = enumValue?.ToString() ?? "";
                var member = underlyingNullable.GetMember(memberName).FirstOrDefault();
                var enumMember = member?.GetCustomAttribute<EnumMemberAttribute>();
                return JsonConvert.SerializeObject(enumMember?.Value ?? memberName);
            }

            if (IsDictionaryType(underlyingNullable))
            {
                return "{}";
            }

            if (typeof(IEnumerable).IsAssignableFrom(underlyingNullable) && underlyingNullable != typeof(string))
            {
                return "[]";
            }

            var token = BuildRequiredObjectToken(underlyingNullable, visiting);
            return token?.ToString(Formatting.None) ?? "{}";
        }

        private static bool IsDictionaryType(Type type)
        {
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                return true;
            }

            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static bool IsRequiredProperty(PropertyInfo property)
        {
            if (property.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>() != null)
            {
                return true;
            }

            var jsonProperty = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>();
            return jsonProperty != null && jsonProperty.Required != Required.Default;
        }

        private static JToken? BuildRequiredObjectToken(Type type, HashSet<Type> visiting)
        {
            if (visiting.Contains(type))
            {
                return null;
            }

            visiting.Add(type);

            var obj = new JObject();
            foreach (var property in GetSerializableProperties(type))
            {
                if (!IsRequiredProperty(property))
                {
                    continue;
                }

                var jsonProperty = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>();
                var jsonName = jsonProperty?.PropertyName ?? property.Name;
                var valueToken = CreateSampleToken(property.PropertyType, visiting);
                obj[jsonName] = valueToken ?? JValue.CreateNull();
            }

            visiting.Remove(type);

            return obj;
        }

        private static JToken? CreateSampleToken(Type type, HashSet<Type> visiting)
        {
            var underlyingNullable = Nullable.GetUnderlyingType(type) ?? type;
            if (TryCreateScalar(underlyingNullable, out var scalarValue))
            {
                return scalarValue == null ? JValue.CreateNull() : JToken.FromObject(scalarValue);
            }

            if (underlyingNullable.IsEnum)
            {
                var enumValue = Enum.GetValues(underlyingNullable).GetValue(0);
                var memberName = enumValue?.ToString() ?? "";
                var member = underlyingNullable.GetMember(memberName).FirstOrDefault();
                var enumMember = member?.GetCustomAttribute<EnumMemberAttribute>();
                return JToken.FromObject(enumMember?.Value ?? memberName);
            }

            if (IsDictionaryType(underlyingNullable))
            {
                return new JObject();
            }

            if (typeof(IEnumerable).IsAssignableFrom(underlyingNullable) && underlyingNullable != typeof(string))
            {
                return new JArray();
            }

            return BuildRequiredObjectToken(underlyingNullable, visiting);
        }

        private static IEnumerable<PropertyInfo> GetSerializableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(property => GetTypeDepth(property.DeclaringType))
                .ThenBy(property => property.MetadataToken)
                .GroupBy(property =>
                {
                    var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
                    return jsonProperty?.PropertyName ?? property.Name;
                }, StringComparer.Ordinal)
                .Select(group => group.First());
        }

        private static int GetTypeDepth(Type? type)
        {
            var depth = 0;
            while (type != null)
            {
                depth++;
                type = type.BaseType;
            }

            return depth;
        }

        private static Uri BuildExampleUri()
        {
            var builder = new UriBuilder(Uri.UriSchemeHttps, "example.com");
            return builder.Uri;
        }
    }

    internal static class ClientMethodInvoker
    {
        public static MethodInfo GetClientMethod(string methodName)
        {
            var methods = typeof(Client).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(method => method.Name == methodName)
                .ToList();

            if (methods.Count != 1)
            {
                throw new InvalidOperationException($"Expected exactly one method named {methodName}, found {methods.Count}.");
            }

            return methods[0];
        }

        public static object?[] CreateArguments(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var args = new object?[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                args[i] = SampleValueFactory.CreateValue(parameter.ParameterType);
            }

            return args;
        }

        public static async Task<object?> InvokeAsync(MethodInfo method, object instance, object?[] args)
        {
            var result = method.Invoke(instance, args);
            if (result is Task task)
            {
                await task.ConfigureAwait(false);
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task);
            }

            return result;
        }
    }

    [TestFixture]
    public class HttpApiClientCoverageTests
    {
        private static string GetSourceFilePath()
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "httpApiClient.cs");
        }

        private static HttpResponseMessage CreateJsonResponse(int statusCode, string json)
        {
            return new HttpResponseMessage((HttpStatusCode)statusCode)
            {
                Content = new StringContent(json ?? string.Empty, Encoding.UTF8, "application/json")
            };
        }

        public static IEnumerable<ApiMethodCase> AllMethodCases()
        {
            var sourceText = System.IO.File.ReadAllText(GetSourceFilePath());
            return ClientSourceParser.GetCases(sourceText);
        }

        public static IEnumerable<ApiMethodCase> NonNullResponseCases()
        {
            return AllMethodCases().Where(c => !c.IsUnexpectedStatus);
        }

        public static IEnumerable<ApiMethodCase> DeserializedResponseCases()
        {
            return NonNullResponseCases().Where(c => c.ResponseTypeName != null);
        }

        public static IEnumerable<ApiMethodCase> UnexpectedStatusCases()
        {
            return AllMethodCases().Where(c => c.IsUnexpectedStatus);
        }

        [Test]
        public void BaseUrl_AppendsTrailingSlash()
        {
            var handler = new QueueHttpMessageHandler();
            var client = new Client(new HttpClient(handler));
            var baseUri = new UriBuilder(Uri.UriSchemeHttps, "example.com").Uri.GetLeftPart(UriPartial.Authority);
            client.BaseUrl = baseUri;

            Assert.That(client.BaseUrl, Is.EqualTo("https://example.com/"));

            var trailingSlashUri = new UriBuilder(Uri.UriSchemeHttps, "example.com").Uri.ToString();
            client.BaseUrl = trailingSlashUri;
            Assert.That(client.BaseUrl, Is.EqualTo("https://example.com/"));
        }

        [Test]
        public void AdditionalProperties_InitializeAndReplace()
        {
            var types = typeof(Client).Assembly.GetTypes()
                .Where(type => type.Namespace == "YourSdk")
                .Where(type => type.GetProperty("AdditionalProperties", BindingFlags.Public | BindingFlags.Instance) != null)
                .ToList();

            foreach (var type in types)
            {
                var instance = SampleValueFactory.CreateDefaultInstance(type);
                if (instance == null)
                {
                    continue;
                }

                var property = type.GetProperty("AdditionalProperties", BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    continue;
                }

                var initial = property.GetValue(instance) as IDictionary<string, object>;
                Assert.That(initial, Is.Not.Null, $"{type.Name} should initialize AdditionalProperties.");

                var replacement = new Dictionary<string, object>();
                property.SetValue(instance, replacement);
                var updated = property.GetValue(instance) as IDictionary<string, object>;
                Assert.That(updated, Is.SameAs(replacement), $"{type.Name} should store AdditionalProperties.");
            }
        }

        [Test]
        public void ConvertToString_HandlesCommonTypes()
        {
            var handler = new QueueHttpMessageHandler();
            var client = new Client(new HttpClient(handler));
            var method = typeof(Client).GetMethod("ConvertToString", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(method, Is.Not.Null);

            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var enumValue = UserRole.Learner;

            Assert.That(method!.Invoke(client, new object?[] { null, culture }), Is.EqualTo(""));
            Assert.That(method.Invoke(client, new object?[] { true, culture }), Is.EqualTo("true"));
            Assert.That(method.Invoke(client, new object?[] { new byte[] { 1, 2 }, culture }), Is.EqualTo(Convert.ToBase64String(new byte[] { 1, 2 })));
            Assert.That(method.Invoke(client, new object?[] { new[] { "a", "b" }, culture }), Is.EqualTo("a,b"));
            Assert.That(method.Invoke(client, new object?[] { new[] { 1, 2 }, culture }), Is.EqualTo("1,2"));
            Assert.That(method.Invoke(client, new object?[] { enumValue, culture }), Is.EqualTo("learner"));
        }

        [Test]
        public async Task ReadObjectResponseAsync_UsesStringOrStream()
        {
            var handler = new QueueHttpMessageHandler();
            var client = new TestClient(new HttpClient(handler));
            var headers = new Dictionary<string, IEnumerable<string>>();

            client.ReadResponseAsString = true;
            var stringResponse = CreateJsonResponse(200, SampleValueFactory.CreateSampleJson(typeof(BackendResponse), false));
            var stringResult = await client.ReadObjectResponseAsyncPublic<BackendResponse>(stringResponse, headers, CancellationToken.None);
            Assert.That(stringResult.Result, Is.Not.Null);
            Assert.That(stringResult.Text, Does.Contain("message"));

            client.ReadResponseAsString = false;
            var streamResponse = CreateJsonResponse(200, SampleValueFactory.CreateSampleJson(typeof(BackendResponse), false));
            var streamResult = await client.ReadObjectResponseAsyncPublic<BackendResponse>(streamResponse, headers, CancellationToken.None);
            Assert.That(streamResult.Result, Is.Not.Null);
        }

        [Test]
        public void ReadObjectResponseAsync_ThrowsOnInvalidJson()
        {
            var handler = new QueueHttpMessageHandler();
            var client = new TestClient(new HttpClient(handler));
            var response = CreateJsonResponse(200, "not-json");
            var headers = new Dictionary<string, IEnumerable<string>>();

            client.ReadResponseAsString = true;
            Assert.ThrowsAsync<ApiException>(async () => await client.ReadObjectResponseAsyncPublic<BackendResponse>(response, headers, CancellationToken.None));

            client.ReadResponseAsString = false;
            Assert.ThrowsAsync<ApiException>(async () => await client.ReadObjectResponseAsyncPublic<BackendResponse>(response, headers, CancellationToken.None));
        }

        [Test]
        public void SwaggerResponse_StoresValues()
        {
            var headers = new Dictionary<string, IEnumerable<string>>();
            var response = new SwaggerResponse(200, headers);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Headers, Is.SameAs(headers));

            var typed = new SwaggerResponse<string>(201, headers, "ok");
            Assert.That(typed.Result, Is.EqualTo("ok"));
        }

        [Test]
        public void ApiException_ToStringIncludesResponse()
        {
            var headers = new Dictionary<string, IEnumerable<string>>();
            var exception = new ApiException("boom", 500, "oops", headers, null);
            Assert.That(exception.ToString(), Does.Contain("oops"));
        }

        [TestCaseSource(nameof(NonNullResponseCases))]
        public async Task ClientMethods_HandleKnownStatusCodes(ApiMethodCase apiCase)
        {
            var handler = new QueueHttpMessageHandler();
            var httpClient = new HttpClient(handler);
            var client = new Client(httpClient)
            {
                ReadResponseAsString = apiCase.StatusCode % 2 == 0
            };

            var method = ClientMethodInvoker.GetClientMethod(apiCase.MethodName);
            var args = ClientMethodInvoker.CreateArguments(method);

            if (apiCase.ResponseTypeName != null)
            {
                var responseType = ResolveResponseType(apiCase.ResponseTypeName);
                var json = SampleValueFactory.CreateSampleJson(responseType, false);
                handler.Enqueue(CreateJsonResponse(apiCase.StatusCode, json));
            }
            else
            {
                handler.Enqueue(CreateJsonResponse(apiCase.StatusCode, string.Empty));
            }

            if (apiCase.IsSuccess)
            {
                var result = await ClientMethodInvoker.InvokeAsync(method, client, args);
                switch (result)
                {
                    case SwaggerResponse swaggerResponse:
                        Assert.That(swaggerResponse.StatusCode, Is.EqualTo(apiCase.StatusCode), $"Expected status code {apiCase.StatusCode} from {apiCase}.");
                        break;
                    case FileResponse fileResponse:
                        Assert.That(fileResponse.StatusCode, Is.EqualTo(apiCase.StatusCode), $"Expected status code {apiCase.StatusCode} from {apiCase}.");
                        fileResponse.Dispose();
                        break;
                    default:
                        Assert.Fail($"Expected a SwaggerResponse or FileResponse from {apiCase}, but received {result?.GetType().FullName ?? "null"}.");
                        break;
                }
            }
            else
            {
                Assert.That(
                    async () => await ClientMethodInvoker.InvokeAsync(method, client, args),
                    Throws.InstanceOf<ApiException>());
            }
        }

        [TestCaseSource(nameof(DeserializedResponseCases))]
        public void ClientMethods_ThrowOnNullResponse(ApiMethodCase apiCase)
        {
            var handler = new QueueHttpMessageHandler();
            var httpClient = new HttpClient(handler);
            var client = new Client(httpClient)
            {
                ReadResponseAsString = apiCase.StatusCode % 2 == 1
            };

            var method = ClientMethodInvoker.GetClientMethod(apiCase.MethodName);
            var args = ClientMethodInvoker.CreateArguments(method);

            handler.Enqueue(CreateJsonResponse(apiCase.StatusCode, "null"));

            Assert.ThrowsAsync<ApiException>(async () => await ClientMethodInvoker.InvokeAsync(method, client, args));
        }

        [TestCaseSource(nameof(UnexpectedStatusCases))]
        public void ClientMethods_ThrowOnUnexpectedStatus(ApiMethodCase apiCase)
        {
            var handler = new QueueHttpMessageHandler();
            var httpClient = new HttpClient(handler);
            var client = new Client(httpClient)
            {
                ReadResponseAsString = true
            };

            var method = ClientMethodInvoker.GetClientMethod(apiCase.MethodName);
            var args = ClientMethodInvoker.CreateArguments(method);

            handler.Enqueue(CreateJsonResponse(apiCase.StatusCode, "{\"error\":\"unexpected\"}"));

            Assert.ThrowsAsync<ApiException>(async () => await ClientMethodInvoker.InvokeAsync(method, client, args));
        }

        private static Type ResolveResponseType(string responseTypeName)
        {
            if (responseTypeName.Contains("<", StringComparison.Ordinal))
            {
                return ResolveGenericType(responseTypeName);
            }

            return ResolveNonGenericType(responseTypeName);
        }

        private static Type ResolveGenericType(string responseTypeName)
        {
            var typeName = responseTypeName.Trim();
            var genericStart = typeName.IndexOf('<');
            var genericEnd = typeName.LastIndexOf('>');
            if (genericStart < 0 || genericEnd <= genericStart)
            {
                return ResolveNonGenericType(typeName);
            }

            var genericDefinitionName = typeName.Substring(0, genericStart);
            var argumentsList = typeName.Substring(genericStart + 1, genericEnd - genericStart - 1);
            var argumentTypeNames = SplitGenericArguments(argumentsList);
            var argumentTypes = argumentTypeNames.Select(ResolveResponseType).ToArray();

            var definitionWithArity = $"{genericDefinitionName}`{argumentTypes.Length}";
            var genericDefinition = Type.GetType(definitionWithArity) ?? ResolveNonGenericType(definitionWithArity);
            return genericDefinition.MakeGenericType(argumentTypes);
        }

        private static IEnumerable<string> SplitGenericArguments(string argumentsList)
        {
            var depth = 0;
            var start = 0;
            for (var i = 0; i < argumentsList.Length; i++)
            {
                var ch = argumentsList[i];
                if (ch == '<')
                {
                    depth++;
                }
                else if (ch == '>')
                {
                    depth--;
                }
                else if (ch == ',' && depth == 0)
                {
                    yield return argumentsList.Substring(start, i - start).Trim();
                    start = i + 1;
                }
            }

            yield return argumentsList.Substring(start).Trim();
        }

        private static Type ResolveNonGenericType(string typeName)
        {
            var normalizedTypeName = typeName.Trim();
            switch (normalizedTypeName)
            {
                case "string":
                    return typeof(string);
                case "object":
                    return typeof(object);
                case "bool":
                    return typeof(bool);
                case "byte":
                    return typeof(byte);
                case "short":
                    return typeof(short);
                case "int":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "float":
                    return typeof(float);
                case "double":
                    return typeof(double);
                case "decimal":
                    return typeof(decimal);
                case "Guid":
                    return typeof(Guid);
                case "DateTime":
                    return typeof(DateTime);
                case "byte[]":
                    return typeof(byte[]);
            }

            var assembly = typeof(Client).Assembly;
            var match = assembly.GetTypes().FirstOrDefault(type => type.Name == normalizedTypeName || type.FullName == normalizedTypeName);
            if (match != null)
            {
                return match;
            }

            return Type.GetType(normalizedTypeName, throwOnError: true)!;
        }
    }
}
