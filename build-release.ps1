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
ElseIf (Test-Path "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Current\Bin\MSBuild.exe"){
	$msbuildpath="C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Current\Bin\MSBuild.exe"
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
	$csprojPath = Join-Path $CURRENTPATH ".\RdlEngine\RdlEngine.csproj"
	$xml = [xml](Get-Content $csprojPath)
	$theVersion.Value = $xml.Project.PropertyGroup.Version
}

Get-ChildItem .\ -include bin,obj,build-output -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

$Version=""
GetVersions([ref]$Version)
Write-Host  $Version
dotnet restore "./MajorsilenceReporting.sln"
dotnet restore "./MajorsilenceReporting-ReportServer.sln"

# ************* Begin net48 anycpu *********************************************

& "$msbuildpath" "$CURRENTPATH\MajorsilenceReporting.sln" /verbosity:minimal /p:Configuration="Release" /property:Platform="Any CPU" /target:clean /target:rebuild

$buildoutputpath_designer="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-designer-net48-anycpu"
$buildoutputpath_desktop="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-desktop-net48-anycpu"
$buildoutputpath_rdlcmd="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-rdlcmd-net48-anycpu"
$buildoutputpath_viewer="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-viewer-net48-anycpu"
$buildoutputpath_mapfile="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-mapfile-net48-anycpu"
$buildoutputpath_reportserver="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-reportserver-net48-anycpu"

Remove-Item "$buildoutputpath_designer" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_designer"
Remove-Item "$buildoutputpath_desktop" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_desktop"
Remove-Item "$buildoutputpath_rdlcmd" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_rdlcmd"
Remove-Item "$buildoutputpath_viewer" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_viewer"
Remove-Item "$buildoutputpath_mapfile" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_mapfile"
Remove-Item "$buildoutputpath_reportserver" -Recurse -ErrorAction Ignore
mkdir "$buildoutputpath_reportserver"

Copy-Item .\RdlDesign\bin\Release\net48\ -Destination "$buildoutputpath_designer\" -Recurse
Copy-Item .\RdlDesktop\bin\Release\net48\ -Destination "$buildoutputpath_desktop\" -Recurse
Copy-Item .\RdlCmd\bin\Release\net48\ -Destination "$buildoutputpath_rdlcmd\" -Recurse
Copy-Item .\RdlViewer\bin\Release\net48\ -Destination "$buildoutputpath_viewer\" -Recurse
Copy-Item .\RdlMapFile\bin\Release\net48\ -Destination "$buildoutputpath_mapfile\" -Recurse

cd Release-Builds
cd build-output	
..\7za.exe a $Version-majorsilence-reporting-designer-net48-anycpu.zip majorsilence-reporting-designer-net48-anycpu\
..\7za.exe a $Version-majorsilence-reporting-desktop-net48-anycpu.zip majorsilence-reporting-desktop-net48-anycpu\
..\7za.exe a $Version-majorsilence-reporting-rdlcmd-net48-anycpu.zip majorsilence-reporting-rdlcmd-net48-anycpu\
..\7za.exe a $Version-majorsilence-reporting-viewer-net48-anycpu.zip majorsilence-reporting-viewer-net48-anycpu\
..\7za.exe a $Version-majorsilence-reporting-mapfile-net48-anycpu.zip majorsilence-reporting-mapfile-net48-anycpu\
cd "$CURRENTPATH"


& "$msbuildpath" "$CURRENTPATH\MajorsilenceReporting-ReportServer.sln" /verbosity:minimal /p:Configuration="Release" /property:Platform="Any CPU"
& "$msbuildpath" "$CURRENTPATH\MajorsilenceReporting-ReportServer.sln" /verbosity:minimal /p:Configuration="Release" /property:Platform="Any CPU" /property:DeployOnBuild=true /property:PublishProfile='FolderProfile'

Copy-Item .\ReportServer\bin\app.publish\ -Destination "$buildoutputpath_reportserver\" -Recurse
cd Release-Builds
cd build-output	
..\7za.exe a $Version-majorsilence-reporting-reportserver-net48-anycpu.zip majorsilence-reporting-reportserver-net48-anycpu\
cd "$CURRENTPATH"

# ************* End net48 *********************************************


# ************* Begin PHP *********************************************
$buildoutputpath_php="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-php"
delete_files "$buildoutputpath_php"
mkdir "$buildoutputpath_php"

Copy-Item .\RdlDesktop\bin\Release\net48\config.xml "$buildoutputpath_php\config.xml"
Copy-Item .\RdlCmd\bin\Release\net48\ -Destination "$buildoutputpath_php\" -Recurse

Copy-Item ".\LanguageWrappers\php\config.php" "$buildoutputpath_php\config.php"
Copy-Item ".\LanguageWrappers\php\report.php" "$buildoutputpath_php\report.php"

cd Release-Builds
cd build-output	
..\7za.exe a $Version-majorsilence-reporting-build-php.zip majorsilence-reporting-php\
cd "$CURRENTPATH"

# ************* End PHP *********************************************



# ************* Begin Python *********************************************
$buildoutputpath_python="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-python"
delete_files "$buildoutputpath_python"
mkdir "$buildoutputpath_python"

Copy-Item .\RdlDesktop\bin\Release\net48\config.xml "$buildoutputpath_python\config.xml"
Copy-Item .\RdlCmd\bin\Release\net48\ -Destination "$buildoutputpath_python\" -Recurse
Copy-Item ".\LanguageWrappers\python\config.py" "$buildoutputpath_python\config.py"
Copy-Item ".\LanguageWrappers\python\report.py" "$buildoutputpath_python\report.py"

cd Release-Builds
cd build-output	
..\7za.exe a $Version-majorsilence-reporting-python.zip majorsilence-reporting-python\
cd "$CURRENTPATH"
# ************* End Python *********************************************


# ************* Begin Ruby *********************************************
$buildoutputpath_ruby="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-ruby"
delete_files "$buildoutputpath_ruby"
mkdir "$buildoutputpath_ruby"

Copy-Item .\RdlDesktop\bin\Release\net48\config.xml "$buildoutputpath_ruby\config.xml"
Copy-Item .\RdlCmd\bin\Release\net48\ -Destination "$buildoutputpath_ruby\" -Recurse

Copy-Item ".\LanguageWrappers\ruby\config.rb" "$buildoutputpath_ruby\config.rb"
Copy-Item ".\LanguageWrappers\ruby\report.rb" "$buildoutputpath_ruby\report.rb"

cd Release-Builds
cd build-output	
..\7za.exe a $Version-majorsilence-reporting-ruby.zip majorsilence-reporting-ruby\
cd "$CURRENTPATH"

# ************* End Ruby *********************************************


# ************* Nuget ************************************************
Get-ChildItem -Recurse -Exclude "$CURRENTPATH\Release-Builds\build-output" .\*.nupkg | Copy-Item -Destination "$CURRENTPATH\Release-Builds\build-output" -Force -ErrorAction SilentlyContinue
