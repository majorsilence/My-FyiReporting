REM ************* Begin CORE *********************************************
REM http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package


REM Core reporting nuget package
cd nuget/MajorsilenceReporting-Core
rm -rf lib
rm -rf content
rm -rf ..\MajorsilenceReporting-Core-Build

mkdir lib
mkdir content
cd lib
mkdir net40
cd ..

copy ..\..\..\DataProviders\bin\Release\DataProviders.dll lib\net40\DataProviders.dll /Y
copy ..\..\..\RdlCri\bin\Release\RdlCri.dll lib\net40\RdlCri.dll /Y
copy ..\..\..\RdlEngine\bin\Release\RdlEngine.dll lib\net40\RdlEngine.dll /Y
REM make this a nuget dependency ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ICSharpCode.SharpZipLib.dll /Y

REM make this a nuget dependency copy "..\References\dot net 4\itextsharp.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\itextsharp.dll /Y
copy ..\..\..\RdlEngine\bin\Release\RdlEngineConfig.xml content\RdlEngineConfig.xml /Y
copy ..\..\..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml content\RdlEngineConfig.Linux.xml /Y


mkdir lib\net40\ru-RU
copy ..\..\..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll lib\net40\ru-RU\RdlEngine.resources.dll /Y


cd ..

nuget pack "%CD%\MajorsilenceReporting-Core\MajorsilenceReporting-Core.nuspec" -OutputDirectory "%CD%\..\build-output"

cd ..

REM ************* End CORE *********************************************




REM ************* Begin Viewer *********************************************
REM make a seperate package 

cd nuget/MajorsilenceReporting-Viewer
rm -rf lib
rm -rf content
rm -rf ..\MajorsilenceReporting-Viewer-Build

mkdir ..\MajorsilenceReporting-Viewer-Build
mkdir lib
mkdir content
cd lib
mkdir net40
cd ..

copy ..\..\..\RdlViewer\bin\Release\RdlViewer.dll lib\net40\RdlViewer.dll /Y

mkdir lib\net40\ru-RU
copy ..\..\..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll lib\net40\ru-RU\RdlViewer.resources.dll /Y

cd ..

nuget pack "%CD%\MajorsilenceReporting-Viewer\MajorsilenceReporting-Viewer.nuspec" -OutputDirectory "%CD%\..\build-output"

cd ..


REM ************* End Viewer *********************************************


# ************* Begin Asp.net *********************************************
# make a seperate package 

cd nuget/MajorsilenceReporting-Asp
rm -rf lib
rm -rf content


mkdir lib
cd lib
mkdir net40
cd ..

copy ../../..//RdlAsp/bin/Release/RdlAsp.dll lib/net40/RdlAsp.dll /Y

cd ..

nuget pack "%CD%/MajorsilenceReporting-Asp/MajorsilenceReporting-Asp.nuspec" -OutputDirectory "%CD%/../build-output"

cd ..


# ************* End Asp.net *********************************************



REM future nuget packages



REM make a seperate package copy ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.dll /Y

REM make a separate package copy ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlCrossPlatformViewer.dll /Y
REM copy "..\References\dot net 4\Xwt.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.dll /Y
REM copy "..\References\dot net 4\Xwt.Gtk.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.Gtk.dll /Y
REM copy "..\References\dot net 4\Xwt.WPF.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.WPF.dll /Y
REM copy "..\References\dot net 3.5\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\zxing.dll /Y
