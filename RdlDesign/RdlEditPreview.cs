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
using ScintillaNET;

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
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private fyiReporting.RdlViewer.RdlViewer rdlPreview;
		private System.Windows.Forms.TabPage tpDesign;
		private DesignCtl dcDesign;

		public FindTab FindTab;
	
		public delegate void RdlChangeHandler(object sender, EventArgs e);
		public event RdlChangeHandler OnRdlChanged;
		public event DesignCtl.HeightEventHandler OnHeightChanged;
        public event RdlChangeHandler OnSelectionChanged;
        public event RdlChangeHandler OnSelectionMoved;
		public event RdlChangeHandler OnReportItemInserted;
		public event RdlChangeHandler OnDesignTabChanged;
		public event DesignCtl.OpenSubreportEventHandler OnOpenSubreport; 

		// When toggling between the items we need to track who has latest changes
		DesignTabs _DesignChanged;			// last designer that triggered change
		DesignTabs _CurrentTab = DesignTabs.Design;
        private DesignRuler dcTopRuler;
		private DesignRuler dcLeftRuler;

		private Scintilla scintilla1;				// file position; for use with search
		private bool noFireRDLTextChanged;

		// Indicators 0-7 could be in use by a lexer
		// so we'll use indicator 8 to highlight words.
		const int SEARCH_INDICATOR_NUM = 8;

		public RdlEditPreview()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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

			//ScintillaNET Init
			ConfigureScintillaStyle(scintilla1);
			scintilla1.SetSavePoint();
		}

		void scintilla1_TextChanged(object sender, EventArgs e)
		{
			_DesignChanged = DesignTabs.Edit;
			if (noFireRDLTextChanged)
				return;

			if (OnRdlChanged != null)
			{
				OnRdlChanged(this, e);
			}
		}

		private void ConfigureScintillaStyle(ScintillaNET.Scintilla scintilla)
		{
			// Reset the styles
			scintilla.StyleResetDefault();
			scintilla.Styles[Style.Default].Font = "Consolas";
			scintilla.Styles[Style.Default].Size = 10;
			scintilla.StyleClearAll();

			// Set the XML Lexer
			scintilla.Lexer = Lexer.Xml;

			// Show line numbers
			scintilla.Margins[0].Width = 40;

			// Enable folding
			scintilla.SetProperty("fold", "1");
			scintilla.SetProperty("fold.compact", "1");
			scintilla.SetProperty("fold.html", "1");

			// Use Margin 2 for fold markers
			scintilla.Margins[2].Type = MarginType.Symbol;
			scintilla.Margins[2].Mask = Marker.MaskFolders;
			scintilla.Margins[2].Sensitive = true;
			scintilla.Margins[2].Width = 20;

			// Reset folder markers
			for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
			{
				scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
				scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
			}

			// Style the folder markers
			scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
			scintilla.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
			scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
			scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
			scintilla.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
			scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
			scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
			scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
			scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

			// Enable automatic folding
			scintilla.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;

			// Set the Styles
			scintilla.StyleResetDefault();
			// I like fixed font for XML
			scintilla.Styles[Style.Default].Font = "Courier";
			scintilla.Styles[Style.Default].Size = 10;
			scintilla.StyleClearAll();
			scintilla.Styles[Style.Xml.Attribute].ForeColor = Color.Red;
			scintilla.Styles[Style.Xml.Entity].ForeColor = Color.Red;
			scintilla.Styles[Style.Xml.Comment].ForeColor = Color.Green;
			scintilla.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
			scintilla.Styles[Style.Xml.TagEnd].ForeColor = Color.Blue;
			scintilla.Styles[Style.Xml.DoubleString].ForeColor = Color.DeepPink;
			scintilla.Styles[Style.Xml.SingleString].ForeColor = Color.DeepPink;
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

		internal DesignTabs DesignTab
		{
			get {return _CurrentTab;}
			set 
			{
				tcEHP.SelectedIndex = (int)value;				
			}
		}
 		
		internal void SetFocus()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Edit:
					scintilla1.Focus();
					break;
				case DesignTabs.Preview:
					rdlPreview.Focus();
					break;
				case DesignTabs.Design:
					dcDesign.SetFocus();
					break;
			}
		}

        internal void ShowEditLines(bool bShow)
        {
			scintilla1.Margins[0].Width = bShow ? 40 : 0;
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
				if (_CurrentTab == DesignTabs.Design)
					return dcDesign.ReportSource;
				else 
					return scintilla1.Text;
			}
			set 
			{
				if (_CurrentTab == DesignTabs.Edit)
					SetTextToScintilla(value);
				else
				{
					dcDesign.ReportSource = value;
				}
				if (_CurrentTab == DesignTabs.Preview)
				{
					_CurrentTab = DesignTabs.Design;
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
			if (_CurrentTab == DesignTabs.Design)
				dcDesign.ApplyStyleToSelected(name, v);
		}

		public void SetSelectedText(string v)
		{
			if (_CurrentTab == DesignTabs.Design)
				dcDesign.SetSelectedText(v);
		}

		public bool CanEdit
		{
			get 
			{ 
				return _CurrentTab != DesignTabs.Preview;
			}
		}

		private bool modified = false;

		public bool Modified
		{
			get 
			{ 
				return modified || scintilla1.Modified;
			}
			set 
			{ 
				_DesignChanged = _CurrentTab;
				modified = value;
				if (value == false)
					scintilla1.SetSavePoint();
			}
		}

		public string UndoDescription
		{
			get 
			{ 
				return _CurrentTab == DesignTabs.Design? dcDesign.UndoDescription: "";
			}
		}

		public void StartUndoGroup(string description)
		{
			if (_CurrentTab == DesignTabs.Design)
				dcDesign.StartUndoGroup(description);
		}

		public void EndUndoGroup(bool keepChanges)
		{
			if (_CurrentTab == DesignTabs.Design)
				dcDesign.EndUndoGroup(keepChanges);
		}

		public bool CanUndo
		{
			get 
			{ 
				switch (_CurrentTab)
				{
					case DesignTabs.Design:
						return dcDesign.CanUndo;
					case DesignTabs.Edit:
						return scintilla1.CanUndo;
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
					case DesignTabs.Design:
						return dcDesign.CanUndo;
					case DesignTabs.Edit:
						return scintilla1.CanRedo;
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
					case DesignTabs.Design:
						return dcDesign.SelectionCount;
					case DesignTabs.Edit:
						return scintilla1.SelectedText.Length;
                    case DesignTabs.Preview:
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
					case DesignTabs.Design:
						return dcDesign.SelectedText;
					case DesignTabs.Edit:
						return scintilla1.SelectedText;
                    case DesignTabs.Preview:
                        return rdlPreview.SelectText;
                    default:
						return "";
				}
			}
			set 
			{
				if (_CurrentTab == DesignTabs.Edit && String.IsNullOrWhiteSpace(value))
					scintilla1.ClearSelections();
				else if (_CurrentTab == DesignTabs.Design && value.Length == 0)
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
				case DesignTabs.Design:
					dcDesign.ClearUndo();
					break;
				case DesignTabs.Edit:
					scintilla1.EmptyUndoBuffer();
					break;
				default:
					break;
			}
		}

		public void Undo()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Design:
					dcDesign.Undo();
					break;
				case DesignTabs.Edit:
					scintilla1.Undo();
					break;
				default:
					break;
			}
		}

		public void Redo()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Design:
					dcDesign.Redo();
					break;
				case DesignTabs.Edit:
					scintilla1.Redo();
					break;
				default:
					break;
			}
		}

		public void Cut()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Design:
					dcDesign.Cut();
					break;
				case DesignTabs.Edit:
					scintilla1.Cut();
					break;
				default:
					break;
			}
		}

		public void Copy()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Design:
					dcDesign.Copy();
					break;
				case DesignTabs.Edit:
					scintilla1.Copy();
					break;
                case DesignTabs.Preview:
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
				case DesignTabs.Design:
					dcDesign.Clear();
					break;
				case DesignTabs.Edit:
					scintilla1.Clear();
					break;
				default:
					break;
			}
		}
		
		public void Paste()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Design:
					dcDesign.Paste();
					break;
				case DesignTabs.Edit:
					scintilla1.Paste();
					break;
				default:
					break;
			}
		}

		public void SelectAll()
		{
			switch (_CurrentTab)
			{
				case DesignTabs.Design:
					dcDesign.SelectAll();
					break;
				case DesignTabs.Edit:
					scintilla1.SelectAll();
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
				return scintilla1.CurrentLine + 1;
			}
		}

		public int CurrentCh
		{
			get 
			{
				return scintilla1.GetColumn(scintilla1.CurrentPosition);
			}
		}

        public bool SelectionTool
        {
            get
            {
                if (_CurrentTab == DesignTabs.Preview)
                {
                    return rdlPreview.SelectTool;
                }
                else
                    return false;
            }
            set
            {
                if (_CurrentTab == DesignTabs.Preview)
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

		public void FindNext(Control ctl, string str, bool matchCase, bool revertSearch, bool showEndMsg = true)
		{
			if (_CurrentTab != DesignTabs.Edit)
				return;
			
			scintilla1.SearchFlags = matchCase ? SearchFlags.MatchCase : SearchFlags.None;
			HighlightWord(str);
			if (revertSearch)
			{
				scintilla1.TargetStart = scintilla1.SelectionStart;
				scintilla1.TargetEnd = 0;
			}
			else
			{
				scintilla1.TargetStart = scintilla1.SelectionEnd;
				scintilla1.TargetEnd = scintilla1.TextLength;
			}
			var pos = scintilla1.SearchInTarget(str);
			if (pos == -1)
			{
				if (showEndMsg)
					MessageBox.Show(ctl, Strings.RdlEditPreview_ShowI_ReachedEndDocument);
			}
			else
			{
				scintilla1.GotoPosition(pos);
				scintilla1.SelectionStart = scintilla1.TargetStart;
				scintilla1.SelectionEnd = scintilla1.TargetEnd;
			}
		}

		private void HighlightWord(string text)
		{
			// Remove all uses of our indicator
			scintilla1.IndicatorCurrent = SEARCH_INDICATOR_NUM;
			scintilla1.IndicatorClearRange(0, scintilla1.TextLength);

			// Update indicator appearance
			scintilla1.Indicators[SEARCH_INDICATOR_NUM].Style = IndicatorStyle.StraightBox;
			scintilla1.Indicators[SEARCH_INDICATOR_NUM].Under = true;
			scintilla1.Indicators[SEARCH_INDICATOR_NUM].ForeColor = Color.Orange;
			scintilla1.Indicators[SEARCH_INDICATOR_NUM].OutlineAlpha = 50;
			scintilla1.Indicators[SEARCH_INDICATOR_NUM].Alpha = 30;

			// Search the document
			scintilla1.TargetStart = 0;
			scintilla1.TargetEnd = scintilla1.TextLength;
			while (scintilla1.SearchInTarget(text) != -1)
			{
				// Mark the search results with the current indicator
				scintilla1.IndicatorFillRange(scintilla1.TargetStart, scintilla1.TargetEnd - scintilla1.TargetStart);

				// Search the remainder of the document
				scintilla1.TargetStart = scintilla1.TargetEnd;
				scintilla1.TargetEnd = scintilla1.TextLength;
			}
		}

		public void ClearSearchHighlight()
		{
			scintilla1.IndicatorCurrent = SEARCH_INDICATOR_NUM;
			scintilla1.IndicatorClearRange(0, scintilla1.TextLength);
			FindTab = null;
		}

		public void ReplaceNext(Control ctl, string str, string strReplace, bool matchCase)
		{
			if (_CurrentTab != DesignTabs.Edit)
				return;

			if (String.Compare(scintilla1.SelectedText, str, !matchCase) == 0)
			{
				scintilla1.ReplaceSelection(strReplace);
			}
			else
			{
				FindNext(ctl, str, matchCase, false);
				if (String.Compare(scintilla1.SelectedText, str, !matchCase) == 0)
					scintilla1.ReplaceSelection(strReplace);
			}
		}

		public void ReplaceAll(Control ctl, string str, string strReplace, bool matchCase)
		{			
			if (_CurrentTab != DesignTabs.Edit)
				return;

			scintilla1.TargetStart = 0;
			scintilla1.TargetEnd = scintilla1.TextLength;
			scintilla1.SearchFlags = matchCase ? SearchFlags.MatchCase : SearchFlags.None;
			while (scintilla1.SearchInTarget(str) != -1)
			{
				scintilla1.ReplaceTarget(strReplace);

				// Search the remainder of the document
				scintilla1.TargetStart = scintilla1.TargetEnd;
				scintilla1.TargetEnd = scintilla1.TextLength;
			}
		}

		public void Goto(Control ctl, int nLine)
		{
			if (_CurrentTab != DesignTabs.Edit)
				return;

			if(nLine > scintilla1.Lines.Count)
				nLine = scintilla1.Lines.Count;
			scintilla1.Lines[nLine-1].Goto();
			scintilla1.SelectionStart = scintilla1.Lines[nLine - 1].Position;
			scintilla1.SelectionEnd = scintilla1.Lines[nLine - 1].EndPosition;
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
			this.scintilla1 = new ScintillaNET.Scintilla();
			this.tpBrowser = new System.Windows.Forms.TabPage();
			this.rdlPreview = new fyiReporting.RdlViewer.RdlViewer();
			this.tcEHP.SuspendLayout();
			this.tpEditor.SuspendLayout();
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
			this.tpEditor.Controls.Add(this.scintilla1);
			resources.ApplyResources(this.tpEditor, "tpEditor");
			this.tpEditor.Name = "tpEditor";
			this.tpEditor.Tag = "edit";
			// 
			// scintilla1
			// 
			resources.ApplyResources(this.scintilla1, "scintilla1");
			this.scintilla1.Lexer = ScintillaNET.Lexer.Xml;
			this.scintilla1.Name = "scintilla1";
			this.scintilla1.UseTabs = false;
			this.scintilla1.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.scintilla1_UpdateUI);
			this.scintilla1.TextChanged += new System.EventHandler(this.scintilla1_TextChanged);
			// 
			// tpBrowser
			// 
			this.tpBrowser.Controls.Add(this.rdlPreview);
			resources.ApplyResources(this.tpBrowser, "tpBrowser");
			this.tpBrowser.Name = "tpBrowser";
			this.tpBrowser.Tag = "preview";
			// 
			// rdlPreview
			// 
			this.rdlPreview.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this.rdlPreview, "rdlPreview");
			this.rdlPreview.dSubReportGetContent = null;
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
			this.rdlPreview.Zoom = 0.5495112F;
			this.rdlPreview.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
			// 
			// RdlEditPreview
			// 
			this.Controls.Add(this.tcEHP);
			this.Name = "RdlEditPreview";
			resources.ApplyResources(this, "$this");
			this.tcEHP.ResumeLayout(false);
			this.tpEditor.ResumeLayout(false);
			this.tpBrowser.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void dcDesign_ReportChanged(object sender, System.EventArgs e)
		{
			_DesignChanged = DesignTabs.Design;
			if (!Modified)
				SetTextToScintilla(dcDesign.ReportSource);

			if (OnRdlChanged != null)
			{
				OnRdlChanged(this, e);
			}
		}

		private void SetTextToScintilla(string text)
		{
			bool firstSet = String.IsNullOrEmpty(scintilla1.Text);
			noFireRDLTextChanged = firstSet;
			scintilla1.Text = text;
			if (firstSet)
			{
				scintilla1.EmptyUndoBuffer();
				scintilla1.SetSavePoint();
				noFireRDLTextChanged = false;
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
			DesignTabs tag = (DesignTabs)tc.SelectedIndex;

			// Sync up the various pane whenever they switch so the editor is always accurate
			switch (_DesignChanged)
			{	// Sync up the editor in every case
				case DesignTabs.Design:
					// sync up the editor
					SetTextToScintilla(dcDesign.ReportSource);
					break;
				case DesignTabs.Edit:
				case DesignTabs.Preview:
					break;
			}

			// Below sync up the changed item
			if (tag == DesignTabs.Preview)
			{
				if (rdlPreview.SourceRdl != scintilla1.Text)			// sync up preview
					this.rdlPreview.SourceRdl = scintilla1.Text;
			}
			else if (tag == DesignTabs.Design)
			{
				if (_DesignChanged != DesignTabs.Design)
				{
					try
					{
						dcDesign.ReportSource = scintilla1.Text;
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
			if (_CurrentTab == DesignTabs.Design)
				return dcDesign.ReportSource;
			else
				return scintilla1.Text;
		}

		public void SetRdlText(string text)
		{
			if (_CurrentTab == DesignTabs.Design)
			{
				try
				{
					dcDesign.ReportSource = text;
					dcDesign.Refresh();
					SetTextToScintilla(text);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, Strings.RdlEditPreview_Show_Report);
					SetTextToScintilla(text);
					tcEHP.SelectedIndex = (int)DesignTabs.Edit;	// Force current tab to edit syntax
					_DesignChanged = DesignTabs.Edit;
				}
			}
			else 
			{
				SetTextToScintilla(text);
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

		private void scintilla1_UpdateUI(object sender, UpdateUIEventArgs e)
		{
			if ((e.Change & UpdateChange.Selection) > 0)
			{
				if (OnSelectionChanged != null)
				{
					OnSelectionChanged(this, e);
				}
			}
		}
	}

	public enum DesignTabs
	{
		Design,
		Edit,
		Preview
	}
}
