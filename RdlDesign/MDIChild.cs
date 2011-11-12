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
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Xml;
using fyiReporting.RDL;
using fyiReporting.RdlViewer;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// RdlReader is a application for displaying reports based on RDL.
	/// </summary>
	internal class MDIChild : Form
	{
		public delegate void RdlChangeHandler(object sender, EventArgs e);
		public event RdlChangeHandler OnSelectionChanged;
		public event RdlChangeHandler OnSelectionMoved;
		public event RdlChangeHandler OnReportItemInserted;
		public event RdlChangeHandler OnDesignTabChanged;
		public event DesignCtl.OpenSubreportEventHandler OnOpenSubreport;
        public event DesignCtl.HeightEventHandler OnHeightChanged;

		private fyiReporting.RdlDesign.RdlEditPreview rdlDesigner;
		string _SourceFile;
        TabPage _Tab;               // TabPage for this MDI Child

		public MDIChild(int width, int height)
		{
			this.rdlDesigner = new fyiReporting.RdlDesign.RdlEditPreview();
			this.SuspendLayout();
			// 
			// rdlDesigner
			// 
			this.rdlDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rdlDesigner.Location = new System.Drawing.Point(0, 0);
			this.rdlDesigner.Name = "rdlDesigner";
			this.rdlDesigner.Size = new System.Drawing.Size(width, height);
			this.rdlDesigner.TabIndex = 0;
			// register event for RDL changed.
			rdlDesigner.OnRdlChanged += new RdlEditPreview.RdlChangeHandler(rdlDesigner_RdlChanged);
            rdlDesigner.OnHeightChanged += new DesignCtl.HeightEventHandler(rdlDesigner_HeightChanged);
            rdlDesigner.OnSelectionChanged += new RdlEditPreview.RdlChangeHandler(rdlDesigner_SelectionChanged);
			rdlDesigner.OnSelectionMoved += new RdlEditPreview.RdlChangeHandler(rdlDesigner_SelectionMoved);
			rdlDesigner.OnReportItemInserted += new RdlEditPreview.RdlChangeHandler(rdlDesigner_ReportItemInserted);
			rdlDesigner.OnDesignTabChanged += new RdlEditPreview.RdlChangeHandler(rdlDesigner_DesignTabChanged);
			rdlDesigner.OnOpenSubreport += new DesignCtl.OpenSubreportEventHandler(rdlDesigner_OpenSubreport);

			// 
			// MDIChild
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(width, height);
			this.Controls.Add(this.rdlDesigner);
			this.Name = "";
			this.Text = "";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MDIChild_Closing);

			this.ResumeLayout(false);
		}

        internal TabPage Tab
        {
            get { return _Tab; }
            set { _Tab = value; }
        }

		public RdlEditPreview Editor
		{
			get
			{
				return rdlDesigner.CanEdit? rdlDesigner: null;	// only return when it can edit
			}
		}

		public RdlEditPreview RdlEditor
		{
			get
			{
				return rdlDesigner;			// always return
			}
		}

        public void ShowEditLines(bool bShow)
        {
            rdlDesigner.ShowEditLines(bShow);
        }

        internal void ShowPreviewWaitDialog(bool bShow)
        {
            rdlDesigner.ShowPreviewWaitDialog(bShow);
        }
        internal bool ShowReportItemOutline
        {
            get { return rdlDesigner.ShowReportItemOutline; }
            set { rdlDesigner.ShowReportItemOutline = value; }
        }

		public string CurrentInsert
		{
			get {return rdlDesigner.CurrentInsert; }
			set 
			{
				rdlDesigner.CurrentInsert = value;
			}
		}

		public int CurrentLine
		{
			get {return rdlDesigner.CurrentLine; }
		}

		public int CurrentCh
		{
			get {return rdlDesigner.CurrentCh; }
		}

		internal string DesignTab
		{
			get {return rdlDesigner.DesignTab;}
			set { rdlDesigner.DesignTab = value; }
		}

		internal DesignXmlDraw DrawCtl
		{
			get {return rdlDesigner.DrawCtl;}
		}

		public XmlDocument ReportDocument
		{
			get {return rdlDesigner.ReportDocument;}
		}
		
		internal void SetFocus()
		{
			rdlDesigner.SetFocus();
		}

		public StyleInfo SelectedStyle
		{
			get {return rdlDesigner.SelectedStyle;}
		}
		
		public string SelectionName
		{
			get {return rdlDesigner.SelectionName;}
		}
		
		public PointF SelectionPosition
		{
			get {return rdlDesigner.SelectionPosition;}
		}

		public SizeF SelectionSize
		{
			get {return rdlDesigner.SelectionSize;}
		}
		
		public void ApplyStyleToSelected(string name, string v)
		{
			rdlDesigner.ApplyStyleToSelected(name, v);
		}

		public bool FileSave()
		{
			string file = SourceFile;
			if (file == "" || file == null)			// if no file name then do SaveAs
			{
				return FileSaveAs();
			}
			string rdl = GetRdlText();

			return FileSave(file, rdl);
		}

		private bool FileSave(string file, string rdl)
		{
			StreamWriter writer=null;
			bool bOK=true;
			try
			{
				writer = new StreamWriter(file);
				writer.Write(rdl);
				//				editRDL.ClearUndo();
				//				editRDL.Modified = false;
				//				SetTitle();
				//				statusBar.Text = "Saved " + curFileName;
			}
			catch (Exception ae)
			{
				bOK=false;
				MessageBox.Show(ae.Message + "\r\n" +  ae.StackTrace);
				//				statusBar.Text = "Save of file '" + curFileName + "' failed";
			}
			finally
			{
				writer.Close();
			}
			if (bOK)
				this.Modified=false;
			return bOK;
		}

		public bool Export(string type)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Export to " + type.ToUpper();
			switch (type.ToLower())
			{
                case "csv":
                    sfd.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
                    break;
                case "xml":
					sfd.Filter = "XML file (*.xml)|*.xml|All files (*.*)|*.*";
					break;
				case "pdf":
					sfd.Filter = "PDF file (*.pdf)|*.pdf|All files (*.*)|*.*";
					break;
                case "tif": case "tiff":
                    sfd.Filter = "TIF file (*.tif, *.tiff)|*.tiff;*.tif|All files (*.*)|*.*";
                    break;
                case "rtf":
                    sfd.Filter = "RTF file (*.rtf)|*.rtf|All files (*.*)|*.*";
                    break;
                case "xlsx": case "excel":
                    type = "xlsx";
                    sfd.Filter = "Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    break;
                case "html":
				case "htm":
					sfd.Filter = "Web Page (*.html, *.htm)|*.html;*.htm|All files (*.*)|*.*";
                    break;
				case "mhtml":
				case "mht":
					sfd.Filter = "MHT (*.mht)|*.mhtml;*.mht|All files (*.*)|*.*";
					break;
				default:
					throw new Exception("Only HTML, MHT, XML, CSV, RTF, Excel, TIF and PDF are allowed as Export types.");
			}
			sfd.FilterIndex = 1;

            if (SourceFile != null)
				sfd.FileName = Path.GetFileNameWithoutExtension(SourceFile) + "." + type;
			else
				sfd.FileName = "*." + type;
            try
            {
                if (sfd.ShowDialog(this) != DialogResult.OK)
                    return false;

                // save the report in the requested rendered format 
                bool rc = true;
                // tif can be either in color or black and white; ask user what they want
                if (type == "tif" || type == "tiff")
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to display colors in TIF?", "Export", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.No)
                        type = "tifbw";
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

		public bool FileSaveAs()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "RDL files (*.rdl)|*.rdl|All files (*.*)|*.*";
			sfd.FilterIndex = 1;

			string file = SourceFile;

			sfd.FileName = file == null? "*.rdl": file;
            try
            {
                if (sfd.ShowDialog(this) != DialogResult.OK)
                    return false;

                // User wants to save!
                string rdl = GetRdlText();
                if (FileSave(sfd.FileName, rdl))
                {	// Save was successful
                    Text = sfd.FileName;
                    Tab.Text = Path.GetFileName(sfd.FileName);
                    _SourceFile = sfd.FileName;
                    Tab.ToolTipText = sfd.FileName;
                    return true;
                }
            }
            finally
            {
                sfd.Dispose();
            }
			return false;
		}
 
		public string GetRdlText()
		{
			return this.rdlDesigner.GetRdlText();
		}

		public bool Modified
		{
			get {return rdlDesigner.Modified;}
			set 
			{
				rdlDesigner.Modified = value;
				SetTitle();
			}
		}

		/// <summary>
		/// The RDL file that should be displayed.
		/// </summary>
		public string SourceFile
		{
			get {return _SourceFile;}
			set 
			{
				_SourceFile = value;
				string rdl = GetRdlSource();
				this.rdlDesigner.SetRdlText(rdl == null? "": rdl);
			}
		}

		public string SourceRdl
		{
			get {return this.rdlDesigner.GetRdlText();}
			set	{this.rdlDesigner.SetRdlText(value);}
		}

		private string GetRdlSource()
		{
			StreamReader fs=null;
			string prog=null;
			try
			{
				fs = new StreamReader(_SourceFile);
				prog = fs.ReadToEnd();
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}

			return prog;
		}

		/// <summary>
		/// Number of pages in the report.
		/// </summary>
		public int PageCount
		{
			get {return this.rdlDesigner.PageCount;}
		}

		/// <summary>
		/// Current page in view on report
		/// </summary>
		public int PageCurrent
		{
			get {return this.rdlDesigner.PageCurrent;}
		}

		/// <summary>
		/// Page height of the report.
		/// </summary>
		public float PageHeight
		{
			get {return this.rdlDesigner.PageHeight;}  
		}
/// <summary>
/// Turns the Selection Tool on in report preview
/// </summary>
        public bool SelectionTool
        {
            get
            {
               return this.rdlDesigner.SelectionTool;
            }
            set
            {
               this.rdlDesigner.SelectionTool = value;
            }
        }

		/// <summary>
		/// Page width of the report.
		/// </summary>
		public float PageWidth
		{
			get {return this.rdlDesigner.PageWidth;}
		}

		/// <summary>
		/// Zoom 
		/// </summary>
		public float Zoom
		{
			get {return this.rdlDesigner.Zoom;}
			set {this.rdlDesigner.Zoom = value;}
		}

		/// <summary>
		/// ZoomMode 
		/// </summary>
		public ZoomEnum ZoomMode
		{
			get {return this.rdlDesigner.ZoomMode;}
			set {this.rdlDesigner.ZoomMode = value;}
		}
 
		/// <summary>
		/// Print the report.  
		/// </summary>
		public void Print(PrintDocument pd)
		{
			this.rdlDesigner.Print(pd);
		}

		public void SaveAs(string filename, string ext)
		{
			rdlDesigner.SaveAs(filename, ext);
		}

		private void MDIChild_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if (!OkToClose())
            {
                e.Cancel = true;
                return;
            }

            if (Tab == null)
                return;

            Control ctl = Tab.Parent;
            ctl.Controls.Remove(Tab);
            Tab.Tag = null;             // this is the Tab reference to this
            Tab = null;
		}

		public bool OkToClose()
		{
			if (!this.Modified)
				return true;

			DialogResult r = 
					MessageBox.Show(this, String.Format("Do you want to save changes you made to '{0}'?",
					_SourceFile==null?"Untitled":Path.GetFileName(_SourceFile)), 
					"fyiReporting Designer",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button3);

			bool bOK = true;
			if (r == DialogResult.Cancel)
				bOK = false;
			else if (r == DialogResult.Yes)
			{
				if (!FileSave())
					bOK = false;
			}
			return bOK;
		}

		private void rdlDesigner_RdlChanged(object sender, System.EventArgs e)
		{
			SetTitle();
		}

        private void rdlDesigner_HeightChanged(object sender, HeightEventArgs e)
        {
            if (OnHeightChanged != null)
                OnHeightChanged(this, e);
        }

		private void rdlDesigner_SelectionChanged(object sender, System.EventArgs e)
		{
			if (OnSelectionChanged != null)
				OnSelectionChanged(this, e);
		}

		private void rdlDesigner_DesignTabChanged(object sender, System.EventArgs e)
		{
			if (OnDesignTabChanged != null)
				OnDesignTabChanged(this, e);
		}

		private void rdlDesigner_ReportItemInserted(object sender, System.EventArgs e)
		{
			if (OnReportItemInserted != null)
				OnReportItemInserted(this, e);
		}

		private void rdlDesigner_SelectionMoved(object sender, System.EventArgs e)
		{
			if (OnSelectionMoved != null)
				OnSelectionMoved(this, e);
		}

		private void rdlDesigner_OpenSubreport(object sender, SubReportEventArgs e)
		{
			if (OnOpenSubreport != null)
			{
				OnOpenSubreport(this, e);
			}
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MDIChild));
			// 
			// MDIChild
			// 
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MDIChild";

		}

		private void SetTitle()
		{
			string title = this.Text;
			if (title.Length < 1)
				return;
			char m = title[title.Length-1];
			if (this.Modified)
			{
				if (m != '*')
					title = title + "*";
			}
			else if (m == '*')
				title = title.Substring(0, title.Length-1);

			if (title != this.Text)
				this.Text = title;
			return;
		}

		public fyiReporting.RdlViewer.RdlViewer Viewer
		{
			get {return rdlDesigner.Viewer;}
		}

	}
}
