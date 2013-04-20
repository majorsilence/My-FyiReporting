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

        public RdlUserControl()
        {
            InitializeComponent();


            _GetPassword = new RDL.NeedPassword(this.GetPassword);


            rdlEditPreview1.OnSelectionChanged += SelectionChanged;
            rdlEditPreview1.OnReportItemInserted += ReportItemInserted;
            rdlEditPreview1.OnOpenSubreport += OpenSubReportEvent;
            rdlEditPreview1.OnHeightChanged += HeightChanged;
            rdlEditPreview1.OnSelectionMoved += SelectionChanged;
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


        private void openToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void cutToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void pasteToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void undoToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void textboxToolStripButton1_Click(object sender, EventArgs e)
        {
            if (ctlInsertCurrent != null)
            {
                ctlInsertCurrent.Checked = false;
            }

            ToolStripButton ctl = (ToolStripButton)sender;
            ctlInsertCurrent = ctl.Checked ? ctl : null;

            CurrentInsert = ctlInsertCurrent == null ? null : (string)ctlInsertCurrent.Tag;

        }

        private void chartToolStripButton1_Click(object sender, EventArgs e)
        {
            if (ctlInsertCurrent != null)
                ctlInsertCurrent.Checked = false;

            ToolStripButton ctl = (ToolStripButton)sender;
            ctlInsertCurrent = ctl.Checked ? ctl : null;

            CurrentInsert = ctlInsertCurrent == null ? null : (string)ctlInsertCurrent.Tag;
        }

        private void tableToolStripButton1_Click(object sender, EventArgs e)
        {
            if (ctlInsertCurrent != null)
                ctlInsertCurrent.Checked = false;

            ToolStripButton ctl = (ToolStripButton)sender;
            ctlInsertCurrent = ctl.Checked ? ctl : null;

            CurrentInsert = ctlInsertCurrent == null ? null : (string)ctlInsertCurrent.Tag;
        }

        private void listToolStripButton1_Click(object sender, EventArgs e)
        {
            if (ctlInsertCurrent != null)
                ctlInsertCurrent.Checked = false;

            ToolStripButton ctl = (ToolStripButton)sender;
            ctlInsertCurrent = ctl.Checked ? ctl : null;

            CurrentInsert = ctlInsertCurrent == null ? null : (string)ctlInsertCurrent.Tag;
        }

        private void imageToolStripButton1_Click(object sender, EventArgs e)
        {
            if (ctlInsertCurrent != null)
                ctlInsertCurrent.Checked = false;

            ToolStripButton ctl = (ToolStripButton)sender;
            ctlInsertCurrent = ctl.Checked ? ctl : null;

            CurrentInsert = ctlInsertCurrent == null ? null : (string)ctlInsertCurrent.Tag;
        }

        private void matrixToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void subreportToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void rectangleToolStripButton1_Click(object sender, EventArgs e)
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
    }
}
