#!/bin/bash

generate_guid() {
  if command -v uuidgen &> /dev/null; then
    uuidgen | tr -d '-' | tr '[:upper:]' '[:lower:]'
  else
    cat /proc/sys/kernel/random/uuid | tr -d '-'
  fi
}

generate_meta() {
  local path="$1"
  local is_dir="$2"
  local meta_path="${path}.meta"
  local guid
  local content

  if [ -f "$meta_path" ]; then return; fi

  guid=$(generate_guid)

  if [ "$is_dir" = true ]; then
    content="fileFormatVersion: 2
guid: ${guid}
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  licenseType: Free"
  elif [[ "$path" == *.cs ]]; then
    content="fileFormatVersion: 2
guid: ${guid}
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  licenseType: Free"
  else
    content="fileFormatVersion: 2
guid: ${guid}
DefaultImporter:
  externalObjects: {}
  userData: 
  licenseType: Free"
  fi

  echo "$content" > "$meta_path"
  echo "Created: $meta_path"
}

process_dir() {
  local dir="$1"

  for entry in "$dir"/*; do
    if [ -d "$entry" ]; then
      generate_meta "$entry" true
      process_dir "$entry"
    elif [ -f "$entry" ] && [[ "$entry" != *.meta ]]; then
      generate_meta "$entry" false
    fi
  done
}

TARGET="${1:-.}"
generate_meta "$TARGET" true
process_dir "$TARGET"
