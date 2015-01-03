
REM requires you have gow 0.8.0 or newer installed (see https://github.com/bmatzelle/gow).  Or version of sed with in place editing in your path

REM Increment minor number by 1
REM See http://www.codeproject.com/Articles/31236/How-To-Update-Assembly-Version-Number-Automaticall

.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\DataProviders\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\LibRdlCrossPlatformViewer\Properties\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\LibRdlWpfViewer\Properties\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlAsp\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlCmd\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlCri\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlDesign\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlDesktop\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlEngine\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlGtkViewer\RdlGtkViewer\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlMapFile\Properties\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlViewer\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlViewer\RdlReader\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\ReportServer\Properties\AssemblyInfo.cs"


REM retrieve new version number
REM See http://stackoverflow.com/questions/4044579/how-to-get-output-of-a-for-loop-into-a-variable-in-a-batch-file
cd "%CD%\..\RdlEngine"
for /f "tokens=2 delims=()" %%a in ('findstr AssemblyVersion AssemblyInfo.cs') do set Version=%%~a

set Version=%Version:.*=%
echo %Version%
cd ..
cd Release-Builds

REM Update NUGET spec files
sed.exe -i "5s/.*/    <version>%Version%<\/version>/" "%CD%\nuget\MajorsilenceReporting-Asp\MajorsilenceReporting-Asp.nuspec"
sed.exe -i "5s/.*/    <version>%Version%<\/version>/" "%CD%\nuget\MajorsilenceReporting-Core\MajorsilenceReporting-Core.nuspec"
sed.exe -i "5s/.*/    <version>%Version%<\/version>/" "%CD%\nuget\MajorsilenceReporting-XwtViewer\MajorsilenceReporting-XwtViewer.nuspec"
sed.exe -i "5s/.*/    <version>%Version%<\/version>/" "%CD%\nuget\MajorsilenceReporting-Viewer\MajorsilenceReporting-Viewer.nuspec"

del "sed*"
