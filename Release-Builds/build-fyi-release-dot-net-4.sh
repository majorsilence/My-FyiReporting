#!/usr/bin/env bash
set -e # exit on first error
set -u # exit on using unset variable

# Platform optios: "AnyCPU", "x64", "AnyCPU"
# /p:Configuration="Debug" or "Release"

CURRENTPATH=`pwd`
VERSION_FULL_LINE=$(grep -F "AssemblyVersion" "$CURRENTPATH/../RdlEngine/AssemblyInfo.cs")
VERSION_WITH_ASTERIK=$(echo $VERSION_FULL_LINE | awk -F\" '{print $(NF-1)}')
VERSION="${VERSION_WITH_ASTERIK%??}"

# ************* Begin AnyCPU .NET 4*********************************************
# Seems to be the only option that matter on linux

xbuild "../MajorsilenceReporting-Linux.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU"

rm -rf ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU
mkdir -p ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU

cp ../DataProviders/bin/Release/DataProviders.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/DataProviders.dll 
cp ../RdlAsp/bin/Release/RdlAsp.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlAsp.dll 
cp ../RdlCmd/bin/Release/RdlCmd.exe ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlCmd.exe 
cp ../RdlCri/bin/Release/RdlCri.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlCri.dll 
cp ../RdlDesign/bin/Release/RdlDesigner.exe ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlDesigner.exe 
cp ../RdlDesktop/bin/Release/RdlDesktop.exe ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlDesktop.exe 
cp ../RdlDesktop/bin/Release/config.xml ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/config.xml 
cp ../RdlEngine/bin/Release/RdlEngine.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlEngine.dll 
cp ../RdlEngine/bin/Release/ICSharpCode.SharpZipLib.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ICSharpCode.SharpZipLib.dll  
cp "../References/dot net 4/itextsharp.dll" ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/itextsharp.dll 
cp ../RdlEngine/bin/Release/RdlEngineConfig.xml ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlEngineConfig.xml 
cp ../RdlEngine/bin/Release/RdlEngineConfig.Linux.xml ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlEngineConfig.Linux.xml 
cp ../RdlMapFile/bin/Release/RdlMapFile.exe ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlMapFile.exe 
cp ../RdlViewer/bin/Release/RdlViewer.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlViewer.dll 
cp ../RdlViewer/RdlReader/bin/Release/RdlReader.exe ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/RdlReader.exe 
cp "../References/dot net 3.5/zxing.dll" ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/zxing.dll
cp ../LibRdlCrossPlatformViewer/bin/Release/LibRdlCrossPlatformViewer.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/LibRdlCrossPlatformViewer.dll
cp "../References/dot net 4/Xwt.dll" ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/Xwt.dll
cp "../References/dot net 4/Xwt.Gtk.dll" ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/Xwt.Gtk.dll
cp "../References/dot net 4/Xwt.WPF.dll" ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/Xwt.WPF.dll

mkdir -p ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU
cp ../RdlDesign/bin/Release/ru-RU/RdlDesigner.resources.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU/RdlDesigner.resources.dll
cp ../RdlDesktop/bin/Release/ru-RU/RdlDesktop.resources.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU/RdlDesktop.resources.dll
cp ../RdlEngine/bin/Release/ru-RU/RdlEngine.resources.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU/RdlEngine.resources.dll
cp ../RdlMapFile/bin/Release/ru-RU/RdlMapFile.resources.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU/RdlMapFile.resources.dll
cp ../RdlViewer/bin/Release/ru-RU/RdlViewer.resources.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU/RdlViewer.resources.dll
cp ../RdlViewer/RdlReader/bin/Release/ru-RU/RdlReader.resources.dll ./build-output/majorsilence-reporting-build-dot-net-4-AnyCPU/ru-RU/RdlReader.resources.dll

cd build-output	
zip -r "$VERSION-majorsilence-reporting-build-dot-net-4-AnyCPU.zip" majorsilence-reporting-build-dot-net-4-AnyCPU/
cd ..

# ************* End AnyCPU .NET 4*********************************************
