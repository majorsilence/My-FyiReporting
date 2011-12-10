REM Platform options: "x86", "x64", "Any CPU"
REM /p:Configuration="Debug" or "Release"


REM ************* Begin ANY CPU *********************************************

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

REM "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "..\%CD%\..\OracleSP\OracleSp.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlAsp\RdlAsp.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlDesign\RdlDesign.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlDesktop\RdlDesktop.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlMapFile\RdlMapFile.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4




del .\my-fyi-build-dot-net-2 /Q
mkdir .\my-fyi-build-dot-net-2

copy ..\DataProviders\bin\Release\DataProviders.dll .\my-fyi-build-dot-net-2\DataProviders.dll /Y
REM copy ..\OracleSp\bin\Release\OracleSp.dll .\my-fyi-build-dot-net-2\OracleSp.dll /Y
copy ..\RdlAsp\bin\Release\RdlAsp.dll .\my-fyi-build-dot-net-2\RdlAsp.dll /Y
copy ..\RdlCmd\bin\Release\RdlCmd.exe .\my-fyi-build-dot-net-2\RdlCmd.exe /Y
copy ..\RdlCri\bin\Release\RdlCri.dll .\my-fyi-build-dot-net-2\RdlCri.dll /Y
copy ..\RdlDesign\bin\Release\RdlDesigner.exe .\my-fyi-build-dot-net-2\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\Release\RdlDesktop.exe .\my-fyi-build-dot-net-2\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\Release\config.xml .\my-fyi-build-dot-net-2\config.xml /Y
copy ..\RdlEngine\bin\Release\RdlEngine.dll .\my-fyi-build-dot-net-2\RdlEngine.dll /Y
copy ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\my-fyi-build-dot-net-2\ICSharpCode.SharpZipLib.dll /Y
copy ..\RdlEngine\bin\Release\Mono.Security.dll .\my-fyi-build-dot-net-2\Mono.Security.dll /Y
copy ..\RdlEngine\bin\Release\Npgsql.dll .\my-fyi-build-dot-net-2\Npgsql.dll /Y
copy ..\RdlEngine\bin\Release\System.Data.SQLite.dll .\my-fyi-build-dot-net-2\System.Data.SQLite.dll /Y
copy ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\my-fyi-build-dot-net-2\RdlEngineConfig.xml /Y
copy ..\RdlMapFile\bin\Release\RdlMapFile.exe .\my-fyi-build-dot-net-2\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\Release\RdlViewer.dll .\my-fyi-build-dot-net-2\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\Release\RdlReader.exe .\my-fyi-build-dot-net-2\RdlReader.exe /Y

7za.exe a my-fyi-build-dot-net-2.zip my-fyi-build-dot-net-2\

REM ************* End ANY CPU *********************************************


REM ************* Begin x86 *********************************************

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

REM "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "..\%CD%\..\OracleSP\OracleSp.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlAsp\RdlAsp.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlDesign\RdlDesign.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlDesktop\RdlDesktop.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlMapFile\RdlMapFile.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4




del .\my-fyi-build-dot-net-2-x86 /Q
mkdir .\my-fyi-build-dot-net-2-x86

copy ..\DataProviders\bin\x86\Release\DataProviders.dll .\my-fyi-build-dot-net-2-x86\DataProviders.dll /Y
REM copy ..\OracleSp\bin\x86\Release\OracleSp.dll .\my-fyi-build-dot-net-2-x86\OracleSp.dll /Y
copy ..\RdlAsp\bin\x86\Release\RdlAsp.dll .\my-fyi-build-dot-net-2-x86\RdlAsp.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\my-fyi-build-dot-net-2-x86\RdlCmd.exe /Y
copy ..\RdlCri\bin\x86\Release\RdlCri.dll .\my-fyi-build-dot-net-2-x86\RdlCri.dll /Y
copy ..\RdlDesign\bin\x86\Release\RdlDesigner.exe .\my-fyi-build-dot-net-2-x86\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x86\Release\RdlDesktop.exe .\my-fyi-build-dot-net-2-x86\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x86\Release\config.xml .\my-fyi-build-dot-net-2-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngine.dll .\my-fyi-build-dot-net-2-x86\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x86\Release\ICSharpCode.SharpZipLib.dll .\my-fyi-build-dot-net-2-x86\ICSharpCode.SharpZipLib.dll /Y
copy ..\RdlEngine\bin\x86\Release\Mono.Security.dll .\my-fyi-build-dot-net-2-x86\Mono.Security.dll /Y
copy ..\RdlEngine\bin\x86\Release\Npgsql.dll .\my-fyi-build-dot-net-2-x86\Npgsql.dll /Y
copy ..\RdlEngine\bin\x86\Release\System.Data.SQLite.dll .\my-fyi-build-dot-net-2-x86\System.Data.SQLite.dll /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\my-fyi-build-dot-net-2-x86\RdlEngineConfig.xml /Y
copy ..\RdlMapFile\bin\x86\Release\RdlMapFile.exe .\my-fyi-build-dot-net-2-x86\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\x86\Release\RdlViewer.dll .\my-fyi-build-dot-net-2-x86\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe .\my-fyi-build-dot-net-2-x86\RdlReader.exe /Y

7za.exe a my-fyi-build-dot-net-2-x86.zip my-fyi-build-dot-net-2-x86\

REM ************* End x86 *********************************************
