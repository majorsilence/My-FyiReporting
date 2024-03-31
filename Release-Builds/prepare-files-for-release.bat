
REM Increment minor number by 1
REM See http://www.codeproject.com/Articles/31236/How-To-Update-Assembly-Version-Number-Automaticall
REM See https://github.com/TownSuite/AssemblyInfoUtil

.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\DataProviders\DataProviders.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\LibRdlCrossPlatformViewer\LibRdlCrossPlatformViewer.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\LibRdlWpfViewer\LibRdlWpfViewer.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlAsp\RdlAsp.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlCmd\RdlCmd.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlCri\RdlCri.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlDesign\RdlDesign.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlDesktop\RdlDesktop.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlEngine\RdlEngine.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlGtkViewer\RdlGtkViewer\AssemblyInfo.cs"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlMapFile\RdlMapFile.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlViewer\RdlViewer.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\RdlViewer\RdlReader\RdlViewer.csproj"
.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\ReportServer\Properties\AssemblyInfo.cs"


