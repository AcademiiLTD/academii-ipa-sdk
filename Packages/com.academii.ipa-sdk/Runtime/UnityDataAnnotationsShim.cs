using System;

// Unity 2021 package compilation does not provide System.ComponentModel.DataAnnotations
// by default. The generated DTOs only use these as metadata, so minimal attribute
// stubs are sufficient for package consumers.
namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredAttribute : Attribute
    {
        public bool AllowEmptyStrings { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class Required : RequiredAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class StringLengthAttribute : Attribute
    {
        public StringLengthAttribute(int maximumLength)
        {
            MaximumLength = maximumLength;
        }

        public int MaximumLength { get; }

        public int MinimumLength { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class StringLength : StringLengthAttribute
    {
        public StringLength(int maximumLength)
            : base(maximumLength)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RangeAttribute : Attribute
    {
        public RangeAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public RangeAttribute(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public object Minimum { get; }

        public object Maximum { get; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class Range : RangeAttribute
    {
        public Range(int minimum, int maximum)
            : base(minimum, maximum)
        {
        }

        public Range(double minimum, double maximum)
            : base(minimum, maximum)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MinLengthAttribute : Attribute
    {
        public MinLengthAttribute(int length)
        {
            Length = length;
        }

        public int Length { get; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class MinLength : MinLengthAttribute
    {
        public MinLength(int length)
            : base(length)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MaxLengthAttribute : Attribute
    {
        public MaxLengthAttribute(int length)
        {
            Length = length;
        }

        public int Length { get; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class MaxLength : MaxLengthAttribute
    {
        public MaxLength(int length)
            : base(length)
        {
        }
    }
}
