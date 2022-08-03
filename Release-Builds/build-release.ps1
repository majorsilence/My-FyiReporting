#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$CURRENTPATH=$pwd.Path

# Platform options: "x86", "x64", "x64"
# /p:Configuration="Debug" or "Release"

# set msbuildpath="%ProgramFiles(x86)%\MSBuild\14.0\bin\MSBuild.exe"
$msbuildpath="C:\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
If (Test-Path "C:\BuildTools\MSBuild\Current\Bin\MSBuild.exe"){
	$msbuildpath="C:\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
}
ElseIf (Test-Path "C:\BuildTools\MSBuild\17.0\Bin\MSBuild.exe"){
	$msbuildpath="C:\BuildTools\MSBuild\17.0\Bin\MSBuild.exe"
}
Else {
	$msbuildpath="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
}


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

$buildoutputpath_x64="$CURRENTPATH\build-output\majorsilence-reporting-x64"
Remove-Item "$buildoutputpath_x64" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_x64"




Copy-Item ..\DataProviders\bin\Release\DataProviders.dll "$buildoutputpath_x64\DataProviders.dll"
Copy-Item ..\DataProviders\bin\Release\DataProviders.xml "$buildoutputpath_x64\DataProviders.xml"
# Copy-Item ..\OracleSp\bin\x64\Release\OracleSp.dll "$buildoutputpath_x64\OracleSp.dll"
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.dll "$buildoutputpath_x64\RdlAsp.dll"
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.xml "$buildoutputpath_x64\RdlAsp.xml"
Copy-Item ..\RdlCmd\bin\x64\Release\RdlCmd.exe "$buildoutputpath_x64\RdlCmd.exe"
Copy-Item ..\RdlCri\bin\Release\RdlCri.dll "$buildoutputpath_x64\RdlCri.dll"
Copy-Item ..\RdlCri\bin\Release\RdlCri.xml "$buildoutputpath_x64\RdlCri.xml"
Copy-Item ..\RdlDesign\bin\x64\Release\RdlDesigner.exe "$buildoutputpath_x64\RdlDesigner.exe"
Copy-Item ..\RdlDesktop\bin\x64\Release\RdlDesktop.exe "$buildoutputpath_x64\RdlDesktop.exe"
Copy-Item ..\RdlDesktop\bin\x64\Release\config.xml "$buildoutputpath_x64\config.xml"
Copy-Item ..\packages\jacobslusser.ScintillaNET.3.5.6\lib\net40\ScintillaNET.dll "$buildoutputpath_x64\ScintillaNET.dll"
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.dll "$buildoutputpath_x64\RdlEngine.dll"
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.xml "$buildoutputpath_x64\RdlEngine.xml"
Copy-Item ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll "$buildoutputpath_x64\ICSharpCode.SharpZipLib.dll"
Copy-Item "..\References\dot net 4\64bit\System.Data.SQLite.dll" "$buildoutputpath_x64\System.Data.SQLite.dll"
Copy-Item "..\packages\iTextSharp-LGPL.4.1.6\lib\iTextSharp.dll" "$buildoutputpath_x64\iTextSharp.dll"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml "$buildoutputpath_x64\RdlEngineConfig.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml "$buildoutputpath_x64\RdlEngineConfig.Linux.xml"
Copy-Item ..\RdlMapFile\bin\x64\Release\RdlMapFile.exe "$buildoutputpath_x64\RdlMapFile.exe"
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.dll "$buildoutputpath_x64\RdlViewer.dll"
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.xml "$buildoutputpath_x64\RdlViewer.xml"
Copy-Item ..\RdlViewer\bin\Release\EncryptionProvider.dll "$buildoutputpath_x64\EncryptionProvider.dll"
Copy-Item ..\RdlViewer\RdlReader\bin\x64\Release\RdlReader.exe "$buildoutputpath_x64\RdlReader.exe"
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll "$buildoutputpath_x64\LibRdlWpfViewer.dll"
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.xml "$buildoutputpath_x64\LibRdlWpfViewer.xml"
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll "$buildoutputpath_x64\LibRdlCrossPlatformViewer.dll"
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.xml "$buildoutputpath_x64\LibRdlCrossPlatformViewer.xml"
Copy-Item "..\References\dot net 4\Xwt.dll" "$buildoutputpath_x64\Xwt.dll"
Copy-Item "..\References\dot net 4\Xwt.Gtk.dll" "$buildoutputpath_x64\Xwt.Gtk.dll"
Copy-Item "..\References\dot net 4\Xwt.WPF.dll" "$buildoutputpath_x64\Xwt.WPF.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.dll" "$buildoutputpath_x64\zxing.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.presentation.dll" "$buildoutputpath_x64\zxing.presentation.dll"

mkdir "$buildoutputpath_x64\ru-RU"
Copy-Item ..\RdlDesign\bin\x64\Release\ru-RU\RdlDesigner.resources.dll "$buildoutputpath_x64\ru-RU\RdlDesigner.resources.dll"
Copy-Item ..\RdlDesktop\bin\x64\Release\ru-RU\RdlDesktop.resources.dll "$buildoutputpath_x64\ru-RU\RdlDesktop.resources.dll"
Copy-Item ..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll "$buildoutputpath_x64\ru-RU\RdlEngine.resources.dll"
Copy-Item ..\RdlMapFile\bin\x64\Release\ru-RU\RdlMapFile.resources.dll "$buildoutputpath_x64\ru-RU\RdlMapFile.resources.dll"
Copy-Item ..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll "$buildoutputpath_x64\ru-RU\RdlViewer.resources.dll"
Copy-Item ..\RdlViewer\RdlReader\bin\x64\Release\ru-RU\RdlReader.resources.dll "$buildoutputpath_x64\ru-RU\RdlReader.resources.dll"

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-x64.zip majorsilence-reporting-build-x64\
cd ..

# ************* End x64 *********************************************


# ************* Begin x86 *********************************************

& "$msbuildpath" "$CURRENTPATH\..\MajorsilenceReporting.sln" /verbosity:minimal /property:Configuration="Release" /property:Platform="x86" /target:clean /target:rebuild

$buildoutputpath_x86="$CURRENTPATH\build-output\majorsilence-reporting-x86"
Remove-Item "$buildoutputpath_x86" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_x86"

Copy-Item ..\DataProviders\bin\Release\DataProviders.dll "$buildoutputpath_x86\DataProviders.dll"
Copy-Item ..\DataProviders\bin\Release\DataProviders.xml "$buildoutputpath_x86\DataProviders.xml"
# Copy-Item ..\OracleSp\bin\x86\Release\OracleSp.dll "$buildoutputpath_x86\OracleSp.dll"
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.dll "$buildoutputpath_x86\RdlAsp.dll"
Copy-Item ..\RdlAsp\bin\Release\RdlAsp.xml "$buildoutputpath_x86\RdlAsp.xml"
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe "$buildoutputpath_x86\RdlCmd.exe"
Copy-Item ..\RdlCri\bin\Release\RdlCri.dll "$buildoutputpath_x86\RdlCri.dll"
Copy-Item ..\RdlCri\bin\Release\RdlCri.xml "$buildoutputpath_x86\RdlCri.xml"
Copy-Item ..\RdlDesign\bin\x86\Release\RdlDesigner.exe "$buildoutputpath_x86\RdlDesigner.exe"
Copy-Item ..\RdlDesktop\bin\x86\Release\RdlDesktop.exe "$buildoutputpath_x86\RdlDesktop.exe"
Copy-Item ..\packages\jacobslusser.ScintillaNET.3.5.6\lib\net40\ScintillaNET.dll "$buildoutputpath_x86\ScintillaNET.dll"
Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml "$buildoutputpath_x86\config.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.dll "$buildoutputpath_x86\RdlEngine.dll"
Copy-Item ..\RdlEngine\bin\Release\RdlEngine.xml "$buildoutputpath_x86\RdlEngine.xml"
Copy-Item ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll "$buildoutputpath_x86\ICSharpCode.SharpZipLib.dll"
Copy-Item "..\References\dot net 4\32bit\System.Data.SQLite.dll" "$buildoutputpath_x86\System.Data.SQLite.dll"
Copy-Item "..\packages\iTextSharp-LGPL.4.1.6\lib\iTextSharp.dll" "$buildoutputpath_x86\iTextSharp.dll"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml "$buildoutputpath_x86\RdlEngineConfig.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml "$buildoutputpath_x86\RdlEngineConfig.Linux.xml"
Copy-Item ..\RdlMapFile\bin\x86\Release\RdlMapFile.exe "$buildoutputpath_x86\RdlMapFile.exe"
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.dll "$buildoutputpath_x86\RdlViewer.dll"
Copy-Item ..\RdlViewer\bin\Release\RdlViewer.xml "$buildoutputpath_x86\RdlViewer.xml"
Copy-Item ..\RdlViewer\bin\Release\EncryptionProvider.dll "$buildoutputpath_x86\EncryptionProvider.dll"
Copy-Item ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe "$buildoutputpath_x86\RdlReader.exe"
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll "$buildoutputpath_x86\LibRdlWpfViewer.dll"
Copy-Item ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.xml "$buildoutputpath_x86\LibRdlWpfViewer.xml"
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll "$buildoutputpath_x86\LibRdlCrossPlatformViewer.dll"
Copy-Item ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.xml "$buildoutputpath_x86\LibRdlCrossPlatformViewer.xml"
Copy-Item "..\References\dot net 4\Xwt.dll" "$buildoutputpath_x86\Xwt.dll"
Copy-Item "..\References\dot net 4\Xwt.Gtk.dll" "$buildoutputpath_x86\Xwt.Gtk.dll"
Copy-Item "..\References\dot net 4\Xwt.WPF.dll" "$buildoutputpath_x86\Xwt.WPF.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.dll" "$buildoutputpath_x86\zxing.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.presentation.dll" "$buildoutputpath_x86\zxing.presentation.dll"

mkdir "$buildoutputpath_x86\ru-RU"
Copy-Item ..\RdlDesign\bin\x86\Release\ru-RU\RdlDesigner.resources.dll "$buildoutputpath_x86\ru-RU\RdlDesigner.resources.dll"
Copy-Item ..\RdlDesktop\bin\x86\Release\ru-RU\RdlDesktop.resources.dll "$buildoutputpath_x86\ru-RU\RdlDesktop.resources.dll"
Copy-Item ..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll "$buildoutputpath_x86\ru-RU\RdlEngine.resources.dll"
Copy-Item ..\RdlMapFile\bin\x86\Release\ru-RU\RdlMapFile.resources.dll "$buildoutputpath_x86\ru-RU\RdlMapFile.resources.dll"
Copy-Item ..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll "$buildoutputpath_x86\ru-RU\RdlViewer.resources.dll"
Copy-Item ..\RdlViewer\RdlReader\bin\x86\Release\ru-RU\RdlReader.resources.dll "$buildoutputpath_x86\ru-RU\RdlReader.resources.dll"

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-x86.zip majorsilence-reporting-x86\
cd ..

# ************* End x86 *********************************************



# ************* Begin PHP *********************************************
$buildoutputpath_php_x86="$CURRENTPATH\build-output\majorsilence-reporting-php-x86"
delete_files "$buildoutputpath_php_x86"
mkdir "$buildoutputpath_php_x86"

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml "$buildoutputpath_php_x86\config.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml "$buildoutputpath_php_x86\RdlEngineConfig.xml"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" "$buildoutputpath_php_x86\RdlCri.dll"
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe "$buildoutputpath_php_x86\RdlCmd.exe"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" "$buildoutputpath_php_x86\DataProviders.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" "$buildoutputpath_php_x86\RdlEngine.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" "$buildoutputpath_php_x86\ICSharpCode.SharpZipLib.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" "$buildoutputpath_php_x86\RdlViewer.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.dll" "$buildoutputpath_php_x86\zxing.dll"

Copy-Item "..\LanguageWrappers\php\config.php" "$buildoutputpath_php_x86\config.php"
Copy-Item "..\LanguageWrappers\php\report.php" "$buildoutputpath_php_x86\report.php"

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-php-x86.zip majorsilence-reporting-php-x86\
cd ..

# ************* End PHP *********************************************



# ************* Begin Python *********************************************
$buildoutputpath_python_x86="$CURRENTPATH\build-output\majorsilence-reporting-python-x86"
delete_files "$buildoutputpath_python_x86"
mkdir "$buildoutputpath_python_x86"

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml "$buildoutputpath_python_x86\config.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml "$buildoutputpath_python_x86\RdlEngineConfig.xml"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" "$buildoutputpath_python_x86\RdlCri.dll"
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe "$buildoutputpath_python_x86\RdlCmd.exe"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" "$buildoutputpath_python_x86\DataProviders.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" "$buildoutputpath_python_x86\RdlEngine.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" "$buildoutputpath_python_x86\ICSharpCode.SharpZipLib.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" "$buildoutputpath_python_x86\RdlViewer.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.dll" "$buildoutputpath_python_x86\zxing.dll"

Copy-Item "..\LanguageWrappers\python\config.py" "$buildoutputpath_python_x86\config.py"
Copy-Item "..\LanguageWrappers\python\report.py" "$buildoutputpath_python_x86\report.py"

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-python-x86.zip majorsilence-reporting-python-x86\
cd ..
# ************* End Python *********************************************


# ************* Begin Ruby *********************************************
$buildoutputpath_ruby_x86="$CURRENTPATH\build-output\majorsilence-reporting-ruby-x86"
delete_files "$buildoutputpath_ruby_x86"
mkdir "$buildoutputpath_ruby_x86"

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml "$buildoutputpath_ruby_x86\config.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml "$buildoutputpath_ruby_x86\RdlEngineConfig.xml"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" "$buildoutputpath_ruby_x86\RdlCri.dll"
Copy-Item ..\RdlCmd\bin\x86\Release\RdlCmd.exe "$buildoutputpath_ruby_x86\RdlCmd.exe"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" "$buildoutputpath_ruby_x86\DataProviders.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" "$buildoutputpath_ruby_x86\RdlEngine.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" "$buildoutputpath_ruby_x86\ICSharpCode.SharpZipLib.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" "$buildoutputpath_ruby_x86\RdlViewer.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.dll" "$buildoutputpath_ruby_x86\zxing.dll"

Copy-Item "..\LanguageWrappers\ruby\config.rb" "$buildoutputpath_ruby_x86\config.rb"
Copy-Item "..\LanguageWrappers\ruby\report.rb" "$buildoutputpath_ruby_x86\report.rb"

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-ruby-x86.zip majorsilence-reporting-ruby-x86\
cd ..

# ************* End Ruby *********************************************

$buildoutputpath_viewer_x86="$CURRENTPATH\build-output\majorsilence-reporting-viewer-x86"
delete_files "$buildoutputpath_viewer_x86"
mkdir "$buildoutputpath_viewer_x86"

Copy-Item ..\RdlDesktop\bin\x86\Release\config.xml "$buildoutputpath_viewer_x86\config.xml"
Copy-Item ..\RdlEngine\bin\Release\RdlEngineConfig.xml "$buildoutputpath_viewer_x86\RdlEngineConfig.xml"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" "$buildoutputpath_viewer_x86\RdlCri.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe" "$buildoutputpath_viewer_x86\RdlReader.exe"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" "$buildoutputpath_viewer_x86\DataProviders.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" "$buildoutputpath_viewer_x86\RdlEngine.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" "$buildoutputpath_viewer_x86\ICSharpCode.SharpZipLib.dll"
Copy-Item "$CURRENTPATH\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" "$buildoutputpath_viewer_x86\RdlViewer.dll"
Copy-Item "..\packages\ZXing.Net.0.16.8\lib\net40\zxing.dll" "$buildoutputpath_viewer_x86\zxing.dll"
delete_files "$buildoutputpath_viewer_x86\RdlViewerSC.pdb"

cd build-output	
..\7za.exe a $Version-majorsilence-reporting-viewer-x86.zip majorsilence-reporting-viewer-x86\
cd ..


