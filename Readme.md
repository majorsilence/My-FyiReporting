# Majorsilence Reporting (formerly My-FyiReporting)

If you have any question about Majorsilence Reporting or do you want to contribute a discussion group for Majorsilence Reporting is available here:

https://groups.google.com/d/forum/myfyireporting


|         |Linux |Mac | Win | Win(AppeyVeyor) |
|---------|:------:|:------:|:------:|:------:|
|**Master**| [![linux](https://github.com/majorsilence/My-FyiReporting/actions/workflows/linux.yml/badge.svg?branch=master)](https://github.com/majorsilence/My-FyiReporting/actions/workflows/linux.yml) | [![mac](https://github.com/majorsilence/My-FyiReporting/actions/workflows/mac.yml/badge.svg?branch=master)](https://github.com/majorsilence/My-FyiReporting/actions/workflows/mac.yml) | [![.github/workflows/windows.yml](https://github.com/majorsilence/My-FyiReporting/actions/workflows/windows.yml/badge.svg?branch=master)](https://github.com/majorsilence/My-FyiReporting/actions/workflows/windows.yml) | [![Build status appveyor](https://ci.appveyor.com/api/projects/status/a44n015bli95rmpw?svg=true)](https://ci.appveyor.com/project/majorsilence/my-fyireporting) | 



# Documentation
See the [projects wiki](https://github.com/majorsilence/My-FyiReporting/wiki).

# Download

See the [downloads page](https://github.com/majorsilence/My-FyiReporting/wiki/Downloads).

Alternatively if you want keep up with the latest version you can always use Git

    git clone https://github.com/majorsilence/My-FyiReporting.git

# Introduction
Majorsilence Reporting is a powerful, open-source .NET reporting framework designed for developers who need to create, design, and deliver rich, reports. Supporting modern .NET versions (8.0), it provides a flexible and extensible platform for building reports from a variety of data sources. With a drag-and-drop designer, multiple viewer options, and cross-platform support, Majorsilence Reporting is ideal for both desktop and web applications. Whether you need to generate reports programmatically or empower users with a visual designer (windows only), this project offers the tools and documentation to get you started quickly.

**The core of Majorsilence Reporting supports Linux and macOS for server-side application report generation. Only the WinForms-based designer and viewer are Windows-only.**

# Quick start

Add these nuget packages to your project.

```bash
dotnet add package Majorsilence.Reporting.RdlCreator.SkiaSharp
dotnet add package Majorsilence.Reporting.RdlEngine.SkiaSharp
dotnet add package Majorsilence.Reporting.RdlCri.SkiaSharp
```

You are now ready to create and generate reports.

## c# example connected to an sql database

See [Database Providers](https://github.com/majorsilence/My-FyiReporting/wiki/Database-Providers-Howto).

```cs
using Majorsilence.Reporting.RdlCreator;

string dataProvider = "[PLACEHOLDER/Json/Microsoft.Data.SqlClient/MySQL.NET/Firebird.NET 2.0/Microsoft.Data.Sqlite/PostgreSQL";
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

See [Contribute](https://github.com/majorsilence/My-FyiReporting/wiki/Contribute).

