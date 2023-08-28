#!/bin/bash
set -eu

pushd "$(dirname "$0")" > /dev/null
SCRIPT_PATH=$(pwd)
popd > /dev/null
source "$SCRIPT_PATH/common.sh"

if [ -z "${arch-}" ]; then
    PS3='Build for which arch? '
    select arch in "x64" "aarch64"; do
        if [ -z "$arch" ]; then
            echo "invalid option"
        else
            break
        fi
    done
fi

if [ "$arch" == "aarch64" ]; then
    FFMPEG_FLAGS+=(
        --enable-cross-compile
        --arch=aarch64
        --cross-prefix=aarch64-linux-gnu-
    )
elif [ "$arch" == "x64" ]; then
    FFMPEG_FLAGS+=(
        --enable-vaapi
        --enable-vdpau
        --enable-hwaccel='h264_vaapi,h264_vdpau'
        --enable-hwaccel='hevc_vaapi,hevc_vdpau'
        --enable-hwaccel='vp8_vaapi,vp8_vdpau'
        --enable-hwaccel='vp9_vaapi,vp9_vdpau'
    )
fi

FFMPEG_FLAGS+=(
    --target-os=linux
)

pushd . > /dev/null
prep_ffmpeg "linux-$arch"
build_ffmpeg
popd > /dev/null

# Delete symlinked libraries to prevent weird behaviour with GitHub actions
find "linux-$arch" -type l -delete
# Rename library.so.A.B.C to library.so
for f in "linux-$arch"/*.so.*.*.*; do
    mv -v "$f" "${f%.*.*.*}"
done
