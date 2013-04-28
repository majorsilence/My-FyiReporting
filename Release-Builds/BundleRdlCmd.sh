cd ../RdlCmd

xbuild "RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /m:4

cd ./bin/Release/

mkbundle -o RdlCmd --deps RdlCmd.exe RdlEngine.dll itextsharp.dll Mono.Security.dll Npgsql.dll ICSharpCode.SharpZipLib.dll --static

cd ../../../Release-Builds

rm -rf ./linux-rdlcmd-bundle
mkdir ./linux-rdlcmd-bundle

cp ../RdlCmd/bin/Release/RdlCmd ./linux-rdlcmd-bundle/RdlCmd
