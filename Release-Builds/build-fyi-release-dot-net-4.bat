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




del .\my-fyi-build-dot-net-4-x64 /Q
mkdir .\my-fyi-build-dot-net-4-x64

copy ..\DataProviders\bin\x64\Release\DataProviders.dll .\my-fyi-build-dot-net-4-x64\DataProviders.dll /Y
REM copy ..\OracleSp\bin\x64\Release\OracleSp.dll .\my-fyi-build-dot-net-4-x64\OracleSp.dll /Y
copy ..\RdlAsp\bin\x64\Release\RdlAsp.dll .\my-fyi-build-dot-net-4-x64\RdlAsp.dll /Y
copy ..\RdlCmd\bin\x64\Release\RdlCmd.exe .\my-fyi-build-dot-net-4-x64\RdlCmd.exe /Y
copy ..\RdlCri\bin\x64\Release\RdlCri.dll .\my-fyi-build-dot-net-4-x64\RdlCri.dll /Y
copy ..\RdlDesign\bin\x64\Release\RdlDesigner.exe .\my-fyi-build-dot-net-4-x64\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x64\Release\RdlDesktop.exe .\my-fyi-build-dot-net-4-x64\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x64\Release\config.xml .\my-fyi-build-dot-net-4-x64\config.xml /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngine.dll .\my-fyi-build-dot-net-4-x64\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x64\Release\ICSharpCode.SharpZipLib.dll .\my-fyi-build-dot-net-4-x64\ICSharpCode.SharpZipLib.dll /Y
copy "..\References\dot net 4\Mono.Security.dll" .\my-fyi-build-dot-net-4-x64\Mono.Security.dll /Y
copy "..\References\dot net 4\Npgsql.dll" .\my-fyi-build-dot-net-4-x64\Npgsql.dll /Y
copy "..\References\dot net 4\64bit\System.Data.SQLite.dll" .\my-fyi-build-dot-net-4-x64\System.Data.SQLite.dll /Y
copy ..\RdlEngine\bin\x64\Release\RdlEngineConfig.xml .\my-fyi-build-dot-net-4-x64\RdlEngineConfig.xml /Y
copy ..\RdlMapFile\bin\x64\Release\RdlMapFile.exe .\my-fyi-build-dot-net-4-x64\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\x64\Release\RdlViewer.dll .\my-fyi-build-dot-net-4-x64\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x64\Release\RdlReader.exe .\my-fyi-build-dot-net-4-x64\RdlReader.exe /Y

7za.exe a my-fyi-build-dot-net-4-x64.zip my-fyi-build-dot-net-4-x64\

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




del .\my-fyi-build-dot-net-4-x86 /Q
mkdir .\my-fyi-build-dot-net-4-x86

copy ..\DataProviders\bin\x86\Release\DataProviders.dll .\my-fyi-build-dot-net-4-x86\DataProviders.dll /Y
REM copy ..\OracleSp\bin\x86\Release\OracleSp.dll .\my-fyi-build-dot-net-4-x86\OracleSp.dll /Y
copy ..\RdlAsp\bin\x86\Release\RdlAsp.dll .\my-fyi-build-dot-net-4-x86\RdlAsp.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\my-fyi-build-dot-net-4-x86\RdlCmd.exe /Y
copy ..\RdlCri\bin\x86\Release\RdlCri.dll .\my-fyi-build-dot-net-4-x86\RdlCri.dll /Y
copy ..\RdlDesign\bin\x86\Release\RdlDesigner.exe .\my-fyi-build-dot-net-4-x86\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x86\Release\RdlDesktop.exe .\my-fyi-build-dot-net-4-x86\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x86\Release\config.xml .\my-fyi-build-dot-net-4-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngine.dll .\my-fyi-build-dot-net-4-x86\RdlEngine.dll /Y
copy ..\RdlEngine\bin\x86\Release\ICSharpCode.SharpZipLib.dll .\my-fyi-build-dot-net-4-x86\ICSharpCode.SharpZipLib.dll /Y
copy "..\References\dot net 4\Mono.Security.dll" .\my-fyi-build-dot-net-4-x86\Mono.Security.dll /Y
copy "..\References\dot net 4\Npgsql.dll" .\my-fyi-build-dot-net-4-x86\Npgsql.dll /Y
copy "..\References\dot net 4\32bit\System.Data.SQLite.dll" .\my-fyi-build-dot-net-4-x86\System.Data.SQLite.dll /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\my-fyi-build-dot-net-4-x86\RdlEngineConfig.xml /Y
copy ..\RdlMapFile\bin\x86\Release\RdlMapFile.exe .\my-fyi-build-dot-net-4-x86\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\x86\Release\RdlViewer.dll .\my-fyi-build-dot-net-4-x86\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe .\my-fyi-build-dot-net-4-x86\RdlReader.exe /Y

7za.exe a my-fyi-build-dot-net-4-x86.zip my-fyi-build-dot-net-4-x86\

REM ************* End x86 *********************************************


REM ************* ILMerge RdlReader *********************************************

del .\my-fyi-build-dot-net-4-viewer-x86 /Q
mkdir .\my-fyi-build-dot-net-4-viewer-x86

"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /targetplatform:v4 /target:winexe /out:"%CD%\my-fyi-build-dot-net-4-viewer-x86\RdlViewerSC.exe" "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe" "%CD%\..\DataProviders\bin\x86\Release\DataProviders.dll" "%CD%\..\RdlCri\bin\x86\Release\RdlCri.dll" "%CD%\..\RdlEngine\bin\x86\Release\RdlEngine.dll" "%CD%\..\RdlEngine\bin\x86\Release\ICSharpCode.SharpZipLib.dll" "%CD%\..\RdlViewer\bin\x86\Release\RdlViewer.dll" 

copy ..\RdlDesktop\bin\x86\Release\config.xml .\my-fyi-build-dot-net-4-viewer-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\my-fyi-build-dot-net-4-viewer-x86\RdlEngineConfig.xml /Y
del .\my-fyi-build-dot-net-4-viewer-x86\RdlViewerSC.pdb


7za.exe a my-fyi-build-dot-net-4-viewer-x86.zip my-fyi-build-dot-net-4-viewer-x86\
