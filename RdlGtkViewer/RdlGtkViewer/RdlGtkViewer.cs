
using System;

using fyiReporting.RDL;


namespace RdlGtkViewer
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RdlGtkViewer : Gtk.Bin
	{

		private Poppler.Document pdf;
		private int pageIndex = 0;
		private int pageHeight = 0;
		
		public RdlGtkViewer ()
		{
			this.Build ();
			
			scrolledwindow1.Vadjustment.ValueChanged += HandleScrollChanged;
		}
				
		private void RenderPage (ref Gtk.Image img) 
		{
	        
			Poppler.Page page = this.pdf.GetPage(this.pageIndex);
			double width=0D;
			double height=0D;
			page.GetSize(out width, out height);
			pageHeight = (int)height;
			
			// It is important to set the image to have the correct size
			img.Pixbuf  = new  Gdk.Pixbuf (Gdk.Colorspace.Rgb,  false, 8, (int)width, (int)height);
			Gdk.Pixbuf pixbuf = img.Pixbuf;
			
			page.RenderToPixbuf(0, 0, (int)width, (int)height, 1.0, 0, pixbuf);
			img.Pixbuf = pixbuf;
			vboxImages.Add (img);		
			
    		}
		
		/// <summary>
		/// Loads the pdf.  This is the function you call when you want to display a pdf.
		/// </summary>
		/// <param name='pdfFileName'>
		/// Pdf file name.
		/// </param>
		public void LoadPdf(string pdfFileName)
		{			
			pdf = Poppler.Document.NewFromFile(pdfFileName, "");
			PageCountLabel.Text = @"/" + (pdf.NPages - 1).ToString();	
			CurrentPage.Value = 0;
			CurrentPage.Adjustment.Upper = pdf.NPages-1;
		
			foreach (Gtk.Widget w in vboxImages.AllChildren)
			{
				vboxImages.Remove(w);
			}
				
			for (this.pageIndex = 0; this.pageIndex < pdf.NPages; this.pageIndex++)
			{
				Gdk.Pixbuf pixbuf = new Gdk.Pixbuf (Gdk.Colorspace.Rgb, false, 8, 0, 0);
				Gtk.Image img = new Gtk.Image();
				img.Pixbuf = pixbuf;		
				img.Name = "image1";
								
				//vboxImages.Add (img);
				RenderPage(ref img);
			}
			
			this.ShowAll();
		}
				
		/// <summary>
		/// Raises the next button clicked event.  Only used in single page mode.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		protected void OnNextButtonClicked (object sender, System.EventArgs e)
		{
			if (pdf.NPages > (int)CurrentPage.Value)
			{
				CurrentPage.Value = CurrentPage.Value + 1;
			}
		}
		
		/// <summary>
		/// Raises the previous button clicked event. Only used in single page mode.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		protected void OnPreviousButtonClicked (object sender, System.EventArgs e)
		{	
			if ((int)CurrentPage.Value > 0)
			{
				CurrentPage.Value = CurrentPage.Value - 1;
			}
		}

		protected void OnFirstPageButtonClicked (object sender, System.EventArgs e)
		{		
			CurrentPage.Value = 0;
		}

		protected void OnLastPageButtonClicked (object sender, System.EventArgs e)
		{	
			CurrentPage.Value = pdf.NPages;
		}
		
		private int previousPage=0;
		protected void OnCurrentPageValueChanged (object sender, System.EventArgs e)
		{
			
			if (_ignorePageChange)
			{
				// If the page is changed because of scrolling skip everything below.
				// However still set the previous page incase the user starts using the button.
				previousPage = (int)CurrentPage.Value;
				return;
			}
			
			if (CurrentPage.Value == previousPage)
			{
				return;
			}
			
		
			int pageDifference = Math.Abs(previousPage - (int)CurrentPage.Value);
			int moveHeight = ((vboxImages.Spacing + pageHeight) * pageDifference);
			
			if ((int)CurrentPage.Value == 0)
			{
				scrolledwindow1.Hadjustment.Value = scrolledwindow1.Hadjustment.Lower;
				scrolledwindow1.Vadjustment.Value = scrolledwindow1.Vadjustment.Lower;
			}
			else if ((int)CurrentPage.Value == pdf.NPages)
			{
				scrolledwindow1.Hadjustment.Value = scrolledwindow1.Hadjustment.Lower;
				scrolledwindow1.Vadjustment.Value = scrolledwindow1.Vadjustment.Upper-pageHeight;
			}
			else if (previousPage > (int)CurrentPage.Value)
			{ // Move back one page
				scrolledwindow1.Hadjustment.Value = scrolledwindow1.Hadjustment.Lower;
				scrolledwindow1.Vadjustment.Value = scrolledwindow1.Vadjustment.Value - moveHeight;	
			}
			else if((int)CurrentPage.Value > previousPage)
			{ // Move forward 1 page
				scrolledwindow1.Hadjustment.Value = scrolledwindow1.Hadjustment.Lower;
				scrolledwindow1.Vadjustment.Value = scrolledwindow1.Vadjustment.Value + moveHeight;
					
			}
			
			
			previousPage = (int)CurrentPage.Value;
		}
		
		
		protected void OnPrintButtonClicked (object sender, System.EventArgs e)
		{
			Gtk.PrintOperation print = new Gtk.PrintOperation ();    
			// Tell the Print Operation how many pages there are
			print.NPages = this.pdf.NPages;

			print.BeginPrint += new Gtk.BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new Gtk.DrawPageHandler (OnDrawPage);
			print.EndPrint += new Gtk.EndPrintHandler (OnEndPrint);

			// Run the Print Operation and tell it what it should do (Export, Preview, Print, PrintDialog)
			// And provide a parent window if applicable
			print.Run (Gtk.PrintOperationAction.PrintDialog, null);
			print = null;
		}
		
		/// <summary>
        /// OnBeginPrint - Load up the Document to be printed and analyze it.
        /// </summary>
        /// <param name="obj">The Print Operation</param>
        /// <param name="args">The BeginPrintArgs passed by the Print Operation</param>
        private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
        {            
            			
        }
		
		/// <summary>
        /// OnDrawPage - Draws the Content of each Page to be printed
        /// </summary>
        /// <param name="obj">The Print Operation</param>
        /// <param name="args">The DrawPageArgs passed by the Print Operation</param>
        private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
        {
            // Create a Print Context from the Print Operation
            Gtk.PrintContext context = args.Context;

            // Create a Cairo Context from the Print Context
            Cairo.Context cr = context.CairoContext;
            
            Poppler.Page pg = this.pdf.GetPage(args.PageNr);
			pg.RenderForPrintingWithOptions(cr, Poppler.PrintFlags.Document);
                  
        }

        /// <summary>
        /// OnEndPrint - Executed at the end of the Print Operation
        /// </summary>
        /// <param name="obj">The Print Operation</param>
        /// <param name="args">The EndPrintArgs passed by the Print Operation</param>
        private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
        {
        }
		
		

		protected void OnSaveButtonClicked (object sender, System.EventArgs e)
		{
			
			object []param= new object[4];
			param[0] = "Cancel";
			param[1] = Gtk.ResponseType.Cancel;
			param[2] = "Save";
			param[3] = Gtk.ResponseType.Accept;
			
			Gtk.FileChooserDialog fc=
			new Gtk.FileChooserDialog("Save File As",
			                            null,
			                            Gtk.FileChooserAction.Save,
			                            param);
	
			
			
			
			if (fc.Run() == (int)Gtk.ResponseType.Accept) 
			{
				try
				{
					Uri file = new Uri(fc.Filename);
					pdf.SaveACopy(file.AbsoluteUri);
				}
				catch(Exception ex)
				{
					Gtk.MessageDialog m = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info,
						Gtk.ButtonsType.Ok, false, 
						"Error Saving Copy of PDF." + System.Environment.NewLine + ex.Message);
						
					m.Run();
					m.Destroy();
				}
			}
			//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
			fc.Destroy();
		}		
		
		private bool _ignorePageChange = false;
		void HandleScrollChanged(object sender, EventArgs e) 
		{
		    // vertical value changed
			int currentVAdjustment = (int)scrolledwindow1.Vadjustment.Value;
			int currentPage = (int)CurrentPage.Value;		
			
			int truePageHeight = vboxImages.Spacing + pageHeight;
			
			int scrollPage = currentVAdjustment / truePageHeight;
			if (scrollPage != currentPage)
			{
				_ignorePageChange = true;
				CurrentPage.Value = scrollPage;
				_ignorePageChange = false;
			}
		}
		
	}
}

