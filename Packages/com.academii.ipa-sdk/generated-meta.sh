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
  local meta_path="${path}.meta"

  if [ -f "$meta_path" ]; then return; fi

  local guid=$(generate_guid)

  cat > "$meta_path" <<EOF
fileFormatVersion: 2
guid: ${guid}
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  licenseType: Free
EOF

  echo "Created: $meta_path"
}

process_dir() {
  local dir="$1"

  for entry in "$dir"/*; do
    if [ -d "$entry" ]; then
      process_dir "$entry"
      generate_meta "$entry"
    elif [[ "$entry" == *.cs ]]; then
      generate_meta "$entry"
    fi
  done
}

process_dir "${1:-.}"
