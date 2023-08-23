#!/bin/bash
set -eu

pushd "$(dirname "$0")" > /dev/null
SCRIPT_PATH=$(pwd)
popd > /dev/null
source "$SCRIPT_PATH/common.sh"

if [ -z "${arch-}" ]; then
    PS3='Build for which arch? '
    select arch in "arm64" "x86_64"; do
        if [ -z "$arch" ]; then
            echo "invalid option"
        else
            break
        fi
    done
fi

FFMPEG_FLAGS+=(
    --target-os=darwin
    --arch=$arch
    --enable-cross-compile
    --extra-cflags="-arch $arch"
    --extra-ldflags="-arch $arch"
)

pushd . > /dev/null
prep_ffmpeg "macOS-$arch"
build_ffmpeg
popd

# Rename dylibs that have a full version string (A.B.C) in their filename
# Example: avcodec.58.10.72.dylib -> avcodec.58.dylib
pushd . > /dev/null
cd "macOS-$arch"
for f in *.*.*.*.dylib; do
    [ -f "$f" ] || continue
    mv -v "$f" "${f%.*.*.*}.dylib"
done
popd

echo "-> Fixing dylibs paths..."
BUILDPATH="macOS-$arch"
LIBS="libavcodec.58.dylib libavdevice.58.dylib libavfilter.7.dylib libavformat.58.dylib libavutil.56.dylib libswresample.3.dylib libswscale.5.dylib"
for f in $LIBS; do
    install_name_tool "$BUILDPATH/$f" -id "@loader_path/$f" \
        -change $BUILDPATH/libavcodec.58.dylib @loader_path/libavcodec.58.dylib \
        -change $BUILDPATH/libavfilter.7.dylib @loader_path/libavfilter.7.dylib \
        -change $BUILDPATH/libavformat.58.dylib @loader_path/libavformat.58.dylib \
        -change $BUILDPATH/libavutil.56.dylib @loader_path/libavutil.56.dylib \
        -change $BUILDPATH/libswscale.5.dylib @loader_path/libswscale.5.dylib
done
