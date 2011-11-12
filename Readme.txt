My-FyiReporting is a fork of fyiReporting.  I cannot stress this enough.  This is a FORK.
The main purpose is to make sure that I have a copy of fyiReporting since that project seems to be dead.

I know there are other forks but last I checked they also seemed to be dead.

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
