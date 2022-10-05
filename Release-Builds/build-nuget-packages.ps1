#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$CURRENTPATH=$pwd.Path


function delete_files([string]$path)
{
	If (Test-Path $path){
		Write-Host "Deleting path $path" -ForegroundColor Green
		Remove-Item -recurse -force $path
	}
}

# ************* Begin CORE *********************************************
# http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package


# Core reporting nuget package
cd nuget/MajorsilenceReporting-Core
delete_files "lib"
delete_files "content"
delete_files "..\MajorsilenceReporting-Core-Build"

mkdir lib
mkdir content
cd lib
mkdir net48
cd ..

# net 40
Copy-Item ..\..\build-output\majorsilence-reporting-x86\DataProviders.dll lib\net48\DataProviders.dll 
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlCri.dll lib\net48\RdlCri.dll
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlEngine.dll lib\net48\RdlEngine.dll
Copy-Item ..\..\build-output\majorsilence-reporting-x86\DataProviders.xml lib\net48\DataProviders.xml
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlCri.xml lib\net48\RdlCri.xml
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlEngine.xml lib\net48\RdlEngine.xml
mkdir lib\net48\ru-RU
Copy-Item ..\..\build-output\majorsilence-reporting-x86\ru-RU\RdlEngine.resources.dll lib\net48\ru-RU\RdlEngine.resources.dll

# Content
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlEngineConfig.xml content\RdlEngineConfig.xml
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlEngineConfig.Linux.xml content\RdlEngineConfig.Linux.xml

cd ..
$CD=$(Get-Location).Path
nuget pack "$CD\MajorsilenceReporting-Core\MajorsilenceReporting-Core.nuspec" -OutputDirectory "$CD\..\build-output"

cd ..

# ************* End CORE *********************************************




# ************* Begin Viewer *********************************************
# make a seperate package 

cd nuget/MajorsilenceReporting-Viewer
delete_files "lib"
delete_files "content"
delete_files "..\MajorsilenceReporting-Viewer-Build"

mkdir lib
mkdir content
cd lib
mkdir net48
cd ..

# net48
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlViewer.dll lib\net48\RdlViewer.dll
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlViewer.xml lib\net48\RdlViewer.xml
Copy-Item ..\..\build-output\majorsilence-reporting-x86\EncryptionProvider.dll lib\net48\EncryptionProvider.dll
mkdir lib\net48\ru-RU
Copy-Item ..\..\build-output\majorsilence-reporting-x86\ru-RU\RdlViewer.resources.dll lib\net48\ru-RU\RdlViewer.resources.dll

cd ..
$CD=$(Get-Location).Path
nuget pack "$CD\MajorsilenceReporting-Viewer\MajorsilenceReporting-Viewer.nuspec" -OutputDirectory "$CD\..\build-output"

cd ..


# ************* End Viewer *********************************************


# ************* Begin Asp.net *********************************************

cd nuget/MajorsilenceReporting-Asp
delete_files "lib"
delete_files "content"


mkdir lib
cd lib
mkdir net48
cd ..

# net48
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlAsp.dll lib\net48\RdlAsp.dll
Copy-Item ..\..\build-output\majorsilence-reporting-x86\RdlAsp.xml lib\net48\RdlAsp.xml

cd ..

nuget pack "$CD\MajorsilenceReporting-Asp\MajorsilenceReporting-Asp.nuspec" -OutputDirectory "$CD\..\build-output"

cd ..


# ************* End Asp.net *********************************************


# ************* Begin XwtViewer *********************************************
# make a seperate package 

cd nuget/MajorsilenceReporting-XwtViewer
delete_files "lib"
delete_files "content"


mkdir lib
cd lib
mkdir net48
# xwt only support .net 4
cd ..


Copy-Item ..\..\build-output\majorsilence-reporting-x86\LibRdlCrossPlatformViewer.dll lib\net48\LibRdlCrossPlatformViewer.dll
Copy-Item ..\..\build-output\majorsilence-reporting-x86\LibRdlCrossPlatformViewer.xml lib\net48\LibRdlCrossPlatformViewer.xml
Copy-Item "..\..\build-output\majorsilence-reporting-x86\Xwt.dll" lib\net48\Xwt.dll
Copy-Item "..\..\build-output\majorsilence-reporting-x86\Xwt.Gtk.dll" lib\net48\Xwt.Gtk.dll
Copy-Item "..\..\build-output\majorsilence-reporting-x86\Xwt.WPF.dll" lib\net48\Xwt.WPF.dll

cd ..
$CD=$(Get-Location).Path
nuget pack "$CD\MajorsilenceReporting-XwtViewer\MajorsilenceReporting-XwtViewer.nuspec" -OutputDirectory "$CD\..\build-output"

cd ..


# ************* End XwtViewer *********************************************




# ************* Begin WpfViewer *********************************************
# make a seperate package 

cd nuget/MajorsilenceReporting-WpfViewer
delete_files "lib"
delete_files "content"


mkdir lib
cd lib
mkdir net48
cd ..

# net48
Copy-Item ..\..\build-output\majorsilence-reporting-x86\LibRdlWpfViewer.dll lib\net48\LibRdlWpfViewer.dll
Copy-Item ..\..\build-output\majorsilence-reporting-x86\LibRdlWpfViewer.xml lib\net48\LibRdlWpfViewer.xml

cd ..

$CD=$(Get-Location).Path
nuget pack "$CD\MajorsilenceReporting-WpfViewer\MajorsilenceReporting-WpfViewer.nuspec" -OutputDirectory "$CD\..\build-output"

cd ..


# ************* End WpfViewer *********************************************



# future nuget packages



# make a seperate package Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-x64\LibRdlWpfViewer.dll /Y
