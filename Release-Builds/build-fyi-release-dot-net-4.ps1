#!/usr/bin/env powershell
$ErrorActionPreference = "Stop"
$CURRENTPATH=$pwd.Path

# Platform options: "x86", "x64", "x64"
# /p:Configuration="Debug" or "Release"

# set msbuildpath="%ProgramFiles(x86)%\MSBuild\14.0\bin\MSBuild.exe"
$msbuildpath="c:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"

function delete_files([string]$path)
{
	If (Test-Path $path){
		Write-Host "Deleting path $path" -ForegroundColor Green
		Remove-Item -recurse -force $path
	}
}

function GetVersions([ref]$theVersion)
{
	$path = "$CURRENTPATH/../RdlEngine/AssemblyInfo.cs"
	$pattern = '\[assembly: AssemblyVersion\("(.*)"\)\]'
	(Get-Content $path) | ForEach-Object{
		if($_ -match $pattern){
			# We have found the matching line
			$version = [string]$matches[1] 
			$theVersion.value= $version -replace "\.\*", ""
			
			$_
		} else {
			# Output line as is
			$_
		}
	} | Set-Content $path
}

$Version=""
GetVersions([ref]$Version)
Write-Host $Version
nuget restore "../MajorsilenceReporting.sln"

# ************* Begin x64 *********************************************

& "$msbuildpath" "$CURRENTPATH\..\MajorsilenceReporting.sln" /verbosity:minimal /p:Configuration="Release" /property:Platform="x64" /target:clean /target:rebuild

Remove-Item "$CURRENTPATH\build-output\majorsilence-reporting-build-dot-net-4-x64" -Recurse -ErrorAction Ignore
mkdir "$CURRENTPATH\build-output\majorsilence-reporting-build-dot-net-4-x64"


Copy-Item ..\DataProviders\bin\Release\DataProviders.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\DataProviders.dll
Copy-Item ..\DataProviders\bin\Release\DataProviders.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\DataProviders.xml
# Copy-Item ..\OracleSp\bin\x64\Release\OracleSp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\OracleSp.dll
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlAsp.dll
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlAsp.xml
Copy-Item ..\RdlCmd\bin\x64\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlCmd.exe
Copy-Item ..\RdlCri\bin\Release\RdlCri.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlCri.dll
Copy-Item ..\RdlCri\bin\Release\RdlCri.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlCri.xml
Copy-Item ..\RdlDesign\bin\x64\Release\RdlDesigner.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlDesigner.exe
Copy-Item ..\RdlDesktop\bin\x64\Release\RdlDesktop.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlDesktop.exe
Copy-Item ..\RdlDesktop\bin\x64\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\config.xml
Copy-Item ..\packages\jacobslusser.ScintillaNET.3.5.6\lib\net40\ScintillaNET.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ScintillaNET.dll
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngine.dll
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngine.xml
Copy-Item ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ICSharpCode.SharpZipLib.dll
Copy-Item "..\References\dot net 4\64bit\System.Data.SQLite.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\System.Data.SQLite.dll
Copy-Item "..\packages\iTextSharp-LGPL.4.1.6\lib\iTextSharp.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\iTextSharp.dll
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngineConfig.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngineConfig.Linux.xml
Copy-Item ..\RdlMapFile\bin\x64\Release\RdlMapFile.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlMapFile.exe
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlViewer.dll
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlViewer.xml
Copy-Item ..\RdlViewer\bin\Release\EncryptionProvider.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\EncryptionProvider.dll
Copy-Item ..\RdlViewer\RdlReader\bin\x64\Release\RdlReader.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlReader.exe
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.dll
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.xml
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlCrossPlatformViewer.dll
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlCrossPlatformViewer.xml
Copy-Item "..\References\dot net 4\Xwt.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.dll
Copy-Item "..\References\dot net 4\Xwt.Gtk.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.Gtk.dll
Copy-Item "..\References\dot net 4\Xwt.WPF.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.WPF.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\zxing.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.presentation.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\zxing.presentation.dll

mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU
Copy-Item ..\RdlDesign\bin\x64\Release\ru-RU\RdlDesigner.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlDesigner.resources.dll
Copy-Item ..\RdlDesktop\bin\x64\Release\ru-RU\RdlDesktop.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlDesktop.resources.dll
Copy-Item ..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlEngine.resources.dll
Copy-Item ..\RdlMapFile\bin\x64\Release\ru-RU\RdlMapFile.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlMapFile.resources.dll
Copy-Item ..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlViewer.resources.dll
Copy-Item ..\RdlViewer\RdlReader\bin\x64\Release\ru-RU\RdlReader.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlReader.resources.dll

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-dot-net-4-x64.zip majorsilence-reporting-build-dot-net-4-x64\
cd ..

# ************* End x64 *********************************************


# ************* Begin x86 *********************************************

& "$msbuildpath" "$CURRENTPATH\..\MajorsilenceReporting.sln" /verbosity:minimal /property:Configuration="Release" /property:Platform="x86" /target:clean /target:rebuild

Remove-Item "$CURRENTPATH\build-output\majorsilence-reporting-build-dot-net-4-x86" -Recurse -ErrorAction Ignore
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x86

Copy-Item ..\DataProviders\bin\Release\DataProviders.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\DataProviders.dll
Copy-Item ..\DataProviders\bin\Release\DataProviders.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\DataProviders.xml
# Copy-Item ..\OracleSp\bin\x86\Release\OracleSp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\OracleSp.dll
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlAsp.dll
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlAsp.xml
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCmd.exe
Copy-Item ..\RdlCri\bin\Release\RdlCri.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCri.dll
Copy-Item ..\RdlCri\bin\Release\RdlCri.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCri.xml
Copy-Item ..\RdlDesign\bin\x86\Release\RdlDesigner.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlDesigner.exe
Copy-Item ..\RdlDesktop\bin\x86\Release\RdlDesktop.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlDesktop.exe
Copy-Item ..\packages\jacobslusser.ScintillaNET.3.5.6\lib\net40\ScintillaNET.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ScintillaNET.dll
Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\config.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngine.dll
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngine.xml
Copy-Item ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ICSharpCode.SharpZipLib.dll
Copy-Item "..\References\dot net 4\32bit\System.Data.SQLite.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\System.Data.SQLite.dll
Copy-Item "..\packages\iTextSharp-LGPL.4.1.6\lib\iTextSharp.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\iTextSharp.dll
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.Linux.xml
Copy-Item ..\RdlMapFile\bin\x86\Release\RdlMapFile.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlMapFile.exe
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlViewer.dll
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlViewer.xml
Copy-Item ..\RdlViewer\bin\Release\EncryptionProvider.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\EncryptionProvider.dll
Copy-Item ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlReader.exe
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlWpfViewer.dll
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlWpfViewer.xml
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlCrossPlatformViewer.dll
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlCrossPlatformViewer.xml
Copy-Item "..\References\dot net 4\Xwt.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.dll
Copy-Item "..\References\dot net 4\Xwt.Gtk.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.Gtk.dll
Copy-Item "..\References\dot net 4\Xwt.WPF.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.WPF.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\zxing.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.presentation.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\zxing.presentation.dll

mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU
Copy-Item ..\RdlDesign\bin\x86\Release\ru-RU\RdlDesigner.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlDesigner.resources.dll
Copy-Item ..\RdlDesktop\bin\x86\Release\ru-RU\RdlDesktop.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlDesktop.resources.dll
Copy-Item ..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlEngine.resources.dll
Copy-Item ..\RdlMapFile\bin\x86\Release\ru-RU\RdlMapFile.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlMapFile.resources.dll
Copy-Item ..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlViewer.resources.dll
Copy-Item ..\RdlViewer\RdlReader\bin\x86\Release\ru-RU\RdlReader.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlReader.resources.dll

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-dot-net-4-x86.zip majorsilence-reporting-build-dot-net-4-x86\
cd ..

# ************* End x86 *********************************************



# ************* Begin PHP *********************************************
delete_files .\build-output\majorsilence-reporting-build-dot-net-4-php-x86
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-php-x86

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\config.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlEngineConfig.xml
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlCri.dll
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlCmd.exe
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\DataProviders.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlEngine.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\ICSharpCode.SharpZipLib.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlViewer.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\zxing.dll

Copy-Item "..\LanguageWrappers\php\config.php" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\config.php
Copy-Item "..\LanguageWrappers\php\report.php" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\report.php

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-dot-net-4-php-x86.zip majorsilence-reporting-build-dot-net-4-php-x86\
cd ..

# ************* End PHP *********************************************



# ************* Begin Python *********************************************
delete_files .\build-output\majorsilence-reporting-build-dot-net-4-python-x86
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-python-x86

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\config.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlEngineConfig.xml
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlCri.dll
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlCmd.exe
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\DataProviders.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlEngine.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\ICSharpCode.SharpZipLib.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlViewer.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\zxing.dll

Copy-Item "..\LanguageWrappers\python\config.py" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\config.py
Copy-Item "..\LanguageWrappers\python\report.py" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\report.py

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-dot-net-4-python-x86.zip majorsilence-reporting-build-dot-net-4-python-x86\
cd ..
# ************* End Python *********************************************


# ************* Begin Ruby *********************************************
delete_files .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\config.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlEngineConfig.xml
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlCri.dll
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlCmd.exe
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\DataProviders.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlEngine.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\ICSharpCode.SharpZipLib.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlViewer.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\zxing.dll

Copy-Item "..\LanguageWrappers\ruby\config.rb" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\config.rb
Copy-Item "..\LanguageWrappers\ruby\report.rb" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\report.rb

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-dot-net-4-ruby-x86.zip majorsilence-reporting-build-dot-net-4-ruby-x86\
cd ..

# ************* End Ruby *********************************************

delete_files .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\config.xml
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlEngineConfig.xml
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlCri.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlReader.exe
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\DataProviders.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlEngine.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\ICSharpCode.SharpZipLib.dll
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlViewer.dll
Copy-Item "..\packages\ZXing.Net.0.16.4\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\zxing.dll
delete_files .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlViewerSC.pdb

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-dot-net-4-viewer-x86.zip majorsilence-reporting-build-dot-net-4-viewer-x86\
cd ..


