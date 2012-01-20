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

rm -rf ./my-fyi-build-dot-net-4-AnyCPU
mkdir ./my-fyi-build-dot-net-4-AnyCPU

cp ../DataProviders/bin/Release/DataProviders.dll ./my-fyi-build-dot-net-4-AnyCPU/DataProviders.dll 
# copy ../OracleSp/bin/Release/OracleSp.dll ./my-fyi-build-dot-net-4-AnyCPU/OracleSp.dll 
cp ../RdlAsp/bin/Release/RdlAsp.dll ./my-fyi-build-dot-net-4-AnyCPU/RdlAsp.dll 
cp ../RdlCmd/bin/Release/RdlCmd.exe ./my-fyi-build-dot-net-4-AnyCPU/RdlCmd.exe 
cp ../RdlCri/bin/Release/RdlCri.dll ./my-fyi-build-dot-net-4-AnyCPU/RdlCri.dll 
cp ../RdlDesign/bin/Release/RdlDesigner.exe ./my-fyi-build-dot-net-4-AnyCPU/RdlDesigner.exe 
cp ../RdlDesktop/bin/Release/RdlDesktop.exe ./my-fyi-build-dot-net-4-AnyCPU/RdlDesktop.exe 
cp ../RdlDesktop/bin/Release/config.xml ./my-fyi-build-dot-net-4-AnyCPU/config.xml 
cp ../RdlEngine/bin/Release/RdlEngine.dll ./my-fyi-build-dot-net-4-AnyCPU/RdlEngine.dll 
cp ../RdlEngine/bin/Release/ICSharpCode.SharpZipLib.dll ./my-fyi-build-dot-net-4-AnyCPU/ICSharpCode.SharpZipLib.dll 
cp "../References/dot net 4/Mono.Security.dll" ./my-fyi-build-dot-net-4-AnyCPU/Mono.Security.dll 
cp "../References/dot net 4/Npgsql.dll" ./my-fyi-build-dot-net-4-AnyCPU/Npgsql.dll  
cp ../RdlEngine/bin/Release/RdlEngineConfig.xml ./my-fyi-build-dot-net-4-AnyCPU/RdlEngineConfig.xml 
cp ../RdlMapFile/bin/Release/RdlMapFile.exe ./my-fyi-build-dot-net-4-AnyCPU/RdlMapFile.exe 
cp ../RdlViewer/bin/Release/RdlViewer.dll ./my-fyi-build-dot-net-4-AnyCPU/RdlViewer.dll 
cp ../RdlViewer/RdlReader/bin/Release/RdlReader.exe ./my-fyi-build-dot-net-4-AnyCPU/RdlReader.exe 

zip -r my-fyi-build-dot-net-4-AnyCPU.zip my-fyi-build-dot-net-4-AnyCPU/

# ************* End AnyCPU *********************************************
