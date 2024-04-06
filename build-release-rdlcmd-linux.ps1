#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$CURRENTPATH=$pwd.Path

function GetVersions([ref]$theVersion)
{
	$csprojPath = Join-Path $CURRENTPATH "./RdlEngine/RdlEngine.csproj"
	$xml = [xml](Get-Content $csprojPath)
	$theVersion.Value = $xml.Project.PropertyGroup.Version
}

function BuildRdlCmd{
	Get-ChildItem ./ -include bin,obj,build-output -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

	$Version=""
	GetVersions([ref]$Version)
	Write-Host  $Version
	dotnet restore "./MajorsilenceReporting.sln"

	Write-Output "Linux x64 rdlcmd build"

	$buildoutputpath_rdlcmd_linux="$CURRENTPATH/Release-Builds/build-output/majorsilence-reporting-rdlcmd-linux-x64"
	rmf -rf $buildoutputpath_rdlcmd_linux
	mkdir -p $buildoutputpath_rdlcmd_linux

	dotnet publish RdlCmd -c Release -r linux-x64 -f net6.0 -p:PublishReadyToRun=true --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
	Copy-Item ./RdlCmd/bin/Release/net6.0/linux-x64/publish -Destination "$buildoutputpath_rdlcmd_linux/" -Recurse

	cd Release-Builds
	cd build-output	
	7za a $Version-majorsilence-reporting-rdlcmd-linux-x64.zip majorsilence-reporting-rdlcmd-linux-x64\
	cd "$CURRENTPATH"
}

param (
    [Parameter(Mandatory=$false)]
    [string]$arg
)

if ($arg -eq "build") {
    BuildRdlCmd
} else {
	Write-Output "Usage: ./build-release-linux.ps1 docker"
	Write-Output "net6.0 for linux-x64 is the last version that System.Drawing can be enabled."
	Write-Output "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only"
    docker run --memory 4gb --rm -v ${CURRENTPATH}:/src mcr.microsoft.com/dotnet/sdk:6.0 bash -c "cd /src && ./build-release-linux.ps1 build"
}

		