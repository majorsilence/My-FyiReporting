/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml;
using System.IO;
using RdlReader.Resources;
using Majorsilence.Reporting.Rdl;
using Majorsilence.Reporting.RdlViewer;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Majorsilence.Reporting.RdlReader
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// RdlReader is a application for displaying reports based on RDL.
    /// </summary>
    public partial class RdlReader : IMessageFilter
    {
        static readonly string optFileName = Path.Combine(Majorsilence.Reporting.Rdl.Utility.Paths.MajorsilenceRoamingFolder(), "readerstate.xml");
        SortedList _RecentFiles = null;

        /// <summary>
        /// Uri, Parameter
        /// </summary>
        Dictionary<Uri, String> _CurrentFiles = null;			// temporary variable for current files
        private Rdl.NeedPassword _GetPassword;
        private string _DataSourceReferencePassword = null;
        private bool bMono;

        public RdlReader(bool mono)
        {
            this.DoubleBuffered = true;

            bMono = mono;
            GetStartupState();

            InitializeComponent();

            BuildMenus();
            // CustomReportItem load 
            RdlEngineConfig.GetCustomReportTypes();

            Application.AddMessageFilter(this);

            this.Closing += new System.ComponentModel.CancelEventHandler(this.RdlReader_Closing);
            _GetPassword = new Rdl.NeedPassword(this.GetPassword);

            this.Load += RdlReader_Load;
        }

        public async void RdlReader_Load(object sender, EventArgs e) 
        { 
            // open up the current files if any
            if (_CurrentFiles != null)
            {
                foreach (var dict in _CurrentFiles)
                {
                    MDIChild mc = new MDIChild(this.ClientRectangle.Width * 3 / 4, this.ClientRectangle.Height * 3 / 4);
                    mc.MdiParent = this;
                    mc.Viewer.GetDataSourceReferencePassword = _GetPassword;

                    await mc.SetSourceFile(dict.Key);
                    if (dict.Value != string.Empty)
                    {
                        mc.Parameters = dict.Value;
                    }

                    mc.Text = dict.Key.LocalPath;

                    if (_CurrentFiles.Count == 1)
                    {
                        mc.WindowState = FormWindowState.Maximized;
                    }

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

        #endregion


        /// <summary>
        /// Uri, Parameters
        /// </summary>
        private static Dictionary<Uri, String> _startUpFiles;

        private const string SET_DEFAULT_PRINTER = "Default";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bMono = false;
            string[] args = Environment.GetCommandLineArgs();

            string reportFile = string.Empty;
            string parameters = string.Empty;
            string printerName = string.Empty;

            for (int i = 0; i < args.Length; i++)
            {
                string argValue = args[i];
                if (argValue.ToLower() == "/m" || argValue.ToLower() == "-m")
                {
                    // user want to run with mono simplifications
                    bMono = true;
                }
                else if (argValue == "-r")
                {
                    reportFile = args[i + 1];
                    if (System.IO.Path.GetDirectoryName(reportFile) == string.Empty)
                    {
                        // Try to find the file in the current working directory
                        reportFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), System.IO.Path.GetFileName(reportFile));
                    }
                }
                else if (argValue == "-p")
                {
                    parameters = args[i + 1];
                }
                // if we have nothing or key after -print then we have to set printer by default
                else if (argValue == "-print")
                {
                    if (args.Length > i + 1 && !args[i + 1].StartsWith("-"))
                    {
                        printerName = args[i + 1];
                    }
                    else
                    {
                        printerName = SET_DEFAULT_PRINTER;
                    }
                }
            }

            if (reportFile == string.Empty)
            {
                // keep backwards compatiablity from when it worked with only a filename being passed in

                if (args.Length > 1)
                {
                    if (args[1].Length >= 5)
                    {
                        reportFile = args[1];
                        if (System.IO.Path.GetDirectoryName(reportFile) == string.Empty)
                        {
                            // Try to find the file in the current working directory
                            reportFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), System.IO.Path.GetFileName(reportFile));
                        }
                    }
                }
            }

            if (reportFile != string.Empty)
            {
                if (File.Exists(reportFile))
                {
                    if (!string.IsNullOrWhiteSpace(printerName))
                    {
                        // HACK: async
                        Task.Run(async ()=> await SilentPrint(reportFile, parameters, printerName)).GetAwaiter().GetResult();
                        return;
                    }

                    _startUpFiles = new Dictionary<Uri, string>();
                    _startUpFiles.Add(new Uri(reportFile), parameters);
                }
                else
                {
                    MessageBox.Show(string.Format(Strings.RdlReader_ShowD_ReportNotLoaded, reportFile), Strings.RdlReader_Show_MyFyiReporting, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (bMono == false)
            {
                Application.EnableVisualStyles();
                Application.DoEvents();				// when Mono this goes into a loop
            }

            Application.Run(new RdlReader(bMono));
        }

        public static async Task SilentPrint(string reportPath, string parameters, string printerName = null)
        {
            var rdlViewer = new Majorsilence.Reporting.RdlViewer.RdlViewer();
            rdlViewer.Visible = false;
            await rdlViewer.SetSourceFile(new Uri(reportPath));
            rdlViewer.Parameters = parameters;
            await rdlViewer.Rebuild();

            var pd = new PrintDocument();
            pd.DocumentName = rdlViewer.SourceFile.LocalPath;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = rdlViewer.PageCount;
            pd.PrinterSettings.MaximumPage = rdlViewer.PageCount;
            pd.PrinterSettings.MinimumPage = 1;
            pd.DefaultPageSettings.Landscape = rdlViewer.PageWidth > rdlViewer.PageHeight;
            pd.PrintController = new StandardPrintController();
            // convert pt to hundredths of an inch.
            pd.DefaultPageSettings.PaperSize = new PaperSize(
                "",
                (int)((rdlViewer.PageWidth / 72.27) * 100),
                (int)((rdlViewer.PageHeight / 72.27) * 100));

            if (!string.IsNullOrWhiteSpace(printerName) && printerName != SET_DEFAULT_PRINTER)
            {
                pd.DefaultPageSettings.PrinterSettings.PrinterName = printerName;
            }

            try
            {
                if (pd.PrinterSettings.PrintRange == PrintRange.Selection)
                {
                    pd.PrinterSettings.FromPage = rdlViewer.PageCurrent;
                }

                await rdlViewer.Print(pd);
            }
            catch (Exception ex)
            {
#if !DEBUG
                const string rdlreaderlog = "RdlReaderLog.txt";
                if (!File.Exists(rdlreaderlog))
                {
                    File.Create(rdlreaderlog).Dispose();
                }

                File.AppendAllLines(
                    rdlreaderlog,
                    new[] { string.Format("[{0}] {1}", DateTime.Now.ToString("dd.MM.yyyy H:mm:ss"), ex.Message) });
#endif
                Debug.WriteLine(Strings.RdlReader_ShowC_PrintError + ex.Message);
            }
        }

        private void BuildMenus()
        {
            // FILE MENU

            ToolStripMenuItem menuRecentItem = new ToolStripMenuItem(string.Empty);
            recentFilesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuRecentItem });
            fileToolStripMenuItem.DropDownOpening += new EventHandler(menuFile_Popup);

            // Intialize the recent file menu
            RecentFilesMenu();
            // Edit menu
            editToolStripMenuItem.DropDownOpening += new EventHandler(this.menuEdit_Popup);

            // VIEW MENU


            pageLayoutToolStripMenuItem.DropDownOpening += new EventHandler(this.menuPL_Popup);
            viewToolStripMenuItem.DropDownOpening += new EventHandler(this.menuView_Popup);

            // Add the Window menu
            windowToolStripMenuItem.DropDownOpening += new EventHandler(this.menuWnd_Popup);

            // MAIN

            IsMdiContainer = true;

        }

        private void menuFile_Popup(object sender, EventArgs e)
        {
            // These menus require an MDIChild in order to work
            bool bEnable = this.MdiChildren.Length > 0 ? true : false;
            closeToolStripMenuItem.Enabled = bEnable;
            saveAsToolStripMenuItem.Enabled = bEnable;
            printToolStripMenuItem.Enabled = bEnable;

            // Recent File is enabled when there exists some files 
            recentFilesToolStripMenuItem.Enabled = this._RecentFiles.Count <= 0 ? false : true;
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

        private async void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = Strings.RdlReader_menuFileOpen_Click_Filter;
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    await CreateMDIChild(new Uri(file), false);
                }
                RecentFilesMenu();
            }
        }

        private async void menuRecentItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            Uri file = new Uri(m.Text.Substring(2));

            await CreateMDIChild(file, true);
        }

        // Create an MDI child.   Only creates it if not already open
        private async Task CreateMDIChild(Uri file, bool bMenuUpdate)
        {
            MDIChild mcOpen = null;
            if (file != null)
            {
                foreach (MDIChild mc in this.MdiChildren)
                {
                    if (file == mc.SourceFile)
                    {							// we found it
                        mcOpen = mc;
                        break;
                    }
                }
            }
            if (mcOpen == null)
            {
                MDIChild mc = new MDIChild(this.ClientRectangle.Width * 3 / 4, this.ClientRectangle.Height * 3 / 4);
                mc.MdiParent = this;
                mc.Viewer.GetDataSourceReferencePassword = _GetPassword;
                await mc.SetSourceFile(file);
                mc.Text = file == null ? string.Empty : file.LocalPath;
                NoteRecentFiles(file, bMenuUpdate);
                mc.Show();
            }
            else
                mcOpen.Activate();
        }

        private async void menuFilePrint_Click(object sender, EventArgs e)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null)
                return;
            if (printChild != null)			// already printing
            {
                MessageBox.Show(Strings.RdlReader_ShowC_PrintOneFile);
                return;
            }

            printChild = mc;

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = mc.SourceFile.LocalPath;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = mc.Viewer.PageCount;
            pd.PrinterSettings.MaximumPage = mc.Viewer.PageCount;
            pd.PrinterSettings.MinimumPage = 1;
            if (mc.Viewer.PageWidth > mc.Viewer.PageHeight)
                pd.DefaultPageSettings.Landscape = true;
            else
                pd.DefaultPageSettings.Landscape = false;

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
                    await mc.Viewer.Print(pd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Strings.RdlReader_ShowC_PrintError + ex.Message);
                }
            }
            printChild = null;
        }

        private async void menuFileSaveAs_Click(object sender, EventArgs e)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Strings.RdlReader_menuFileSaveAs_Click_FilesFilter;
            sfd.FilterIndex = 1;

            Uri file = mc.SourceFile;

            if (file != null)
            {
                int index = file.LocalPath.LastIndexOf('.');
                if (index > 1)
                    sfd.FileName = file.LocalPath.Substring(0, index) + ".pdf";
                else
                    sfd.FileName = "*.pdf";

            }
            else
                sfd.FileName = "*.pdf";

            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;

            // save the report in a rendered format 
            string ext = null;
            int i = sfd.FileName.LastIndexOf('.');
            if (i < 1)
                ext = string.Empty;
            else
                ext = sfd.FileName.Substring(i + 1).ToLower();

            OutputPresentationType type = OutputPresentationType.Internal;
            switch (ext)
            {
                case "pdf":
                    type = OutputPresentationType.PDF;
                    break;
                case "xml":
                    type = OutputPresentationType.XML;
                    break;
                case "html":
                case "htm":
                    type = OutputPresentationType.HTML;
                    break;
                case "csv":
                    type = OutputPresentationType.CSV;
                    break;
                case "rtf":
                    type = OutputPresentationType.RTF;
                    break;
                case "mht":
                case "mhtml":
                    type = OutputPresentationType.MHTML;
                    break;
                case "xlsx":
                    type = sfd.FilterIndex == 7 ? OutputPresentationType.ExcelTableOnly : OutputPresentationType.Excel2007;
                    break;
                case "tif":
                case "tiff":
                    type = OutputPresentationType.TIF;
                    break;
                default:
                    MessageBox.Show(this,
                        String.Format(Strings.RdlReader_SaveG_NotValidFileType, sfd.FileName), Strings.RdlReader_SaveG_SaveAsError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            if (type == OutputPresentationType.TIF)
            {
                DialogResult dr = MessageBox.Show(this, Strings.RdlReader_ShowF_WantSaveColorsInTIF, Strings.RdlReader_ShowF_Export, MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.No)
                    type = OutputPresentationType.TIFBW;
                else if (dr == DialogResult.Cancel)
                    return;
            }

            if (type != OutputPresentationType.Internal)
            {
                try { await mc.Viewer.SaveAs(sfd.FileName, type); }
                catch (Exception ex)
                {
                    MessageBox.Show(this,
                        ex.Message, Strings.RdlReader_SaveG_SaveAsError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private async void menuFind_Click(object sender, System.EventArgs ea)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null)
                return;

            if (!mc.Viewer.ShowFindPanel)
                mc.Viewer.ShowFindPanel = true;
            await mc.Viewer.FindNext();
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
            copyToolStripMenuItem.Enabled = bEnable;
            findToolStripMenuItem.Enabled = bEnable;
            selectionToolToolStripMenuItem.Enabled = bEnable;
            if (!bEnable)
                return;

            MDIChild mc = this.ActiveMdiChild as MDIChild;
            copyToolStripMenuItem.Enabled = mc.Viewer.CanCopy;
            selectionToolToolStripMenuItem.Checked = mc.Viewer.SelectTool;
        }
        private void menuView_Popup(object sender, EventArgs e)
        {
            // These menus require an MDIChild in order to work
            bool bEnable = this.MdiChildren.Length > 0 ? true : false;
            zoomToToolStripMenuItem.Enabled = bEnable;
            actualSizeToolStripMenuItem.Enabled = bEnable;
            fitPageToolStripMenuItem.Enabled = bEnable;
            fitWidthToolStripMenuItem.Enabled = bEnable;
            pageLayoutToolStripMenuItem.Enabled = bEnable;
            if (!bEnable)
                return;

            // Now handle checking the correct sizing menu
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            actualSizeToolStripMenuItem.Checked = fitPageToolStripMenuItem.Checked = fitWidthToolStripMenuItem.Checked = false;

            if (mc.Viewer.ZoomMode == ZoomEnum.FitWidth)
                fitWidthToolStripMenuItem.Checked = true;
            else if (mc.Viewer.ZoomMode == ZoomEnum.FitPage)
                fitPageToolStripMenuItem.Checked = true;
            else if (mc.Viewer.Zoom == 1)
                actualSizeToolStripMenuItem.Checked = true;

        }

        private void menuPL_Popup(object sender, EventArgs e)
        {
            MDIChild mc = this.ActiveMdiChild as MDIChild;
            if (mc == null)
                return;

            singlePageToolStripMenuItem.Checked = continuousToolStripMenuItem.Checked =
                    facingToolStripMenuItem.Checked = continuousFacingToolStripMenuItem.Checked = false; ;

            switch (mc.Viewer.ScrollMode)
            {
                case ScrollModeEnum.Continuous:
                    continuousToolStripMenuItem.Checked = true;
                    break;
                case ScrollModeEnum.ContinuousFacing:
                    continuousFacingToolStripMenuItem.Checked = true;
                    break;
                case ScrollModeEnum.Facing:
                    facingToolStripMenuItem.Checked = true;
                    break;
                case ScrollModeEnum.SinglePage:
                    singlePageToolStripMenuItem.Checked = true;
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
            bool bEnable = this.MdiChildren.Length > 0 ? true : false;

            cascadeToolStripMenuItem.Enabled = bEnable;
            tileToolStripMenuItem.Enabled = bEnable;
            closeAllToolStripMenuItem.Enabled = bEnable;
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

        private void NoteRecentFiles(Uri name, bool bResetMenu)
        {
            if (name == null)
            {
                return;
            }

            if (_RecentFiles.ContainsValue(name.LocalPath))
            {	// need to move it to top of list; so remove old one
                int loc = _RecentFiles.IndexOfValue(name.LocalPath);
                _RecentFiles.RemoveAt(loc);
            }
            if (_RecentFiles.Count >= 5)
            {
                _RecentFiles.RemoveAt(0);	// remove the first entry
            }
            _RecentFiles.Add(DateTime.Now, name.LocalPath);
            if (bResetMenu)
                RecentFilesMenu();
            return;
        }

        private void RecentFilesMenu()
        {

            recentFilesToolStripMenuItem.DropDownItems.Clear();
            int mi = 1;
            for (int i = _RecentFiles.Count - 1; i >= 0; i--)
            {
                string menuText = string.Format("&{0} {1}", mi++, (string)(_RecentFiles.GetValueList()[i]));
                ToolStripMenuItem m = new ToolStripMenuItem(menuText);
                m.Click += new EventHandler(this.menuRecentItem_Click);
                recentFilesToolStripMenuItem.DropDownItems.Add(m);
            }
        }

        private void GetStartupState()
        {
            _RecentFiles = new SortedList();
            _CurrentFiles = new Dictionary<Uri, string>();

            if (_startUpFiles != null)
            {
                foreach (var dict in _startUpFiles)
                {
                    _CurrentFiles.Add(dict.Key, dict.Value);
                }
            }

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.PreserveWhitespace = false;
                xDoc.Load(optFileName);
                XmlNode xNode;
                xNode = xDoc.SelectSingleNode("//readerstate");

                // Loop thru all the child nodes
                foreach (XmlNode xNodeLoop in xNode.ChildNodes)
                {
                    switch (xNodeLoop.Name)
                    {
                        case "RecentFiles":
                            DateTime now = DateTime.Now;
                            now = now.Subtract(new TimeSpan(0, 1, 0, 0, 0));	// subtract an hour
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
                            if (_startUpFiles != null)
                                break;                          // not add if startUpFiles exists                        
                            foreach (XmlNode xN in xNodeLoop.ChildNodes)
                            {
                                string file = xN.InnerText.Trim();
                                if (File.Exists(file))			// only add it if it exists
                                {
                                    if (_CurrentFiles.ContainsKey(new Uri(file)) == false)
                                    {
                                        _CurrentFiles.Add(new Uri(file), string.Empty);
                                    }
                                }
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
                    Uri file = mc.SourceFile;
                    if (file == null)
                        continue;
                    xN = xDoc.CreateElement("file");
                    xN.InnerText = file.LocalPath;
                    xFiles.AppendChild(xN);
                }

                // Loop thru recent files list
                xFiles = xDoc.CreateElement("RecentFiles");
                xDS.AppendChild(xFiles);
                foreach (string f in _RecentFiles.Values)
                {
                    xN = xDoc.CreateElement("file");
                    xN.InnerText = f;
                    xFiles.AppendChild(xN);
                }

                Directory.CreateDirectory(Path.GetDirectoryName(optFileName)); //Create directory if not exist
                xDoc.Save(optFileName);
            }
            catch { }		// still want to leave even on error

            return;
        }
    }
}
