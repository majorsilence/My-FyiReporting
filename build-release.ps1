#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$CURRENTPATH=$pwd.Path

# /p:Configuration="Debug", "Debug-DrawingCompat", "Release", "Release-DrawingCompat"
$pConfiguration="Release"
$pTargetFramework="net8.0-windows"
$pTargetFrameworkGeneric="net8.0"

function delete_files([string]$path)
{
	If (Test-Path $path){
		Write-Host "Deleting path $path" -ForegroundColor Green
		Remove-Item -recurse -force $path
	}
}

function GetVersions([ref]$theVersion)
{
	$csprojPath = Join-Path $CURRENTPATH ".\Directory.Build.props"
	$xml = [xml](Get-Content $csprojPath)
	$theVersion.Value = $xml.Project.PropertyGroup.Version
}

Get-ChildItem .\ -include bin,obj,build-output -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

$Version=""
GetVersions([ref]$Version)
Write-Host  $Version

dotnet restore "./MajorsilenceReporting.sln"
# ************* Begin anycpu *********************************************
dotnet build "$CURRENTPATH\MajorsilenceReporting.sln" --configuration Release-DrawingCompat --verbosity minimal
dotnet build "$CURRENTPATH\MajorsilenceReporting.sln" --configuration $pConfiguration --verbosity minimal

$buildoutputpath_designer="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-designer-$pTargetFramework-anycpu"
$buildoutputpath_desktop="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-desktop-$pTargetFrameworkGeneric-anycpu"
$buildoutputpath_rdlcmd="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-rdlcmd-$pTargetFrameworkGeneric-anycpu"
$buildoutputpath_viewer="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-viewer-$pTargetFramework-anycpu"
$buildoutputpath_mapfile="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-mapfile-$pTargetFramework-anycpu"

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

Copy-Item .\RdlDesign\bin\$pConfiguration\$pTargetFramework\ -Destination "$buildoutputpath_designer\" -Recurse
Copy-Item .\RdlDesktop\bin\$pConfiguration\$pTargetFrameworkGeneric\ -Destination "$buildoutputpath_desktop\" -Recurse
Copy-Item .\RdlCmd\bin\$pConfiguration\$pTargetFrameworkGeneric\ -Destination "$buildoutputpath_rdlcmd\" -Recurse
Copy-Item .\RdlViewer\bin\$pConfiguration\$pTargetFramework\ -Destination "$buildoutputpath_viewer\" -Recurse
Copy-Item .\RdlMapFile\bin\$pConfiguration\$pTargetFramework\ -Destination "$buildoutputpath_mapfile\" -Recurse

cd Release-Builds
cd build-output	
..\7za.exe a -tzip $Version-majorsilence-reporting-designer-$pTargetFramework-anycpu.zip majorsilence-reporting-designer-$pTargetFramework-anycpu\
..\7za.exe a -tzip $Version-majorsilence-reporting-desktop-$pTargetFrameworkGeneric-anycpu.zip majorsilence-reporting-desktop-$pTargetFrameworkGeneric-anycpu\
..\7za.exe a -tzip $Version-majorsilence-reporting-rdlcmd-$pTargetFrameworkGeneric-anycpu.zip majorsilence-reporting-rdlcmd-$pTargetFrameworkGeneric-anycpu\
..\7za.exe a -tzip $Version-majorsilence-reporting-viewer-$pTargetFramework-anycpu.zip majorsilence-reporting-viewer-$pTargetFramework-anycpu\
..\7za.exe a -tzip $Version-majorsilence-reporting-mapfile-$pTargetFramework-anycpu.zip majorsilence-reporting-mapfile-$pTargetFramework-anycpu\
cd "$CURRENTPATH"


# ************* End anycpu *********************************************


# ************* Begin PHP *********************************************
$buildoutputpath_php="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-php"
delete_files "$buildoutputpath_php"
mkdir "$buildoutputpath_php"

Copy-Item .\RdlDesktop\bin\$pConfiguration\$pTargetFrameworkGeneric\config.xml "$buildoutputpath_php\config.xml"
Copy-Item .\RdlCmd\bin\$pConfiguration\$pTargetFrameworkGeneric\ -Destination "$buildoutputpath_php\" -Recurse

Copy-Item ".\LanguageWrappers\php\config.php" "$buildoutputpath_php\config.php"
Copy-Item ".\LanguageWrappers\php\report.php" "$buildoutputpath_php\report.php"

cd Release-Builds
cd build-output	
..\7za.exe a -tzip $Version-majorsilence-reporting-build-php.zip majorsilence-reporting-php\
cd "$CURRENTPATH"

# ************* End PHP *********************************************



# ************* Begin Python *********************************************
$buildoutputpath_python="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-python"
delete_files "$buildoutputpath_python"
mkdir "$buildoutputpath_python"

Copy-Item .\RdlDesktop\bin\$pConfiguration\$pTargetFrameworkGeneric\config.xml "$buildoutputpath_python\config.xml"
Copy-Item .\RdlCmd\bin\$pConfiguration\$pTargetFrameworkGeneric\ -Destination "$buildoutputpath_python\" -Recurse
Copy-Item ".\LanguageWrappers\python\report.py" "$buildoutputpath_python\report.py"

cd Release-Builds
cd build-output	
..\7za.exe a -tzip $Version-majorsilence-reporting-python.zip majorsilence-reporting-python\
cd "$CURRENTPATH"
# ************* End Python *********************************************


# ************* Begin Ruby *********************************************
$buildoutputpath_ruby="$CURRENTPATH\Release-Builds\build-output\majorsilence-reporting-ruby"
delete_files "$buildoutputpath_ruby"
mkdir "$buildoutputpath_ruby"

Copy-Item .\RdlDesktop\bin\$pConfiguration\$pTargetFrameworkGeneric\config.xml "$buildoutputpath_ruby\config.xml"
Copy-Item .\RdlCmd\bin\$pConfiguration\$pTargetFrameworkGeneric\ -Destination "$buildoutputpath_ruby\" -Recurse

Copy-Item ".\LanguageWrappers\ruby\report.rb" "$buildoutputpath_ruby\report.rb"

cd Release-Builds
cd build-output	
..\7za.exe a -tzip $Version-majorsilence-reporting-ruby.zip majorsilence-reporting-ruby\
cd "$CURRENTPATH"

# ************* End Ruby *********************************************


# ************* Nuget ************************************************
Get-ChildItem -Recurse -Exclude "$CURRENTPATH\Release-Builds\build-output" .\*.nupkg | Copy-Item -Destination "$CURRENTPATH\Release-Builds\build-output" -Force -ErrorAction SilentlyContinue
