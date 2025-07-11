# Majorsilence Reporting (formerly My-FyiReporting)

If you have any question about Majorsilence Reporting or do you want to contribute a discussion group for Majorsilence Reporting is available here:

https://groups.google.com/d/forum/myfyireporting


|         |Linux |Win(Deploy) |
|---------|:------:|:------:|
|**Release**|NA | [![Build status appveyor](https://ci.appveyor.com/api/projects/status/a44n015bli95rmpw?svg=true)](https://ci.appveyor.com/project/majorsilence/my-fyireporting) | 

# Documentation
See the [projects wiki](https://github.com/majorsilence/My-FyiReporting/wiki).

# Download

See the [downloads page](https://github.com/majorsilence/My-FyiReporting/wiki/Downloads).

Alternatively if you want keep up with the latest version you can always use Git

    git clone https://github.com/majorsilence/My-FyiReporting.git

# Introduction
Majorsilence Reporting is a powerful, open-source .NET reporting framework designed for developers who need to create, design, and deliver rich, interactive reports. Supporting modern .NET versions (8.0), it provides a flexible and extensible platform for building reports from a variety of data sources. With a drag-and-drop designer, multiple viewer options, and cross-platform support, Majorsilence Reporting is ideal for both desktop and web applications. Whether you need to generate reports programmatically or empower users with a visual designer, this project offers the tools and documentation to get you started quickly.

Also check this [projects wiki](https://github.com/majorsilence/My-FyiReporting/wiki) as information will be slowly added.

# Quick start

nuget package

```xml
<PackageReference Include="Majorsilence.Reporting.RdlCreator.SkiaSharp" />
<PackageReference Include="Majorsilence.Reporting.RdlCri.SkiaSharp" />
```

## c# example connected to an sql database
```cs
using Majorsilence.Reporting.RdlCreator;

var create = new Majorsilence.Reporting.RdlCreator.Create();

var report = await create.GenerateRdl(dataProvider,
    connectionString,
    "SELECT CategoryID, CategoryName, Description FROM Categories",
    pageHeaderText: "DataProviderTest TestMethod1");

string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "PLACEHOLDER.pdf");
var ofs = new Majorsilence.Reporting.Rdl.OneFileStreamGen(filepath, true);
await report.RunGetData(null);
await report.RunRender(ofs, Majorsilence.Reporting.Rdl.OutputPresentationType.PDF);
```

## c# example, create a pdf document

```cs
using Majorsilence.Reporting.RdlCreator;

var document = new Majorsilence.Reporting.RdlCreator.Document()
{
    Description = "Sample report",
    Author = "John Doe",
    PageHeight = "11in",
    PageWidth = "8.5in",
    //Width = "7.5in",
    TopMargin = ".25in",
    LeftMargin = ".25in",
    RightMargin = ".25in",
    BottomMargin = ".25in"
}
.WithPage((page) =>
{
    page.WithHeight("10in")
    .WithWidth("7.5in")
    .WithText(new Text
    {
        Name = "TheSimplePageText",
        Top = ".1in",
        Left = ".1in",
        Width = "6in",
        Height = ".25in",
        Value = new Value { Text = "Text Area 1" },
        Style = new Style { FontSize = "12pt", FontWeight = "Bold" }
    });
});

using var fileStream = new FileStream("PLACEHOLDER.pdf", FileMode.Create, FileAccess.Write);
await document.Create(fileStream);
```


# Development
Majorsilence Reporting is developed with the following workflow:

* Nothing happens for weeks or months
* Someone needs it to do something it doesn't already do
* That person implements that something and submits a pull request
* Repeat

If it doesn't have a feature that you want it to have, add it.  If it has a bug you need fixed, fix it.

## libgdiplus

Useful for winforms projects on windows but otherwise avoid this.

Avoid by using the "Debug-DrawingCompat" or "Release-DrawingCompat" targets to use the skiasharp drawing backend.

### Mac - Legacy

Install mono-libgdiplus.

```bash
brew install mono-libgdiplus
```

For report generation to work set the DYLD_LIBRARY_PATH environment variable:

```bash
DYLD_LIBRARY_PATH=$DYLD_LIBRARY_PATH:/opt/homebrew/lib
```

### Ubuntu - Legacy

Install libgdiplus.

```bash
apt install libgdiplus
```

For report generation to work set the LD_LIBRARY_PATH environment variable:

```bash
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/lib
```

## Contribute:
All contributions welcome.  I'll try to respond the same day to any emails or pull requests.  Or within a few 
days at the most.  Small pull requests are best as they are easier to review.

See the wiki page [https://github.com/majorsilence/My-FyiReporting/wiki/Contribute](https://github.com/majorsilence/My-FyiReporting/wiki/Contribute)

### Core Team

* [majorsilence](https://github.com/majorsilence) (Peter Gill)
* [Gankov](https://github.com/Gankov)


### Contributors

A big thanks to all of Majorsilence Reporting [contributors](https://github.com/majorsilence/My-FyiReporting/graphs/contributors).

# Layout:

* DataProviders
* RdlAsp.Mvc
	* asp.net core mvc controllers
* RdlCMD
	* Command line tools
* RdlCri
	* Custom Report Controls
* RdlDesign
	* Is the main graphical drag and drop designer used to create reports.
* RdlDesktop
	* Simple server
* RdlEngine
	* Main engine.  Is referenced in many of the other projects
* RdlGtkViewer
	* A Gtk# 2 (gtk-sharp) viewer
* RdlGtk3
    * GtkSharp 3 core components
* RdlGtk3Viewer
    * GtkSharp 3 viewer
* RdlReader
    * Viewer executable
* RdlViewer
	* View controls
	* Disabled COM interop
* RdlMapFile
	 * Map viewer
* RdlTest
	 * Tests
* ReportSever
	* aspx webforms - v4 branch
* RdlCreator
	* Code first report creation and generation


# RDL Compliance
Report file format specifications can be obtained from microsoft.  I believe fyiReporting is currently mostly 
compatible with RDL 2005.  If you want to add more features see the specifications.

* RDL specifications: [http://msdn.microsoft.com/en-us/library/dd297486%28v=sql.100%29.aspx](http://msdn.microsoft.com/en-us/library/dd297486%28v=sql.100%29.aspx)
* 2005 direct link: [http://download.microsoft.com/download/c/2/0/c2091a26-d7bf-4464-8535-dbc31fb45d3c/rdlNov05.pdf](http://download.microsoft.com/download/c/2/0/c2091a26-d7bf-4464-8535-dbc31fb45d3c/rdlNov05.pdf)
* 2008 direct link: [http://download.microsoft.com/download/6/5/7/6575f1c8-4607-48d2-941d-c69622e11c32/RDL_spec_08.pdf](http://download.microsoft.com/download/6/5/7/6575f1c8-4607-48d2-941d-c69622e11c32/RDL_spec_08.pdf)
* 2008 R2 direct link: [http://download.microsoft.com/download/B/E/1/BE1AABB3-6ED8-4C3C-AF91-448AB733B1AF/Report%20Definition.xps](http://download.microsoft.com/download/B/E/1/BE1AABB3-6ED8-4C3C-AF91-448AB733B1AF/Report%20Definition.xps)

# User interface tutorials
ReportingCloud (another fork) has made some tutorials for using the designer and creating reports. 
[http://sourceforge.net/projects/reportingcloud/files/](http://sourceforge.net/projects/reportingcloud/files/)



