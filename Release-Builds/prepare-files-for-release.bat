
REM Increment minor number by 1
REM See http://www.codeproject.com/Articles/31236/How-To-Update-Assembly-Version-Number-Automaticall
REM See https://github.com/TownSuite/AssemblyInfoUtil

.\tools\AssemblyInfoUtil.exe -inc:2 "%CD%\..\Directory.Build.props"
