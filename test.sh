#!/usr/bin/env bash

main() {
  local LIBS=
  local OUTPUT_DIR="Build/Test"
  local OUT="${OUTPUT_DIR}/test.dll"
  local UNITY_ENGINE_DIR="${HOME}/Unity/Hub/Editor/2019.4.1f1/Editor/Data/Managed/UnityEngine"

  for i in "${UNITY_ENGINE_DIR}/"*.dll; do
    if [ "${LIBS}" = "" ]; then
      LIBS="${i}"
    else
      LIBS="${LIBS},${i}"
    fi
  done

  mkdir -p "${OUTPUT_DIR}"

  set -x

  # $(find Assets/Scripts -name "*.cs")

  mcs \
    $(find Assets/Scripts -name "*.cs") \
    $(find Test -name "*.cs") \
    -target:library \
    -r:"${LIBS}" \
    -r:NUnit.3.12.0/lib/net45/nunit.framework.dll \
    -out:"${OUT}"

  export MONO_PATH="$(pwd)/NUnit.ConsoleRunner.3.11.1/tools:$(pwd)/NUnit.3.12.0/lib/net45:${UNITY_ENGINE_DIR}"

  mono ./NUnit.ConsoleRunner.3.11.1/tools/nunit3-console.exe "${OUT}"
}

main "$@"
