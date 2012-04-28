using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();	
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnZoomInActionActivated (object sender, System.EventArgs e)
	{
		throw new System.NotImplementedException ();
	}

	protected void OnZoomOutActionActivated (object sender, System.EventArgs e)
	{
		throw new System.NotImplementedException ();
	}

	protected void OnPrintActionActivated (object sender, System.EventArgs e)
	{
		throw new System.NotImplementedException ();
	}

	protected void OnPdfActionActivated (object sender, System.EventArgs e)
	{
		throw new System.NotImplementedException ();
	}

	protected void OnRefreshActionActivated (object sender, System.EventArgs e)
	{
		throw new System.NotImplementedException ();
	}

	protected void OnFileOpen_Activated (object sender, System.EventArgs e)
	{
			object []param= new object[4];
			param[0] = "Cancel";
			param[1] = Gtk.ResponseType.Cancel;
			param[2] = "Open";
			param[3] = Gtk.ResponseType.Accept;
			
			Gtk.FileChooserDialog fc=
			new Gtk.FileChooserDialog("Open File",
			                            null,
			                            Gtk.FileChooserAction.Open,
			                            param);
		
			Gtk.FileFilter rdlFilter = new Gtk.FileFilter();
			rdlFilter.Name = "RDL";
		
			//fc.AddFilter(rdlFilter);
		
		
		if (fc.Run() == (int)Gtk.ResponseType.Accept) 
			{
				try
				{
													
					string filename = fc.Filename;		
				
					if (System.IO.File.Exists(filename))
					{
						this.reportviewer1.LoadReport(new Uri(filename));
					}
				
				}
				catch(Exception ex)
				{
					Gtk.MessageDialog m = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info,
						Gtk.ButtonsType.Ok, false, 
						"Error Opening File." + System.Environment.NewLine + ex.Message);
						
					m.Run();
					m.Destroy();
				}
			}
			//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
			fc.Destroy();
		
			
	}
}
