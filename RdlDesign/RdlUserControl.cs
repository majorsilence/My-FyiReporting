using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fyiReporting.RdlViewer;
using fyiReporting.RDL;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Drawing.Printing;

namespace fyiReporting.RdlDesign
{
    public partial class RdlUserControl : UserControl
    {


        private RDL.NeedPassword _GetPassword;
        private bool bGotPassword = false;
        private string _DataSourceReferencePassword = null;
        public delegate void RdlChangeHandler(object sender, EventArgs e);

        private ToolStripButton ctlInsertCurrent = null;
        private ToolStripMenuItem ctlMenuInsertCurrent = null;
        bool bSuppressChange = false;
        private bool _ShowProperties = true;

        public event RdlDesign.RdlDesignerSavedFileEventHandler SavedFileEvent;

        public RdlUserControl()
        {
            InitializeComponent();


            _GetPassword = new RDL.NeedPassword(this.GetPassword);


            rdlEditPreview1.OnSelectionChanged += SelectionChanged;
            rdlEditPreview1.OnReportItemInserted += ReportItemInserted;
            rdlEditPreview1.OnOpenSubreport += OpenSubReportEvent;
            rdlEditPreview1.OnHeightChanged += HeightChanged;
            rdlEditPreview1.OnSelectionMoved += SelectionMoved;
            mainProperties.HidePropertiesClicked += delegate(object sender, EventArgs e)
            {
                ShowProperties(!_ShowProperties);
            };
        }

        public fyiReporting.RdlViewer.RdlViewer Viewer
        {
            get { return rdlEditPreview1.Viewer; }
        }

        internal void ResetPassword()
        {
            bGotPassword = false;
            _DataSourceReferencePassword = null;
        }

        Uri _SourceFile;
        /// <summary>
        /// The RDL file that should be displayed.
        /// </summary>
        public Uri SourceFile
        {
            get { return _SourceFile; }
            set
            {
                _SourceFile = value;
                string rdl = GetRdlSource();
                this.rdlEditPreview1.SetRdlText(rdl == null ? "" : rdl);
            }
        }

        private string GetRdlSource()
        {
            StreamReader fs = null;
            string prog = null;
            try
            {
   

                fs = new StreamReader(_SourceFile.LocalPath);
                prog = fs.ReadToEnd();
            }
            catch
            { }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return prog;
        }

        public string SourceRdl
        {
            get { return this.rdlEditPreview1.GetRdlText(); }
            set { this.rdlEditPreview1.SetRdlText(value); }
        }

        public string CurrentInsert
        {
            get { return rdlEditPreview1.CurrentInsert; }
            set
            {
                rdlEditPreview1.CurrentInsert = value;
            }
        }

        internal string GetPassword()
        {
            if (bGotPassword)
            {
                return _DataSourceReferencePassword;
            }

            using (DataSourcePassword dlg = new DataSourcePassword())
            {
                DialogResult rc = dlg.ShowDialog();
                bGotPassword = true;
                if (rc == DialogResult.OK)
                {
                    _DataSourceReferencePassword = dlg.PassPhrase;
                }

                return _DataSourceReferencePassword;
            }
        }

        internal RDL.NeedPassword SharedDatasetPassword
        {
            get { return _GetPassword; }
        }

        private void newToolStripButton1_Click(object sender, EventArgs e)
        {
            using (DialogDatabase dlgDB = new DialogDatabase(this))
            {
                dlgDB.StartPosition = FormStartPosition.CenterParent;
                dlgDB.FormBorderStyle = FormBorderStyle.SizableToolWindow;

                // show modally
                dlgDB.ShowDialog();
                if (dlgDB.DialogResult == DialogResult.Cancel)
                    return;
                string rdl = dlgDB.ResultReport;

                // Create the MDI child using the RDL syntax the wizard generates
                LoadReport(null, rdl, false);
         

            }
        }

        public bool FileSave()
        {
            Uri file = SourceFile;
            if (file == null || file.LocalPath == "")		// if no file name then do SaveAs
            {
                return FileSaveAs();
            }
            string rdl = GetRdlText();

            return FileSave(file, rdl);
        }


        private bool FileSave(Uri file, string rdl)
        {
            StreamWriter writer = null;
            bool bOK = true;
            try
            {
                writer = new StreamWriter(file.LocalPath);
                writer.Write(rdl);
                //				editRDL.ClearUndo();
                //				editRDL.Modified = false;
                //				SetTitle();
                //				statusBar.Text = "Saved " + curFileName;
            }
            catch (Exception ae)
            {
                bOK = false;
                MessageBox.Show(ae.Message + "\r\n" + ae.StackTrace);
                //				statusBar.Text = "Save of file '" + curFileName + "' failed";
            }
            finally
            {
                writer.Close();
            }
            if (bOK)
                this.Modified = false;
            return bOK;
        }

        public string GetRdlText()
        {
            return this.rdlEditPreview1.GetRdlText();
        }

        public bool Modified
        {
            get { return rdlEditPreview1.Modified; }
            set
            {
                rdlEditPreview1.Modified = value;
                SetTitle();
            }
        }

        private void SetTitle()
        {
            string title = this.Text;
            if (title.Length < 1)
                return;
            char m = title[title.Length - 1];
            if (this.Modified)
            {
                if (m != '*')
                    title = title + "*";
            }
            else if (m == '*')
                title = title.Substring(0, title.Length - 1);

            if (title != this.Text)
                this.Text = title;
            return;
        }


        // Create an MDI child.   Only creates it if not already open.
        private void LoadReport(Uri file, string rdl, bool bMenuUpdate)
        {

     
            try
            {



                Viewer.GetDataSourceReferencePassword = _GetPassword;
                if (file != null)
                {
                    this.Viewer.Folder = System.IO.Path.GetDirectoryName(file.LocalPath);
                    this.SourceFile = file;
                    this.Text = System.IO.Path.GetFileName(file.LocalPath);
                    this.Viewer.Folder = System.IO.Path.GetDirectoryName(file.LocalPath);
                    this.Viewer.ReportName = System.IO.Path.GetFileNameWithoutExtension(file.LocalPath);
                }
                else
                {
                    this.SourceRdl = rdl;
                    this.Viewer.ReportName = this.Text = "Untitled";
                }

                ShowEditLines(true);
                ShowReportItemOutline = false;
                ShowPreviewWaitDialog(true);
        
   
       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

       
 
        }

        public void ShowEditLines(bool bShow)
        {
            rdlEditPreview1.ShowEditLines(bShow);
        }

        internal bool ShowReportItemOutline
        {
            get { return rdlEditPreview1.ShowReportItemOutline; }
            set { rdlEditPreview1.ShowReportItemOutline = value; }
        }

        internal void ShowPreviewWaitDialog(bool bShow)
        {
            rdlEditPreview1.ShowPreviewWaitDialog(bShow);
        }


        /// <summary>
        /// Zoom 
        /// </summary>
        public float Zoom
        {
            get { return rdlEditPreview1.Zoom; }
            set { rdlEditPreview1.Zoom = value; }
        }

        /// <summary>
        /// ZoomMode 
        /// </summary>
        public ZoomEnum ZoomMode
        {
            get { return rdlEditPreview1.ZoomMode; }
            set { rdlEditPreview1.ZoomMode = value; }
        }

        private void ReportItemInserted(object sender, System.EventArgs e)
        {

            // turn off the current selection after an item is inserted
            if (ctlInsertCurrent != null)
            {
                ctlInsertCurrent.Checked = false;
                CurrentInsert = null;
                ctlInsertCurrent = null;
            }
            if (ctlMenuInsertCurrent != null)
            {
                ctlMenuInsertCurrent.Checked = false;
                CurrentInsert = null;
                ctlMenuInsertCurrent = null;
            }
        }

        private void OpenSubReportEvent(object sender, SubReportEventArgs e)
        {
  
            Uri file = new Uri(this.Viewer.Folder);
            if (e.SubReportName[0] == Path.DirectorySeparatorChar)
            {
                file = new Uri(file.LocalPath + e.SubReportName);
            }
            else
            {
                file = new Uri(file.LocalPath + Path.DirectorySeparatorChar + e.SubReportName + ".rdl");
            }

            LoadReport(file, null, true);
        }

        private void HeightChanged(object sender, HeightEventArgs e)
        {
            if (e.Height == null)
            {
                return;
            }

            RegionInfo rinfo = new RegionInfo(CultureInfo.CurrentCulture.LCID);
            float h = DesignXmlDraw.GetSize(e.Height);
            string sh;
            if (rinfo.IsMetric)
            {
                sh = string.Format("   height={0:0.00}cm        ",
                        h / (DesignXmlDraw.POINTSIZED / 2.54d));
            }
            else
            {
                sh = string.Format("   height={0:0.00}\"        ",
                        h / DesignXmlDraw.POINTSIZED);
            }
          
        }

        private void SelectionMoved(object sender, System.EventArgs e)
        {
            SetStatusNameAndPosition();
        }

        private void SelectionChanged(object sender, System.EventArgs e)
        {
            // handle edit tab first
            if (rdlEditPreview1.DesignTab == "edit")
            {
                SetStatusNameAndPosition();
                return;
            }

            bSuppressChange = true;			// don't process changes in status bar

            SetStatusNameAndPosition();
            this.EnableEditTextBox();	// handling enabling/disabling of textbox

            StyleInfo si = rdlEditPreview1.SelectedStyle;
            if (si == null)
                return;

            if (centerAlignToolStripButton2 != null)
                centerAlignToolStripButton2.Checked = si.TextAlign == TextAlignEnum.Center ? true : false;
            if (leftAlignToolStripButton2 != null)
                leftAlignToolStripButton2.Checked = si.TextAlign == TextAlignEnum.Left ? true : false;
            if (rightAlignToolStripButton3 != null)
                rightAlignToolStripButton3.Checked = si.TextAlign == TextAlignEnum.Right ? true : false;
            if (boldToolStripButton1 != null)
                boldToolStripButton1.Checked = si.IsFontBold() ? true : false;
            if (italiacToolStripButton1 != null)
                italiacToolStripButton1.Checked = si.FontStyle == FontStyleEnum.Italic ? true : false;
            if (underlineToolStripButton2 != null)
                underlineToolStripButton2.Checked = si.TextDecoration == TextDecorationEnum.Underline ? true : false;
            if (fontToolStripComboBox1 != null)
                fontToolStripComboBox1.Text = si.FontFamily;
            if (fontSizeToolStripComboBox1 != null)
            {
                string rs = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.#}", si.FontSize);
                fontSizeToolStripComboBox1.Text = rs;
            }
            if (foreColorPicker1 != null)
            {
                foreColorPicker1.Text = si.Color.IsEmpty ? si.ColorText : ColorTranslator.ToHtml(si.Color);
            }
            if (backColorPicker1 != null)
            {
                backColorPicker1.Text = si.BackgroundColor.IsEmpty ? si.BackgroundColorText : ColorTranslator.ToHtml(si.BackgroundColor);
            }

            bSuppressChange = false;
        }


        private void EnableEditTextBox()
        {

            bool bEnable = false;

            if (this.ctlEditTextbox == null ||
              rdlEditPreview1.DesignTab != "design" || rdlEditPreview1.DrawCtl.SelectedCount != 1)
            { }
            else
            {
                XmlNode tn = rdlEditPreview1.DrawCtl.SelectedList[0] as XmlNode;
                if (tn != null && tn.Name == "Textbox")
                {
                    ctlEditTextbox.Text = rdlEditPreview1.DrawCtl.GetElementValue(tn, "Value", "");
                    bEnable = true;
                }
            }
            if (ctlEditTextbox != null)
            {
                if (!bEnable)
                    ctlEditTextbox.Text = "";
                ctlEditTextbox.Enabled = bEnable;
                fxToolStripLabel1.Enabled = bEnable;
            }
        }

        private void SetStatusNameAndPosition()
        {

            SetProperties();


            return;
        }


        private void SetProperties()
        {
            if (!_ShowProperties || rdlEditPreview1.DesignTab != "design")
                mainProperties.ResetSelection(null, null);
            else
                mainProperties.ResetSelection(rdlEditPreview1.DrawCtl, rdlEditPreview1.DesignCtl);
        }

        internal void ShowProperties(bool bShow)
        {
            _ShowProperties = bShow;
            if (!_ShowProperties || rdlEditPreview1.DesignTab != "design")
                mainProperties.ResetSelection(null, null);
            else
                mainProperties.ResetSelection(rdlEditPreview1.DrawCtl, rdlEditPreview1.DesignCtl);

            mainProperties.Visible = splitContainer1.Panel2.Visible = _ShowProperties;
            if (splitContainer1.Panel2.Visible == false)
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
                ButtonShowProperties.Visible = true;
            }
            else 
            {
                ButtonShowProperties.Visible = false;
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel2.Show();
            }

        }

        public void ApplyStyleToSelected(string name, string v)
        {

            rdlEditPreview1.ApplyStyleToSelected(name, v);
        }

        /// <summary>
        /// Turns the Selection Tool on in report preview
        /// </summary>
        public bool SelectionTool
        {
            get
            {
                return rdlEditPreview1.SelectionTool;
            }
            set
            {
                rdlEditPreview1.SelectionTool = value;
            }
        }

        public bool Export(fyiReporting.RDL.OutputPresentationType type)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export to " + type.ToString().ToUpper();
            switch (type)
            {
                case OutputPresentationType.CSV:
                    sfd.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.XML:
                    sfd.Filter = "XML file (*.xml)|*.xml|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.PDF:
                case OutputPresentationType.PDFOldStyle:
                    sfd.Filter = "PDF file (*.pdf)|*.pdf|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.TIF:
                    sfd.Filter = "TIF file (*.tif, *.tiff)|*.tiff;*.tif|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.RTF:
                    sfd.Filter = "RTF file (*.rtf)|*.rtf|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.Word:
                    sfd.Filter = "DOC file (*.doc)|*.doc|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.Excel:
                    sfd.Filter = "Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.HTML:
                    sfd.Filter = "Web Page (*.html, *.htm)|*.html;*.htm|All files (*.*)|*.*";
                    break;
                case OutputPresentationType.MHTML:
                    sfd.Filter = "MHT (*.mht)|*.mhtml;*.mht|All files (*.*)|*.*";
                    break;
                default:
                    throw new Exception("Only HTML, MHT, XML, CSV, RTF, DOC, Excel, TIF and PDF are allowed as Export types.");
            }
            sfd.FilterIndex = 1;

            if (SourceFile != null)
            {
                sfd.FileName = Path.GetFileNameWithoutExtension(SourceFile.LocalPath) + "." + type;
            }
            else
            {
                sfd.FileName = "*." + type;
            }

            try
            {
                if (sfd.ShowDialog(this) != DialogResult.OK)
                    return false;

                // save the report in the requested rendered format 
                bool rc = true;
                // tif can be either in color or black and white; ask user what they want
                if (type == OutputPresentationType.TIF)
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to display colors in TIF?", "Export", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.No)
                        type = OutputPresentationType.TIFBW;
                    else if (dr == DialogResult.Cancel)
                        return false;
                }
                try { SaveAs(sfd.FileName, type); }
                catch (Exception ex)
                {
                    MessageBox.Show(this,
                        ex.Message, "Export Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rc = false;
                }
                return rc;
            }
            finally
            {
                sfd.Dispose();
            }
        }

        public void SaveAs(string filename, OutputPresentationType type)
        {
            rdlEditPreview1.SaveAs(filename, type);
        }

        public int PageCount
        {
            get { return rdlEditPreview1.PageCount; }
        }


        /// <summary>
        /// Page height of the report.
        /// </summary>
        public float PageHeight
        {
            get { return rdlEditPreview1.PageHeight; }
        }


        /// <summary>
        /// Page width of the report.
        /// </summary>
        public float PageWidth
        {
            get { return rdlEditPreview1.PageWidth; }
        }

        /// <summary>
        /// Current page in view on report
        /// </summary>
        public int PageCurrent
        {
            get { return rdlEditPreview1.PageCurrent; }
        }

        /// <summary>
        /// Print the report.  
        /// </summary>
        public void Print(PrintDocument pd)
        {
            rdlEditPreview1.Print(pd);
        }


        private void openToolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(SourceFile.LocalPath);
            }
            catch
            {
            }

            ofd.DefaultExt = "rdl";
            ofd.Filter = "Report files (*.rdl;*rdlc)|*.rdl;*.rdlc|" +
                "All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            try
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
   
                    OpenReport(new Uri(ofd.FileName), null);

                }
            }
            finally
            {
                ofd.Dispose();
            }
        }

        private void OpenReport(Uri file, string rdl)
        {

            try
            {
              
  
                Viewer.GetDataSourceReferencePassword = _GetPassword;
                if (file != null)
                {
                    Viewer.Folder = Path.GetDirectoryName(file.LocalPath);
                    SourceFile = file;
                    Text = Path.GetFileName(file.LocalPath);
                    Viewer.Folder = Path.GetDirectoryName(file.LocalPath);
                    Viewer.ReportName = Path.GetFileNameWithoutExtension(file.LocalPath);
                }
                else
                {
                    SourceRdl = rdl;
                    Viewer.ReportName = Text = "Untitled";
                }
                ShowEditLines(true);
                ShowReportItemOutline = this.ShowReportItemOutline;
                ShowPreviewWaitDialog(false);
           
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        public bool FileSaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RDL files (*.rdl)|*.rdl|All files (*.*)|*.*";
            sfd.FilterIndex = 1;

            Uri file = SourceFile;

            sfd.FileName = file == null ? "*.rdl" : file.LocalPath;
            try
            {
                if (sfd.ShowDialog(this) != DialogResult.OK)
                    return false;

                // User wants to save!
                string rdl = GetRdlText();
                if (FileSave(new Uri(sfd.FileName), rdl))
                {	// Save was successful
                    Text = sfd.FileName;
                    _SourceFile = new Uri(sfd.FileName);
                    return true;
                }
            }
            finally
            {
                sfd.Dispose();
            }
            return false;
        }

        private void saveToolStripButton1_Click(object sender, EventArgs e)
        {

            if (!FileSave())
                return;

            if (this.SavedFileEvent != null)
            {
                this.SavedFileEvent(this, new RdlDesignerSavedFileEvent(SourceFile));
            }

            if (rdlEditPreview1.CanEdit)
            {
                rdlEditPreview1.ClearUndo();
            }


            return;
        }

        private void cutToolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.ctlEditTextbox != null && ctlEditTextbox.Focused)
            {
                ctlEditTextbox.Cut();
                return;
            }

            RdlEditPreview e1 = rdlEditPreview1;
            if (e1 == null)
                return;

            if (e1.SelectionLength > 0)
                e1.Cut();
        }


        private void copyToolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.ctlEditTextbox != null && ctlEditTextbox.Focused)
            {
                ctlEditTextbox.Copy();
                return;
            }

            RdlEditPreview e1 = rdlEditPreview1;
            if (e1 == null)
                return;

            if (e1.SelectionLength > 0)
                e1.Copy();
        }

        private void pasteToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void undoToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        
        private void lineToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void ctlEditTextbox_Validated(object sender, EventArgs e)
        {

        }

        private void ctlEditTextbox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void mainTB_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void ToolStripButtons_Clicked(object sender, EventArgs e)
        {
            if (ctlInsertCurrent != null)
                ctlInsertCurrent.Checked = false;

            ToolStripButton ctl = (ToolStripButton)sender;
            ctlInsertCurrent = ctl.Checked ? ctl : null;

            CurrentInsert = ctlInsertCurrent == null ? null : (string)ctlInsertCurrent.Tag;
        }

        private void boldToolStripButton1_Click(object sender, EventArgs e)
        {

            ApplyStyleToSelected("FontWeight", boldToolStripButton1.Checked ? "Bold" : "Normal");
            SetProperties();

        }

        private void italiacToolStripButton1_Click(object sender, EventArgs e)
        {

            ApplyStyleToSelected("FontStyle", italiacToolStripButton1.Checked ? "Italic" : "Normal");
            SetProperties();

        }

        private void underlineToolStripButton2_Click(object sender, EventArgs e)
        {

            ApplyStyleToSelected("TextDecoration", underlineToolStripButton2.Checked ? "Underline" : "None");
            SetProperties();
        }

        private void leftAlignToolStripButton2_Click(object sender, EventArgs e)
        {
            TextAlignEnum ta = TextAlignEnum.General;

            if (sender == leftAlignToolStripButton2)
            {
                ta = TextAlignEnum.Left;
                leftAlignToolStripButton2.Checked = true;
                rightAlignToolStripButton3.Checked = centerAlignToolStripButton2.Checked = false;
            }
            else if (sender == rightAlignToolStripButton3)
            {
                ta = TextAlignEnum.Right;
                rightAlignToolStripButton3.Checked = true;
                leftAlignToolStripButton2.Checked = centerAlignToolStripButton2.Checked = false;
            }
            else if (sender == centerAlignToolStripButton2)
            {
                ta = TextAlignEnum.Center;
                centerAlignToolStripButton2.Checked = true;
                rightAlignToolStripButton3.Checked = leftAlignToolStripButton2.Checked = false;
            }

            ApplyStyleToSelected("TextAlign", ta.ToString());
            SetProperties();

        }

        private void fontToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("FontFamily", fontToolStripComboBox1.Text);
                SetProperties();
            }
        }

        private void fontToolStripComboBox1_Validated(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("FontFamily", fontToolStripComboBox1.Text);
                SetProperties();
            }
        }

        private void fontSizeToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("FontSize", fontSizeToolStripComboBox1.Text + "pt");
                SetProperties();
            }
        }

        private void fontSizeToolStripComboBox1_Validated(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("FontSize", fontSizeToolStripComboBox1.Text + "pt");
                SetProperties();
            }
        }


        private bool isPrinting = false;
        private void printToolStripButton2_Click(object sender, EventArgs e)
        {

            if (isPrinting == true)			// already printing
            {
                MessageBox.Show("Can only print one file at a time.", "RDL Design");
                return;
            }

            isPrinting = true;

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = SourceFile.LocalPath;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = PageCount;
            pd.PrinterSettings.MaximumPage = PageCount;
            pd.PrinterSettings.MinimumPage = 1;
            pd.DefaultPageSettings.Landscape = PageWidth > PageHeight ? true : false;

            // Set the paper size.
            if (SourceRdl != null)
            {
                System.Xml.XmlDocument docxml = new System.Xml.XmlDocument();
                docxml.LoadXml(SourceRdl);

                float height = 11;
                float width = 8.5f;
                XmlNodeList heightList = docxml.GetElementsByTagName("PageHeight");
                for (int i = 0; i < heightList.Count; i++)
                {
                    height = float.Parse(heightList[i].InnerText.Replace("in", "")) * 100;
                }

                XmlNodeList widthList = docxml.GetElementsByTagName("PageWidth");
                for (int i = 0; i < widthList.Count; i++)
                {
                    width = float.Parse(widthList[i].InnerText.Replace("in", "")) * 100;
                }

                pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", (int)width, (int)height);
            }
            using (PrintDialog dlg = new PrintDialog())
            {
                dlg.Document = pd;
                dlg.AllowSelection = true;
                dlg.AllowSomePages = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (pd.PrinterSettings.PrintRange == PrintRange.Selection)
                        {
                            pd.PrinterSettings.FromPage = PageCurrent;
                        }
                        Print(pd);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Print error: " + ex.Message, "RDL Design");
                    }
                }
                isPrinting = false;
            }
        }

        private void zoomToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (zoomToolStripComboBox1.Text)
            {
                case "Actual Size":
                    Zoom = 1;
                    break;
                case "Fit Page":
                    ZoomMode = ZoomEnum.FitPage;
                    break;
                case "Fit Width":
                    ZoomMode = ZoomEnum.FitWidth;
                    break;
                default:
                    string s = zoomToolStripComboBox1.Text.Substring(0, zoomToolStripComboBox1.Text.Length - 1);
                    float z;
                    try
                    {
                        z = Convert.ToSingle(s) / 100f;
                        Zoom = z;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Zoom Value Invalid");
                    }
                    break;
            }
        }

        private void selectToolStripButton2_Click(object sender, EventArgs e)
        {
            SelectionTool = selectToolStripButton2.Checked;

        }

        private void pdfToolStripButton2_Click(object sender, EventArgs e)
        {

            Export(fyiReporting.RDL.OutputPresentationType.PDF);

        }

        private void htmlToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.HTML);
        }

        private void excelToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.Excel);
        }

        private void XmlToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.XML);
        }

        private void MhtToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.MHTML);
        }

        private void CsvToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.CSV);
        }

        private void RtfToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.RTF);
        }

        private void TifToolStripButton2_Click(object sender, EventArgs e)
        {
            Export(fyiReporting.RDL.OutputPresentationType.TIF);
        }

        private void foreColorPicker1_Validated(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("Color", foreColorPicker1.Text);
                SetProperties();
            }
        }

        private void foreColorPicker1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("Color", foreColorPicker1.Text);
                SetProperties();
            }
        }

        private void backColorPicker1_Validated(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("BackgroundColor", backColorPicker1.Text);
                SetProperties();
            }
        }

        private void backColorPicker1_Click(object sender, EventArgs e)
        {
            if (!bSuppressChange)
            {
                ApplyStyleToSelected("BackgroundColor", backColorPicker1.Text);
                SetProperties();
            }
        }

        private void ButtonShowProperties_Click(object sender, EventArgs e)
        {
            ShowProperties(!_ShowProperties);
        }

    }
}
