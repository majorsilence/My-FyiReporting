#!/usr/bin/env bash

set -e

# This script builds a macOS package for a Wine application.

# See https://github.com/majorsilence/wine-bundler

export WINEPREFIX=/opt/majorsilence/wine64
export WINEARCH=win64

mkdir -p /opt/majorsilence && apt update \
    && dpkg --add-architecture i386 \
    && apt update \
    && apt install -y --no-install-recommends software-properties-common gnupg2 icnsutils xz-utils zip bc wget curl imagemagick icoutils rsync sed coreutils jq grep \
    && mkdir -pm755 /etc/apt/keyrings \
    && wget -O /etc/apt/keyrings/winehq-archive.key https://dl.winehq.org/wine-builds/winehq.key \
    && wget -NP /etc/apt/sources.list.d/ https://dl.winehq.org/wine-builds/ubuntu/dists/noble/winehq-noble.sources \
    && apt update \
    && apt install -y winehq-devel winbind cabextract xvfb \
    && mkdir -p $WINEPREFIX \
    && apt-get clean

# download bash script https://raw.githubusercontent.com/majorsilence/wine-bundler/refs/heads/master/wine-bundler
wget -O /opt/majorsilence/wine-bundler https://raw.githubusercontent.com/majorsilence/wine-bundler/refs/heads/master/wine-bundler \
	&& chmod +x /opt/majorsilence/wine-bundler

mkdir -p /opt/majorsilence/wine64/drive_c/app
echo 'Directory created'
cp -r /build-output/Release-Builds/build-output/majorsilence-reporting-designer-net8.0-windows-anycpu/net8.0-windows/* /opt/majorsilence/wine64/drive_c/app
echo 'Files copied'
echo 'Changed directory'
mkdir -p /build-output/build/workingdirectory/
mkdir -p /build-output/macpackage-output/
cd /build-output/macpackage-output/
/opt/majorsilence/wine-bundler -i /build-output/Release-Builds/build-output/majorsilence-reporting-designer-net8.0-windows-anycpu/net8.0-windows/reporting.ico -n \\"Majorsilence Reporting Designer\\" -c en_US.UTF-8 -w devel -a win64 -p /opt/majorsilence/wine64 -t /build-output/build/workingdirectory/.cache -s 'c:\\app\\RdlDesigner.exe'

ls -la /build-output/macpackage-output/
