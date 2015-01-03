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

copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\DataProviders.dll lib\net40\DataProviders.dll /Y
copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCri.dll lib\net40\RdlCri.dll /Y
copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngine\bin\Release\RdlEngine.dll lib\net40\RdlEngine.dll /Y
REM make this a nuget dependency ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ICSharpCode.SharpZipLib.dll /Y

REM make this a nuget dependency copy "..\References\dot net 4\itextsharp.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\itextsharp.dll /Y
copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.xml content\RdlEngineConfig.xml /Y
copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.Linux.xml content\RdlEngineConfig.Linux.xml /Y


mkdir lib\net40\ru-RU
copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlEngine.resources.dll lib\net40\ru-RU\RdlEngine.resources.dll /Y


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

copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlViewer.dll lib\net40\RdlViewer.dll /Y

mkdir lib\net40\ru-RU
copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlViewer.resources.dll lib\net40\ru-RU\RdlViewer.resources.dll /Y

cd ..

nuget pack "%CD%\MajorsilenceReporting-Viewer\MajorsilenceReporting-Viewer.nuspec" -OutputDirectory "%CD%\..\build-output"

cd ..


REM ************* End Viewer *********************************************


REM ************* Begin Asp.net *********************************************

cd nuget/MajorsilenceReporting-Asp
rm -rf lib
rm -rf content


mkdir lib
cd lib
mkdir net40
cd ..

copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlAsp.dll lib\net40\RdlAsp.dll /Y

cd ..

nuget pack "%CD%\MajorsilenceReporting-Asp\MajorsilenceReporting-Asp.nuspec" -OutputDirectory "%CD%\..\build-output"

cd ..


REM ************* End Asp.net *********************************************


REM ************* Begin XwtViewer *********************************************
REM make a seperate package 

cd nuget/MajorsilenceReporting-XwtViewer
rm -rf lib
rm -rf content


mkdir lib
cd lib
mkdir net40
cd ..


copy ..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlCrossPlatformViewer.dll lib\net40\LibRdlCrossPlatformViewer.dll /Y
copy "..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.dll" lib\net40\Xwt.dll /Y
copy "..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.Gtk.dll" lib\net40\Xwt.Gtk.dll /Y
copy "..\..\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.WPF.dll" lib\net40\Xwt.WPF.dll /Y

cd ..

nuget pack "%CD%\MajorsilenceReporting-XwtViewer\MajorsilenceReporting-XwtViewer.nuspec" -OutputDirectory "%CD%\..\build-output"

cd ..


REM ************* End XwtViewer *********************************************




REM future nuget packages



REM make a seperate package copy ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.dll /Y
