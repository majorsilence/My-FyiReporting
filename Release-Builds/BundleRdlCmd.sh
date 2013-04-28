
# Instructions (Ubuntu)
# install build-essential, xbuild

cd ../RdlCmd

xbuild "RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" /m:4

cd ./bin/Release/

mkbundle -o RdlCmd --deps RdlCmd.exe RdlEngine.dll itextsharp.dll Mono.Security.dll Npgsql.dll ICSharpCode.SharpZipLib.dll --static

# Support building with static glibc
#mkbundle -c -o RdlCmd.c -oo RdlCmd.o --deps RdlCmd.exe RdlEngine.dll itextsharp.dll Mono.Security.dll Npgsql.dll ICSharpCode.SharpZipLib.dll --static

#gcc -o RdlCmd RdlCmd.c RdlCmd.o -static-libgcc -I /usr/include/mono-2.0/

cd ../../../Release-Builds

rm -rf ./linux-rdlcmd-bundle
mkdir ./linux-rdlcmd-bundle

cp ../RdlCmd/bin/Release/RdlCmd ./linux-rdlcmd-bundle/RdlCmd
