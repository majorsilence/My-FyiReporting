REM Platform options: "x86", "x64", "x64"
REM /p:Configuration="Debug" or "Release"

set msbuildpath="%ProgramFiles(x86)%\MSBuild\14.0\bin\MSBuild.exe"

REM retrieve new version number
REM See http://stackoverflow.com/questions/4044579/how-to-get-output-of-a-for-loop-into-a-variable-in-a-batch-file
cd "%CD%\..\RdlEngine"
for /f "tokens=2 delims=()" %%a in ('findstr AssemblyVersion AssemblyInfo.cs') do set Version=%%~a

set Version=%Version:.*=%
echo %Version%
cd ..
cd Release-Builds

nuget restore "../MajorsilenceReporting.sln"

REM ************* Begin x64 *********************************************

%msbuildpath% "%CD%\..\MajorsilenceReporting.sln" /verbosity:minimal /p:Configuration="Release";Platform="x64" /t:clean;rebuild /m:4

del .\build-output\majorsilence-reporting-build-dot-net-4-x64 /Q
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x64

copy ..\DataProviders\bin\Release\DataProviders.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\DataProviders.dll /Y
copy ..\DataProviders\bin\Release\DataProviders.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\DataProviders.xml /Y
REM copy ..\OracleSp\bin\x64\Release\OracleSp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\OracleSp.dll /Y
copy ..\RdlAsp\bin\Release\RdlAsp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlAsp.dll /Y
copy ..\RdlAsp\bin\Release\RdlAsp.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlAsp.xml /Y
copy ..\RdlCmd\bin\x64\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlCmd.exe /Y
copy ..\RdlCri\bin\Release\RdlCri.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlCri.dll /Y
copy ..\RdlCri\bin\Release\RdlCri.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlCri.xml /Y
copy ..\RdlDesign\bin\x64\Release\RdlDesigner.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x64\Release\RdlDesktop.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x64\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\config.xml /Y
copy ..\RdlEngine\bin\Release\RdlEngine.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngine.dll /Y
copy ..\RdlEngine\bin\Release\RdlEngine.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngine.xml /Y
copy ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ICSharpCode.SharpZipLib.dll /Y
copy "..\References\dot net 4\64bit\System.Data.SQLite.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\System.Data.SQLite.dll /Y
copy "..\packages\iTextSharp-LGPL.4.1.6\lib\iTextSharp.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\iTextSharp.dll /Y
copy ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngineConfig.xml /Y
copy ..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlEngineConfig.Linux.xml /Y
copy ..\RdlMapFile\bin\x64\Release\RdlMapFile.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\Release\RdlViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlViewer.dll /Y
copy ..\RdlViewer\bin\Release\RdlViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlViewer.xml /Y
copy ..\RdlViewer\RdlReader\bin\x64\Release\RdlReader.exe .\build-output\majorsilence-reporting-build-dot-net-4-x64\RdlReader.exe /Y
copy ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.dll /Y
copy ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlWpfViewer.xml /Y
copy ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlCrossPlatformViewer.dll /Y
copy ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x64\LibRdlCrossPlatformViewer.xml /Y
copy "..\References\dot net 4\Xwt.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.dll /Y
copy "..\References\dot net 4\Xwt.Gtk.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.Gtk.dll /Y
copy "..\References\dot net 4\Xwt.WPF.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\Xwt.WPF.dll /Y
copy "..\packages\ZXing.Net.0.14.0.1\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\zxing.dll
copy "..\packages\ZXing.Net.0.14.0.1\lib\net40\zxing.presentation.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x64\zxing.presentation.dll

mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU
copy ..\RdlDesign\bin\x64\Release\ru-RU\RdlDesigner.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlDesigner.resources.dll /Y
copy ..\RdlDesktop\bin\x64\Release\ru-RU\RdlDesktop.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlDesktop.resources.dll /Y
copy ..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlEngine.resources.dll /Y
copy ..\RdlMapFile\bin\x64\Release\ru-RU\RdlMapFile.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlMapFile.resources.dll /Y
copy ..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlViewer.resources.dll /Y
copy ..\RdlViewer\RdlReader\bin\x64\Release\ru-RU\RdlReader.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x64\ru-RU\RdlReader.resources.dll /Y

cd build-output	
..\7za.exe a %Version%-majorsilence-reporting-build-dot-net-4-x64.zip majorsilence-reporting-build-dot-net-4-x64\
cd ..

REM ************* End x64 *********************************************


REM ************* Begin x86 *********************************************

%msbuildpath% "%CD%\..\MajorsilenceReporting.sln" /verbosity:minimal /p:Configuration="Release";Platform="x86" /t:clean;rebuild /m:4

del .\build-output\majorsilence-reporting-build-dot-net-4-x86 /Q
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x86

copy ..\DataProviders\bin\Release\DataProviders.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\DataProviders.dll /Y
copy ..\DataProviders\bin\Release\DataProviders.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\DataProviders.xml /Y
REM copy ..\OracleSp\bin\x86\Release\OracleSp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\OracleSp.dll /Y
copy ..\RdlAsp\bin\Release\RdlAsp.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlAsp.dll /Y
copy ..\RdlAsp\bin\Release\RdlAsp.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlAsp.xml /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCmd.exe /Y
copy ..\RdlCri\bin\Release\RdlCri.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCri.dll /Y
copy ..\RdlCri\bin\Release\RdlCri.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlCri.xml /Y
copy ..\RdlDesign\bin\x86\Release\RdlDesigner.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlDesigner.exe /Y
copy ..\RdlDesktop\bin\x86\Release\RdlDesktop.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlDesktop.exe /Y
copy ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\config.xml /Y
copy ..\RdlEngine\bin\Release\RdlEngine.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngine.dll /Y
copy ..\RdlEngine\bin\Release\RdlEngine.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngine.xml /Y
copy ..\RdlEngine\bin\Release\ICSharpCode.SharpZipLib.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ICSharpCode.SharpZipLib.dll /Y
copy "..\References\dot net 4\32bit\System.Data.SQLite.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\System.Data.SQLite.dll /Y
copy "..\packages\iTextSharp-LGPL.4.1.6\lib\iTextSharp.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\iTextSharp.dll /Y
copy ..\RdlEngine\bin\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.xml /Y
copy ..\RdlEngine\bin\Release\RdlEngineConfig.Linux.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlEngineConfig.Linux.xml /Y
copy ..\RdlMapFile\bin\x86\Release\RdlMapFile.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlMapFile.exe /Y
copy ..\RdlViewer\bin\Release\RdlViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlViewer.dll /Y
copy ..\RdlViewer\bin\Release\RdlViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlViewer.xml /Y
copy ..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe .\build-output\majorsilence-reporting-build-dot-net-4-x86\RdlReader.exe /Y
copy ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlWpfViewer.dll /Y
copy ..\LibRdlWpfViewer\bin\Release\LibRdlWpfViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlWpfViewer.xml /Y
copy ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlCrossPlatformViewer.dll /Y
copy ..\LibRdlCrossPlatformViewer\bin\Release\LibRdlCrossPlatformViewer.xml .\build-output\majorsilence-reporting-build-dot-net-4-x86\LibRdlCrossPlatformViewer.xml /Y
copy "..\References\dot net 4\Xwt.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.dll /Y
copy "..\References\dot net 4\Xwt.Gtk.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.Gtk.dll /Y
copy "..\References\dot net 4\Xwt.WPF.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\Xwt.WPF.dll /Y
copy "..\packages\ZXing.Net.0.14.0.1\lib\net40\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\zxing.dll
copy "..\packages\ZXing.Net.0.14.0.1\lib\net40\zxing.presentation.dll" .\build-output\majorsilence-reporting-build-dot-net-4-x86\zxing.presentation.dll

mkdir .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU
copy ..\RdlDesign\bin\x86\Release\ru-RU\RdlDesigner.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlDesigner.resources.dll /Y
copy ..\RdlDesktop\bin\x86\Release\ru-RU\RdlDesktop.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlDesktop.resources.dll /Y
copy ..\RdlEngine\bin\Release\ru-RU\RdlEngine.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlEngine.resources.dll /Y
copy ..\RdlMapFile\bin\x86\Release\ru-RU\RdlMapFile.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlMapFile.resources.dll /Y
copy ..\RdlViewer\bin\Release\ru-RU\RdlViewer.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlViewer.resources.dll /Y
copy ..\RdlViewer\RdlReader\bin\x86\Release\ru-RU\RdlReader.resources.dll .\build-output\majorsilence-reporting-build-dot-net-4-x86\ru-RU\RdlReader.resources.dll /Y

cd build-output	
..\7za.exe a %Version%-majorsilence-reporting-build-dot-net-4-x86.zip majorsilence-reporting-build-dot-net-4-x86\
cd ..

REM ************* End x86 *********************************************



REM ************* Begin PHP *********************************************
del .\build-output\majorsilence-reporting-build-dot-net-4-php-x86 /Q
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-php-x86

copy ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlEngineConfig.xml /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlCri.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlCmd.exe /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\DataProviders.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlEngine.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\ICSharpCode.SharpZipLib.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\RdlViewer.dll /Y
copy "..\References\dot net 3.5\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\zxing.dll

copy "..\LanguageWrappers\php\config.php" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\config.php
copy "..\LanguageWrappers\php\report.php" .\build-output\majorsilence-reporting-build-dot-net-4-php-x86\report.php

cd build-output	
..\7za.exe a %Version%-majorsilence-reporting-build-dot-net-4-php-x86.zip majorsilence-reporting-build-dot-net-4-php-x86\
cd ..

REM ************* End PHP *********************************************



REM ************* Begin Python *********************************************
del .\build-output\majorsilence-reporting-build-dot-net-4-python-x86 /Q
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-python-x86

copy ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlEngineConfig.xml /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlCri.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlCmd.exe /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\DataProviders.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlEngine.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\ICSharpCode.SharpZipLib.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\RdlViewer.dll /Y
copy "..\References\dot net 3.5\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\zxing.dll

copy "..\LanguageWrappers\python\config.py" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\config.py
copy "..\LanguageWrappers\python\report.py" .\build-output\majorsilence-reporting-build-dot-net-4-python-x86\report.py

cd build-output	
..\7za.exe a %Version%-majorsilence-reporting-build-dot-net-4-python-x86.zip majorsilence-reporting-build-dot-net-4-python-x86\
cd ..
REM ************* End Python *********************************************


REM ************* Begin Ruby *********************************************
del .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86 /Q
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86

copy ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlEngineConfig.xml /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlCri.dll /Y
copy ..\RdlCmd\bin\x86\Release\RdlCmd.exe .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlCmd.exe /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\DataProviders.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlEngine.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\ICSharpCode.SharpZipLib.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\RdlViewer.dll /Y
copy "..\References\dot net 3.5\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\zxing.dll

copy "..\LanguageWrappers\ruby\config.rb" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\config.rb
copy "..\LanguageWrappers\ruby\report.rb" .\build-output\majorsilence-reporting-build-dot-net-4-ruby-x86\report.rb

cd build-output	
..\7za.exe a %Version%-majorsilence-reporting-build-dot-net-4-ruby-x86.zip majorsilence-reporting-build-dot-net-4-ruby-x86\
cd ..

REM ************* End Ruby *********************************************

del .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86 /Q
mkdir .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86

copy ..\RdlDesktop\bin\x86\Release\config.xml .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\config.xml /Y
copy ..\RdlEngine\bin\x86\Release\RdlEngineConfig.xml .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlEngineConfig.xml /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlCri.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlCri.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlReader.exe" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlReader.exe /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\DataProviders.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\DataProviders.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlEngine.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlEngine.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\ICSharpCode.SharpZipLib.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\ICSharpCode.SharpZipLib.dll /Y
copy "%CD%\..\RdlViewer\RdlReader\bin\x86\Release\RdlViewer.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlViewer.dll /Y
copy "..\References\dot net 3.5\zxing.dll" .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\zxing.dll
del .\build-output\majorsilence-reporting-build-dot-net-4-viewer-x86\RdlViewerSC.pdb

cd build-output	
..\7za.exe a %Version%-majorsilence-reporting-build-dot-net-4-viewer-x86.zip majorsilence-reporting-build-dot-net-4-viewer-x86\
cd ..


