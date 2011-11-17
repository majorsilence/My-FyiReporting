REM Platform options: "x86", "x64", "Any CPU"
REM /p:Configuration="Debug" or "Release"


REM ************* Begin ANY CPU *********************************************

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\DataProviders\DataProviders.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

REM "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "..\%CD%\..\OracleSP\OracleSp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlAsp\RdlAsp.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCmd\RdlCmd.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlCri\RdlCri.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlDesign\RdlDesign.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlDesktop\RdlDesktop.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlEngine\RdlEngine.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlMapFile\RdlMapFile.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "%CD%\..\RdlViewer\RdlViewer.sln" /toolsversion:4.0 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

REM ************* End ANY CPU *********************************************


del .\my-fyi-build-dot-net-4 /Q
mkdir .\my-fyi-build-dot-net-4

copy ..\DataProviders\bin\Release\DataProviders.dll .\my-fyi-build-dot-net-4\DataProviders.dll /Y
REM copy ..\OracleSp\bin\Release\OracleSp.dll .\my-fyi-build-dot-net-4\OracleSp.dll /Y
copy ..\RdlAsp\bin\Release\RdlAsp.dll .\my-fyi-build-dot-net-4\RdlAsp.dll /Y
copy ..\RdlCmd\bin\Release\RdlCmd.exe .\my-fyi-build-dot-net-4\RdlCmd.exe /Y
copy ..\RdlCri\bin\Release\RdlCri.dll .\my-fyi-build-dot-net-4\RdlCri.dll /Y
copy ..\RdlDesign\bin\Release\RdlDesigner.exe .\my-fyi-build-dot-net-4\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\Release\RdlDesktop.exe .\my-fyi-build-dot-net-4\RdlDesktop.exe /Y
copy ..\RdlEngine\bin\Release\RdlEngine.dll .\my-fyi-build-dot-net-4\RdlEngine.dll /Y
copy ..\RdlMapFile\bin\Release\RdlMapFile.exe .\my-fyi-build-dot-net-4\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\Release\RdlViewer.dll .\my-fyi-build-dot-net-4\RdlViewer.dll /Y
copy ..\RdlViewer\RdlReader\bin\Release\RdlReader.exe .\my-fyi-build-dot-net-4\RdlReader.exe /Y

7za.exe a my-fyi-build-dot-net-4.zip my-fyi-build-dot-net-4\


