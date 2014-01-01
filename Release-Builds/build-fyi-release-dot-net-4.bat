REM Platform options: "x86", "x64", "x64"
REM /p:Configuration="Debug" or "Release"


REM ************* Begin x64 *********************************************

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
REM "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "..\%CD%\..\OracleSP\OracleSp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlAsp\RdlAsp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlDesign\RdlDesign.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlDesktop\RdlDesktop.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlMapFile\RdlMapFile.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\LibRdlWpfViewer\LibRdlWpfViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\LibRdlCrossPlatformViewer\LibRdlCrossPlatformViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4

del .\majorsilence-reporting-build-dot-net-4-x64 /Q
mkdir .\majorsilence-reporting-build-dot-net-4-x64

copy ..\DataProviders\bin\x64\Release\DataProviders.dll .\majorsilence-reporting-build-dot-net-4-x64\DataProviders.dll /Y
REM copy ..\OracleSp\bin\x64\Release\OracleSp.dll .\majorsilence-reporting-build-dot-net-4-x64\OracleSp.dll /Y
copy ..\RdlAsp\bin\x64\Release\RdlAsp.dll .\majorsilence-reporting-build-dot-net-4-x64\RdlAsp.dll /Y
copy ..\RdlCmd\bin\x64\Release\RdlCmd.exe .\majorsilence-reporting-build-dot-net-4-x64\RdlCmd.exe /Y
copy ..\RdlCri\bin\x64\Release\RdlCri.dll .\majorsilence-reporting-build-dot-net-4-x64\RdlCri.dll /Y
copy ..\RdlDesign\bin\x64\Release\RdlDesigner.exe .\majorsilence-reporting-build-dot-net-4-x64\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x64\Release\RdlDesktop.exe .\majorsilence-reporting-build-dot-net-4-x64\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x64\Release\config.xml .\majorsilence-reporting-build-dot-net-4-x64\config.xml /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngine.dll .\majorsilence-reporting-build-dot-net-4-x64\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x64\Release\ICSharpCode.SharpZipLib.dll .\majorsilence-reporting-build-dot-net-4-x64\ICSharpCode.SharpZipLib.dll /Y
copy "..\References\dot net 4\Mono.Security.dll" .\majorsilence-reporting-build-dot-net-4-x64\Mono.Security.dll /Y
copy "..\References\dot net 4\Npgsql.dll" .\majorsilence-reporting-build-dot-net-4-x64\Npgsql.dll /Y
copy "..\References\dot net 4\64bit\System.Data.SQLite.dll" .\majorsilence-reporting-build-dot-net-4-x64\System.Data.SQLite.dll /Y
copy "..\References\dot net 4\itextsharp.dll" .\majorsilence-reporting-build-dot-net-4-x64\itextsharp.dll /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngineConfig.xml .\majorsilence-reporting-build-dot-net-4-x64\RdlEngineConfig.xml /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngineConfig.Linux.xml .\majorsilence-reporting-build-dot-net-4-x64\RdlEngineConfig.Linux.xml /Y
copy ..\RdlMapFile\bin\x64\Release\RdlMapFile.exe .\majorsilence-reporting-build-dot-net-4-x64\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\x64\Release\RdlViewer.dll .\majorsilence-reporting-build-dot-net-4-x64\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x64\Release\RdlReader.exe .\majorsilence-reporting-build-dot-net-4-x64\RdlReader.exe /Y
copy ..\LibRdlWpfViewer\bin\x64\Release\LibRdlWpfViewer.dll .\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.dll /Y
copy ..\LibRdlCrossPlatformViewer\bin\x64\Release\LibRdlCrossPlatformViewer.dll .\majorsilence-reporting-build-dot-net-4-x64\LibRdlCrossPlatformViewer.dll /Y
copy "..\References\dot net 4\Xwt.dll" .\majorsilence-reporting-build-dot-net-4-x64\Xwt.dll /Y
copy "..\References\dot net 4\Xwt.Gtk.dll" .\majorsilence-reporting-build-dot-net-4-x64\Xwt.Gtk.dll /Y
copy "..\References\dot net 4\Xwt.WPF.dll" .\majorsilence-reporting-build-dot-net-4-x64\Xwt.WPF.dll /Y

7za.exe a majorsilence-reporting-build-dot-net-4-x64.zip majorsilence-reporting-build-dot-net-4-x64\

REM ************* End x64 *********************************************


REM ************* Begin x86 *********************************************

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
REM "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "..\%CD%\..\OracleSP\OracleSp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlAsp\RdlAsp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlDesign\RdlDesign.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlDesktop\RdlDesktop.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlMapFile\RdlMapFile.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\LibRdlWpfViewer\LibRdlWpfViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\LibRdlCrossPlatformViewer\LibRdlCrossPlatformViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

del .\majorsilence-reporting-build-dot-net-4-x86 /Q
mkdir .\majorsilence-reporting-build-dot-net-4-x86

copy ..\DataProviders\bin\x86\Release\DataProviders.dll .\majorsilence-reporting-build-dot-net-4-x86\DataProviders.dll /Y
REM copy ..\OracleSp\bin\x86\Release\OracleSp.dll .\majorsilence-reporting-build-dot-net-4-x86\OracleSp.dll /Y
copy ..\RdlAsp\bin\x86\Release\RdlAsp.dll .\majorsilence-reporting-build-dot-net-4-x86\RdlAsp.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\majorsilence-reporting-build-dot-net-4-x86\RdlCmd.exe /Y
copy ..\RdlCri\bin\x86\Release\RdlCri.dll .\majorsilence-reporting-build-dot-net-4-x86\RdlCri.dll /Y
copy ..\RdlDesign\bin\x86\Release\RdlDesigner.exe .\majorsilence-reporting-build-dot-net-4-x86\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x86\Release\RdlDesktop.exe .\majorsilence-reporting-build-dot-net-4-x86\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x86\Release\config.xml .\majorsilence-reporting-build-dot-net-4-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngine.dll .\majorsilence-reporting-build-dot-net-4-x86\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x86\Release\ICSharpCode.SharpZipLib.dll .\majorsilence-reporting-build-dot-net-4-x86\ICSharpCode.SharpZipLib.dll /Y
copy "..\References\dot net 4\Mono.Security.dll" .\majorsilence-reporting-build-dot-net-4-x86\Mono.Security.dll /Y
copy "..\References\dot net 4\Npgsql.dll" .\majorsilence-reporting-build-dot-net-4-x86\Npgsql.dll /Y
copy "..\References\dot net 4\32bit\System.Data.SQLite.dll" .\majorsilence-reporting-build-dot-net-4-x86\System.Data.SQLite.dll /Y
copy "..\References\dot net 4\itextsharp.dll" .\majorsilence-reporting-build-dot-net-4-x86\itextsharp.dll /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.Linux.xml .\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.Linux.xml /Y
copy ..\RdlMapFile\bin\x86\Release\RdlMapFile.exe .\majorsilence-reporting-build-dot-net-4-x86\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\x86\Release\RdlViewer.dll .\majorsilence-reporting-build-dot-net-4-x86\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe .\majorsilence-reporting-build-dot-net-4-x86\RdlReader.exe /Y
copy ..\LibRdlWpfViewer\bin\x86\Release\LibRdlWpfViewer.dll .\majorsilence-reporting-build-dot-net-4-x86\LibRdlWpfViewer.dll /Y
copy ..\LibRdlCrossPlatformViewer\bin\x86\Release\LibRdlCrossPlatformViewer.dll .\majorsilence-reporting-build-dot-net-4-x86\LibRdlCrossPlatformViewer.dll /Y
copy "..\References\dot net 4\Xwt.dll" .\majorsilence-reporting-build-dot-net-4-x86\Xwt.dll /Y
copy "..\References\dot net 4\Xwt.Gtk.dll" .\majorsilence-reporting-build-dot-net-4-x86\Xwt.Gtk.dll /Y
copy "..\References\dot net 4\Xwt.WPF.dll" .\majorsilence-reporting-build-dot-net-4-x86\Xwt.WPF.dll /Y

7za.exe a majorsilence-reporting-build-dot-net-4-x86.zip majorsilence-reporting-build-dot-net-4-x86\

REM ************* End x86 *********************************************



REM ************* Begin PHP *********************************************


REM ************* End PHP *********************************************



REM ************* ILMerge RdlReader *********************************************

del .\majorsilence-reporting-build-dot-net-4-viewer-x86 /Q
mkdir .\majorsilence-reporting-build-dot-net-4-viewer-x86

copy ..\RdlDesktop\bin\x86\Release\config.xml .\majorsilence-reporting-build-dot-net-4-viewer-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlEngineConfig.xml /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlCri.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe" .\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlReader.exe /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\majorsilence-reporting-build-dot-net-4-viewer-x86\DataProviders.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlEngine.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\majorsilence-reporting-build-dot-net-4-viewer-x86\ICSharpCode.SharpZipLib.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlViewer.dll /Y
del .\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlViewerSC.pdb


7za.exe a majorsilence-reporting-build-dot-net-4-viewer-x86.zip majorsilence-reporting-build-dot-net-4-viewer-x86\
