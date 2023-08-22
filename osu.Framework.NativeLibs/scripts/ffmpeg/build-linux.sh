#!/bin/bash
set -eu

pushd "$(dirname "$0")" > /dev/null
SCRIPT_PATH=$(pwd)
popd > /dev/null
source "$SCRIPT_PATH/common.sh"

FFMPEG_FLAGS+=(
    --target-os=linux
)

pushd . > /dev/null
prep_ffmpeg linux-x64
build_ffmpeg
popd
