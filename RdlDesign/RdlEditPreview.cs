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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Xml;
using fyiReporting.RDL;
using fyiReporting.RdlDesign.Resources;
using fyiReporting.RdlViewer;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for RdlEditPreview.
	/// </summary>
	internal class RdlEditPreview : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabControl tcEHP;
		private System.Windows.Forms.TabPage tpEditor;
		private System.Windows.Forms.TabPage tpBrowser;
		private System.Windows.Forms.RichTextBox tbEditor;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private fyiReporting.RdlViewer.RdlViewer rdlPreview;
		private System.Windows.Forms.TabPage tpDesign;
		private DesignCtl dcDesign;
	
		public delegate void RdlChangeHandler(object sender, EventArgs e);
		public event RdlChangeHandler OnRdlChanged;
		public event DesignCtl.HeightEventHandler OnHeightChanged;
        public event RdlChangeHandler OnSelectionChanged;
        public event RdlChangeHandler OnSelectionMoved;
		public event RdlChangeHandler OnReportItemInserted;
		public event RdlChangeHandler OnDesignTabChanged;
		public event DesignCtl.OpenSubreportEventHandler OnOpenSubreport; 

		// When toggling between the items we need to track who has latest changes
		string _DesignChanged;			// last designer that triggered change
		string _CurrentTab="design";
        private DesignEditLines pbLines;
        private DesignRuler dcTopRuler;
        private DesignRuler dcLeftRuler;

		int filePosition=0;				// file position; for use with search

		public RdlEditPreview()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            pbLines.Editor = tbEditor;
			rdlPreview.Zoom=1;				// force default zoom to 1
			// initialize the design tab
            dcTopRuler = new DesignRuler();
            dcLeftRuler = new DesignRuler();
            dcLeftRuler.Vertical = true;    // need to set before setting  Design property
            dcDesign = new DesignCtl();
            dcTopRuler.Design = dcDesign;   // associate rulers with design ctl
            dcLeftRuler.Design = dcDesign;

            tpDesign.Controls.Add(dcTopRuler);
            tpDesign.Controls.Add(dcLeftRuler);
            tpDesign.Controls.Add(dcDesign);

            // Top ruler
            dcTopRuler.Height = 14;
            dcTopRuler.Width = tpDesign.Width;
            dcTopRuler.Dock = DockStyle.Top;
            dcTopRuler.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            dcTopRuler.Enabled = false;

            // Left ruler
            dcLeftRuler.Width = 14;
            dcLeftRuler.Height = tpDesign.Height;
            dcLeftRuler.Dock = DockStyle.Left;
            dcLeftRuler.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            dcLeftRuler.Enabled = false;

            dcTopRuler.Offset = dcLeftRuler.Width;
            dcLeftRuler.Offset = dcTopRuler.Height;

           // dcDesign.Dock = System.Windows.Forms.DockStyle.Bottom;
            dcDesign.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			dcDesign.Location = new System.Drawing.Point(dcLeftRuler.Width, dcTopRuler.Height);
			dcDesign.Name = "dcDesign";
			dcDesign.Size = new System.Drawing.Size(tpDesign.Width-dcLeftRuler.Width, tpDesign.Height-dcTopRuler.Height);
			dcDesign.TabIndex = 0;
			dcDesign.ReportChanged += new System.EventHandler(dcDesign_ReportChanged);
            dcDesign.HeightChanged += new DesignCtl.HeightEventHandler(dcDesign_HeightChanged);
            dcDesign.SelectionChanged += new System.EventHandler(dcDesign_SelectionChanged);
			dcDesign.SelectionMoved += new System.EventHandler(dcDesign_SelectionMoved);
			dcDesign.ReportItemInserted += new System.EventHandler(dcDesign_ReportItemInserted);
			dcDesign.OpenSubreport += new DesignCtl.OpenSubreportEventHandler(dcDesign_OpenSubreport);

			tbEditor.SelectionChanged +=new EventHandler(tbEditor_SelectionChanged);
            // adjust size of line box by measuring a large #
#if !MONO
            using (Graphics g = this.CreateGraphics())
            {
                this.pbLines.Width = (int) (g.MeasureString("99999", tbEditor.Font).Width);
            }
#endif

		}
 
		internal DesignCtl DesignCtl
		{
			get {return dcDesign;}
		}

        internal RdlViewer.RdlViewer PreviewCtl
        {
            get { return rdlPreview; }
        }
 
		internal DesignXmlDraw DrawCtl
		{
			get {return dcDesign.DrawCtl;}
		}

		public XmlDocument ReportDocument
		{
			get {return dcDesign.ReportDocument;}
		}

        internal string CurrentTab { get { return _CurrentTab; } }

		internal string DesignTab
		{
			get {return _CurrentTab;}
			set 
			{
				switch (value)
				{
					case "design":
						tcEHP.SelectedIndex = 0;
						break;						
					case "edit":
						tcEHP.SelectedIndex = 1;
						break;
					case "preview":
						tcEHP.SelectedIndex = 2;
						break;
				}
			}
		}
 		
		internal void SetFocus()
		{
			switch (_CurrentTab)
			{
				case "edit":
					tbEditor.Focus();
					break;
				case "preview":
					rdlPreview.Focus();
					break;
				case "design":
					dcDesign.SetFocus();
					break;
			}
		}

        internal void ShowEditLines(bool bShow)
        {
            pbLines.Visible = bShow;
        }

        internal void ShowPreviewWaitDialog(bool bShow)
        {
            rdlPreview.ShowWaitDialog = bShow;
        }

        internal bool ShowReportItemOutline
        {
            get {return dcDesign.ShowReportItemOutline;}
            set {dcDesign.ShowReportItemOutline = value;}
        }

		override public string Text
		{
			get 
			{
				if (_CurrentTab == "design")
					return dcDesign.ReportSource;
				else
					return tbEditor.Text; 
			}
			set 
			{  
				if (_CurrentTab == "edit")
					tbEditor.Text = value;
				else
				{
					dcDesign.ReportSource = value;
				}
				if (_CurrentTab == "preview")
				{
					_CurrentTab = "design";
					tcEHP.SelectedIndex = 0;	// Force current tab to design
				}
			}
		}
		
		public StyleInfo SelectedStyle
		{
			get {return dcDesign.SelectedStyle;}
		}
		
		public string SelectionName
		{
			get {return dcDesign.SelectionName;}
		}
		
		public PointF SelectionPosition
		{
			get {return dcDesign.SelectionPosition;}
		}
		
		public SizeF SelectionSize
		{
			get {return dcDesign.SelectionSize;}
		}

		public void ApplyStyleToSelected(string name, string v)
		{
			if (_CurrentTab == "design")
				dcDesign.ApplyStyleToSelected(name, v);
		}

		public void SetSelectedText(string v)
		{
			if (_CurrentTab == "design")
				dcDesign.SetSelectedText(v);
		}

		public bool CanEdit
		{
			get 
			{ 
				return _CurrentTab == "edit" || _CurrentTab == "design"? true: false;
			}
		}

		public bool Modified
		{
			get 
			{ 
				return tbEditor.Modified; 
			}
			set 
			{ 
				_DesignChanged = _CurrentTab;
				tbEditor.Modified = value;
			}
		}

		public string UndoDescription
		{
			get 
			{ 
				return _CurrentTab == "design"? dcDesign.UndoDescription: "";
			}
		}

		public void StartUndoGroup(string description)
		{
			if (_CurrentTab == "design")
				dcDesign.StartUndoGroup(description);
		}

		public void EndUndoGroup(bool keepChanges)
		{
			if (_CurrentTab == "design")
				dcDesign.EndUndoGroup(keepChanges);
		}

		public bool CanUndo
		{
			get 
			{ 
				switch (_CurrentTab)
				{
					case "design":
						return dcDesign.CanUndo;
					case "edit":
						return tbEditor.CanUndo;
					default:
						return false;
				}
			}
		}

		public bool CanRedo
		{
			get 
			{ 
				switch (_CurrentTab)
				{
					case "design":
						return dcDesign.CanUndo;
					case "edit":
						return tbEditor.CanUndo;
					default:
						return false;
				}
			}
		}

		public int SelectionLength
		{
			get 
			{ 
				switch (_CurrentTab)
				{
					case "design":
						return dcDesign.SelectionCount;
					case "edit":
						return tbEditor.SelectionLength;
                    case "preview":
                        return rdlPreview.CanCopy ? 1 : 0;
                    default:
						return 0;
				}
			}
		}

		public string SelectedText
		{
			get 
			{ 
				switch (_CurrentTab)
				{
					case "design":
						return dcDesign.SelectedText;
					case "edit":
						return tbEditor.SelectedText;
                    case "preview":
                        return rdlPreview.SelectText;
                    default:
						return "";
				}
			}
			set 
			{ 
				if (_CurrentTab == "edit" && tbEditor.SelectionLength >= 0) 
					tbEditor.SelectedText = value; 
				else if (_CurrentTab == "design" && value.Length == 0)
					dcDesign.Delete();
			}
		}

		public void CleanUp()
		{
		}

		public void ClearUndo()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.ClearUndo();
					break;
				case "edit":
					tbEditor.ClearUndo();
					break;
				default:
					break;
			}
		}

		public void Undo()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.Undo();
					break;
				case "edit":
					tbEditor.Undo();
					break;
				default:
					break;
			}
		}

		public void Redo()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.Redo();
					break;
				case "edit":
					tbEditor.Redo();
					break;
				default:
					break;
			}
		}

		public void Cut()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.Cut();
					break;
				case "edit":
					tbEditor.Cut();
					break;
				default:
					break;
			}
		}

		public void Copy()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.Copy();
					break;
				case "edit":
					tbEditor.Copy();
					break;
                case "preview":
                    rdlPreview.Copy();
                    break;
				default:
					break;
			}
		}

		public void Clear()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.Clear();
					break;
				case "edit":
					tbEditor.Clear();
					break;
				default:
					break;
			}
		}
		
		public void Paste()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.Paste();
					break;
				case "edit":
					PasteText();
					break;
				default:
					break;
			}
		}

		void PasteText()
		{
			// The Paste function of RTF carries too much information; formatting etc.
			//   We only allow pasting of text information
			IDataObject iData = Clipboard.GetDataObject();
			if (iData == null)
				return;
			if (!iData.GetDataPresent(DataFormats.Text)) 
				return;

			string t = (string) iData.GetData(DataFormats.Text);
			this.tbEditor.SelectedText = t;
		}

		public void SelectAll()
		{
			switch (_CurrentTab)
			{
				case "design":
					dcDesign.SelectAll();
					break;
				case "edit":
					tbEditor.SelectAll();
					break;
				default:
					break;
			}
		}

		public string CurrentInsert
		{
			get {return dcDesign.CurrentInsert; }
			set 
			{
				dcDesign.CurrentInsert = value;
			}
		}

		public int CurrentLine
		{
			get 
			{
				int v = tbEditor.SelectionStart;
				return this.tbEditor.GetLineFromCharIndex(v)+1;
			}
		}

		public int CurrentCh
		{
			get 
			{
				int v = tbEditor.SelectionStart;
				int line = tbEditor.GetLineFromCharIndex(v);

				// Go back a character at a time until you hit previous line
				int c=0;
				while (--v >= 0 && line == tbEditor.GetLineFromCharIndex(v))
					c++;

				return c+1;
			}
		}

        public bool SelectionTool
        {
            get
            {
                if (_CurrentTab == "preview")
                {
                    return rdlPreview.SelectTool;
                }
                else
                    return false;
            }
            set
            {
                if (_CurrentTab == "preview")
                {
                    rdlPreview.SelectTool = value;
                }
            }
        }
		/// <summary>
		/// Zoom 
		/// </summary>
		public float Zoom
		{
			get {return this.rdlPreview.Zoom;}
			set {this.rdlPreview.Zoom = value;}
		}

		/// <summary>
		/// ZoomMode 
		/// </summary>
		public ZoomEnum ZoomMode
		{
			get {return this.rdlPreview.ZoomMode;}
			set {this.rdlPreview.ZoomMode = value;}
		}

		public void FindNext(Control ctl, string str, bool matchCase)
		{
			FindNext(ctl, str, matchCase, true);
		}

		public void FindNext(Control ctl, string str, bool matchCase, bool showEndMsg)
		{
			if (_CurrentTab != "edit")
				return;
            try
            {
                int nStart = tbEditor.Find(str, filePosition,
                    matchCase ? RichTextBoxFinds.MatchCase : RichTextBoxFinds.None);

                if (nStart < 0)
                {
                    if (showEndMsg)
                        MessageBox.Show(ctl, Strings.RdlEditPreview_ShowI_ReachedEndDocument);
                    filePosition = 0;
                    return;
                }
                int nLength = str.Length;

                tbEditor.ScrollToCaret();
                filePosition = nStart + nLength;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Strings.RdlEditPreview_Show_ErrorFind, ex.Message), Strings.RdlEditPreview_Show_InternalError);
                filePosition = 0;       // restart at 0
            }
		}

		public void ReplaceNext(Control ctl, string str, string strReplace, bool matchCase)
		{			
			if (_CurrentTab != "edit")
				return;
			try
			{
				int nStart = tbEditor.Find(str, filePosition, 
					matchCase? RichTextBoxFinds.MatchCase: RichTextBoxFinds.None);
				int nLength = str.Length;
				
				tbEditor.Text = tbEditor.Text.Remove(nStart, nLength);
				tbEditor.Text = tbEditor.Text.Insert(nStart, strReplace);
                tbEditor.Modified = true;
				tbEditor.ScrollToCaret();
			}
			catch (Exception e)
			{
				e.ToString();
				MessageBox.Show(ctl, Strings.RdlEditPreview_ShowI_ReachedEndDocument);
				filePosition = 0;
			}
		}

		public void ReplaceAll(Control ctl, string str, string strReplace, bool matchCase)
		{			
			if (_CurrentTab != "edit")
				return;
			try
			{
				int nStart = tbEditor.Text.IndexOf(str, filePosition);
				int nLength = str.Length;
				
				tbEditor.Select(nStart, nLength);
				tbEditor.Text= tbEditor.Text.Replace(str, strReplace);
                tbEditor.Modified = true;
                tbEditor.ScrollToCaret();
				filePosition = nStart+nLength;
			}
			catch (Exception e)
			{
				e.ToString();
				MessageBox.Show(ctl, Strings.RdlEditPreview_ShowI_ReachedEndDocument);
				filePosition = 0;
			}
		}

		public void Goto(Control ctl, int nLine)
		{	
			if (_CurrentTab != "edit")
				return;

			int offset = 0;
            nLine = Math.Min(Math.Max(0, nLine - 1), tbEditor.Lines.Length - 1);	// don't go off the ends

            offset = tbEditor.GetFirstCharIndexFromLine(nLine);    

//   Before .Net 2
//			for ( int i = 0; i < nLine - 1 && i < tbEditor.Lines.Length; ++i ) 
//				offset += this.tbEditor.Lines[i].Length + 1; 

			Control savectl = this.ActiveControl;
			tbEditor.Focus(); 
			tbEditor.Select( offset, this.tbEditor.Lines[nLine].Length);
			this.ActiveControl = savectl;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdlEditPreview));
			this.tcEHP = new System.Windows.Forms.TabControl();
			this.tpDesign = new System.Windows.Forms.TabPage();
			this.tpEditor = new System.Windows.Forms.TabPage();
			this.tbEditor = new System.Windows.Forms.RichTextBox();
			this.pbLines = new fyiReporting.RdlDesign.DesignEditLines();
			this.tpBrowser = new System.Windows.Forms.TabPage();
			this.rdlPreview = new fyiReporting.RdlViewer.RdlViewer();
			this.tcEHP.SuspendLayout();
			this.tpEditor.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbLines)).BeginInit();
			this.tpBrowser.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcEHP
			// 
			resources.ApplyResources(this.tcEHP, "tcEHP");
			this.tcEHP.Controls.Add(this.tpDesign);
			this.tcEHP.Controls.Add(this.tpEditor);
			this.tcEHP.Controls.Add(this.tpBrowser);
			this.tcEHP.Name = "tcEHP";
			this.tcEHP.SelectedIndex = 0;
			this.tcEHP.SelectedIndexChanged += new System.EventHandler(this.tcEHP_SelectedIndexChanged);
			// 
			// tpDesign
			// 
			resources.ApplyResources(this.tpDesign, "tpDesign");
			this.tpDesign.Name = "tpDesign";
			this.tpDesign.Tag = "design";
			// 
			// tpEditor
			// 
			resources.ApplyResources(this.tpEditor, "tpEditor");
			this.tpEditor.Controls.Add(this.tbEditor);
			this.tpEditor.Controls.Add(this.pbLines);
			this.tpEditor.Name = "tpEditor";
			this.tpEditor.Tag = "edit";
			// 
			// tbEditor
			// 
			this.tbEditor.AcceptsTab = true;
			resources.ApplyResources(this.tbEditor, "tbEditor");
			this.tbEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbEditor.HideSelection = false;
			this.tbEditor.Name = "tbEditor";
			this.tbEditor.TextChanged += new System.EventHandler(this.tbEditor_TextChanged);
			// 
			// pbLines
			// 
			resources.ApplyResources(this.pbLines, "pbLines");
			this.pbLines.Name = "pbLines";
			this.pbLines.TabStop = false;
			// 
			// tpBrowser
			// 
			resources.ApplyResources(this.tpBrowser, "tpBrowser");
			this.tpBrowser.Controls.Add(this.rdlPreview);
			this.tpBrowser.Name = "tpBrowser";
			this.tpBrowser.Tag = "preview";
			// 
			// rdlPreview
			// 
			resources.ApplyResources(this.rdlPreview, "rdlPreview");
			this.rdlPreview.Cursor = System.Windows.Forms.Cursors.Default;
			this.rdlPreview.Folder = null;
			this.rdlPreview.HighlightAll = false;
			this.rdlPreview.HighlightAllColor = System.Drawing.Color.Fuchsia;
			this.rdlPreview.HighlightCaseSensitive = false;
			this.rdlPreview.HighlightItemColor = System.Drawing.Color.Aqua;
			this.rdlPreview.HighlightPageItem = null;
			this.rdlPreview.HighlightText = null;
			this.rdlPreview.Name = "rdlPreview";
			this.rdlPreview.PageCurrent = 1;
			this.rdlPreview.Parameters = "";
			this.rdlPreview.ReportName = null;
			this.rdlPreview.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous;
			this.rdlPreview.SelectTool = false;
			this.rdlPreview.ShowFindPanel = false;
			this.rdlPreview.ShowParameterPanel = true;
			this.rdlPreview.ShowWaitDialog = true;
			this.rdlPreview.SourceFile = null;
			this.rdlPreview.SourceRdl = null;
			this.rdlPreview.UseTrueMargins = true;
			this.rdlPreview.Zoom = 0.5690382F;
			this.rdlPreview.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
			// 
			// RdlEditPreview
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.tcEHP);
			this.Name = "RdlEditPreview";
			this.tcEHP.ResumeLayout(false);
			this.tpEditor.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbLines)).EndInit();
			this.tpBrowser.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void tbEditor_TextChanged(object sender, System.EventArgs e)
		{
			_DesignChanged = "edit";

			if (OnRdlChanged != null)
			{
				OnRdlChanged(this, e);
			}
		}
		
		private void dcDesign_ReportChanged(object sender, System.EventArgs e)
		{
			_DesignChanged = "design";
			tbEditor.Modified = true;

			if (OnRdlChanged != null)
			{
				OnRdlChanged(this, e);
			}
		}

        private void dcDesign_HeightChanged(object sender, HeightEventArgs e)
        {
            if (OnHeightChanged != null)
            {
                OnHeightChanged(this, e);
            }
        }
		
		private void dcDesign_ReportItemInserted(object sender, System.EventArgs e)
		{
			if (OnReportItemInserted != null)
			{
				OnReportItemInserted(this, e);
			}
		}

		private void dcDesign_OpenSubreport(object sender, SubReportEventArgs e)
		{
			if (OnOpenSubreport != null)
			{
				OnOpenSubreport(this, e);
			}
		}
		
		private void dcDesign_SelectionChanged(object sender, System.EventArgs e)
		{
			if (OnSelectionChanged != null)
			{
				OnSelectionChanged(this, e);
			}
		}
		
		private void dcDesign_SelectionMoved(object sender, System.EventArgs e)
		{
			if (OnSelectionMoved != null)
			{
				OnSelectionMoved(this, e);
			}
		}
		
		private void tcEHP_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TabControl tc = (TabControl) sender;
			string tag = (string) tc.TabPages[tc.SelectedIndex].Tag;

			// Sync up the various pane whenever they switch so the editor is always accurate
			switch (_DesignChanged)
			{	// Sync up the editor in every case
				case "design":
					// sync up the editor
					tbEditor.Text = dcDesign.ReportSource;
					tbEditor.Modified = true;
					break;
				case "edit":
				case "preview":
					break;
				default:	// this can happen the first time 
					if (tbEditor.Text.Length <= 0)
						tbEditor.Text = dcDesign.ReportSource;
					break;
			}

			// Below sync up the changed item
			if (tag == "preview")
			{
				if (rdlPreview.SourceRdl != tbEditor.Text)			// sync up preview
					this.rdlPreview.SourceRdl = this.tbEditor.Text;
			}
			else if (tag == "design")
			{
				if (_DesignChanged != "design" && _DesignChanged != null)
				{
					try
					{
						dcDesign.ReportSource = tbEditor.Text;
					}
					catch (Exception ge)
					{
						MessageBox.Show(ge.Message, Strings.RdlEditPreview_Show_Report);
						tc.SelectedIndex = 1;	// Force current tab to edit syntax
						return;
					}
				}
			}

			_CurrentTab = tag;
			if (OnDesignTabChanged != null)
				OnDesignTabChanged(this, e);
		}

		/// <summary>
		/// Print the report.  
		/// </summary>
		public void Print(PrintDocument pd)
		{
			this.rdlPreview.Print(pd);
		}

        public void SaveAs(string filename, OutputPresentationType type)
		{
			this.rdlPreview.SaveAs(filename, type);
		}

		public string GetRdlText()
		{
			if (_CurrentTab == "design")
				return dcDesign.ReportSource;
			else	
				return this.tbEditor.Text;
		}

		public void SetRdlText(string text)
		{
			if (_CurrentTab == "design")
			{
				try
				{
					dcDesign.ReportSource = text;
					dcDesign.Refresh();
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, Strings.RdlEditPreview_Show_Report);
					this.tbEditor.Text = text;
					tcEHP.SelectedIndex = 1;	// Force current tab to edit syntax
					_DesignChanged = "edit";
				}
			}
			else
			{
				this.tbEditor.Text = text;
			}
		}

		/// <summary>
		/// Number of pages in the report.
		/// </summary>
		public int PageCount
		{
			get {return this.rdlPreview.PageCount;}
		}

		/// <summary>
		/// Current page in view on report
		/// </summary>
		public int PageCurrent
		{
			get {return this.rdlPreview.PageCurrent;}
		}

		/// <summary>
		/// Page height of the report.
		/// </summary>
		public float PageHeight
		{
			get {return this.rdlPreview.PageHeight;}
		}

		/// <summary>
		/// Page width of the report.
		/// </summary>
		public float PageWidth
		{
			get {return this.rdlPreview.PageWidth;}
		}

		public fyiReporting.RdlViewer.RdlViewer Viewer
		{
			get {return this.rdlPreview;}
		}

		private void tbEditor_SelectionChanged(object sender, EventArgs e)
		{
			if (OnSelectionChanged != null)
			{
				OnSelectionChanged(this, e);
			}
		}
	}
}
