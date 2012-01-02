using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		this.rdlgtkviewer1.LoadReport(
			new Uri(@"/home/peter/Projects/My-FyiReporting/Examples/SqliteExamples/SimpleTest3WithParameters.rdl"), 
			"TestParam1=Hello and Goodbye&TestParam2=Testing parameter 2");
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
