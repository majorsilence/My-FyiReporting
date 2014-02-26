REM Platform options: "x86", "x64", "x64"
REM /p:Configuration="Debug" or "Release"


REM ************* Begin x64 *********************************************

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4


"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4




del .\majorsilence-reporting-build-dot-net-2-x64 /Q
mkdir .\majorsilence-reporting-build-dot-net-2-x64


copy ..\DataProviders\bin\x64\Release\DataProviders.dll .\majorsilence-reporting-build-dot-net-2-x64\DataProviders.dll /Y
copy ..\RdlCmd\bin\x64\Release\RdlCmd.exe .\majorsilence-reporting-build-dot-net-2-x64\RdlCmd.exe /Y
copy ..\RdlCri\bin\x64\Release\RdlCri.dll .\majorsilence-reporting-build-dot-net-2-x64\RdlCri.dll /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngine.dll .\majorsilence-reporting-build-dot-net-2-x64\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x64\Release\ICSharpCode.SharpZipLib.dll .\majorsilence-reporting-build-dot-net-2-x64\ICSharpCode.SharpZipLib.dll /Y
copy ..\RdlEngine\bin\x64\Release\Mono.Security.dll .\majorsilence-reporting-build-dot-net-2-x64\Mono.Security.dll /Y
copy ..\RdlEngine\bin\x64\Release\Npgsql.dll .\majorsilence-reporting-build-dot-net-2-x64\Npgsql.dll /Y
copy ..\RdlEngine\bin\x64\Release\System.Data.SQLite.dll .\majorsilence-reporting-build-dot-net-2-x64\System.Data.SQLite.dll /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngineConfig.xml .\majorsilence-reporting-build-dot-net-2-x64\RdlEngineConfig.xml /Y
copy ..\RdlViewer\bin\x64\Release\RdlViewer.dll .\majorsilence-reporting-build-dot-net-2-x64\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x64\Release\RdlReader.exe .\majorsilence-reporting-build-dot-net-2-x64\RdlReader.exe /Y
copy "..\References\dot net 3.5\zxing.dll" .\majorsilence-reporting-build-dot-net-2-x64\zxing.dll

7za.exe a majorsilence-reporting-build-dot-net-2-x64.zip majorsilence-reporting-build-dot-net-2-x64\

REM ************* End x64 *********************************************


REM ************* Begin x86 *********************************************

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86";TargetFrameworkVersion=v3.5 /t:clean;rebuild /m:4




del .\majorsilence-reporting-build-dot-net-2-x86 /Q
mkdir .\majorsilence-reporting-build-dot-net-2-x86

copy ..\DataProviders\bin\x86\Release\DataProviders.dll .\majorsilence-reporting-build-dot-net-2-x86\DataProviders.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\majorsilence-reporting-build-dot-net-2-x86\RdlCmd.exe /Y
copy ..\RdlCri\bin\x86\Release\RdlCri.dll .\majorsilence-reporting-build-dot-net-2-x86\RdlCri.dll /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngine.dll .\majorsilence-reporting-build-dot-net-2-x86\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x86\Release\ICSharpCode.SharpZipLib.dll .\majorsilence-reporting-build-dot-net-2-x86\ICSharpCode.SharpZipLib.dll /Y
copy ..\RdlEngine\bin\x86\Release\Mono.Security.dll .\majorsilence-reporting-build-dot-net-2-x86\Mono.Security.dll /Y
copy ..\RdlEngine\bin\x86\Release\Npgsql.dll .\majorsilence-reporting-build-dot-net-2-x86\Npgsql.dll /Y
copy ..\RdlEngine\bin\x86\Release\System.Data.SQLite.dll .\majorsilence-reporting-build-dot-net-2-x86\System.Data.SQLite.dll /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\majorsilence-reporting-build-dot-net-2-x86\RdlEngineConfig.xml /Y
copy ..\RdlViewer\bin\x86\Release\RdlViewer.dll .\majorsilence-reporting-build-dot-net-2-x86\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe .\majorsilence-reporting-build-dot-net-2-x86\RdlReader.exe /Y
copy "..\References\dot net 3.5\zxing.dll" .\majorsilence-reporting-build-dot-net-2-x86\zxing.dll

7za.exe a majorsilence-reporting-build-dot-net-2-x86.zip majorsilence-reporting-build-dot-net-2-x86\

REM ************* End x86 *********************************************



