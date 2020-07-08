#!/usr/bin/env bash

set -e

THIS_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

main() {
  local UNITY="${HOME}/Unity/Hub/Editor/2019.4.1f1/Editor/Unity"
  local ROOT_DIR="${THIS_DIR}"

  "${UNITY}" \
    -batchmode \
    -projectPath "${ROOT_DIR}" \
    -runEditorTests \
    -testResults "${ROOT_DIR}/TestResult.xml"
}

main_2() {
  local UNITY="${HOME}/Unity/Hub/Editor/2019.4.1f1/Editor/Unity"
  local ROOT_DIR="${THIS_DIR}"

  "${UNITY}" \
    -batchmode \
    -runTests \
    -projectPath "${ROOT_DIR}" \
    -testResults "${ROOT_DIR}/TestResult.xml" \
    -testPlatform editmode
}

main_1() {
  local LIBS=
  local ROOT_DIR="${THIS_DIR}"
  local OUTPUT_DIR="${ROOT_DIR}/Build/Test"
  local OUT="${OUTPUT_DIR}/test.dll"
  local UNITY_DATA="${HOME}/Unity/Hub/Editor/2019.4.1f1/Editor/Data"
  local MONO_BLEEDING_EDGE="${UNITY_DATA}/MonoBleedingEdge"
  local MONO_BLEEDING_EDGE_BIN="${UNITY_DATA}/MonoBleedingEdge/bin"
  local UNITY_ENGINE_DIR="${UNITY_DATA}/Managed/UnityEngine"
  local MCS="${MONO_BLEEDING_EDGE_BIN}/mcs"
  local MONO="${MONO_BLEEDING_EDGE_BIN}/mono"

  local NUNIT_CONSOLE="$(find "${MONO_BLEEDING_EDGE}" -name "nunit-console.exe")"
  local NUNIT_DLL="$(find "${MONO_BLEEDING_EDGE}" -name "nunit.framework.dll")"
  local MONO_45="${MONO_BLEEDING_EDGE}/lib/mono/4.5"

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

  #export MONO_PATH="$(pwd)/NUnit.ConsoleRunner.3.11.1/tools:$(pwd)/NUnit.3.12.0/lib/net45:${UNITY_ENGINE_DIR}"
  export MONO_PATH="${MONO_45}:${UNITY_ENGINE_DIR}"
  export TERM=xterm

  pushd "${MONO_BLEEDING_EDGE_BIN}"
  . "${MONO_BLEEDING_EDGE_BIN}/mono-env"

  "${MCS}" \
    $(find "${ROOT_DIR}/Assets/Scripts" -name "*.cs") \
    $(find "${ROOT_DIR}/Test}" -name "*.cs") \
    -target:library \
    -r:"${LIBS}" \
    -r:"${NUNIT_DLL}" \
    -out:"${OUT}"

  #"${MONO}" "${NUNIT_CONSOLE}" "${OUT}"
  #"./nunit-console" "${OUT}"

  popd
}

main "$@"
