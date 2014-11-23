#!/usr/bin/env bash
set -e # exit on first error
set -u # exit on using unset variable

# Platform optios: "AnyCPU", "x64", "AnyCPU"
# /p:Configuration="Debug" or "Release"


# ************* Begin AnyCPU *********************************************
# Seems to be the only option that matter on linux

xbuild "../MajorsilenceReporting-Linux.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU"

rm -rf ./majorsilence-reporting-build-dot-net-4-AnyCPU
mkdir ./majorsilence-reporting-build-dot-net-4-AnyCPU

cp ../DataProviders/bin/Release/DataProviders.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/DataProviders.dll 
# copy ../OracleSp/bin/Release/OracleSp.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/OracleSp.dll 
cp ../RdlAsp/bin/Release/RdlAsp.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlAsp.dll 
cp ../RdlCmd/bin/Release/RdlCmd.exe ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlCmd.exe 
cp ../RdlCri/bin/Release/RdlCri.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlCri.dll 
cp ../RdlDesign/bin/Release/RdlDesigner.exe ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlDesigner.exe 
cp ../RdlDesktop/bin/Release/RdlDesktop.exe ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlDesktop.exe 
cp ../RdlDesktop/bin/Release/config.xml ./majorsilence-reporting-build-dot-net-4-AnyCPU/config.xml 
cp ../RdlEngine/bin/Release/RdlEngine.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlEngine.dll 
cp ../RdlEngine/bin/Release/ICSharpCode.SharpZipLib.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/ICSharpCode.SharpZipLib.dll 
cp "../References/dot net 4/Mono.Security.dll" ./majorsilence-reporting-build-dot-net-4-AnyCPU/Mono.Security.dll 
cp "../References/dot net 4/Npgsql.dll" ./majorsilence-reporting-build-dot-net-4-AnyCPU/Npgsql.dll 
cp "../References/dot net 4/itextsharp.dll" ./majorsilence-reporting-build-dot-net-4-AnyCPU/itextsharp.dll 
cp ../RdlEngine/bin/Release/RdlEngineConfig.xml ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlEngineConfig.xml 
cp ../RdlMapFile/bin/Release/RdlMapFile.exe ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlMapFile.exe 
cp ../RdlViewer/bin/Release/RdlViewer.dll ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlViewer.dll 
cp ../RdlViewer/RdlReader/bin/Release/RdlReader.exe ./majorsilence-reporting-build-dot-net-4-AnyCPU/RdlReader.exe 
cp "../References/dot net 3.5/zxing.dll" ./majorsilence-reporting-build-dot-net-4-AnyCPU/zxing.dll

zip -r majorsilence-reporting-build-dot-net-4-AnyCPU.zip majorsilence-reporting-build-dot-net-4-AnyCPU/

# ************* End AnyCPU *********************************************

cd nuget/My-FyiReporting
rm -rf lib
rm -rf ../My-FyiReporting-Build

mkdir ../My-FyiReporting-Build
mkdir lib
cd lib
mkdir net40
cd ..

cp -R ../../majorsilence-reporting-build-dot-net-4-AnyCPU/* lib/net40

nuget pack "My-FyiReporting.nuspec" -OutputDirectory "../My-FyiReporting-Build"
