"FYIReporting Designer is a report and charting system based on Microsoft's Report Definition Language (RDL). 
Tabular, free form, matrix, charts are fully supported. HTML, PDF, XML, .Net Control, and printing supported. 
A WYSIWYG designer allows you to create reports without knowledge of RDL. Wizards are available for creating new 
reports and for inserting new tables, matrixes, and charts into existing reports." (http://www.fyireporting.com/)

My-FyiReporting is a fork of fyiReporting.  I cannot stress this enough.  This is a FORK.
The main purpose is to make sure that I have a copy of fyiReporting since that project seems to be dead.

I know there are other forks but last I checked they also seemed to be dead.  The main fyiReporting site is 
found at http://www.fyireporting.com/ and it has exmpales found at http://www.fyireporting.com/helpv4/index.php and 
the forum is http://www.fyireporting.com/forum/index.php.  However has I said above for the most part it is a dead 
project.

Also check this projects wiki as information will be slowly added.

This is currently built with visual studio 2008 and targets .net 2.  It should be easy to use visual studio 2010
and target .net 4,  just upgrade the solutions.

Layout:

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
		* I am currently not sure what this is
		* References RdlEngine
	* RdlDesign\RdlDesign.sln
		* Is the main graphical drag and drop designer used to create reports.
		* References RdlEngine
		* References RdlViewer
	* RdlDesktop\RdlDesktop.sln
		* References RdlEngine
	* RdlEngine\RdlEngine.sln
		* Main engine.  Is referenced in many of the other projects
	* RdlViewer
		* References RdlEngine
		* Disabled COM interop
	* RdlMapFile\RdlMapFile.sln
		 *Something to do about maps
	* RdlTest\RdlTests.sln
		 *Tests
	* ReportSever\



Contribute:
* All contributions welcome.  I'll try to respond the same day to any emails or pull requests.  Or within a few days at the most.

*Create github account if you already do not have one
*Create a fork of My-FyiReporting master branch
*Do your changes
*Commit your changes to your for
*Send a pull request and I'll commit the changes or send feed back

Wiki:
*Cleanup current wiki documents
*Add new tutorials
*Add new examples
*Add new documentation

Open Positions:
*Every project and every feature, for example
*DataProviders Maintainer
*RdlEngine Maintainer
*RdlDesigner Maintainer
*RdlViewer Maintainer
*RdlMapFile Maintainer

RDL Compliance
Report file format specifications can be obtained from microsoft.  I believe fyiReporting is currently mostly compatiable with
RDL 2005.  If you want to add more features see the specfications.

RDL specifications: http://msdn.microsoft.com/en-us/library/dd297486%28v=sql.100%29.aspx
2005 direct link: http://download.microsoft.com/download/c/2/0/c2091a26-d7bf-4464-8535-dbc31fb45d3c/rdlNov05.pdf
2008 direct link: http://download.microsoft.com/download/6/5/7/6575f1c8-4607-48d2-941d-c69622e11c32/RDL_spec_08.pdf
2008 R2 direct link: http://download.microsoft.com/download/B/E/1/BE1AABB3-6ED8-4C3C-AF91-448AB733B1AF/Report%20Definition.xps

User interface tutorials:
ReportingCloud (another fork) has made some tutorials for using the designer and creating reports. 
http://sourceforge.net/projects/reportingcloud/files/




TODO (if not already patches on fyi reporting forum):  
* Drag and Drop report into designer
* Pass report path to RdlDesigner.exe when executing and have it open the report
* Can shrink on horizontal space if row is suppressed
* Merge patches from fyiReporting forum
* Create a user control in the designer executable that other application can use
