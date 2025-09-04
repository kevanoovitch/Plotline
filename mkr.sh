#!/usr/bin/env bash
set -euo pipefail

# Minimal build/run helper for Plotline
# Usage:
#   ./mkr.sh run [-- <app-args>]
#   ./mkr.sh build [dotnet build args]
#   ./mkr.sh clean [dotnet clean args]
#   ./mkr.sh test  [dotnet test args]
#   ./mkr.sh restore [dotnet restore args]

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$SCRIPT_DIR"
SLN="$ROOT/Plotline.sln"
APP_PROJ="$ROOT/App/PlotLineApp.csproj"

cyan() { echo -e "\033[36m$*\033[0m"; }
red() { echo -e "\033[31m$*\033[0m"; }

cmd=${1:-help}
shift || true

if [[ ! -f "$SLN" ]]; then
  red "Solution not found at $SLN"
  exit 1
fi

case "$cmd" in
  run)
    cyan "Running app (project: $APP_PROJ)"
    (cd "$ROOT" && dotnet run --project "$APP_PROJ" -- "$@")
    ;;
  build)
    cyan "Building solution ($SLN)"
    (cd "$ROOT" && dotnet build "$SLN" "$@")
    ;;
  clean)
    cyan "Cleaning solution ($SLN)"
    (cd "$ROOT" && dotnet clean "$SLN" "$@")
    ;;
  test)
    cyan "Testing solution ($SLN)"
    (cd "$ROOT" && dotnet test "$SLN" "$@")
    ;;
  restore)
    cyan "Restoring solution ($SLN)"
    (cd "$ROOT" && dotnet restore "$SLN" "$@")
    ;;
  help|*)
    cat <<EOF
Plotline helper

Commands:
  run [-- <args>]   Run the app project
  build [args]      Build the solution
  clean [args]      Clean the solution
  test  [args]      Run tests in the solution
  restore [args]    Restore NuGet packages

Examples:
  ./mkr.sh run
  ./mkr.sh build -c Release
  ./mkr.sh clean
  ./mkr.sh test -c Debug
EOF
    ;;
esac

