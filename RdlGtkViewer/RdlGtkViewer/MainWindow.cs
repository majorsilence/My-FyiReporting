using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		this.rdlgtkviewer1.LoadReport(
			@"/home/peter/Projects/My-FyiReporting/Examples/SqliteExamples/SimpleTest2.rdl");
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
