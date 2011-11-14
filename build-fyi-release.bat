REM Platform options: "x86", "x64", "Any CPU"
REM /p:Configuration="Debug" or "Release"


REM ************* Begin ANY CPU *********************************************

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\DataProviders\DataProviders.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

REM "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\OracleSP\OracleSp.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlAsp\RdlAsp.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlCmd\RdlCmd.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlCri\RdlCri.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlDesign\RdlDesign.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlDesktop\RdlDesktop.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlEngine\RdlEngine.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlMapFile\RdlMapFile.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "%CD%\RdlViewer\RdlViewer.sln" /toolsversion:3.5 /p:Configuration="Release";Platform="Any CPU" /t:clean;rebuild /m:4

REM ************* End ANY CPU *********************************************


del .\my-fyi-build /Q
mkdir .\my-fyi-build

copy .\DataProviders\bin\Release\DataProviders.dll .\my-fyi-build\DataProviders.dll /Y
REM copy .\OracleSp\bin\Release\OracleSp.dll .\my-fyi-build\OracleSp.dll /Y
copy .\RdlAsp\bin\Release\RdlAsp.dll .\my-fyi-build\RdlAsp.dll /Y
copy .\RdlCmd\bin\Release\RdlCmd.exe .\my-fyi-build\RdlCmd.exe /Y
copy .\RdlCri\bin\Release\RdlCri.dll .\my-fyi-build\RdlCri.dll /Y
copy .\RdlDesign\bin\Release\RdlDesigner.exe .\my-fyi-build\RdlDesigner.exe /Y
copy .\RdlDesktop\bin\Release\RdlDesktop.exe .\my-fyi-build\RdlDesktop.exe /Y
copy .\RdlEngine\bin\Release\RdlEngine.dll .\my-fyi-build\RdlEngine.dll /Y
copy .\RdlMapFile\bin\Release\RdlMapFile.exe .\my-fyi-build\RdlMapFile.exe /Y
copy .\RdlViewer\bin\Release\RdlViewer.dll .\my-fyi-build\RdlViewer.dll /Y
copy .\RdlViewer\RdlReader\bin\Release\RdlReader.exe .\my-fyi-build\RdlReader.exe /Y

7za.exe a my-fyi-build.zip my-fyi-build\


