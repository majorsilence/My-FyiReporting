# Majorsilence Reporting (formerly My-FyiReporting)

If you have any question about Majorsilence Reporting or do you want to contribute a discussion group for Majorsilence Reporting is available here:

https://groups.google.com/d/forum/myfyireporting


|         |Linux |Windows |
|---------|:------:|:------:|
|**Release**|NA | [![Build status appveyor](https://ci.appveyor.com/api/projects/status/a44n015bli95rmpw?svg=true)](https://ci.appveyor.com/project/majorsilence/my-fyireporting) |

# Documentation
See the [projects wiki](https://github.com/majorsilence/My-FyiReporting/wiki).

# Download

See the [downloads page](https://github.com/majorsilence/My-FyiReporting/wiki/Downloads).

Alternatively if you want keep up with the latest version you can always use Git

    git clone https://github.com/majorsilence/My-FyiReporting.git

# Introduction
"FYIReporting Designer is a report and charting system based on Microsoft's Report Definition Language (RDL). 
Tabular, free form, matrix, charts are fully supported. HTML, PDF, XML, .Net Control, and printing supported. 
A WYSIWYG designer allows you to create reports without knowledge of RDL. Wizards are available for creating new 
reports and for inserting new tables, matrixes, and charts into existing reports." (http://www.fyireporting.com/)

Use the report viewer .NET controls from ASP.NET, WPF, or Winforms using C#, F#, VB.NET, IronPython, or any 
other .NET language.  The winform viewer also works in linux using mono.  An experimental Gtk/WPF/Cocoa viewer also exists. 

Majorsilence Reporting started as My-FyiReporting which was a fork of fyiReporting after it died. It has been rebranded as Majorsilence Reporting to make it clearer that it is a separate forked project.  Majorsilence Reporting is a fork of fyiReporting.  I cannot stress this enough.  This is a FORK.
The main purpose is to make sure that I have a copy of fyiReporting since that project seems to be dead.  I am leaving the
github project named My-FyiReporting so links are not broken.  All branding will eventually be replaced with Majorsilence Reporting.

Also check this [projects wiki](https://github.com/majorsilence/My-FyiReporting/wiki) as information will be slowly added.

Majorsilence Reporting is currently built with visual studio 2010 and 2012 and targets .net 4.0.  You can also run the build script in 
the Release-Builds folder to build .net 3.5 (may be broken) and 4.0 packages.  

# Development
Majorsilence Reporting is developed with the following workflow:

* Nothing happens for weeks
* Someone needs it to do something it doesn't already do
* That person implements that something and submits a pull request
* Repeat
If it doesn't have a feature that you want it to have, add it.  If it has a bug you need fixed, fix it.

## Contribute:
All contributions welcome.  I'll try to respond the same day to any emails or pull requests.  Or within a few 
days at the most.  Small pull requests are best as they are easier to review.

See the wiki page [https://github.com/majorsilence/My-FyiReporting/wiki/Contribute](https://github.com/majorsilence/My-FyiReporting/wiki/Contribute)

### Core Team

* [majorsilence](https://github.com/majorsilence) (Peter Gill)
* [Gankov](https://github.com/Gankov)


### Contributors

A big thanks to all of Majorsilence Reporting contributors:

* [ausadmin](https://github.com/ausadmin)
* [Geek648](https://github.com/Geek648)
* [kobruleht](https://github.com/kobruleht)
* [jzielke](https://github.com/jzielke)
* [gam6itko](https://github.com/gam6itko)
* [tsliang](https://github.com/tsliang)
* [mohsenalikhani](https://github.com/mohsenalikhani)
* [mgroves](https://github.com/mgroves)
* [sobolev88](https://github.com/sobolev88)


# Layout:

* DataProviders\DataProviders.sln
* Images\
* OracleSp\OracleSp.sln
	* Requires Oracle Data Provider for .NET
* RdlAsp\RdlAsp.sln
	* Asp controls to display reports in asp.net and silverlight pages.
	* References RdlEngine
* RdlCMD\RdlCmd.sln
	* Command line tools
	* References RdlEngine
* RdlCri\RdlCri.sln
	* Custom Report Controls
	* References RdlEngine
* RdlDesign\RdlDesign.sln
	* Is the main graphical drag and drop designer used to create reports.
	* References RdlEngine
	* References RdlViewer
* RdlDesktop\RdlDesktop.sln
	* References RdlEngine
* RdlEngine\RdlEngine.sln
	* Main engine.  Is referenced in many of the other projects
* RdlGtkViewer\RdlGtkViewer.sln
	* A Gtk# (gtk-sharp) viewer
* RdlViewer
	* References RdlEngine
	* Disabled COM interop
* RdlMapFile\RdlMapFile.sln
	 * Map viewer
* RdlTest\RdlTests.sln
	 * Tests
* ReportSever\


 RDL Compliance
Report file format specifications can be obtained from microsoft.  I believe fyiReporting is currently mostly 
compatible with RDL 2005.  If you want to add more features see the specfications.

* RDL specifications: [http://msdn.microsoft.com/en-us/library/dd297486%28v=sql.100%29.aspx](http://msdn.microsoft.com/en-us/library/dd297486%28v=sql.100%29.aspx)
* 2005 direct link: [http://download.microsoft.com/download/c/2/0/c2091a26-d7bf-4464-8535-dbc31fb45d3c/rdlNov05.pdf](http://download.microsoft.com/download/c/2/0/c2091a26-d7bf-4464-8535-dbc31fb45d3c/rdlNov05.pdf)
* 2008 direct link: [http://download.microsoft.com/download/6/5/7/6575f1c8-4607-48d2-941d-c69622e11c32/RDL_spec_08.pdf](http://download.microsoft.com/download/6/5/7/6575f1c8-4607-48d2-941d-c69622e11c32/RDL_spec_08.pdf)
* 2008 R2 direct link: [http://download.microsoft.com/download/B/E/1/BE1AABB3-6ED8-4C3C-AF91-448AB733B1AF/Report%20Definition.xps](http://download.microsoft.com/download/B/E/1/BE1AABB3-6ED8-4C3C-AF91-448AB733B1AF/Report%20Definition.xps)

#User interface tutorials:
ReportingCloud (another fork) has made some tutorials for using the designer and creating reports. 
[http://sourceforge.net/projects/reportingcloud/files/](http://sourceforge.net/projects/reportingcloud/files/)

I know there are other forks but last I checked they also seemed to be dead.  The main fyiReporting site is 
found at http://www.fyireporting.com/ and it has examples found at [http://www.fyireporting.com/helpv4/index.php](http://www.fyireporting.com/helpv4/index.php) and 
the forum is [http://www.fyireporting.com/forum/index.php](http://www.fyireporting.com/forum/index.php).  However has I said above for the most part it is a dead 
project.

UPDATE: ReportFU [http://reportfu.org](http://reportfu.org) is alive?  
UPDATE: NReport [http://nreports.codeplex.com/](http://nreports.codeplex.com/) - Looks dead.

