
# Instructions (Ubuntu)
# install build-essential, xbuild

cd ../RdlCmd

rm ./bin/Release/RdlCmd.exe
# RdlCmd should still build and work with 3.5 and older versions of linux only suport 3.5 and not 4.0
xbuild "RdlCmd-3.5-LinuxSupport.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="AnyCPU" # /m:4

cd ./bin/Release/

rm ./RdlCmd.c
rm ./RdlCmd.o
rm ./RdlCmd

# Support building with static glibc
mkbundle -c -o RdlCmd.c -oo RdlCmd.o --deps RdlCmd.exe RdlEngine.dll itextsharp.dll Mono.Security.dll Npgsql.dll ICSharpCode.SharpZipLib.dll --static

gcc -o RdlCmd -Wall `pkg-config --cflags mono` RdlCmd.c  `pkg-config --libs-only-L mono` -Wl,-Bstatic -lmono -Wl,-Bstatic `pkg-config --libs-only-l mono | sed -e "s/\-lmono //"` -static-libgcc RdlCmd.o




cd ../../../Release-Builds

rm -rf ./linux-rdlcmd-bundle
mkdir ./linux-rdlcmd-bundle

cp ../RdlCmd/bin/Release/RdlCmd ./linux-rdlcmd-bundle/RdlCmd
