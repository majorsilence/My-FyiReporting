

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


