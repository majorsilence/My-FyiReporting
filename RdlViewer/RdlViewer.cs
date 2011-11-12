/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text;
using fyiReporting.RDL;

namespace fyiReporting.RdlViewer
{
	/// <summary>
	/// RdlViewer displays RDL files or syntax. 
	/// </summary>
	public class RdlViewer : System.Windows.Forms.Control
	{
        public delegate void HyperlinkEventHandler(object source, HyperlinkEventArgs e);
        /// <summary>
        /// Hyperlink invoked when report item with hyperlink is clicked on.
        /// </summary>
        public event HyperlinkEventHandler Hyperlink;
        public event EventHandler<SubreportDataRetrievalEventArgs> SubreportDataRetrieval;

        public NeedPassword GetDataSourceReferencePassword = null;
		bool _InPaint=false;
		bool _InLoading=false;
		private string _SourceFileName;		// file name to use
		private string _SourceRdl;			// source Rdl; if provided overrides filename
		private string _Parameters;			// parameters to run the report
		private Report _Report;				// the report
		private string _Folder;				// folder for DataSourceReference (if file name not provided)
		private Pages _pgs;					// the pages of the report to view
		//private PageDrawing _pd;			// draws the pages of a report
		private bool _loadFailed;			// last load of report failed
		private float _leftMargin;			// left margin; calculated based on size of window & scroll style
		// report information
		private float _PageWidth;			// width of page
		private float _PageHeight;			// height of page
		private string _ReportDescription;
		private string _ReportAuthor;
		private string _ReportName;
		private IList _errorMsgs;

		// Zoom
		private float _zoom;				// zoom factor
		private float DpiX;
		private float DpiY;
		private ZoomEnum _zoomMode=ZoomEnum.FitWidth;
		private float _leftGap=10;			// right margin: 10 points
		private float _rightGap=10;			// left margin: 10 points
		private float _pageGap=10;			// gap between pages: 10 points

		// printing 
        private bool _UseTrueMargins = true;    // compensate for non-printable region
		private int printEndPage;			// end page
		private int printCurrentPage;		// current page to print

		// Scrollbars
		private ScrollModeEnum _ScrollMode;
		private VScrollBar _vScroll;
		private ToolTip _vScrollToolTip;
		private HScrollBar _hScroll;

		private PageDrawing _DrawPanel;		// the main drawing panel
		private Button _RunButton;
		private PictureBox _WarningButton;
		private ScrollableControl _ParameterPanel;	// panel for specifying parameters
		private int _ParametersMaxHeight;			// max height of controls in _ParameterPanel
        private RdlViewerFind _FindCtl;
        private string _HighlightText=null;      // text that should be highlighted when drawn
        private bool _HighlightCaseSensitive = false;   // highlight text is case insensitive
        private bool _HighlightAll = false;     // highlight all instances of Highlight text
        private PageItem _HighlightItem = null; // page item to highlight

		private bool _ShowParameters=true;
        private bool _ShowWaitDialog = true;    // show wait dialog when running report
        
		public RdlViewer()
		{
			_SourceFileName=null;
			_SourceRdl=null;
			_Parameters=null;				// parameters to run the report
			_pgs=null;						// the pages of the report to view
			_loadFailed=false;	
			_PageWidth=0;
			_PageHeight=0;
			_ReportDescription=null;
			_ReportAuthor=null;
			_ReportName=null;
			_zoom=-1;						// force zoom to be calculated

			// Get our graphics DPI					   
			Graphics g = null;
			try
			{
				g = this.CreateGraphics(); 
				DpiX = g.DpiX;
				DpiY = g.DpiY;
			}
			catch
			{
				DpiX = DpiY = 96;
			}
			finally
			{
				if (g != null)
					g.Dispose();
			}

			_ScrollMode = ScrollModeEnum.Continuous;

			// Handle the controls
			_vScroll = new VScrollBar();
			_vScroll.Scroll += new ScrollEventHandler(this.VerticalScroll);
			_vScroll.Enabled = false;

			// tooltip 
			_vScrollToolTip = new ToolTip();
			_vScrollToolTip.AutomaticDelay = 100;	// .1 seconds
			_vScrollToolTip.AutoPopDelay = 1000;	// 1 second
			_vScrollToolTip.ReshowDelay = 100;		// .1 seconds
			_vScrollToolTip.InitialDelay = 10;		// .01 seconds
			_vScrollToolTip.ShowAlways = false;
			_vScrollToolTip.SetToolTip(_vScroll, "");

			_hScroll = new HScrollBar();
			_hScroll.Scroll += new ScrollEventHandler(this.HorizontalScroll);
			_hScroll.Enabled = false;

			_DrawPanel = new PageDrawing(null);
            _DrawPanel.Parent = this;
			_DrawPanel.Paint += new PaintEventHandler(this.DrawPanelPaint);
			_DrawPanel.Resize += new EventHandler(this.DrawPanelResize); 
			_DrawPanel.MouseWheel +=new MouseEventHandler(DrawPanelMouseWheel);
            _DrawPanel.KeyDown += new KeyEventHandler(DrawPanelKeyDown);

			_RunButton = new Button();
			_RunButton.Parent = this;
			_RunButton.Text = "Run Report";
			_RunButton.Width = 90;
			_RunButton.FlatStyle = FlatStyle.Flat;
			_RunButton.Click += new System.EventHandler(ParametersViewClick);

			_WarningButton = new PictureBox();
			_WarningButton.Parent = this;
			_WarningButton.Width = 15;
			_WarningButton.Height = 15;
			_WarningButton.Paint +=new PaintEventHandler(_WarningButton_Paint);
			_WarningButton.Click += new System.EventHandler(WarningClick);
			ToolTip tip = new ToolTip();
			tip.AutomaticDelay = 500;
			tip.ShowAlways = true;
			tip.SetToolTip(_WarningButton, "Click to see Report Warnings");

			_ParameterPanel = new ScrollableControl();

            _FindCtl = new RdlViewerFind();
            _FindCtl.Height = 27;
            _FindCtl.Parent = this;
            _FindCtl.Viewer = this;
            _FindCtl.Visible = false;

			this.Layout +=new LayoutEventHandler(RdlViewer_Layout);
			this.SuspendLayout();		 

			// Must be added in this order for DockStyle to work correctly
            this.Controls.Add(_FindCtl);
			this.Controls.Add(_DrawPanel);
			this.Controls.Add(_vScroll);
			this.Controls.Add(_hScroll);
			this.Controls.Add(_ParameterPanel);

			this.ResumeLayout(false);
		}
        public new bool Focus()
        {
            return (this._DrawPanel.Focus());
        } 

        /// <summary>
        /// When true printing will compensate for non-printable area of paper
        /// </summary>
        public bool UseTrueMargins
        {
            get { return _UseTrueMargins; }
            set { _UseTrueMargins = value; }
        }
        /// <summary>
        /// Show the Wait Dialog when retrieving and rendering report when true.
        /// </summary>
        public bool ShowWaitDialog
        {
            get { return _ShowWaitDialog; }
            set { _ShowWaitDialog = value; }
        }
		/// <summary>
		/// True if Parameter panel should be shown. 
		/// </summary>
		public bool ShowParameterPanel
		{
			get 
			{
				LoadPageIfNeeded();
				return _ShowParameters;
			}
			set 
			{
				_ShowParameters = value;
				RdlViewer_Layout(this, null);				// re layout based on new report
			}
		}
        /// <summary>
        /// True when find panel is visible
        /// </summary>
        public bool ShowFindPanel
        {
            get
            {
                return _FindCtl.Visible;
            }
            set
            {
                _FindCtl.Visible = value;
                RdlViewer_Layout(this, null);				// re layout based on new report
            }
        }
        /// <summary>
        /// Causes the find panel to find the next item
        /// </summary>
        public void FindNext()
        {
            _FindCtl.FindNext();
        }
        /// <summary>
        /// The color to use when highlighting the current found item
        /// </summary>
        public Color HighlightItemColor
        {
            get { return _DrawPanel.HighlightItemColor; }
            set { _DrawPanel.HighlightItemColor = value; }
        }
        /// <summary>
        /// The color to use when highlighting all
        /// </summary>
        public Color HighlightAllColor
        {
            get { return _DrawPanel.HighlightAllColor; }
            set { _DrawPanel.HighlightAllColor = value; }
        }

        /// <summary>
        /// The text to highlight when either HighLightAll is on or the HighLightItem is on.
        /// </summary>
        public string HighlightText
        {
            get { return _HighlightText; }
            set 
            {
                _HighlightText = value;
                _DrawPanel.Invalidate();    // force redraw
            }
        }

        /// <summary>
        /// When HighlightText has a value; HighlightAll controls whether
        /// all page items with that text will be highlighted
        /// </summary>
        public bool HighlightAll
        {
            get { return _HighlightAll; }
            set
            {
                _HighlightAll = value;
                if (_HighlightText != null && _HighlightText.Length > 0)
                    _DrawPanel.Invalidate();    // force redraw when need to
            }
        }

        /// <summary>
        /// When HighlightText has a value; HighlightCaseSensitive controls whether
        /// the comparison is case sensitive.
        /// </summary>
        public bool HighlightCaseSensitive
        {
            get { return _HighlightCaseSensitive; }
            set
            {
                _HighlightCaseSensitive = value;
                if (_HighlightText != null && _HighlightText.Length > 0)
                    _DrawPanel.Invalidate();    // force redraw when need to
            }
        }
        /// <summary>
        /// When used with HighlightText; HighlightPageItem will only highlight the selected item.
        /// </summary>
        public PageItem HighlightPageItem
        {
            get { return _HighlightItem; }
            set 
            { 
                _HighlightItem = value;
                _DrawPanel.Invalidate();    // force redraw
            }
        }
/// <summary>
/// Returns the number of pages in the report.  0 is returned if no report has been loaded.
/// </summary>
		public int PageCount
		{
			get 
			{
				LoadPageIfNeeded();
				if (_pgs == null) 
					return 0;
				else
					return _pgs.PageCount;
			}
		}

		/// <summary>
		/// Sets/Returns the page currently showing
		/// </summary>
		public int PageCurrent
		{
			get 
			{
                if (_pgs == null)
                    return 0;
				int pc = (int) (_pgs.PageCount * (long) _vScroll.Value / (double) _vScroll.Maximum)+1; 
				if (pc > _pgs.PageCount)
					pc = _pgs.PageCount;
				return pc;
			}
			set 
			{
                if (_pgs == null)
                    return;
				// Contributed by Henrique (h2a) 07/14/2006
				if(value <= _pgs.PageCount && value >= 1) 
				{ 
//					_vScroll.Value = (int)((double)_vScroll.Maximum / _pgs.PageCount * (value -1)); 

                    double scrollValue = ((double)_vScroll.Maximum * (value - 1)) / _pgs.PageCount;
                    _vScroll.Value = (int) Math.Round( scrollValue);

					string tt = string.Format("Page {0} of {1}", 
						(int) (_pgs.PageCount * (long) _vScroll.Value / (double) _vScroll.Maximum)+1, 
						_pgs.PageCount); 

					_vScrollToolTip.SetToolTip(_vScroll, tt); 

					_DrawPanel.Invalidate(); 
				}
				else
					throw new ArgumentOutOfRangeException("PageCurrent", value, String.Format("Value must be between 1 and {0}.", _pgs.PageCount));
			}
		}

		/// <summary>
		/// Gets the report definition.
		/// </summary>
		public Report Report
		{
			get 
			{
				LoadPageIfNeeded();
				return _Report; 
			}
		}

		/// <summary>
		/// Forces the report to get rebuilt especially after changing parameters or data.
		/// </summary>
        public void Rebuild()
        {
            // Aulofee customization - start. Code added (2 lines) to avoid to execute twice GetPages and so the SQL query (custo end). 
            if (_pgs == null)
            {
                LoadPageIfNeeded();

                if (_Report == null)
                    throw new Exception("Report must be loaded prior to Rebuild being called.");
                // Aulofee customization - start. Code added (2 lines) to avoid to execute twice GetPages and so the SQL query (custo end). 
            }
            else
                _pgs = GetPages(this._Report);
            _DrawPanel.Pgs = _pgs;
            _vScroll.Value = 0;
            CalcZoom();
            _DrawPanel.Invalidate();
        }

        /// <summary>
		/// Gets/Sets the ScrollMode.  
		///		SinglePage: Shows a single page shows in pane.
		///		Continuous: Shows pages as a continuous vertical column.
		///		Facing: Shows first page on right side of pane, then alternating
		///				with single page scrolling.
		///		ContinuousFacing: Shows 1st page on right side of pane, then alternating 
		///				with continuous scrolling.
		/// </summary>
		public ScrollModeEnum ScrollMode
		{
			get { return _ScrollMode; }
			set 
			{ 
				_ScrollMode = value; 
				CalcZoom(); 
				this._DrawPanel.Invalidate();
			}
		}
        /// <summary>
        /// Enables/Disables the selection tool.  The selection tool allows the user
        /// to select text and images on the display and copy it to the clipboard.
        /// </summary>
        public bool SelectTool
        {
            get { return _DrawPanel.SelectTool; }
            set { _DrawPanel.SelectTool = value; }
        }
        /// <summary>
        /// Returns true when one or more PageItems are selected.
        /// </summary>
        public bool CanCopy
        {
            get { return _DrawPanel.CanCopy; }
        }
        /// <summary>
        /// Copies the current selection (if any) to the clipboard.
        /// </summary>
        public void Copy()
        {
            if (!CanCopy)
                return;

            Image im = _DrawPanel.SelectImage;
            if (im == null)
                Clipboard.SetDataObject(SelectText, true);
            else
            {
                Clipboard.SetImage(im);
                im.Dispose();
            }
        }
        /// <summary>
        /// The contents of the selected text.  Tab separate items on same y coordinate;
        /// newline separate items when y coordinate changes.   Order is based on user
        /// selection order.
        /// </summary>
        public string SelectText
        {
            get
            {
                return _DrawPanel.SelectText;
            }
        }

        /// <summary>
		/// Holds a file name that contains the RDL (Report Specification Language).  Setting
		/// this field will cause a new report to be loaded into the viewer.
		/// SourceFile is mutually exclusive with SourceRdl.  Setting SourceFile will nullify SourceRdl.
		/// </summary>
		public string SourceFile
		{
			get 
			{
				return _SourceFileName;
			}
			set 
			{
				_SourceFileName=value;
                if (value != null)
				    _SourceRdl = null;
				_vScroll.Value = _hScroll.Value = 0;
				_pgs = null;				// reset pages, only if SourceRdl is also unavailable
				_DrawPanel.Pgs = null;
				_loadFailed=false;			// attempt to load the report
				if (this.Visible)
				{
					LoadPageIfNeeded();			// force load of report
					this._DrawPanel.Invalidate();
				}
			}
		}

		/// <summary>
		/// Holds the XML source of the report in RDL (Report Specification Language).
		/// SourceRdl is mutually exclusive with SourceFile.  Setting SourceRdl will nullify SourceFile.
		/// </summary>
		public string SourceRdl
		{
			get {return _SourceRdl;}
			set 
			{
				_SourceRdl=value;
                if (value != null)
				    _SourceFileName=null;
				_pgs = null;				// reset pages
				_DrawPanel.Pgs = null;
				_loadFailed=false;			// attempt to load the report	
				_vScroll.Value = _hScroll.Value = 0;
				if (this.Visible)
				{
					LoadPageIfNeeded();			// force load of report
					this._DrawPanel.Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Holds the folder to data source reference files when SourceFileName not available.
		/// </summary>
		public string Folder
		{
			get {return _Folder;}
			set {_Folder = value;}
		}

		/// <summary>
		/// Parameters passed to report when run.  Parameters are separated by '&'.  For example,
		/// OrderID=10023&OrderDate=10/14/2002
		/// Note: these parameters will override the user specified ones.
		/// </summary>
		public string Parameters
		{
			get {return _Parameters;}
			set {_Parameters=value;}
		}

		/// <summary>
		/// The height of the report page (in points) as defined within the report.
		/// </summary>
		public float PageHeight
		{
			get 
			{
				LoadPageIfNeeded();
				return _PageHeight;
			}
		}

		/// <summary>
		/// The width of the report page (in points) as defined within the report.
		/// </summary>
		public float PageWidth
		{
			get 
			{
				LoadPageIfNeeded();
				return _PageWidth;
			}
		}

		/// <summary>
		/// Description of the report.
		/// </summary>
		public string ReportDescription
		{
			get 
			{
				LoadPageIfNeeded();
				return _ReportDescription;
			}
		}

		/// <summary>
		/// Author of the report.
		/// </summary>
		public string ReportAuthor
		{
			get 
			{
				LoadPageIfNeeded();
				return _ReportAuthor;
			}
		}

		/// <summary>
		/// Name of the report.
		/// </summary>
		public string ReportName
		{
			get 
			{
				return _ReportName;
			}
			set {_ReportName = value;}
		}

		/// <summary>
		/// Zoom factor.  For example, .5 is a 50% reduction, 2 is 200% increase.
		/// Setting this value will force ZoomMode to UseZoom.
		/// </summary>
		public float Zoom
		{
			get {return _zoom;}
			set 
			{
				_zoom = value;
				this._zoomMode = ZoomEnum.UseZoom;
				CalcZoom();			// this adjust any scrolling issues
				this._DrawPanel.Invalidate();
			}
		}

		/// <summary>
		/// ZoomMode.  Optionally, allows zoom to dynamically change depending on pane size.
		/// </summary>
		public ZoomEnum ZoomMode
		{
			get {return _zoomMode; }
			set 
			{
				_zoomMode = value; 
				CalcZoom();				// force zoom calculation
				this._DrawPanel.Invalidate();
			}
		}

		/// <summary>
		/// Print the report.
		/// </summary>
		public void Print(PrintDocument pd)
		{
			LoadPageIfNeeded();

			pd.PrintPage += new PrintPageEventHandler(PrintPage);
			printCurrentPage=-1;
			switch (pd.PrinterSettings.PrintRange)
			{
				case PrintRange.AllPages:
					printCurrentPage = 0;
					printEndPage = _pgs.PageCount - 1;
					break;
				case PrintRange.Selection:
					printCurrentPage = pd.PrinterSettings.FromPage - 1;
					printEndPage = pd.PrinterSettings.FromPage - 1;
					break;
				case PrintRange.SomePages:
					printCurrentPage = pd.PrinterSettings.FromPage - 1;
					if (printCurrentPage < 0)
						printCurrentPage = 0;
					printEndPage = pd.PrinterSettings.ToPage - 1;
					if (printEndPage >= _pgs.PageCount)
						printEndPage = _pgs.PageCount - 1;
					break;
			}
			pd.Print();
		}

		private void PrintPage(object sender, PrintPageEventArgs e)
		{
			System.Drawing.Rectangle r = new System.Drawing.Rectangle(0, 0, int.MaxValue, int.MaxValue);
            // account for the non-printable area of the paper
            PointF pageOffset;
            if (this.UseTrueMargins && this._Report != null)
            {
                // The page offset is set in pixels as the Draw method changes the graphics object to use pixels
                // (the origin transform does not get changed by the change in units.  PrintableArea returns
                // numbers in the hundredths of an inch.

                float x = ((e.PageSettings.PrintableArea.X * e.Graphics.DpiX) / 100.0F) - e.Graphics.Transform.OffsetX;
                float y = ((e.PageSettings.PrintableArea.Y * e.Graphics.DpiY) / 100.0F) - e.Graphics.Transform.OffsetY;
                
                // Get the margins in printer pixels (don't use the function!)
                // Points to pixels conversion ((double)x * DpiX / POINTSIZEF)
                float lm = (float)((double)_Report.LeftMarginPoints * e.Graphics.DpiX / POINTSIZEF);
                float tm = (float)((double)_Report.TopMarginPoints * e.Graphics.DpiY / POINTSIZEF);
                // Correct based on the report margin
                if (x > lm)      // left margin is less than the minimum left margin
                    x = 0;
                if (y > tm)      // top margin is less than the minimum top margin
                    y = 0;
                pageOffset = new PointF(-x, -y);
            }
            else
            {
                pageOffset = PointF.Empty;
            }

            _DrawPanel.Draw(e.Graphics, printCurrentPage, r, false, pageOffset);	 

			printCurrentPage++;
			if (printCurrentPage > printEndPage)
				e.HasMorePages = false;
			else
				e.HasMorePages = true;
		}

		/// <summary>
		/// Save the file.  The extension determines the type of file to save.
		/// </summary>
		/// <param name="FileName">Name of the file to be saved to.</param>
		/// <param name="ext">Type of file to save.  Should be "pdf", "xml", "html", "mhtml", "csv", "rtf", "excel", "tif".</param>
		public void SaveAs(string FileName, string type)
		{
			LoadPageIfNeeded();

			string ext = type.ToLower();
			OneFileStreamGen sg = new OneFileStreamGen(FileName, true);	// overwrite with this name
            if (!(ext == "pdf" || ext == "tif" || ext == "tiff" || ext == "tifbw"))
            {
                ListDictionary ld = GetParameters();		// split parms into dictionary
                _Report.RunGetData(ld);                     // obtain the data (again)
            }
			try
			{
				switch(ext)
				{
					case "pdf":	
						_Report.RunRenderPdf(sg, _pgs);
						break;
                    case "tif":
                    case "tiff":
                        _Report.RunRenderTif(sg, _pgs, true);
                        break;
                    case "tifbw":
                        _Report.RunRenderTif(sg, _pgs, false);
                        break;
					case "csv":
						_Report.RunRender(sg, OutputPresentationType.CSV);
						break;
                    case "rtf":
                        _Report.RunRender(sg, OutputPresentationType.RTF);
                        break;
                    case "excel":
                    case "xlsx":
                        _Report.RunRender(sg, OutputPresentationType.Excel);
                        break;
                    case "xml":
                        _Report.RunRender(sg, OutputPresentationType.XML);
                        break;
                    case "html":
                    case "htm":
						_Report.RunRender(sg, OutputPresentationType.HTML);
						break;
					case "mhtml": case "mht":
						_Report.RunRender(sg, OutputPresentationType.MHTML);
						break;
					default:
						throw new Exception("Unsupported file extension for SaveAs");
				}
			}
			finally
			{
				if (sg != null)
				{
					sg.CloseMainStream();
				}

			}
			return;
		}
        
        /// <summary>
        /// Finds the first instance of the search string.
        /// </summary>
        /// <param name="search"></param>
        /// <returns>null if not found</returns>
        public PageItem Find(string search)
        {
            return Find(search, null, RdlViewerFinds.None);
        }

        /// <summary>
        /// Find locates the next string after the passed location.  Use ScrollToPageItem to then
        /// reposition the Viewer on that item
        /// </summary>
        /// <param name="search">Text to search for</param>
        /// <param name="position">PageItem after which to start search.  null starts at beginning</param>
        /// <param name="options">Multiple options can be or'ed together.</param>
        /// <returns>null if not found</returns>
		public PageItem Find(string search, PageItem position, RdlViewerFinds options)
		{
            LoadPageIfNeeded();

            if (_pgs == null || _pgs.Count == 0)       // no report nothing to find
                return null;

            // initialize the loop direction and starting point
            int increment;
            int sPage;
            int sItem;
            if (((options & RdlViewerFinds.Backward) == RdlViewerFinds.Backward))
            {   // set to backward direction
                increment = -1;                 // go backwards
                sPage = _pgs.PageCount - 1;     // start at last page
                sItem = _pgs[sPage].Count - 1;  // start at bottom of last page
            }
            else
            {   // set to forward direction
                increment = 1;
                sPage = 0;
                sItem = 0;
            }

            bool bFirst = true;
            if (position != null)
            {
                sPage = position.Page.PageNumber - 1;   // start on same page as current
                sItem = position.ItemNumber + increment;  //   but on the item after/before the current one
            }

            if (!((options & RdlViewerFinds.MatchCase) == RdlViewerFinds.MatchCase))
                search = search.ToLower();          // should use Culture!!! todo

            PageItem found = null;
            for (int pi = sPage; pi < _pgs.Count && found == null && pi >= 0; pi = pi + increment)
            {
                Page p = _pgs[pi];
                if (bFirst)         // The first time sItem is already set
                    bFirst = false;
                else
                {
                    if (increment < 0)  // we're going backwards?
                        sItem = p.Count - 1;    // yes, start at bottom of page
                    else
                        sItem = 0;              // no, start at top of page
                }
                for (int pii = sItem; pii < p.Count && found == null && pii >= 0; pii = pii + increment)
                {
                    PageText pt = p[pii] as PageText;
                    if (pt == null)
                        continue;

                    if ((options & RdlViewerFinds.MatchCase) == RdlViewerFinds.MatchCase)
                    {
                        if (pt.Text.Contains(search))
                            found = pt;
                    }
                    else
                    {
                        if (pt.Text.ToLower().Contains(search))
                            found = pt;
                    }
                }
            }

            return found;
        }
        
        public void ScrollToPageItem(PageItem pi)
        {
            LoadPageIfNeeded();
            if (_pgs == null || _pgs.PageCount <= 0)    // nothing to scroll to
                return;

            int sPage = 0;
            int sItem = 0;
            int itemVerticalOffset = 0;
            int itemHorzOffset = 0;
            int height = 0;
            int width = 0;
            if (pi != null)
            {
                sPage = pi.Page.PageNumber-1;
                sItem = pi.ItemNumber;
                RectangleF rect = new RectangleF(PixelsX(pi.X + _leftMargin),
                    PixelsY(pi.Y),
                    PixelsX(pi.W),
                    PixelsY(pi.H));
                itemVerticalOffset = (int) (rect.Top);
                itemHorzOffset = (int)rect.Left;
                width = (int)rect.Width;
                height = (int) (rect.Height);
            }

            // set the vertical scroll
            int scroll = (int)((double)_vScroll.Maximum * sPage / _pgs.PageCount) + itemVerticalOffset;
            
            // do we need to scroll vertically?
            if (!(_vScroll.Value <= scroll && _vScroll.Value + _DrawPanel.Height/this.Zoom >= scroll + height))
            {   // item isn't on visible part of window; force scroll
                _vScroll.Value = Math.Min(scroll, Math.Max(0,_vScroll.Maximum - _DrawPanel.Height));
                SetScrollControlsV();
                ScrollEventArgs sa = new ScrollEventArgs(ScrollEventType.ThumbPosition, _vScroll.Maximum + 1); // position is intentionally wrong
                VerticalScroll(_vScroll, sa);
            }

            // set the horizontal scroll
            scroll = itemHorzOffset;

            // do we need to scroll horizontally?
            if (!(_hScroll.Value <= scroll && _hScroll.Value + _DrawPanel.Width / this.Zoom >= scroll + width))
            {   // item isn't on visible part of window; force scroll
                _hScroll.Value = Math.Min(scroll, Math.Max(0, _hScroll.Maximum-_DrawPanel.Width));
                SetScrollControlsH();
                ScrollEventArgs sa = new ScrollEventArgs(ScrollEventType.ThumbPosition, _hScroll.Maximum + 1); // position is intentionally wrong
                HorizontalScroll(_hScroll, sa);
            }
        }

		private void DrawPanelPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Only handle one paint at a time
			lock (this)
			{
				if (_InPaint)
					return;
				_InPaint=true;
			}

			Graphics g = e.Graphics;
			try			// never want to die in here
			{
				if (!_InLoading)				// If we're in the process of loading don't paint
				{
					LoadPageIfNeeded();				// make sure we have something to show
				
					if (_zoom < 0)
						CalcZoom();				// new report or resize client requires new zoom factor
				
					// Draw the page
					_DrawPanel.Draw(g,_zoom, _leftMargin, _pageGap, 
						PointsX(_hScroll.Value), PointsY(_vScroll.Value),	
						e.ClipRectangle, 
                        _HighlightItem, _HighlightText, _HighlightCaseSensitive, _HighlightAll);
				}
			}
			catch (Exception ex)
			{	// don't want to kill process if we die
				using (Font font = new Font("Arial", 8))	
					g.DrawString(ex.Message+"\r\n"+ex.StackTrace, font, Brushes.Black,0,0);
			}		
			
			lock (this)
			{
				_InPaint=false;
			}
		}

		private void DrawPanelResize(object sender, EventArgs e)
		{
			CalcZoom();							// calc zoom
			_DrawPanel.Refresh();
		}

        private float POINTSIZEF = 72.27f;

		private float PointsX(float x)		// pixels to points
		{
            return x * POINTSIZEF / DpiX;
		}

		private float PointsY(float y)
		{
            return y * POINTSIZEF / DpiY;
		}

		private int PixelsX(float x)		// points to pixels
		{
			int r = (int)((double)x * DpiX / POINTSIZEF);
            if (r == 0 && x > .0001f)
                r = 1;
            return r;
        }

		private int PixelsY(float y)
		{
			int r= (int) ((double)y * DpiY / POINTSIZEF);
            if (r == 0 && y > .0001f)
                r = 1;
            return r;
        }

		private void CalcZoom()
		{
			switch (_zoomMode)
			{
				case ZoomEnum.UseZoom:
					if (_zoom <= 0)			// normalize invalid values
						_zoom = 1;
					break;					// nothing to calculate
				case ZoomEnum.FitWidth:
					CalcZoomFitWidth();
					break;
				case ZoomEnum.FitPage:
					CalcZoomFitPage();
					break;
			}
			if (_zoom <= 0)
				_zoom = 1;
			float w = PointsX(_DrawPanel.Width);	// convert to points

			if (w > (this._PageWidth + _leftGap + _rightGap)*_zoom)
				_leftMargin = ((w -(this._PageWidth + _leftGap + _rightGap)*_zoom)/2)/_zoom;
			else
				_leftMargin = _leftGap;
			if (_leftMargin < 0)
				_leftMargin = 0;
			SetScrollControls();			// zoom affects the scroll bars
			return;
		}
		
		private void CalcZoomFitPage()
		{
			try
			{
				float w = PointsX(_DrawPanel.Width);	// convert to points
				float h = PointsY(_DrawPanel.Height);
				float xratio = w / (this._PageWidth + _leftGap + _rightGap);
				float yratio = h / (this._PageHeight + this._pageGap + this._pageGap);	
				_zoom = Math.Min(xratio, yratio);
			}
			catch
			{
				_zoom = 1;			// shouldn't ever happen but this routine must never throw exception
			}
		}

		private void CalcZoomFitWidth()
		{
			try
			{
				float w = PointsX(_DrawPanel.Width);	// convert to points
				float h = PointsY(_DrawPanel.Height);
				_zoom = w / (this._PageWidth + _leftGap + _rightGap);

			}
			catch
			{
				_zoom = 1;			// shouldn't ever happen but this routine must never throw exception
			}
		}

		// Obtain the Pages by running the report
		private Report GetReport()
		{
			string prog;

			// Obtain the source
			if (_loadFailed)	
				prog = GetReportErrorMsg();
			else if (_SourceRdl != null)
				prog = _SourceRdl;
			else if (_SourceFileName != null)
				prog = GetRdlSource();
			else	
				prog = GetReportEmptyMsg();

			// Compile the report
			// Now parse the file
			RDLParser rdlp;
			Report r;
			try
			{
				_errorMsgs = null;
				rdlp =  new RDLParser(prog);
				rdlp.DataSourceReferencePassword = GetDataSourceReferencePassword;
				if (_SourceFileName != null)
					rdlp.Folder = Path.GetDirectoryName(_SourceFileName);
				else
					rdlp.Folder = this.Folder;

				r = rdlp.Parse();
				if (r.ErrorMaxSeverity > 0) 
				{
					_errorMsgs = r.ErrorItems;		// keep a copy of the errors

					int severity = r.ErrorMaxSeverity;
					r.ErrorReset();
					if (severity > 4)
					{
						r = null;			// don't return when severe errors
						_loadFailed=true;
					}
				}
				// If we've loaded the report; we should tell it where it got loaded from
				if (r != null && !_loadFailed)
				{	// Don't care much if this fails; and don't want to null out report if it does
					try 
					{
						if (_SourceFileName != null)
						{
							r.Name = Path.GetFileNameWithoutExtension(_SourceFileName);
							r.Folder = Path.GetDirectoryName(_SourceFileName);
						}
						else
						{
							r.Folder = this.Folder;
							r.Name = this.ReportName;
						}
					}
					catch {}
				}
			}
			catch (Exception ex)
			{
				_loadFailed=true;
				_errorMsgs = new List<string>();		// create new error list
				_errorMsgs.Add(ex.Message);			// put the message in it
				_errorMsgs.Add(ex.StackTrace);		//   and the stack trace
				r = null;
			}

			if (r != null)
			{
				_PageWidth = r.PageWidthPoints;
				_PageHeight = r.PageHeightPoints;
				_ReportDescription = r.Description;
				_ReportAuthor = r.Author;
                r.SubreportDataRetrieval += new EventHandler<SubreportDataRetrievalEventArgs>(r_SubreportDataRetrieval);
				ParametersBuild(r);
			}
			else
			{
				_PageWidth = 0;
				_PageHeight = 0;
				_ReportDescription = null;
				_ReportAuthor = null;
				_ReportName = null;
			}
			return r;
		}

        void r_SubreportDataRetrieval(object sender, SubreportDataRetrievalEventArgs e)
        {
            if (this.SubreportDataRetrieval != null)
                SubreportDataRetrieval(this, e);
        }

		private string GetReportEmptyMsg()
		{
			string prog = "<Report><Width>8.5in</Width><Body><Height>1in</Height><ReportItems><Textbox><Value></Value><Style><FontWeight>Bold</FontWeight></Style><Height>.3in</Height><Width>5 in</Width></Textbox></ReportItems></Body></Report>";
			return prog;
		}

		private string GetReportErrorMsg()
		{
			string data1 = @"<?xml version='1.0' encoding='UTF-8'?>
<Report> 
	<LeftMargin>.4in</LeftMargin><Width>8.5in</Width>
	<Author></Author>
	<DataSources>
		<DataSource Name='DS1'>
			<ConnectionProperties> 
				<DataProvider>xxx</DataProvider>
				<ConnectString></ConnectString>
			</ConnectionProperties>
		</DataSource>
	</DataSources>
	<DataSets>
		<DataSet Name='Data'>
			<Query>
				<DataSourceName>DS1</DataSourceName>
			</Query>
			<Fields>
				<Field Name='Error'> 
					<DataField>Error</DataField>
					<TypeName>String</TypeName>
				</Field>
			</Fields>";
			
			string data2 = @"
		</DataSet>
	</DataSets>
	<PageHeader>
		<Height>1 in</Height>
		<ReportItems>
			<Textbox><Top>.1in</Top><Value>fyiReporting Software, LLC</Value><Style><FontSize>18pt</FontSize><FontWeight>Bold</FontWeight></Style></Textbox>
			<Textbox><Top>.1in</Top><Left>4.25in</Left><Value>=Globals!ExecutionTime</Value><Style><Format>dddd, MMMM dd, yyyy hh:mm:ss tt</Format><FontSize>12pt</FontSize><FontWeight>Bold</FontWeight></Style></Textbox>
			<Textbox><Top>.5in</Top><Value>Errors processing report</Value><Style><FontSize>12pt</FontSize><FontWeight>Bold</FontWeight></Style></Textbox>
		</ReportItems>
	</PageHeader>
	<Body><Height>3 in</Height>
		<ReportItems>
			<Table>
				<Style><BorderStyle>Solid</BorderStyle></Style>
				<TableColumns>
					<TableColumn><Width>7 in</Width></TableColumn>
				</TableColumns>
				<Header>
					<TableRows>
						<TableRow>
							<Height>15 pt</Height>
							<TableCells>
								<TableCell>
									<ReportItems><Textbox><Value>Messages</Value><Style><FontWeight>Bold</FontWeight></Style></Textbox></ReportItems>
								</TableCell>
							</TableCells>
						</TableRow>
					</TableRows>
					<RepeatOnNewPage>true</RepeatOnNewPage>
				</Header>
				<Details>
					<TableRows>
						<TableRow>
							<Height>12 pt</Height>
							<TableCells>
								<TableCell>
									<ReportItems><Textbox Name='ErrorMsg'><Value>=Fields!Error.Value</Value><CanGrow>true</CanGrow></Textbox></ReportItems>
								</TableCell>
							</TableCells>
						</TableRow>
					</TableRows>
				</Details>
			</Table>
		</ReportItems>
	</Body>
</Report>";

			StringBuilder sb = new StringBuilder(data1, data1.Length + data2.Length + 1000);
			// Build out the error messages
			sb.Append("<Rows>");
			foreach (string msg in _errorMsgs)
			{
				sb.Append("<Row><Error>");
				string newmsg = msg.Replace("&", @"&amp;");
				newmsg = newmsg.Replace("<", @"&lt;");
				sb.Append(newmsg);
				sb.Append("</Error></Row>");
			}
			sb.Append("</Rows>");
			sb.Append(data2);
			return sb.ToString();
		}

		private Pages GetPages()
		{
			this._Report = GetReport();
			if (_loadFailed)			// retry on failure; this will get error report
				this._Report = GetReport();

			return GetPages(this._Report);
		}

		private Pages GetPages(Report report)
		{
			Pages pgs=null;

			ListDictionary ld = GetParameters();		// split parms into dictionary

			try
			{
				report.RunGetData(ld);

				pgs = report.BuildPages();

				if (report.ErrorMaxSeverity > 0) 
				{
					if (_errorMsgs == null)
						_errorMsgs = report.ErrorItems;		// keep a copy of the errors
					else
					{
						foreach (string err in report.ErrorItems)
							_errorMsgs.Add(err);
					}

					report.ErrorReset();
				}

			}
			catch (Exception e)
			{
				string msg = e.Message;
			}
			
			return pgs;
		}

		private ListDictionary GetParameters()
		{
			ListDictionary ld= new ListDictionary();
			if (_Parameters == null)
				return ld;				// dictionary will be empty in this case

			// parms are separated by &
			char[] breakChars = new char[] {'&'};
            string parm = _Parameters.Replace("&amp;", '\ufffe'.ToString());    // handle &amp; as user wanted '&'
            string[] ps = parm.Split(breakChars);
			foreach (string p in ps)
			{
				int iEq = p.IndexOf("=");
				if (iEq > 0)
				{
					string name = p.Substring(0, iEq);
					string val = p.Substring(iEq+1);
                    ld.Add(name, val.Replace('\ufffe', '&'));
				}
			}
			return ld;
		}

		private string GetRdlSource()
		{
			StreamReader fs=null;
			string prog=null;
			try
			{
				fs = new StreamReader(_SourceFileName);
				prog = fs.ReadToEnd();
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}

			return prog;
		}

        // 15052008 AJM - Updating Render notification window - This could be improved to show current action in the future
        private void showWait()
        {
            DialogWait wait = new DialogWait(this);
            wait.ShowDialog();
        }

		/// <summary>
		/// Call LoadPageIfNeeded when a routine requires the report to be loaded in order
		/// to fulfill the request.
		/// </summary>
		private void LoadPageIfNeeded()
		{
			if (_pgs == null)
			{
				Cursor savec=null;
                System.Threading.Thread t=null;
				try
				{
                    // 15052008 AJM - Updating Render notification window - This could be improved to show current action in the future
                    if (_ShowWaitDialog)
                    {
                        t = new System.Threading.Thread(new System.Threading.ThreadStart(showWait));
                        t.Start();
                    }
					_InLoading = true;
                    savec = this.Cursor;				// this could take a while so put up wait cursor
					this.Cursor = Cursors.WaitCursor;
					_pgs = GetPages();
					_DrawPanel.Pgs = _pgs;
					CalcZoom();							// this could affect zoom
				}
				finally
				{
					_InLoading = false;
					if (savec != null)
						this.Cursor = savec;
                    if (t != null)
                    {
                        int i = 0;
                        while ((t.ThreadState & System.Threading.ThreadState.AbortRequested) != System.Threading.ThreadState.AbortRequested &&
                            (t.ThreadState & System.Threading.ThreadState.Aborted) != System.Threading.ThreadState.Aborted &&
                            (t.ThreadState & System.Threading.ThreadState.Stopped) != System.Threading.ThreadState.Stopped &&
                            (t.ThreadState & System.Threading.ThreadState.StopRequested) != System.Threading.ThreadState.StopRequested)
                        {
                            try
                            {
                                t.Abort();
                            }
                            catch //(Exception e) PJR don't declare variable as we aren't using it anyway.
                            {
                            }
                            i++;
                        }
                    }
                    
				}
				RdlViewer_Layout(this, null);				// re layout based on new report
			}
		}

		private void ParametersBuild(Report r)
		{
			// Remove all previous controls
			_ParameterPanel.Controls.Clear();
			_ParameterPanel.AutoScroll = true;

			int yPos=10;
			foreach (UserReportParameter rp in r.UserReportParameters)
			{
				if (rp.Prompt == null || rp.Prompt.Length == 0)		// skip parameters that don't have a prompt
					continue;

				// Create a label
				Label label = new Label();
				label.Parent = _ParameterPanel;
				label.AutoSize = true;
				label.Text = rp.Prompt;
				label.Location = new Point(10, yPos);

				// Create a control
				Control v;
				int width = 90;
				if (rp.DisplayValues == null)
				{
					TextBox tb = new TextBox();
					v = tb;
					tb.Height = tb.PreferredHeight;
					tb.Validated += new System.EventHandler(ParametersTextValidated);
				}
				else
				{
					ComboBox cb = new ComboBox();
					// create a label to auto
					Label l = new Label();
					l.AutoSize = true;
					l.Visible = false;

					cb.Leave += new EventHandler(ParametersLeave);
					v = cb;
					width = 0;
					foreach (string s in rp.DisplayValues)
					{
						l.Text = s;
						if (width < l.Width)
							width = l.Width;
						cb.Items.Add(s);
					}																	   
					if (width > 0)
					{						   
						l.Text = "XX";
						width += l.Width;		// give some extra room for the drop down arrow
					}
					else
						width = 90;				// just force the default
				}
				v.Parent = _ParameterPanel;
				v.Width = width;
				v.Location = new Point(label.Location.X+label.Width+5, yPos);
				if (rp.DefaultValue != null)
				{
					StringBuilder sb = new StringBuilder();
					for (int i=0; i < rp.DefaultValue.Length; i++)
					{
						if (i > 0)
							sb.Append(", ");
						sb.Append(rp.DefaultValue[i].ToString());
					}
					v.Text = sb.ToString();
				}
				v.Tag = rp;
								 
				yPos += Math.Max(label.Height, v.Height) + 5;
			}

			this._ParametersMaxHeight = yPos;
		}

		private void ParametersLeave(object sender, EventArgs e)
		{
			ComboBox cb = sender as ComboBox;
			if (cb == null)
				return;

			UserReportParameter rp = cb.Tag as UserReportParameter;
			if (rp == null)
				return;

			try
			{
				rp.Value = cb.Text;			
			}
			catch (ArgumentException ae)
			{
				MessageBox.Show(ae.Message, "Invalid Report Parameter");
			}
		}

		private void ParametersTextValidated(object sender, System.EventArgs e)
		{
			TextBox tb = sender as TextBox;
			if (tb == null)
				return;

			UserReportParameter rp = tb.Tag as UserReportParameter;
			if (rp == null)
				return;

			try
			{
				rp.Value = tb.Text;			
			}
			catch (ArgumentException ae)
			{
				MessageBox.Show(ae.Message, "Invalid Report Parameter");
			}
		}

		private void ParametersViewClick(object sender, System.EventArgs e)
		{
            Cursor.Current = Cursors.WaitCursor; 
            System.Threading.Thread t = null;
            try
            {
                _RunButton.Enabled = false;
                _errorMsgs = null;			// reset the error message
                if (this._Report == null)
                    return;

                // Force parameters to get built
                foreach (Control ctl in _ParameterPanel.Controls)
                {
                    if (ctl.Tag is UserReportParameter)
                    {
                        if (ctl is TextBox)
                            this.ParametersTextValidated(ctl, new EventArgs());
                        else if (ctl is ComboBox)
                            this.ParametersLeave(ctl, new EventArgs());
                    }
                }

                bool bFail = false;
                foreach (UserReportParameter rp in _Report.UserReportParameters)
                {
                    if (rp.Prompt == null)
                        continue;
                    if (rp.Value == null && !rp.Nullable)
                    {
                        MessageBox.Show(string.Format("Parameter '{0}' is required but not provided.", rp.Prompt), "Report Parameter Missing");
                        bFail = true;
                    }
                }
                if (bFail)
                    return;

                if (_ShowWaitDialog)
                {
                    t = new System.Threading.Thread(new System.Threading.ThreadStart(showWait));
                    t.Start();
                }
                _pgs = GetPages(this._Report);
                _DrawPanel.Pgs = _pgs;
                _vScroll.Value = 0;
                CalcZoom();
                _WarningButton.Visible = WarningVisible();
                _DrawPanel.Invalidate();
            }
            catch
            {
                // don't fail out;  occasionally get thread abort exception
            }
            finally
            {
                _RunButton.Enabled = true;
                Cursor.Current = Cursors.Default;
                if (t != null)
                {
                    int i = 0;
                    while (t.ThreadState != System.Threading.ThreadState.AbortRequested && t.ThreadState != System.Threading.ThreadState.Aborted && t.ThreadState != System.Threading.ThreadState.Stopped && t.ThreadState != System.Threading.ThreadState.StopRequested)
                    { t.Abort(); 
                        i++; }
                }
            }

		}
		
		private void WarningClick(object sender, System.EventArgs e)
		{
			if (_errorMsgs == null)
				return;						// shouldn't even be visible if no warnings

			DialogMessages dm = new DialogMessages(_errorMsgs);
			dm.ShowDialog();
			return;
		}

		private void SetScrollControls()
		{
			if (_pgs == null)		// nothing loaded; nothing to do
			{
				_vScroll.Enabled = _hScroll.Enabled = false;
				_vScroll.Value = _hScroll.Value = 0;
				return;
			}
			SetScrollControlsV();
			SetScrollControlsH();
		}

		private void SetScrollControlsV()
		{
			// calculate the vertical scroll needed
			float h = PointsY(_DrawPanel.Height);	// height of pane
			if (_zoom * ((this._PageHeight + this._pageGap) * _pgs.PageCount + this._pageGap) <= h)
			{
				_vScroll.Enabled = false;
				_vScroll.Value = 0;
				return;
			}
			_vScroll.Minimum = 0;
			_vScroll.Maximum = (int) (PixelsY((this._PageHeight + this._pageGap) * _pgs.PageCount + this._pageGap));
			_vScroll.Value = Math.Min(_vScroll.Value, _vScroll.Maximum);
			if (this._zoomMode == ZoomEnum.FitPage)
			{
				_vScroll.LargeChange = (int) (_vScroll.Maximum / _pgs.PageCount);
				_vScroll.SmallChange = _vScroll.LargeChange;
			}
			else
			{
				_vScroll.LargeChange = (int) (Math.Max(_DrawPanel.Height,0) / _zoom);
				_vScroll.SmallChange = _vScroll.LargeChange / 5;
			}
			_vScroll.Enabled = true;
			string tt = string.Format("Page {0} of {1}", 
					(int) (_pgs.PageCount * (long) _vScroll.Value / (double) _vScroll.Maximum)+1, 
					_pgs.PageCount);

			_vScrollToolTip.SetToolTip(_vScroll, tt);
//			switch (_ScrollMode)
//			{
//				case ScrollModeEnum.SinglePage:
//					break;
//				case ScrollModeEnum.Continuous:
//				case ScrollModeEnum.ContinuousFacing:
//				case ScrollModeEnum.Facing:
//					break;
//			}
			return;
		}

		private void SetScrollControlsH()
			{
			// calculate the horizontal scroll needed
			float w = PointsX(_DrawPanel.Width);	// width of pane
			if (_zoomMode == ZoomEnum.FitPage || 
				_zoomMode == ZoomEnum.FitWidth ||
				_zoom * (this._PageWidth + this._leftGap + this._rightGap) <= w)
			{
				_hScroll.Enabled = false;
				_hScroll.Value = 0;
				return;
			}

			_hScroll.Minimum = 0;
			_hScroll.Maximum = (int) (PixelsX(this._PageWidth + this._leftGap + this._rightGap) );
			_hScroll.Value = Math.Min(_hScroll.Value, _hScroll.Maximum);
			_hScroll.LargeChange = (int) (Math.Max(_DrawPanel.Width,0) / _zoom);
			_hScroll.SmallChange = _hScroll.LargeChange / 5;
			_hScroll.Enabled = true;

			return;
		}

		private void HorizontalScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
            if (_hScroll.IsDisposed)
                return;

			if (e.NewValue == _hScroll.Value)	// don't need to scroll if already there
				return;

			_DrawPanel.Invalidate();   
		}

		private void VerticalScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
            if (_vScroll.IsDisposed)
                return;

			if (e.NewValue == _vScroll.Value)	// don't need to scroll if already there
				return;

			string tt = string.Format("Page {0} of {1}", 
				(int) (_pgs.PageCount * (long) _vScroll.Value / (double) _vScroll.Maximum)+1, 
				_pgs.PageCount);
			
			_vScrollToolTip.SetToolTip(_vScroll, tt);

			_DrawPanel.Invalidate();   
		}

		private void DrawPanelMouseWheel(object sender, MouseEventArgs e)
		{
			int wvalue;
            bool bCtrlOn = (Control.ModifierKeys & Keys.Control) == Keys.Control;

            if (bCtrlOn)
            {   // when ctrl key on and wheel rotated we zoom in or out
                float zoom = Zoom;

                if (e.Delta < 0)
                {
                    zoom -= .1f;
                    if (zoom < .1f)
                        zoom = .1f;
                }
                else
                {
                    zoom += .1f;
                    if (zoom > 10)
                        zoom = 10;
                }
                Zoom = zoom;
                _DrawPanel.Refresh();
                return;
            }

			if (e.Delta < 0)
			{
				if (_vScroll.Value < _vScroll.Maximum)
				{
					wvalue = _vScroll.Value + _vScroll.SmallChange;

					_vScroll.Value = (int) Math.Min(_vScroll.Maximum - (_DrawPanel.Height / _zoom), wvalue);
					_DrawPanel.Refresh();
				}
			}
			else 
			{
				if (_vScroll.Value > _vScroll.Minimum)
				{
					wvalue = _vScroll.Value - _vScroll.SmallChange;

					_vScroll.Value = Math.Max(_vScroll.Minimum, wvalue);
					_DrawPanel.Refresh();
				}
			}
		}

		private void DrawPanelKeyDown(object sender, KeyEventArgs e)
		{
			// Force scroll up and down
			if (e.KeyCode == Keys.Down)
			{
                if (!_vScroll.Enabled)
                    return;
                int wvalue = _vScroll.Value + _vScroll.SmallChange;

                _vScroll.Value = (int)Math.Min(_vScroll.Maximum - (_DrawPanel.Height / _zoom), wvalue);
				_DrawPanel.Refresh();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Up)
			{
                if (!_vScroll.Enabled)
                    return;
                _vScroll.Value = Math.Max(_vScroll.Value - _vScroll.SmallChange, 0);
				_DrawPanel.Refresh();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.PageDown)
			{
                if (!_vScroll.Enabled)
                    return;
                _vScroll.Value = Math.Min(_vScroll.Value + _vScroll.LargeChange,
                                        _vScroll.Maximum - _DrawPanel.Height);
				_DrawPanel.Refresh();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.PageUp)
			{
                if (!_vScroll.Enabled)
                    return;
                _vScroll.Value = Math.Max(_vScroll.Value - _vScroll.LargeChange, 0);
				_DrawPanel.Refresh();
				e.Handled = true;
			}
            else if (e.KeyCode == Keys.Home)
            {
                if (!_vScroll.Enabled)
                    return;
                _vScroll.Value = 0;
                _DrawPanel.Refresh();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.End)
            {
                if (!_vScroll.Enabled)
                    return;
                if (_pgs != null && _pgs.Count > 0)
                {
                    Page last = _pgs[_pgs.Count - 1];
                    if (last.Count > 0)
                    {
                        PageItem lastItem = last[last.Count - 1];
                        this.ScrollToPageItem(lastItem);
                        e.Handled = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (!_hScroll.Enabled)
                    return;
                if (e.Control)
                    _hScroll.Value = 0;
                else
                    _hScroll.Value = Math.Max(_hScroll.Value - _hScroll.SmallChange, 0);
                _DrawPanel.Refresh();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (!_hScroll.Enabled)
                    return;
                if (e.Control)
                    _hScroll.Value = _hScroll.Maximum - _DrawPanel.Width;
                else
                    _hScroll.Value = Math.Min(_hScroll.Value + _hScroll.SmallChange,
                                                _hScroll.Maximum - _DrawPanel.Width);
                _DrawPanel.Refresh();
                e.Handled = true;
            }

		}

		private bool WarningVisible()
		{
			if (!_ShowParameters)
				return false;

			return _errorMsgs != null;
		}

		private void RdlViewer_Layout(object sender, LayoutEventArgs e)
		{
			int fHeight = _FindCtl.Visible? _FindCtl.Height: 0;
            int pHeight;
			if (_ShowParameters)
			{	// Only the parameter panel is visible
				_ParameterPanel.Visible = true;
				_RunButton.Visible = true;
				
				_WarningButton.Visible = WarningVisible();

				_ParameterPanel.Location = new Point(0,0);
				_ParameterPanel.Width = this.Width - _RunButton.Width - _WarningButton.Width - 5;
				pHeight = this.Height / 3;
				if (pHeight > _ParametersMaxHeight)
					pHeight = _ParametersMaxHeight;
				if (pHeight < _RunButton.Height + 15)
					pHeight = _RunButton.Height + 15;
				_ParameterPanel.Height = pHeight;
			}
			else
			{
//				pHeight=_RunButton.Height + 15;
				pHeight=0;
				_RunButton.Visible = false;
				_WarningButton.Visible = false;
				_ParameterPanel.Visible = false;
			}
			_DrawPanel.Location = new Point(0, pHeight);
			_DrawPanel.Width = this.Width - _vScroll.Width;
			_DrawPanel.Height = this.Height - _hScroll.Height - pHeight - fHeight;
			_hScroll.Location = new Point(0, this.Height - _hScroll.Height - fHeight);
			_hScroll.Width = _DrawPanel.Width;
			_vScroll.Location = new Point(this.Width - _vScroll.Width, _DrawPanel.Location.Y);
			_vScroll.Height = _DrawPanel.Height;

            if (_FindCtl.Visible)
            {
                _FindCtl.Location = new Point(0, this.Height - _FindCtl.Height);
                _FindCtl.Width = this.Width;
                _FindCtl.BringToFront();
            }

			_RunButton.Location = new Point(this.Width - _RunButton.Width - 2 - _WarningButton.Width, 10);
			_WarningButton.Location = new Point(_RunButton.Location.X + _RunButton.Width + 2, 13);
		}

		private void _WarningButton_Paint(object sender, PaintEventArgs e)
		{
			int midPoint = _WarningButton.Width / 2;
			Graphics g = e.Graphics;
			
			Point[] triangle = new Point[5];
			triangle[0] = triangle[4] = new Point(midPoint-1, 0);
			triangle[1] = new Point(0, _WarningButton.Height-1);
			triangle[2] = new Point(_WarningButton.Width, _WarningButton.Height-1);
			triangle[3] = new Point(midPoint+1, 0);
			g.FillPolygon(Brushes.Yellow, triangle);
			g.DrawPolygon(Pens.Black, triangle);
			g.FillRectangle(Brushes.Red, midPoint - 1, 5, 2, 5);
			g.FillRectangle(Brushes.Red, midPoint - 1, 11, 2, 2);
		}

        internal void InvokeHyperlink(HyperlinkEventArgs hlea)
        {
            if (Hyperlink != null)
                Hyperlink(this, hlea);
        }
    }

    public enum RdlViewerFinds
    {
        None=0,
        MatchCase=1,
        Backward=2
    }

	public enum ScrollModeEnum
	{
		SinglePage,
		Continuous,
		Facing,
		ContinuousFacing
	}

	public enum ZoomEnum
	{
		UseZoom,
		FitPage,
		FitWidth
	}
/// <summary>
/// HyperlinkEventArgs passed when a report item with a hyperlink defined is clicked on
/// </summary>
    public class HyperlinkEventArgs : System.ComponentModel.CancelEventArgs
    {
        string _Hyperlink;      // Hyperlink text
        public HyperlinkEventArgs(string hyperlink)
            : base()
        {
            _Hyperlink = hyperlink;
        }

        public string Hyperlink
        {
            get { return _Hyperlink; }
        }
    }

}
