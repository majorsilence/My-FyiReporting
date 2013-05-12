
# Instructions (Ubuntu)
# install build-essential, xbuild, msttcorefonts

cd ../RdlCmd

rm ./bin/Release/RdlCmd.exe
# RdlCmd should still build and work with 3.5 and older versions of linux only suport 3.5 and not 4.0
xbuild "RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="AnyCPU" # /m:4

cd ./bin/Release/

rm ./RdlCmd.c
rm ./RdlCmd.o
rm ./RdlCmd

# find the command for a specific distribution by watching terminal output
#mkbundle -o RdlCmd --deps RdlCmd.exe RdlEngine.dll itextsharp.dll Mono.Security.dll Npgsql.dll ICSharpCode.SharpZipLib.dll --static

# Support building with static glibc
mkbundle -c -o RdlCmd.c -oo RdlCmd.o --deps RdlCmd.exe RdlEngine.dll RdlCri.dll DataProviders.dll itextsharp.dll Mono.Security.dll Npgsql.dll ICSharpCode.SharpZipLib.dll --static

# Debian 6
# gcc -o RdlCmd -Wall `pkg-config --cflags mono` RdlCmd.c  `pkg-config --libs-only-L mono` -Wl,-Bstatic -lmono -Wl,-Bstatic `pkg-config --libs-only-l mono | sed -e "s/\-lmono //"` -static-libgcc RdlCmd.o

# Ubuntu 12.04
gcc -o RdlCmd -Wall `pkg-config --cflags mono-2` RdlCmd.c  `pkg-config --libs-only-L mono-2` -Wl,-Bstatic -lmono-2.0 -Wl,-Bstatic `pkg-config --libs-only-l mono-2 | sed -e "s/\-lmono-2.0 //"` -static-libgcc RdlCmd.o


cd ../../../Release-Builds

rm -rf ./linux-rdlcmd-bundle
mkdir ./linux-rdlcmd-bundle

cp ../RdlCmd/bin/Release/RdlCmd ./linux-rdlcmd-bundle/RdlCmd
