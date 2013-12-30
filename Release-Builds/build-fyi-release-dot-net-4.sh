# Platform optios: "AnyCPU", "x64", "AnyCPU"
# /p:Configuration="Debug" or "Release"


# ************* Begin AnyCPU *********************************************
# Seems to be the only option that matter on linux

xbuild "../DataProviders/DataProviders.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
# xbuild "../pwd/../OracleSP/OracleSp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlAsp/RdlAsp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlCmd/RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlCri/RdlCri.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlDesign/RdlDesign.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlDesktop/RdlDesktop.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlEngine/RdlEngine.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlMapFile/RdlMapFile.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4
xbuild "../RdlViewer/RdlViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /t:clean;rebuild /m:4

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

zip -r majorsilence-reporting-build-dot-net-4-AnyCPU.zip majorsilence-reporting-build-dot-net-4-AnyCPU/

# ************* End AnyCPU *********************************************
