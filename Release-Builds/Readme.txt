* Install .net 3.5sp1 sdk http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=11310
* Install .net 4.0 sdk http://www.microsoft.com/download/en/details.aspx?id=8279
* Install Gow - choco install gow
* Install nuget command line - choco install nuget.commandline 

1. Update Assembly version numbers in all projects (Run: prepare-files-for-release.bat)
2. Build .NET 4.0 package:
	* Run build-fyi-release-dot-net-4.bat
3. Build Linux package:
	* Run build-fyi-release-dot-net-4.sh
4. Update warsetup project and version number
5. Build warsetup project
6. Build NuGet packages - build-nuget-packages.bat
7. Create Git TAG
8. Post to google group https://groups.google.com/d/forum/myfyireporting


Release Google Group Post Message:

Majorsilence Reporting is a report and charting system based on Microsoft's Report Definition Language (RDL). Tabular, free form, matrix, charts are fully supported. HTML, PDF, XML, .Net Control, and printing supported.  An experimental Gtk/WPF/Cocoa viewer also exist.  There are also language wrappers available for php, python, and ruby that make it easy to generate reports.  The WYSIWYG designer allows you to create reports without knowledge of RDL. Wizards are available for creating new reports and for inserting new tables, matrixes, charts, and barcodes into existing reports.

Majorsilence reporting started as My-FyiReporting which was a fork of fyiReporting after it died. It has been re-branded as Majorsilence Reporting to make it clearer that it is a separate forked project.  Its purpose is to keep the project alive and useful.


[Release Number Goes Here]

[Release Notes Go Here]

Download from: https://github.com/majorsilence/My-FyiReporting/wiki/Downloads

