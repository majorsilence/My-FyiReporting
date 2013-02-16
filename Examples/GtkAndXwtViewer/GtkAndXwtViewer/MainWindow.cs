using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	

	private LibRdlCrossPlatformViewer.ReportViewer rv;
	
	// TODO: add a way that parameters can be entered by an end user.
	private string parameters = "";

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		// Before using any xwt code you must initialize its engine as gtk
		// See https://groups.google.com/forum/?fromgroups=#!topic/xwt-list/9d2kb4cf5GU
		Xwt.Application.Initialize(Xwt.ToolkitType.Gtk);
		Xwt.Engine.Toolkit.ExitUserCode(null);
		
		rv = new LibRdlCrossPlatformViewer.ReportViewer();
		rv.DefaultBackend = LibRdlCrossPlatformViewer.Backend.XwtWinforms;
		
		// Since this is an example I just hard code the report path. 
		// In your own application you will want to provice a method to select reports.
#if DEBUG
		if (System.Environment.MachineName == "GILL-PC")
		{
			rv.LoadReport(new Uri(@"C:\Users\Peter\Projects\My-FyiReporting\Examples\SqliteExamples\SimpleTest1.rdl"));
		}
		else if (System.Environment.MachineName == "gill-desktop")
		{
			rv.LoadReport(new Uri(@"/home/peter/projects/My-FyiReporting/Examples/SqliteExamples/SimpleTest1.rdl"));
		}
#endif
		
		// Here we convert the xwt VBox to a Gtk widget
		Gtk.Widget w = (Gtk.Widget)Xwt.Engine.WidgetRegistry.GetNativeWidget(rv);

		this.Add(w);
		

	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
