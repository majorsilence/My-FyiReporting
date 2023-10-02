#!/bin/bash
set -e

cd "$(dirname "$0")"

ProjectName="RdlDesign"
BinDir=../$ProjectName/bin/x86/Release
Configuration="Release"
NsisOptions=""

# Сборка релиза
msbuild /p:Configuration=${Configuration} /p:Platform=x86 /t:ReportDesigner ../MajorsilenceReporting.sln

# Очистка бин от лишний файлов

rm -v -f ${BinDir}/*.mdb
rm -v -f ${BinDir}/*.pdb
rm -v -f -R ./Files/*

mkdir -p Files
cp -r -v ${BinDir}/* ./Files

wine ~/.wine/drive_c/Program\ Files\ \(x86\)/NSIS/makensis.exe /INPUTCHARSET UTF8 ${NsisOptions} ${ProjectName}.nsi
