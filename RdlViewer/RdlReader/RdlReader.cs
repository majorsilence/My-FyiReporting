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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml;
using System.IO;
using fyiReporting.RDL;
using fyiReporting.RdlViewer;
using System.Runtime.InteropServices;

namespace fyiReporting.RdlReader
{
	/// <summary>
	/// RdlReader is a application for displaying reports based on RDL.
	/// </summary>
    public class RdlReader : System.Windows.Forms.Form, IMessageFilter
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private MDIChild printChild=null;
		SortedList _RecentFiles=null;
		ArrayList _CurrentFiles=null;			// temporary variable for current files
		private RDL.NeedPassword _GetPassword;
		private string _DataSourceReferencePassword=null;
		private bool bMono;

		// Menu declarations
		MenuItem menuFSep1;
		MenuItem menuFSep2;
		MenuItem menuFSep3;
		MenuItem menuOpen;
		MenuItem menuClose;
		MenuItem menuSaveAs;
		MenuItem menuPrint;
		MenuItem menuRecentFile;
		MenuItem menuExit;
		MenuItem menuFile;
		MenuItem menuPLZoomTo;
		MenuItem menuPLActualSize;
		MenuItem menuPLFitPage;
		MenuItem menuPLFitWidth;
		MenuItem menuPLSinglePage;
		MenuItem menuPLContinuous;
		MenuItem menuPLFacing;
		MenuItem menuPLContinuousFacing;
		MenuItem menuPL;
		MenuItem menuView;
		MenuItem menuCascade;
		MenuItem menuTileH;
		MenuItem menuTileV;
		MenuItem menuTile;
		MenuItem menuCloseAll;
		MenuItem menuWindow;
        MenuItem menuFind;
        MenuItem menuSelection;
        MenuItem menuCopy;
        MenuItem menuEdit;
        MainMenu menuMain;	

		public RdlReader(bool mono)
		{
			bMono = mono;
			GetStartupState();
			BuildMenus();
			InitializeComponent();
            Application.AddMessageFilter(this);

			this.Closing += new System.ComponentModel.CancelEventHandler(this.RdlReader_Closing);
			_GetPassword = new RDL.NeedPassword(this.GetPassword);

			// open up the current files if any
			if (_CurrentFiles != null)
			{
				foreach (string file in _CurrentFiles)
				{
					MDIChild mc = new MDIChild(this.ClientRectangle.Width*3/4, this.ClientRectangle.Height*3/4);
					mc.MdiParent = this;
					mc.Viewer.GetDataSourceReferencePassword = _GetPassword;
					mc.SourceFile = file;
					mc.Text = file;
					mc.Show();
				}
				_CurrentFiles = null;		// don't need this any longer
			}

		}
        /// <summary>
        /// Handles mousewheel processing when window under mousewheel doesn't have focus
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool PreFilterMessage(ref Message m)
        {
#if MONO
            return false;
#else
            if (m.Msg == 0x20a)
            {
                // WM_MOUSEWHEEL, find the control at screen position m.LParam
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = WindowFromPoint(pos);
                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                {
                    SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }
            return false;
#endif
        }
#if MONO
#else
        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
#endif

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		string GetPassword()
		{
			if (_DataSourceReferencePassword != null)
				return _DataSourceReferencePassword;

			DataSourcePassword dlg = new DataSourcePassword();
			if (dlg.ShowDialog() == DialogResult.OK)
				_DataSourceReferencePassword = dlg.PassPhrase;

			return _DataSourceReferencePassword;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RdlReader));
			// 
			// RdlReader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 470);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RdlReader";
			this.Text = "fyiReporting Reader";

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			bool bMono;
			string[] args = Environment.GetCommandLineArgs();
			if (args.Length >= 2 &&
				(args[1].ToLower() == "/m" || args[1].ToLower() == "-m"))
			{	// user want to run with mono simplifications
				bMono = true;
			}
			else
			{
				bMono = false;
				Application.EnableVisualStyles();
				Application.DoEvents();				// when Mono this goes into a loop
			}
			Application.Run(new RdlReader(bMono));
		}

		private void BuildMenus()
		{
			// FILE MENU
			menuOpen = new MenuItem("&Open...", new EventHandler(this.menuFileOpen_Click), Shortcut.CtrlO);
			menuClose = new MenuItem("&Close", new EventHandler(this.menuFileClose_Click), Shortcut.CtrlW);
			menuFSep1 = new MenuItem("-");
			menuSaveAs = new MenuItem("&Save As...", new EventHandler(this.menuFileSaveAs_Click), Shortcut.CtrlS);
			menuPrint = new MenuItem("Print...", new EventHandler(this.menuFilePrint_Click), Shortcut.CtrlP);
			menuFSep2 = new MenuItem("-");
			MenuItem menuRecentItem = new MenuItem("");
			menuRecentFile = new MenuItem("Recent &Files");
			menuRecentFile.MenuItems.AddRange(new MenuItem[] { menuRecentItem });
			menuFSep3 = new MenuItem("-");
			menuExit = new MenuItem("E&xit", new EventHandler(this.menuFileExit_Click), Shortcut.CtrlQ);
			
			// Create file menu and add array of sub-menu items
			menuFile = new MenuItem("&File");
			menuFile.Popup +=new EventHandler(this.menuFile_Popup);
			menuFile.MenuItems.AddRange(
				new MenuItem[] { menuOpen, menuClose, menuFSep1, menuSaveAs, menuPrint, menuFSep2, menuRecentFile, menuFSep3, menuExit });

			// Intialize the recent file menu
			RecentFilesMenu();
            // Edit menu
            menuSelection = new MenuItem("&Selection Tool", new EventHandler(this.menuSelection_Click));

            menuCopy = new MenuItem("&Copy", new EventHandler(this.menuCopy_Click), Shortcut.CtrlC);
            menuFind = new MenuItem("&Find", new EventHandler(this.menuFind_Click), Shortcut.CtrlF);
            menuEdit = new MenuItem("&Edit");
            menuEdit.Popup += new EventHandler(this.menuEdit_Popup);
            menuEdit.MenuItems.AddRange(
                new MenuItem[] { menuSelection, new MenuItem("-"), menuCopy, new MenuItem("-"),menuFind });

			// VIEW MENU
			menuPLZoomTo =  new MenuItem("&Zoom To...", new EventHandler(this.menuPLZoomTo_Click));
			menuPLActualSize =  new MenuItem("Act&ual Size", new EventHandler(this.menuPLActualSize_Click));
			menuPLFitPage =  new MenuItem("Fit &Page", new EventHandler(this.menuPLFitPage_Click));
			menuPLFitWidth =  new MenuItem("Fit &Width", new EventHandler(this.menuPLFitWidth_Click));
			menuFSep1 = new MenuItem("-");
			menuPLSinglePage = new MenuItem("Single Page", new EventHandler(this.menuPLSinglePage_Click));
			menuPLContinuous = new MenuItem("Continuous", new EventHandler(this.menuPLContinuous_Click));
			menuPLFacing = new MenuItem("Facing", new EventHandler(this.menuPLFacing_Click));
			menuPLContinuousFacing = new MenuItem("Continuous Facing", new EventHandler(this.menuPLContinuousFacing_Click));

			menuPL = new MenuItem("Page La&yout");
			menuPL.Popup +=new EventHandler(this.menuPL_Popup);
			menuPL.MenuItems.AddRange(
				new MenuItem[] { menuPLSinglePage, menuPLContinuous, menuPLFacing, menuPLContinuousFacing });

			menuView = new MenuItem("&View");
			menuView.Popup +=new EventHandler(this.menuView_Popup);
			menuView.MenuItems.AddRange(
				new MenuItem[] { menuPLZoomTo, menuPLActualSize, menuPLFitPage, menuPLFitWidth, menuFSep1, menuPL });

			// WINDOW MENU
			menuCascade = new MenuItem("&Cascade", new EventHandler(this.menuWndCascade_Click), Shortcut.CtrlShiftJ);

			menuTileH  = new MenuItem("&Horizontally", new EventHandler(this.menuWndTileH_Click), Shortcut.CtrlShiftK);
			menuTileV  = new MenuItem("&Vertically", new EventHandler(this.menuWndTileV_Click), Shortcut.CtrlShiftL);
			menuTile = new MenuItem("&Tile");
			menuTile.MenuItems.AddRange(new MenuItem[] { menuTileH, menuTileV });

			menuCloseAll = new MenuItem("Close &All", new EventHandler(this.menuWndCloseAll_Click), Shortcut.CtrlShiftW);

			// Add the Window menu
			menuWindow = new MenuItem("&Window");
			menuWindow.Popup +=new EventHandler(this.menuWnd_Popup);
			menuWindow.MdiList = true;
			menuWindow.MenuItems.AddRange(new MenuItem[] { menuCascade, menuTile, menuCloseAll });

			// HELP MENU
			MenuItem menuAbout = new MenuItem("&About...", new EventHandler(this.menuHelpAbout_Click));
			MenuItem menuHelp = new MenuItem("&Help");
			menuHelp.MenuItems.AddRange(new MenuItem[] {menuAbout } );

			// MAIN
			menuMain = new MainMenu(new MenuItem[]{menuFile, menuEdit, menuView, menuWindow, menuHelp});	
			IsMdiContainer = true;
			this.Menu = menuMain;   
		}
 
		private void menuFile_Popup(object sender, EventArgs e)
		{
			// These menus require an MDIChild in order to work
			bool bEnable = this.MdiChildren.Length > 0? true: false;
			menuClose.Enabled = bEnable;
			menuSaveAs.Enabled = bEnable;
			menuPrint.Enabled = bEnable;

			// Recent File is enabled when there exists some files 
			menuRecentFile.Enabled = this._RecentFiles.Count <= 0? false: true;
		}

		private void menuFileClose_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Close();
		}

		private void menuFileExit_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		private void menuFileOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Report files (*.rdl)|*.rdl|" +
				"All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.CheckFileExists = true;
			ofd.Multiselect = true;
			if (ofd.ShowDialog(this) == DialogResult.OK)
			{
				foreach (string file in ofd.FileNames)
				{
					CreateMDIChild(file, false);
				}
				RecentFilesMenu();
			}
		}

		private void menuRecentItem_Click(object sender, System.EventArgs e)
		{
			MenuItem m = (MenuItem) sender;
			string file = m.Text.Substring(2);

			CreateMDIChild(file, true);
		}

		// Create an MDI child.   Only creates it if not already open
		private void CreateMDIChild(string file, bool bMenuUpdate)
		{
			MDIChild mcOpen=null;
			if (file != null)
			{
				file = file.Trim();

				foreach (MDIChild mc in this.MdiChildren)
				{
					if (file == mc.SourceFile.Trim())	
					{							// we found it
						mcOpen = mc;
						break;
					}
				}
			}
			if (mcOpen == null)
			{
				MDIChild mc = new MDIChild(this.ClientRectangle.Width*3/4, this.ClientRectangle.Height*3/4);
				mc.MdiParent = this;
				mc.Viewer.GetDataSourceReferencePassword = _GetPassword;
				mc.SourceFile = file;
				mc.Text = file;
				NoteRecentFiles(file, bMenuUpdate);
				mc.Show();
			}
			else
				mcOpen.Activate();
		}

		private void menuFilePrint_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc == null)
				return;
			if (printChild != null)			// already printing
			{
				MessageBox.Show("Can only print one file at a time.");
				return;
			}

			printChild = mc;

			PrintDocument pd = new PrintDocument();
			pd.DocumentName = mc.SourceFile;
			pd.PrinterSettings.FromPage = 1;
			pd.PrinterSettings.ToPage = mc.Viewer.PageCount;
			pd.PrinterSettings.MaximumPage = mc.Viewer.PageCount;
			pd.PrinterSettings.MinimumPage = 1;
			if (mc.Viewer.PageWidth > mc.Viewer.PageHeight)
				pd.DefaultPageSettings.Landscape=true;
			else
				pd.DefaultPageSettings.Landscape=false;

			PrintDialog dlg = new PrintDialog();
			dlg.Document = pd;
			dlg.AllowSelection = true;
			dlg.AllowSomePages = true;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if (pd.PrinterSettings.PrintRange == PrintRange.Selection)
					{
						pd.PrinterSettings.FromPage = mc.Viewer.PageCurrent;
					}
					mc.Viewer.Print(pd);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Print error: " + ex.Message);
				}
			}
			printChild = null;
		}

		private void menuFileSaveAs_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc == null)
				return;

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = 
				"PDF files (*.pdf)|*.pdf|" +
				"XML files (*.xml)|*.xml|" +
				"HTML files (*.html)|*.html|" +
                "CSV files (*.csv)|*.csv|" +
                "RTF files (*.rtf)|*.rtf|" +
                "TIF files (*.tif)|*.tif|" +
                "Excel files (*.xlsx)|*.xlsx|" +
                "MHT files (*.mht)|*.mht";
			sfd.FilterIndex = 1;

			string file = mc.SourceFile;

			if (file != null)
			{
				int index = file.LastIndexOf('.');
				if (index > 1)
					sfd.FileName = file.Substring(0, index) + ".pdf";
				else
					sfd.FileName = "*.pdf";

			}
			else
				sfd.FileName = "*.pdf";

			if (sfd.ShowDialog(this) != DialogResult.OK)
				return;

			// save the report in a rendered format 
			string ext=null;
			int i = sfd.FileName.LastIndexOf('.');
			if (i < 1)
				ext = "";
			else
				ext = sfd.FileName.Substring(i+1).ToLower();
			switch(ext)
			{
				case "pdf":	case "xml": case "html": case "htm": case "csv": case "rtf": 
                case "mht": case "mhtml": case "xlsx": case "tif": case "tiff":
                    if (ext == "tif" || ext == "tiff")
                    {
                        DialogResult dr = MessageBox.Show(this, "Do you want to save colors in TIF file?", "Export", MessageBoxButtons.YesNoCancel);
                        if (dr == DialogResult.No)
                            ext = "tifbw";
                        else if (dr == DialogResult.Cancel)
                            break;
                    }
                    try {mc.Viewer.SaveAs(sfd.FileName, ext);}
					catch (Exception ex)
					{
						MessageBox.Show(this, 
							ex.Message, "Save As Error", 
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					break;
				default:
					MessageBox.Show(this, 
						String.Format("{0} is not a valid file type.  File extension must be PDF, XML, HTML, CSV, MHT, RTF, TIF, XLSX.", sfd.FileName), "Save As Error", 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
			return;
		}

		private void menuHelpAbout_Click(object sender, System.EventArgs ea)
		{
			DialogAbout dlg = new DialogAbout();
			dlg.ShowDialog();
		}

        private void menuCopy_Click(object sender, System.EventArgs ea)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null || !mc.Viewer.CanCopy)
                return;

            mc.Viewer.Copy();
        }

        private void menuFind_Click(object sender, System.EventArgs ea)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null)
                return;

            if (!mc.Viewer.ShowFindPanel)
                mc.Viewer.ShowFindPanel = true;
            mc.Viewer.FindNext();
        }

        private void menuSelection_Click(object sender, System.EventArgs ea)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null)
                return;

            mc.Viewer.SelectTool = !mc.Viewer.SelectTool;
        }
        private void menuEdit_Popup(object sender, EventArgs e)
        {
            // These menus require an MDIChild in order to work
            bool bEnable = this.MdiChildren.Length > 0 ? true : false;
            menuCopy.Enabled = bEnable;
            menuFind.Enabled = bEnable;
            menuSelection.Enabled = bEnable;
            if (!bEnable)
                return;

            MDIChild mc = this.ActiveMdiChild as MDIChild;
            menuCopy.Enabled = mc.Viewer.CanCopy;
            menuSelection.Checked = mc.Viewer.SelectTool;
        } 
		private void menuView_Popup(object sender, EventArgs e)
		{
			// These menus require an MDIChild in order to work
			bool bEnable = this.MdiChildren.Length > 0? true: false;
			menuPLZoomTo.Enabled = bEnable;
			menuPLActualSize.Enabled = bEnable;
			menuPLFitPage.Enabled = bEnable;
			menuPLFitWidth.Enabled = bEnable;
			menuPL.Enabled = bEnable;
			if (!bEnable)
				return;

			// Now handle checking the correct sizing menu
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			menuPLActualSize.Checked = menuPLFitPage.Checked = menuPLFitWidth.Checked = false;

			if (mc.Viewer.ZoomMode == ZoomEnum.FitWidth)
				menuPLFitWidth.Checked = true;
			else if (mc.Viewer.ZoomMode == ZoomEnum.FitPage)
				menuPLFitPage.Checked = true;
			else if (mc.Viewer.Zoom == 1)
				menuPLActualSize.Checked = true;

		}

		private void menuPL_Popup(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc == null)
				return;

			menuPLSinglePage.Checked = menuPLContinuous.Checked = 
					menuPLFacing.Checked = menuPLContinuousFacing.Checked = false;;

			switch (mc.Viewer.ScrollMode)
			{
				case ScrollModeEnum.Continuous:
					menuPLContinuous.Checked = true;
					break;
				case ScrollModeEnum.ContinuousFacing:
					menuPLContinuousFacing.Checked = true;
					break;
				case ScrollModeEnum.Facing:
					menuPLFacing.Checked = true;
					break;
				case ScrollModeEnum.SinglePage:
					menuPLSinglePage.Checked = true;
					break;
			}
		}

		private void menuPLZoomTo_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc == null)				 
				return;

			ZoomTo dlg = new ZoomTo(mc.Viewer);
			dlg.StartPosition = FormStartPosition.CenterParent;
			dlg.ShowDialog();
		}

		private void menuPLActualSize_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.Zoom = 1;
		}

		private void menuPLFitPage_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.ZoomMode = ZoomEnum.FitPage;
		}

		private void menuPLFitWidth_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.ZoomMode = ZoomEnum.FitWidth;
		}

		private void menuPLSinglePage_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.ScrollMode = ScrollModeEnum.SinglePage;
		}

		private void menuPLContinuous_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.ScrollMode = ScrollModeEnum.Continuous;
		}

		private void menuPLFacing_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.ScrollMode = ScrollModeEnum.Facing;
		}

		private void menuPLContinuousFacing_Click(object sender, EventArgs e)
		{
			MDIChild mc = this.ActiveMdiChild as MDIChild;
			if (mc != null)
				mc.Viewer.ScrollMode = ScrollModeEnum.ContinuousFacing;
		}

		private void menuWnd_Popup(object sender, EventArgs e)
		{
			// These menus require an MDIChild in order to work
			bool bEnable = this.MdiChildren.Length > 0? true: false;

			menuCascade.Enabled = bEnable;
			menuTile.Enabled = bEnable;
			menuCloseAll.Enabled = bEnable;
		}

		private void menuWndCascade_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.Cascade);
		}

		private void menuWndCloseAll_Click(object sender, EventArgs e)
		{
			foreach (Form f in this.MdiChildren)
			{
				f.Close();
			}
		}

		private void menuWndTileH_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void menuWndTileV_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileVertical);
		}

		private void RdlReader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveStartupState();
		}
 
		private void NoteRecentFiles(string name, bool bResetMenu)
		{
			if (name == null)
				return;

			name = name.Trim();
			if (_RecentFiles.ContainsValue(name))
			{	// need to move it to top of list; so remove old one
				int loc = _RecentFiles.IndexOfValue(name);
				_RecentFiles.RemoveAt(loc);
			}
			if (_RecentFiles.Count >= 5)
			{
				_RecentFiles.RemoveAt(0);	// remove the first entry
			}
			_RecentFiles.Add(DateTime.Now, name);
			if (bResetMenu)
				RecentFilesMenu();
			return;
		}

		private void RecentFilesMenu()
		{
			menuRecentFile.MenuItems.Clear();
			int mi = 1;
			for (int i=_RecentFiles.Count-1; i >= 0; i--)
			{
				string menuText = string.Format("&{0} {1}", mi++, (string) (_RecentFiles.GetValueList()[i]));
				MenuItem m = new MenuItem(menuText);
				m.Click += new EventHandler(this.menuRecentItem_Click);
				menuRecentFile.MenuItems.Add(m);
			}
		}

		private void GetStartupState()
		{
			string optFileName = AppDomain.CurrentDomain.BaseDirectory + "readerstate.xml";
			_RecentFiles = new SortedList();
			_CurrentFiles = new ArrayList();
			
			try
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.PreserveWhitespace = false;
				xDoc.Load(optFileName);
				XmlNode xNode;
				xNode = xDoc.SelectSingleNode("//readerstate");

				// Loop thru all the child nodes
				foreach(XmlNode xNodeLoop in xNode.ChildNodes)
				{
					switch (xNodeLoop.Name)
					{
						case "RecentFiles":
							DateTime now = DateTime.Now;
							now = now.Subtract(new TimeSpan(0,1,0,0,0));	// subtract an hour
							foreach (XmlNode xN in xNodeLoop.ChildNodes)
							{
								string file = xN.InnerText.Trim();
								if (File.Exists(file))			// only add it if it exists
								{
									_RecentFiles.Add(now, file);
									now = now.AddSeconds(1);
								}
							}
							break;
						case "CurrentFiles":
							foreach (XmlNode xN in xNodeLoop.ChildNodes)
							{
								string file = xN.InnerText.Trim();
								if (File.Exists(file))			// only add it if it exists
									_CurrentFiles.Add(file);
							}
							break;
						default:
							break;
					}
				}
			}
			catch
			{		// Didn't sucessfully get the startup state but don't really care
			}

			return;
		}

		private void SaveStartupState()
		{
			try
			{
				XmlDocument xDoc = new XmlDocument();
				XmlProcessingInstruction xPI;
				xPI = xDoc.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
				xDoc.AppendChild(xPI);

				XmlNode xDS = xDoc.CreateElement("readerstate");
				xDoc.AppendChild(xDS);

				XmlNode xN;
				// Loop thru the current files
				XmlNode xFiles = xDoc.CreateElement("CurrentFiles");
				xDS.AppendChild(xFiles);
				foreach (MDIChild mc in this.MdiChildren)
				{
					string file = mc.SourceFile;
					if (file == null)
						continue;
					xN = xDoc.CreateElement("file");
					xN.InnerText = file;
					xFiles.AppendChild(xN);
				}

				// Loop thru recent files list
				xFiles = xDoc.CreateElement("RecentFiles");
				xDS.AppendChild(xFiles);
				foreach(string f in _RecentFiles.Values)
				{
					xN = xDoc.CreateElement("file");
					xN.InnerText = f;
					xFiles.AppendChild(xN);
				}

				string optFileName = AppDomain.CurrentDomain.BaseDirectory + "readerstate.xml";

				xDoc.Save(optFileName);
			}
			catch{}		// still want to leave even on error

			return ;
		}
	}
}
