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
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text;
using System.Xml;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// DesignCtl is a designer view of an RDL report
	/// </summary>
	public class DesignCtl : System.Windows.Forms.Control
	{ 
		public delegate void OpenSubreportEventHandler(object source, SubReportEventArgs e);
        public delegate void HeightEventHandler(object source, HeightEventArgs e);
        public event System.EventHandler ReportChanged;
        public event System.EventHandler SelectionChanged;
		public event System.EventHandler SelectionMoved;
		public event System.EventHandler ReportItemInserted;
        public event System.EventHandler VerticalScrollChanged;
        public event System.EventHandler HorizontalScrollChanged;
        public event OpenSubreportEventHandler OpenSubreport;
        public event HeightEventHandler HeightChanged;
        bool _InPaint;						// to prevent recursively invoking paint
		// Scrollbars
		private VScrollBar _vScroll;
		private HScrollBar _hScroll;
		private float _DpiX;
		private float _DpiY;
		private XmlDocument _ReportDoc;		// the xml document we're editting
		private Undo _Undo;					//  the undo object tied to the _ReportDoc;
		private string _CurrentInsert;		// current object to insert; null if none

		// Mouse control
		private XmlNode _MouseDownNode;		// XmlNode affected by the mouse down event
		private HitLocationEnum _MouseDownLoc;	// hit location affected by the mouse down event
		private Point _MousePosition= new Point();		// position of the mouse
		private Point _ptRBOriginal = new Point();	// starting position of the mouse (rubber banding)
		private Point _ptRBLast = new Point();		//   last position of mouse (rubber banding)
		private bool _bHaveMouse;			// flag indicates we're rubber banding
		private bool _AdjustScroll = false;	// when adjusting band height we may need to adjust scroll bars

		private DesignXmlDraw _DrawPanel;		// the main drawing panel

		// Context menus

		MenuItem menuCopy;    
		MenuItem menuPaste;
		MenuItem menuDelete;
		MenuItem menuFSep1;
		MenuItem menuSelectAll;
		MenuItem menuFSep2;
		MenuItem menuInsert;
		MenuItem menuProperties;
		ContextMenu menuContext;
		MenuItem menuPropertiesLegend;
		MenuItem menuPropertiesCategoryAxis;
		MenuItem menuPropertiesValueAxis;
		MenuItem menuPropertiesCategoryAxisTitle;
		MenuItem menuPropertiesValueAxisTitle;
        MenuItem menuPropertiesValueAxis2Title;
		MenuItem menuPropertiesChartTitle;

		public DesignCtl()
		{
			// Get our graphics DPI					   
			Graphics g = null;			
			try
			{
				g = this.CreateGraphics(); 
				_DpiX = g.DpiX;
				_DpiY = g.DpiY;
			}
			catch
			{
				_DpiX = _DpiY = 96;
			}
			finally
			{
				if (g != null)
					g.Dispose();
			}

			// Handle the controls
			_vScroll = new VScrollBar();
			_vScroll.Scroll += new ScrollEventHandler(this.VerticalScroll);
			_vScroll.Enabled = false;

			_hScroll = new HScrollBar();
			_hScroll.Scroll += new ScrollEventHandler(this.HorizontalScroll);
			_hScroll.Enabled = false;

			_DrawPanel = new DesignXmlDraw();
			_DrawPanel.Paint += new PaintEventHandler(this.DrawPanelPaint);
			_DrawPanel.MouseUp += new MouseEventHandler(this.DrawPanelMouseUp);
			_DrawPanel.MouseDown += new MouseEventHandler(this.DrawPanelMouseDown);
			_DrawPanel.Resize += new EventHandler(this.DrawPanelResize); 
			_DrawPanel.MouseWheel +=new MouseEventHandler(DrawPanelMouseWheel);
			_DrawPanel.KeyDown += new KeyEventHandler(DrawPanelKeyDown);
			_DrawPanel.MouseMove += new MouseEventHandler(DrawPanelMouseMove);
			_DrawPanel.DoubleClick += new EventHandler(DrawPanelDoubleClick);

			this.Layout +=new LayoutEventHandler(DesignCtl_Layout);
			this.SuspendLayout();		 

			// Must be added in this order for DockStyle to work correctly
			this.Controls.Add(_DrawPanel);
			this.Controls.Add(_vScroll);
			this.Controls.Add(_hScroll);

			this.ResumeLayout(false);

			BuildContextMenus();
		}

        public void SignalReportChanged()
        {
            ReportChanged(this, new EventArgs());
        }

        public void SignalSelectionMoved()
        {
            SelectionMoved(this, new EventArgs());
        }

		public string CurrentInsert
		{
			get {return _CurrentInsert; }
			set 
			{
				_CurrentInsert = value;
			}
		}

        public int VerticalScrollPosition
        {
            get { return _vScroll.Value; }
        }

        public int HorizontalScrollPosition
        {
            get { return _hScroll.Value; }
        }

        internal Color SepColor
        {
            get { return _DrawPanel.SepColor; }
        }

        internal float SepHeight
        {
            get { return _DrawPanel.SepHeight; }
        }

        internal float PageHeaderHeight
        {
            get {return _DrawPanel.PageHeaderHeight; }
        }

        internal float PageFooterHeight
        {
            get { return _DrawPanel.PageFooterHeight; }
        }

        internal float BodyHeight
        {
            get { return _DrawPanel.BodyHeight; }
        }

		public XmlDocument ReportDocument
		{
			get {return _ReportDoc;}
			set 
			{
				_ReportDoc = value;
				if (_ReportDoc != null)
				{
					_Undo = new Undo(_ReportDoc, 300);
					_Undo.GroupsOnly = true;				// don't record changes that we don't group.
				}
				int selCount = _DrawPanel.SelectedCount;
				this._DrawPanel.ReportDocument = _ReportDoc;
				if (selCount > 0)	// changing report document forces change to selection
					SelectionChanged(this, new EventArgs());
			}
		}
 
		internal DesignXmlDraw DrawCtl
		{
			get {return _DrawPanel;}
		}

		public string ReportSource
		{
			get 
			{
				if (_ReportDoc == null)
					return null;
				string result="";
				try
				{
					// Convert the document into a string
					StringWriter sw = new StringWriter();
					XmlTextWriter xtw = new XmlTextWriter(sw);
					xtw.IndentChar = ' ';
					xtw.Indentation = 2;
					xtw.Formatting = Formatting.Indented;
			
					_ReportDoc.WriteContentTo(xtw);
					xtw.Close();
					sw.Close();	
					result = sw.ToString();
					result = result.Replace("xmlns=\"\"", "");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Unable to create RDL syntax");
				}
				return result;
			}
			set 
			{
				if (value == null || value == "")
				{
					ReportDocument = null;
					return;
				}

				XmlDocument xDoc = new XmlDocument();
				
				xDoc.PreserveWhitespace = false;
				xDoc.LoadXml(value);	// this will throw an exception if invalid XML
				ReportDocument = xDoc;
			}
		}

        public void ClearUndo()
		{
			_Undo.Reset();
		}

        internal Undo UndoObject
        {
            get { return _Undo; }
        }
		public void Undo()
		{
			_Undo.undo();
			_DrawPanel.ReportNames = null;	// may not be required; but if reportitem deleted/inserted it must be
            // determine if any of the selected nodes has been affected
            bool clearSelect = false;
            foreach (XmlNode n in _DrawPanel.SelectedList)
            {
                // this is an imperfect test but it shows if the node has been unchained.
                if (n.ParentNode == null)
                {
                    clearSelect = true;
                    break;
                }
            }
            if (clearSelect)
                _DrawPanel.SelectedList.Clear();

			_DrawPanel.Invalidate();   
		}

        public bool ShowReportItemOutline
        {
            get { return _DrawPanel.ShowReportItemOutline; }
            set 
            {
                _DrawPanel.ShowReportItemOutline = value; 
            }
        }

		public void Redo()
		{
		}

		public string UndoDescription
		{
			get { return _Undo.Description; }
		}

		public bool CanUndo
		{
			get { return _Undo.CanUndo; }
		}

		public void StartUndoGroup(string description)
		{
			_Undo.StartUndoGroup(description);
		}

		public void EndUndoGroup(bool keepChanges)
		{
			_Undo.EndUndoGroup(keepChanges);
		}

        public void Align(TextAlignEnum ta)
        {
            if (_DrawPanel.SelectedCount < 1)
                return;

            _Undo.StartUndoGroup("Align");

            _Undo.EndUndoGroup();

            return;
        }

		public void AlignLefts()
		{
            if (_DrawPanel.SelectedCount < 2)
                return;
            
            XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			XmlNode l = _DrawPanel.GetNamedChildNode(model, "Left");
			string left = l == null? "0pt": l.InnerText;

			_Undo.StartUndoGroup("Align");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	// we even reset the first one; in case the attribute wasn't specified
				_DrawPanel.SetElement(xNode, "Left", left);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void AlignRights()
		{
            if (_DrawPanel.SelectedCount < 2)
                return;
            
            XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF mrect = _DrawPanel.GetReportItemRect(model);	// size attributes in points
			if (mrect.Width == float.MinValue)
				return;			// model doesn't have width specified

			float mright = mrect.Left + mrect.Width;	// the right side of the model

			_Undo.StartUndoGroup("Align");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	
				if (xNode == model)
					continue;
				RectangleF nrect = _DrawPanel.GetReportItemRect(xNode);
				if (nrect.Width == float.MinValue)
					continue;

				float nleft = mright - nrect.Width;
				if (nleft < 0)
					nleft = 0;

				string left = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}pt", nleft);
				_DrawPanel.SetElement(xNode, "Left", left);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void AlignCenters()
		{
            if (_DrawPanel.SelectedCount < 2)
                return;
			XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF mrect = _DrawPanel.GetReportItemRect(model);	// size attributes in points
			if (mrect.Width == float.MinValue)
				return;			// model doesn't have width specified

			float mc = mrect.Left + mrect.Width/2;	// the middle of the model

			_Undo.StartUndoGroup("Align");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	
				if (xNode == model)
					continue;
				RectangleF nrect = _DrawPanel.GetReportItemRect(xNode);
				if (nrect.Width == float.MinValue)
					continue;

				float nleft =  (mc - (nrect.Left + nrect.Width/2));
				nleft += nrect.Left;
				if (nleft < 0)
					nleft = 0;

				string left = string.Format(NumberFormatInfo.InvariantInfo,"{0:0.00}pt", nleft);
				_DrawPanel.SetElement(xNode, "Left", left);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}
		
		public void AlignTops()
		{
            if (_DrawPanel.SelectedCount < 2)
                return;
            
            XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			XmlNode t = _DrawPanel.GetNamedChildNode(model, "Top");
			string top = t == null? "0pt": t.InnerText;

			_Undo.StartUndoGroup("Align");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	// we even reset the first one; in case the attribute wasn't specified
				_DrawPanel.SetElement(xNode, "Top", top);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void AlignBottoms()
		{
            if (_DrawPanel.SelectedCount < 2)
                return;
            
            XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF mrect = _DrawPanel.GetReportItemRect(model);	// size attributes in points
			if (mrect.Height == float.MinValue)
				return;			// model doesn't have height specified

			float mbottom = mrect.Top + mrect.Height;	// the bottom side of the model

			_Undo.StartUndoGroup("Align");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	
				if (xNode == model)
					continue;
				RectangleF nrect = _DrawPanel.GetReportItemRect(xNode);
				if (nrect.Height == float.MinValue)
					continue;

				float ntop = mbottom - nrect.Height;
				if (ntop < 0)
					ntop = 0;

				string top = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}pt", ntop);
				_DrawPanel.SetElement(xNode, "Top", top);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void AlignMiddles()
		{
            if (_DrawPanel.SelectedCount < 2)
                return;
            
            XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF mrect = _DrawPanel.GetReportItemRect(model);	// size attributes in points
			if (mrect.Height == float.MinValue)
				return;			// model doesn't have height specified

			float mc = mrect.Top + mrect.Height/2;	// the middle of the model

			_Undo.StartUndoGroup("Align");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	
				if (xNode == model)
					continue;
				RectangleF nrect = _DrawPanel.GetReportItemRect(xNode);
				if (nrect.Height == float.MinValue)
					continue;

				float ntop =  (mc - (nrect.Top + nrect.Height/2));
				ntop += nrect.Top;
				if (ntop < 0)
					ntop = 0;

				string top = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}pt", ntop);
				_DrawPanel.SetElement(xNode, "Top", top);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void SizeHeights()
		{
			XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			XmlNode h = _DrawPanel.GetNamedChildNode(model, "Height");
			if (h == null)
				return;
			string height = h.InnerText;

			_Undo.StartUndoGroup("Size");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	// we even reset the first one; in case the attribute wasn't specified
				_DrawPanel.SetElement(xNode, "Height", height);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void SizeWidths()
		{
			XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			XmlNode w = _DrawPanel.GetNamedChildNode(model, "Width");
			if (w == null)
				return;
			string width = w.InnerText;

			_Undo.StartUndoGroup("Size");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	// we even reset the first one; in case the attribute wasn't specified
				_DrawPanel.SetElement(xNode, "Width", width);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void SizeBoth()
		{
			XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			XmlNode w = _DrawPanel.GetNamedChildNode(model, "Width");
			if (w == null)
				return;
			string width = w.InnerText;

			XmlNode h = _DrawPanel.GetNamedChildNode(model, "Height");
			if (h == null)
				return;
			string height = h.InnerText;

			_Undo.StartUndoGroup("Size");
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{	// we even reset the first one; in case the attribute wasn't specified
				_DrawPanel.SetElement(xNode, "Height", height);
				_DrawPanel.SetElement(xNode, "Width", width);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		void HorzSpacing(float diff)
		{
			XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF rectm = _DrawPanel.GetReportItemRect(model);
			if (rectm.Width == float.MinValue)
				return;

			_Undo.StartUndoGroup("Spacing");
			float x = rectm.Left + rectm.Width + diff;
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{
				if (xNode == model)
					continue;
				string left = string.Format(NumberFormatInfo.InvariantInfo,"{0:0.00}pt", x);
				_DrawPanel.SetElement(xNode, "Left", left);
				RectangleF rectn = _DrawPanel.GetReportItemRect(xNode);
				if (rectn.Width == float.MinValue)
					rectn.Width = 77;
				x += (rectn.Width + diff);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		float HorzSpacingDiff()
		{
			float diff = 0;
			if (_DrawPanel.SelectedList.Count < 2)
				return diff;

			XmlNode m1 = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF r1 = _DrawPanel.GetReportItemRect(m1);
			if (r1.Width == float.MinValue)
				return diff;

			XmlNode m2 = _DrawPanel.SelectedList[1] as XmlNode;
			RectangleF r2 = _DrawPanel.GetReportItemRect(m2);

			diff = r2.Left - (r1.Left + r1.Width);
			if (diff < 0)
				diff = 0;
			return diff;
		}

		public void HorzSpacingMakeEqual()
		{
			if (_DrawPanel.SelectedList.Count < 2)
				return;

			HorzSpacing(HorzSpacingDiff());
		}

		public void HorzSpacingIncrease()
		{
			float diff = HorzSpacingDiff() + 8;
			HorzSpacing(diff);
		}

		public void HorzSpacingDecrease()
		{
			float diff = HorzSpacingDiff() - 8;
			if (diff < 0)
				diff = 0;
			HorzSpacing(diff);
		}

		public void HorzSpacingMakeZero()
		{
			HorzSpacing(0);
		}

		void VertSpacing(float diff)
		{
			XmlNode model = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF rectm = _DrawPanel.GetReportItemRect(model);
			if (rectm.Height == float.MinValue)
				return;

			_Undo.StartUndoGroup("Spacing");
			float y = rectm.Top + rectm.Height + diff;
			foreach (XmlNode xNode in _DrawPanel.SelectedList)
			{
				if (xNode == model)
					continue;
				string top = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}pt", y);
				_DrawPanel.SetElement(xNode, "Top", top);
				RectangleF rectn = _DrawPanel.GetReportItemRect(xNode);
				if (rectn.Height == float.MinValue)
					rectn.Height = 16;
				y += (rectn.Height + diff);
			}
			_Undo.EndUndoGroup();

			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		float VertSpacingDiff()
		{
			float diff = 0;
			if (_DrawPanel.SelectedList.Count < 2)
				return diff;

			XmlNode m1 = _DrawPanel.SelectedList[0] as XmlNode;
			RectangleF r1 = _DrawPanel.GetReportItemRect(m1);
			if (r1.Height == float.MinValue)
				return diff;

			XmlNode m2 = _DrawPanel.SelectedList[1] as XmlNode;
			RectangleF r2 = _DrawPanel.GetReportItemRect(m2);

			diff = r2.Top - (r1.Top + r1.Height);
			if (diff < 0)
				diff = 0;
			return diff;
		}

		public void VertSpacingMakeEqual()
		{
			if (_DrawPanel.SelectedList.Count < 2)
				return;

			VertSpacing(VertSpacingDiff());
		}

		public void VertSpacingIncrease()
		{
			float diff = VertSpacingDiff() + 8;
			VertSpacing(diff);
		}

		public void VertSpacingDecrease()
		{
			float diff = VertSpacingDiff() - 8;
			if (diff < 0)
				diff = 0;
			VertSpacing(diff);
		}

		public void VertSpacingMakeZero()
		{
			VertSpacing(0);
		}

		public void SetPadding(string name, int diff)
		{
			if (_DrawPanel.SelectedList.Count < 1)
				return;

			_Undo.StartUndoGroup("Padding");
			foreach (XmlNode n in _DrawPanel.SelectedList)
			{
				XmlNode sNode = this._DrawPanel.GetCreateNamedChildNode(n, "Style");
				if (diff == 0)
					_DrawPanel.SetElement(sNode, name, "0pt");
				else
				{
					float pns = _DrawPanel.GetSize(sNode, name);
					pns += diff;
					if (pns < 0)
						pns = 0;
					string pad = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.00}pt", pns);
					_DrawPanel.SetElement(sNode, name, pad);
				}
			}
			_Undo.EndUndoGroup();
			ReportChanged(this, new EventArgs());
			_DrawPanel.Invalidate();   
		}

		public void Cut()
		{
			if (_DrawPanel.SelectedCount <= 0)
				return;

			Clipboard.SetDataObject(GetCopy(), true);
			_Undo.StartUndoGroup("Cut");
			_DrawPanel.DeleteSelected();
			_Undo.EndUndoGroup();
			SelectionChanged(this, new EventArgs());
		}

		public void Copy()
		{
			Clipboard.SetDataObject(GetCopy(), true);
		}

		public void Clear()
		{
			return;
		}

		public void Delete()
		{
			if (_DrawPanel.SelectedCount > 0)
			{
				_Undo.StartUndoGroup("Delete");
				_DrawPanel.DeleteSelected();
				_Undo.EndUndoGroup();

				ReportChanged(this, new EventArgs());
				SelectionChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
		}

		public void Paste()
		{
			DoPaste(null,new PointF(0,0));
		}

		public void SelectAll()
		{
			doSelectAll();
		}

		public int SelectionCount
		{
			get { return this._DrawPanel.SelectedCount; }
		}

		public string SelectedText
		{
			get 
			{
				if (_DrawPanel.SelectedCount == 0)
					return "";
				return GetCopy();
			}
		}
		
		public string SelectionName
		{
			get
			{
				if (_DrawPanel.SelectedCount == 0)
					return "";
				if (_DrawPanel.SelectedCount > 1)
					return "Group Selection";
				XmlNode xNode = _DrawPanel.SelectedList[0];
				if (xNode.Name == "TableColumn" || xNode.Name == "TableRow")
					return "";

				XmlAttribute xAttr = xNode.Attributes["Name"];
				if (xAttr == null)
					return "*Unnamed*";

				XmlNode cNode = _DrawPanel.GetReportItemContainer(xNode);
				if (cNode == null)
					return xAttr.Value;
				XmlAttribute cAttr = cNode.Attributes["Name"];
				if (cAttr == null)
					return xAttr.Value + " in " + cNode.Name;

				string title = xAttr.Value + " in " + cNode.Name.Replace("fyi:", "") + " " + cAttr.Value;
				if (!(cNode.Name == "Table" || cNode.Name == "fyi:Grid"))
					return title;

				XmlNode pNode = xNode.ParentNode.ParentNode;
				if (pNode.Name != "TableCell")
					return title;

				XmlNode trNode = pNode.ParentNode.ParentNode;		// should be TableRow
				if (trNode.Name != "TableRow")
					return title;

				// Find the number of the TableRow -- e.g. 1st, 2nd, 3rd, ...
				int trNumber=1;
				foreach (XmlNode n in trNode.ParentNode.ChildNodes)
				{
					if (n == trNode)
						break;
					trNumber++;
				}

				pNode = trNode.ParentNode.ParentNode;			// Details, Header or Footer

				string rowTitle = trNumber > 1? string.Format("{0}({1})", pNode.Name, trNumber): pNode.Name;
 
				if (pNode.Name == "Details")
					return title + " " + rowTitle;

				// We've got a Header or a Footer; could be a group header/footer
				pNode = pNode.ParentNode;
				if (pNode.Name != "TableGroup")
					return title + " " + rowTitle;

				// We're in a group; find out the group name
				XmlNode gNode = this._DrawPanel.GetNamedChildNode(pNode, "Grouping");
				if (gNode == null)
					return title + " " + rowTitle;

				XmlAttribute gAttr = gNode.Attributes["Name"];
				if (gAttr == null)
					return title + " " + rowTitle;

				return title + ", Group " + gAttr.Value + " " + rowTitle;
			}
		}
		
		public PointF SelectionPosition
		{
			get
			{
				if (_DrawPanel.SelectedCount == 0)
					return new PointF(float.MinValue,float.MinValue);
				XmlNode xNode = _DrawPanel.SelectedList[0];
				return _DrawPanel.SelectionPosition(xNode);
			}
		}

		public SizeF SelectionSize
		{
			get
			{
				if (_DrawPanel.SelectedCount == 0)
					return new SizeF(float.MinValue,float.MinValue);
				XmlNode xNode = _DrawPanel.SelectedList[0];
				return _DrawPanel.SelectionSize(xNode);
			}
		}
		
		
		public StyleInfo SelectedStyle
		{
			get
			{
				if (_DrawPanel.SelectedCount == 0)
					return null;
				XmlNode xNode = _DrawPanel.SelectedList[0];
				return _DrawPanel.GetStyleInfo(xNode);
			}
		}

		public void ApplyStyleToSelected(string name, string v)
		{
			if (_DrawPanel.SelectedCount == 0)
				return;

			_Undo.StartUndoGroup("Style");
			_DrawPanel.ApplyStyleToSelected(name, v);
			_Undo.EndUndoGroup(true);
			ReportChanged(this, new EventArgs());
		}

		public void SetSelectedText(string v)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;

			XmlNode tn = _DrawPanel.SelectedList[0] as XmlNode;
			if (tn == null || tn.Name != "Textbox")
				return;

			_Undo.StartUndoGroup("Textbox Value");
			_DrawPanel.SetElement(tn, "Value", v);
			_Undo.EndUndoGroup(true);
			_DrawPanel.Invalidate();	// force a repaint
			ReportChanged(this, new EventArgs());
		}

		private void BuildContextMenus()
		{
			// EDIT MENU
			menuCopy = new MenuItem("&Copy", new EventHandler(this.menuCopy_Click));
			menuPaste = new MenuItem("Paste", new EventHandler(this.menuPaste_Click));
			menuDelete = new MenuItem("&Delete", new EventHandler(this.menuDelete_Click));
			menuFSep1 = new MenuItem("-");
			menuSelectAll = new MenuItem("Select &All", new EventHandler(this.menuSelectAll_Click));
			menuFSep2 = new MenuItem("-");

            List<MenuItem> insertItems = new List<MenuItem>();
            insertItems.Add(new MenuItem("&Chart...", new EventHandler(this.menuInsertChart_Click)));
            insertItems.Add(new MenuItem("&Grid", new EventHandler(this.menuInsertGrid_Click)));
            insertItems.Add(new MenuItem("&Image", new EventHandler(this.menuInsertImage_Click)));
            insertItems.Add(new MenuItem("&Line", new EventHandler(this.menuInsertLine_Click)));
			insertItems.Add(new MenuItem("&List", new EventHandler(this.menuInsertList_Click)));
			insertItems.Add(new MenuItem("&Matrix...", new EventHandler(this.menuInsertMatrix_Click)));
            insertItems.Add(new MenuItem("&Rectangle", new EventHandler(this.menuInsertRectangle_Click)));
            insertItems.Add(new MenuItem("&Subreport", new EventHandler(this.menuInsertSubreport_Click)));
            insertItems.Add(new MenuItem("Ta&ble...", new EventHandler(this.menuInsertTable_Click)));
            insertItems.Add(new MenuItem("&Textbox", new EventHandler(this.menuInsertTextbox_Click)));
            // Now add any CustomReportItems
            BuildContextMenusCustom(insertItems);

			menuInsert = new MenuItem("&Insert");
			menuInsert.MenuItems.AddRange(insertItems.ToArray());

			menuProperties = new MenuItem("&Properties...", new EventHandler(this.menuProperties_Click));

			//
			// Create chart context menu and add array of sub-menu items
			menuPropertiesLegend = new MenuItem("Legend...", new EventHandler(this.menuPropertiesLegend_Click));
			menuPropertiesCategoryAxis = new MenuItem("Category (X) Axis...", new EventHandler(this.menuPropertiesCategoryAxis_Click));
			menuPropertiesValueAxis = new MenuItem("Value (Y) Axis...", new EventHandler(this.menuPropertiesValueAxis_Click));
			
			menuPropertiesCategoryAxisTitle = new MenuItem("Category (X) Axis Title...", new EventHandler(this.menuPropertiesCategoryAxisTitle_Click));
			menuPropertiesValueAxisTitle = new MenuItem("Value (Y) Axis Title...", new EventHandler(this.menuPropertiesValueAxisTitle_Click));
            menuPropertiesValueAxis2Title = new MenuItem("Value (Y) Axis (Right) Title...", new EventHandler(this.menuPropertiesValueAxis2Title_Click));
			menuPropertiesChartTitle = new MenuItem("Title...", new EventHandler(this.menuPropertiesChartTitle_Click));

		}
        
        private void BuildContextMenusCustom(List<MenuItem> items)
        {
            try
            {
                string[] sa = RdlEngineConfig.GetCustomReportTypes();
                if (sa == null || sa.Length == 0)
                    return;

                items.Add(new MenuItem("-"));       // put a separator
                // Add the custom report items to the insert menu
                foreach (string m in sa)
                {
                    MenuItem mi = new MenuItem(m+"...", new EventHandler(this.menuInsertCustomReportItem_Click));
                    mi.Tag = m;
                    items.Add(mi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error building CustomReportItem menus: {0}", ex.Message), "Insert", MessageBoxButtons.OK);
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
				if (this._ReportDoc == null)		// if no report force the simplest one
					CreateEmptyReportDoc();

				_DrawPanel.Draw(g, PointsX(_hScroll.Value), PointsY(_vScroll.Value),	
					e.ClipRectangle);
			}
			catch (Exception ex) 
			{	// don't want to kill process if we die -- put up some kind of error message
				StringFormat format = new StringFormat();
				string msg = string.Format("Error drawing report.  Likely error in syntax.  Switch to syntax and correct report syntax.{0}{1}{0}{2}", 
					Environment.NewLine, ex.Message, ex.StackTrace) ;
				g.DrawString(msg, this.Font, Brushes.Black, new Rectangle(2, 2, this.Width, this.Height), format);

			}		
			
			lock (this)
			{
				_InPaint=false;
			}
		}

		private void CreateEmptyReportDoc()
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.PreserveWhitespace = false;
			xDoc.LoadXml("<Report><Body><Height>0pt</Height></Body></Report>");
			ReportDocument = xDoc;
		}

		private void DrawPanelResize(object sender, EventArgs e)
		{
			_DrawPanel.Refresh();
			SetScrollControls();			

		}

		internal float PointsX(float x)		// pixels to points
		{
            return x * DesignXmlDraw.POINTSIZED / _DpiX;
		}

		internal float PointsY(float y)
		{
            return y * DesignXmlDraw.POINTSIZED / _DpiY;
		}

		internal int PixelsX(float x)		// points to pixels
		{
            return (int)(x * _DpiX / DesignXmlDraw.POINTSIZED);
		}

		internal int PixelsY(float y)
		{
            return (int)(y * _DpiY / DesignXmlDraw.POINTSIZED);
		}


		internal void SetScrollControls()
		{
			if (_ReportDoc == null)		// nothing loaded; nothing to do
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
			int h = PixelsY(_DrawPanel.VerticalMax);	// size we need to show
			int sh = this.Height - _hScroll.Height;
			if (sh > h || sh < 0)
			{
				_vScroll.Enabled = false;
				if (_vScroll.Value != 0)
				{
					_vScroll.Value = 0;
					_DrawPanel.Invalidate();	// force a repaint
				}
				return;
			}

			_vScroll.Minimum = 0;
			_vScroll.Maximum = h;
			int sValue =  Math.Min(_vScroll.Value, _vScroll.Maximum);
			if (_vScroll.Value != sValue)
			{
				_vScroll.Value = sValue;
				_DrawPanel.Invalidate();		// force a repaint
			}
			_vScroll.LargeChange = sh;
			_vScroll.SmallChange = _vScroll.LargeChange / 5;
			_vScroll.Enabled = true;
			return;
		}

		private void SetScrollControlsH()
		{
			int w = PixelsX(_DrawPanel.HorizontalMax);
			int sw = this.Width - _vScroll.Width;
			if (sw > w)
			{
				_hScroll.Enabled = false;
				if (_hScroll.Value != 0)
				{
					_hScroll.Value = 0;
					_DrawPanel.Invalidate();
				}
				return;
			}
			_hScroll.Maximum = w;
			_hScroll.Minimum = 0;
			int sValue =  Math.Min(_hScroll.Value, _hScroll.Maximum);
			if (_hScroll.Value != sValue)
			{
				_hScroll.Value = sValue;
				_DrawPanel.Invalidate();
			}
			if (sw < 0)
				sw = 0;
			_hScroll.LargeChange = sw;
			_hScroll.SmallChange = _hScroll.LargeChange / 5;
			_hScroll.Enabled = true;

			return;
		}

		private void HorizontalScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			if (e.NewValue == _hScroll.Value)	// don't need to scroll if already there
				return;

			_DrawPanel.Invalidate();
            if (HorizontalScrollChanged != null)
                HorizontalScrollChanged(this, new EventArgs());
        }

		private void VerticalScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			if (e.NewValue == _vScroll.Value)	// don't need to scroll if already there
				return;

			_DrawPanel.Invalidate();
            if (VerticalScrollChanged != null)
                VerticalScrollChanged(this, new EventArgs());
		}

		private void DrawPanelMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_Undo.EndUndoGroup(true);

            if (_MouseDownNode != null && _MouseDownNode.Name == "Height")
                HeightChanged(this, new HeightEventArgs(_MouseDownNode, null));     // reset any mousemove
            
			_MouseDownNode = null;
			
			if (this._bHaveMouse)
			{	// Handle the end of the rubber banding
				_bHaveMouse = false;
				// remove last rectangle if necessary
				if (this._ptRBLast.X != -1)
				{
					this.DrawPanelRubberBand(this._ptRBOriginal, this._ptRBLast);
					// Process the rectangle
					Rectangle r = DrawPanelRectFromPoints(this._ptRBOriginal, this._ptRBLast);
					if ((Control.ModifierKeys & Keys.Control) != Keys.Control)	// we allow addition to selection
						_DrawPanel.ClearSelected();
					_DrawPanel.SelectInRectangle(r, PointsX(_hScroll.Value), PointsY(_vScroll.Value));
					SelectionChanged(this, new EventArgs());
				}
				// clear out the points for the next time
				_ptRBOriginal.X = _ptRBOriginal.Y = _ptRBLast.X = _ptRBLast.Y = -1;
			}
			else if (e.Button == MouseButtons.Right)
			{
				DrawPanelContextMenu(new Point(e.X, e.Y));
			}
			if (_AdjustScroll)
			{
				this.SetScrollControls();
				_AdjustScroll = false;
			}
		}

		private void DrawPanelContextMenu(Point p)
		{
			if (_DrawPanel.SelectedCount == 1)
			{
				XmlNode cNode = _DrawPanel.SelectedList[0];
				if (cNode.Name == "Chart")
					DrawPanelContextMenuChart(p, cNode);
				else if (cNode.Name == "Subreport")
					DrawPanelContextMenuSubreport(p, cNode);
				else if (_DrawPanel.InTable(cNode))
					DrawPanelContextMenuTable(p, cNode);
				else if (_DrawPanel.InMatrix(cNode))
					DrawPanelContextMenuMatrix(p, cNode);
				else
					DrawPanelContextMenuDefault(p);
			}
			else
				DrawPanelContextMenuDefault(p);
		}

		private void DrawPanelContextMenuChart(Point p, XmlNode riNode)
		{
			menuContext = new ContextMenu();
			menuContext.Popup +=new EventHandler(this.menuContext_Popup);

			// Get the Category Groupings
			object[] catGroupNames = _DrawPanel.GetChartCategoryGroupNames(riNode);

			MenuItem menuChartInsertCategoryGrouping = new MenuItem("Insert Category Grouping...", 
				new EventHandler(this.menuChartInsertCategoryGrouping_Click));
			MenuItem menuChartEditCategoryGrouping = new MenuItem("Edit Category Grouping");
			MenuItem menuChartDeleteCategoryGrouping = new MenuItem("Delete Category Grouping");
			if (catGroupNames != null)
			{
				foreach (string gname in catGroupNames)
				{
					menuChartEditCategoryGrouping.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuChartEditGrouping_Click)));
					menuChartDeleteCategoryGrouping.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuChartDeleteGrouping_Click)));
				}
			}
			else
			{
				menuChartEditCategoryGrouping.Enabled = false;
				menuChartDeleteCategoryGrouping.Enabled = false;
			}

			// Get the Series Groupings
			object[] serGroupNames = _DrawPanel.GetChartSeriesGroupNames(riNode);

			MenuItem menuChartInsertSeriesGrouping = new MenuItem("Insert Series Grouping...", new EventHandler(this.menuChartInsertSeriesGrouping_Click));
			MenuItem menuChartEditSeriesGrouping = new MenuItem("Edit Series Grouping");
			MenuItem menuChartDeleteSeriesGrouping = new MenuItem("Delete Series Grouping");
			if (serGroupNames != null)
			{
				foreach (string gname in serGroupNames)
				{
					menuChartEditSeriesGrouping.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuChartEditGrouping_Click)));
					menuChartDeleteSeriesGrouping.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuChartDeleteGrouping_Click)));
				}
			}
			else
			{
				menuChartEditSeriesGrouping.Enabled = false;
				menuChartDeleteSeriesGrouping.Enabled = false;
			}

			menuContext.MenuItems.AddRange(
				new MenuItem[] { 
									menuProperties, menuPropertiesLegend,menuPropertiesChartTitle,new MenuItem("-"),
								   menuChartInsertCategoryGrouping, menuChartEditCategoryGrouping,menuChartDeleteCategoryGrouping,new MenuItem("-"),
									menuPropertiesCategoryAxis, menuPropertiesCategoryAxisTitle, new MenuItem("-"),
								   menuChartInsertSeriesGrouping, menuChartEditSeriesGrouping,menuChartDeleteSeriesGrouping,new MenuItem("-"),
								   menuPropertiesValueAxis,menuPropertiesValueAxisTitle,menuPropertiesValueAxis2Title,new MenuItem("-"),
								    menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									menuSelectAll
									});
			menuContext.Show(this, p);
		}

		private void DrawPanelContextMenuDefault(Point p)
		{
			menuContext = new ContextMenu();
			menuContext.Popup +=new EventHandler(this.menuContext_Popup);
			menuContext.MenuItems.AddRange(
				new MenuItem[] {
						menuProperties,new MenuItem("-"), menuCopy, menuPaste, menuDelete, menuFSep1, 
						menuSelectAll, menuFSep2,menuInsert});
			menuContext.Show(this, p);
		}

		private void DrawPanelContextMenuMatrix(Point p, XmlNode riNode)
		{
			menuContext = new ContextMenu();
			menuContext.Popup +=new EventHandler(this.menuContext_Popup);
			// matrix menus
			MenuItem menuMatrixDelete = new MenuItem("Delete Matrix", new EventHandler(this.menuMatrixDelete_Click));
			MenuItem menuMatrixProperties = new MenuItem("Matrix Properties...", new EventHandler(this.menuMatrixProperties_Click));
			
			// Get the column groupings
			MenuItem[] cmenu;			// the ultimate context menu items
			object[] colGroupNames = _DrawPanel.GetMatrixColumnGroupNames(riNode);
			object[] rowGroupNames = _DrawPanel.GetMatrixRowGroupNames(riNode);

			MenuItem menuMatrixInsertColumnGroup = new MenuItem("Insert Column Group...", new EventHandler(this.menuMatrixInsertColumnGroup_Click));
			MenuItem menuMatrixEditColumnGroup= new MenuItem("Edit Column Group");
			MenuItem menuMatrixDeleteColumnGroup= new MenuItem("Delete Column Group");
			if (colGroupNames != null)
			{
				foreach (string gname in colGroupNames)
				{
					menuMatrixEditColumnGroup.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuMatrixEditGroup_Click)));
					menuMatrixDeleteColumnGroup.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuMatrixDeleteGroup_Click)));
				}
			}
			else
			{
				menuMatrixEditColumnGroup.Enabled = false;
				menuMatrixDeleteColumnGroup.Enabled = false;
			}
			MenuItem menuMatrixInsertRowGroup = new MenuItem("Insert Row Group...", new EventHandler(this.menuMatrixInsertRowGroup_Click));
			MenuItem menuMatrixEditRowGroup= new MenuItem("Edit Row Group");
			MenuItem menuMatrixDeleteRowGroup= new MenuItem("Delete Row Group");
			if (rowGroupNames != null)
			{
				foreach (string gname in rowGroupNames)
				{
					menuMatrixEditRowGroup.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuMatrixEditGroup_Click)));
					menuMatrixDeleteRowGroup.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuMatrixDeleteGroup_Click)));
				}
			}
			else
			{
				menuMatrixEditRowGroup.Enabled = false;
				menuMatrixDeleteRowGroup.Enabled = false;
			}
			cmenu = new MenuItem[] {   menuProperties, menuMatrixProperties, new MenuItem("-"),
									   menuMatrixInsertColumnGroup, menuMatrixEditColumnGroup, menuMatrixDeleteColumnGroup, new MenuItem("-"),
									   menuMatrixInsertRowGroup, menuMatrixEditRowGroup, menuMatrixDeleteRowGroup, new MenuItem("-"),
									   menuMatrixDelete, new MenuItem("-"), 
									   menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									   menuSelectAll };

			menuContext.MenuItems.AddRange(cmenu);
			menuContext.Show(this, p);
		}

		private void DrawPanelContextMenuSubreport(Point p, XmlNode sr)
		{
			menuContext = new ContextMenu();
			menuContext.Popup +=new EventHandler(this.menuContext_Popup);

			// get the subreport name
			string name = _DrawPanel.GetElementValue(sr, "ReportName", "");
			if (name == null || name.Length == 0)
			{	// No name; no way to open the subreport
				menuContext.MenuItems.AddRange(
					new MenuItem[] {
									   menuProperties, 
									   new MenuItem("-"), menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									   menuSelectAll, menuFSep2,menuInsert});
			}
			else
			{
				string srmi = "Open " + name;

				menuContext.MenuItems.AddRange(
					new MenuItem[] {
									   menuProperties, new MenuItem(srmi, new EventHandler(this.menuOpenSubreport_Click)),
									   new MenuItem("-"), menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									   menuSelectAll, menuFSep2,menuInsert});
			}
			menuContext.Show(this, p);
		}

		private void DrawPanelContextMenuTable(Point p, XmlNode riNode)
		{
			menuContext = new ContextMenu();
			menuContext.Popup +=new EventHandler(this.menuContext_Popup);
			// table menus
            bool bTable = !_DrawPanel.InGrid(riNode);
            string stype = bTable ? "Table" : "Grid";

			MenuItem menuTableInsertRowBefore = new MenuItem("Insert Row Before", new EventHandler(this.menuTableInsertRowBefore_Click));
			MenuItem menuTableInsertRowAfter = new MenuItem("Insert Row After", new EventHandler(this.menuTableInsertRowAfter_Click));
			MenuItem menuTableDeleteRow = new MenuItem("Delete "+ stype + " Row", new EventHandler(this.menuTableDeleteRow_Click));
			MenuItem menuTableInsertColumnBefore = new MenuItem("Insert Column Before", new EventHandler(this.menuTableInsertColumnBefore_Click));
			MenuItem menuTableInsertColumnAfter = new MenuItem("Insert Column After", new EventHandler(this.menuTableInsertColumnAfter_Click));
			MenuItem menuTableInsertTableGroup = new MenuItem("Insert "+stype+" Group...", new EventHandler(this.menuTableInsertGroup_Click));
			MenuItem menuTableDeleteColumn = new MenuItem("Delete "+stype+" Column", new EventHandler(this.menuTableDeleteColumn_Click));
			MenuItem menuTableDelete = new MenuItem("Delete "+stype, new EventHandler(this.menuTableDelete_Click));
			MenuItem menuTableProperties = new MenuItem(stype+" Properties...", new EventHandler(this.menuTableProperties_Click));

			// the replace items
			MenuItem menuReplTextbox = new MenuItem("&Textbox", new EventHandler(this.menuInsertTextbox_Click));
			MenuItem menuReplRectangle = new MenuItem("&Rectangle", new EventHandler(this.menuInsertRectangle_Click));
			MenuItem menuReplImage = new MenuItem("&Image", new EventHandler(this.menuInsertImage_Click));
			MenuItem menuReplSubreport = new MenuItem("&Subreport", new EventHandler(this.menuInsertSubreport_Click));
			MenuItem menuReplList = new MenuItem("&List", new EventHandler(this.menuInsertList_Click));
			MenuItem menuReplMatrix = new MenuItem("&Matrix...", new EventHandler(this.menuInsertMatrix_Click));
			MenuItem menuReplTable = new MenuItem("Ta&ble...", new EventHandler(this.menuInsertTable_Click));
			MenuItem menuReplChart = new MenuItem("&Chart...", new EventHandler(this.menuInsertChart_Click));
			MenuItem menuRepl = new MenuItem("&Replace Cell with");
			menuRepl.MenuItems.AddRange(new MenuItem[] { menuReplChart, menuReplImage,
															 menuReplList, menuReplMatrix, 
															 menuReplRectangle, menuReplSubreport,
															 menuReplTable, menuReplTextbox});
			
			// If there are any TableGroups then we need menu items to manipulate them
			MenuItem[] cmenu;			// the ultimate context menu items
			object[] tblGroupNames = _DrawPanel.GetTableGroupNames(riNode);
            if (!bTable)
            {
				cmenu = new MenuItem[] {   menuProperties, menuTableProperties, menuRepl, new MenuItem("-"),
									   menuTableInsertColumnBefore, menuTableInsertColumnAfter, new MenuItem("-"),
									   menuTableInsertRowBefore, menuTableInsertRowAfter, new MenuItem("-"),
									   menuTableDeleteColumn, menuTableDeleteRow, menuTableDelete, new MenuItem("-"), 
									   menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									   menuSelectAll };
            }
			else if (tblGroupNames == null)
			{   // Don't need menus for manipulating existing TableGroups
				cmenu = new MenuItem[] {   menuProperties, menuTableProperties, menuRepl, new MenuItem("-"),
									   menuTableInsertColumnBefore, menuTableInsertColumnAfter, new MenuItem("-"),
									   menuTableInsertRowBefore, menuTableInsertRowAfter, new MenuItem("-"),
									   menuTableInsertTableGroup, new MenuItem("-"), 
									   menuTableDeleteColumn, menuTableDeleteRow, menuTableDelete, new MenuItem("-"), 
									   menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									   menuSelectAll };
			}
			else
			{
				MenuItem menuTableEditGroup= new MenuItem("Edit Group");
				MenuItem menuTableDeleteGroup= new MenuItem("Delete Group");
				foreach (string gname in tblGroupNames)
				{
					menuTableEditGroup.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuTableEditGroup_Click)));
					menuTableDeleteGroup.MenuItems.Add(new MenuItem(gname, new EventHandler(this.menuTableDeleteGroup_Click)));
				}
				cmenu = new MenuItem[] {   menuProperties, menuTableProperties, menuRepl, new MenuItem("-"),
									   menuTableInsertColumnBefore, menuTableInsertColumnAfter, new MenuItem("-"),
									   menuTableInsertRowBefore, menuTableInsertRowAfter, new MenuItem("-"),
									   menuTableInsertTableGroup,menuTableEditGroup,menuTableDeleteGroup, new MenuItem("-"), 
									   menuTableDeleteColumn, menuTableDeleteRow, menuTableDelete, new MenuItem("-"), 
									   menuCopy, menuPaste, menuDelete, new MenuItem("-"), 
									   menuSelectAll };
			}
			menuContext.MenuItems.AddRange(cmenu);
			menuContext.Show(this, p);
		}

		private void DrawPanelMouseMove(object sender, MouseEventArgs e)
		{
            XmlNode b=null;					
			HitLocationEnum hle = HitLocationEnum.Inside;
			Point newMousePosition = new Point(e.X, e.Y);

			if (_bHaveMouse)
			{	// we're rubber banding
				// If we drew previously; we'll draw again to remove old rectangle
				if( this._ptRBLast.X != -1 )
				{
					this.DrawPanelRubberBand( this._ptRBOriginal, _ptRBLast );
				}
				_MousePosition = newMousePosition;
				// Update last point;  but don't rubber band outside our client area
				if (newMousePosition.X < 0)
					newMousePosition.X = 0;
				if (newMousePosition.X > _DrawPanel.Width)
					newMousePosition.X = _DrawPanel.Width;
				if (newMousePosition.Y < 0)
					newMousePosition.Y = 0;
				if (newMousePosition.Y > _DrawPanel.Height)
					newMousePosition.Y = _DrawPanel.Height;
				_ptRBLast = newMousePosition;
				if (_ptRBLast.X < 0)
					_ptRBLast.X = 0;
				if (_ptRBLast.Y < 0)
					_ptRBLast.Y = 0;
				// Draw new lines.
				this.DrawPanelRubberBand( _ptRBOriginal, newMousePosition );
				this.Cursor = Cursors.Cross;		// use cross hair to indicate drawing
				return;
			}
			else if (_MouseDownNode != null)
			{  
				if (e.Button != MouseButtons.Left)
					b = _MouseDownNode;
				else 
				{
					b = _MouseDownNode;
					switch (_MouseDownNode.Name)
					{
						case "TableColumn":
						case "RowGrouping":
						case "MatrixColumn":
							hle = HitLocationEnum.TableColumnResize;
							if (e.X == _MousePosition.X)
								break;

							if (_DrawPanel.TableColumnResize(_MouseDownNode, e.X - _MousePosition.X))
							{
								SelectionMoved(this, new EventArgs());
								ReportChanged(this, new EventArgs());
								_AdjustScroll = true;
								_DrawPanel.Invalidate();   
							}
							else	// trying to drag into invalid area; disallow
							{
								Cursor.Position = this.PointToScreen(_MousePosition);
								newMousePosition = this.PointToClient(Cursor.Position);
							}
							break;
						case "TableRow":
						case "ColumnGrouping":
						case "MatrixRow":
							hle = HitLocationEnum.TableRowResize;
							if (e.Y == _MousePosition.Y)
								break;
							if (_DrawPanel.TableRowResize(_MouseDownNode, e.Y - _MousePosition.Y))
							{
								SelectionMoved(this, new EventArgs());
								ReportChanged(this, new EventArgs());
								_DrawPanel.Invalidate();   
							}
							else	// trying to drag into invalid area; disallow
							{
								Cursor.Position = this.PointToScreen(_MousePosition);
								newMousePosition = this.PointToClient(Cursor.Position);
							}
							break;
						case "Height":
							if (e.Y == _MousePosition.Y)
								break;
							if (_DrawPanel.ChangeHeight(_MouseDownNode, e.Y - _MousePosition.Y, 0))
							{
								ReportChanged(this, new EventArgs());
                                HeightChanged(this, new HeightEventArgs(_MouseDownNode, b.InnerText));
								_DrawPanel.Invalidate();   
								_AdjustScroll = true;		// this will force scroll bars to be adjusted on MouseUp
							}
							else	// trying to drag into invalid area; disallow
							{
								Cursor.Position = this.PointToScreen(_MousePosition);
								newMousePosition = this.PointToClient(Cursor.Position);
							}
                            // Force scroll when off end of page
                            //if (e.Y > _DrawPanel.Height)
                            //{
                            //    int hs = _vScroll.Value + _vScroll.SmallChange;
                            //    _vScroll.Value = Math.Min(_vScroll.Maximum, hs);
                            //    _DrawPanel.Refresh();
                            //}

                            break;
						case "Textbox":
						case "Image":
						case "Rectangle":
						case "List":
						case "Table":
                        case "fyi:Grid":
						case "Matrix":
						case "Chart":
						case "Subreport":
						case "Line":
                        case "CustomReportItem":
							hle = this._MouseDownLoc;
							if (e.Y == _MousePosition.Y && e.X == _MousePosition.X)
								break;

							if (_DrawPanel.MoveSelectedItems(e.X - _MousePosition.X, e.Y - _MousePosition.Y, this._MouseDownLoc))
							{
								SelectionMoved(this, new EventArgs());
								ReportChanged(this, new EventArgs());
								_DrawPanel.Invalidate();   
								_AdjustScroll = true;
							}
							else	// trying to drag into invalid area; disallow
							{
								Cursor.Position = this.PointToScreen(_MousePosition);
								newMousePosition = this.PointToClient(Cursor.Position);
							}

							break;
					}
				}
			}
			else
			{
				HitLocation hl = _DrawPanel.HitNode(newMousePosition, PointsX(_hScroll.Value), PointsY(_vScroll.Value));
				if (hl != null)
				{
					b = hl.HitNode;
					hle = hl.HitSpot;
				}
			}

			_MousePosition = newMousePosition;
			DrawPanelSetCursor(b, hle);
		}

		private void DrawPanelMouseDown(object sender, MouseEventArgs e)
		{
			_MousePosition = new Point(e.X, e.Y);		
			HitLocation hl = _DrawPanel.HitNode(_MousePosition, PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			_MouseDownNode = hl == null? null: hl.HitNode;
			_MouseDownLoc = hl == null? HitLocationEnum.Inside: hl.HitSpot;

			if (DrawPanelMouseDownInsert(hl, sender, e))		// Handle ReportItem insertion
				return;

			if (e.Button == MouseButtons.Left)
				_Undo.StartUndoGroup("Move/Size");

			if (DrawPanelMouseDownRubberBand(sender, e))	// Handle rubber banding
				return;

			if (DrawPanelMouseDownTableColumnResize(sender, e))	// Handle column resize
				return;

			if (DrawPanelMouseDownTableRowResize(sender, e))	// Handle row resize
				return;
			
			if (_MouseDownNode.Name == "Height")
			{
				_DrawPanel.ClearSelected();
				SelectionChanged(this, new EventArgs());
                HeightChanged(this, new HeightEventArgs(_MouseDownNode, _MouseDownNode.InnerText)); // Set the height
            }
			else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
			{
				_DrawPanel.AddRemoveSelection(_MouseDownNode);
				SelectionChanged(this, new EventArgs());
			}
			else
			{
				_DrawPanel.SetSelection(_MouseDownNode, true);
				SelectionChanged(this, new EventArgs());
			}

			DrawPanelSetCursor(_MouseDownNode, hl.HitSpot);
		}

        internal void SetSelection(XmlNode node)
        {
            if (node == null)
                _DrawPanel.ClearSelected();
            else
                _DrawPanel.SetSelection(node, false);
            SelectionChanged(this, new EventArgs());
        }

		private bool DrawPanelMouseDownRubberBand(object sender, MouseEventArgs e)
		{
			if (_MouseDownLoc != HitLocationEnum.Inside)
				return false;				// must hit inside a region

			// Now determine if object hit allows for rubber banding
			bool bRubber = false;
			bool bDeselect = true;
			if (_MouseDownNode == null)
				bRubber = true;
			else if (_MouseDownNode.Name == "List" || _MouseDownNode.Name == "Rectangle")
			{
				if (_DrawPanel.SelectedCount == 1 && _DrawPanel.IsNodeSelected(_MouseDownNode))
				{
					bRubber = true;
					bDeselect = false;
				}
			}
			else if (_MouseDownNode.Name == "Body" ||
				_MouseDownNode.Name == "PageHeader" ||
				_MouseDownNode.Name == "PageFooter")
				bRubber = true;
			
			if (!bRubber)
				return false;

			// We have a rubber band operation
			if (e.Button != MouseButtons.Left)
			{
				if (bDeselect)
				{
					_DrawPanel.ClearSelected();
					SelectionChanged(this, new EventArgs());
				}
				return true;		// well no rubber band but it's been handled
			}

			if ((Control.ModifierKeys & Keys.Control) != Keys.Control)	// we allow addition to selection
			{
				if (bDeselect)
				{
					_DrawPanel.ClearSelected();
					SelectionChanged(this, new EventArgs());
				}
			}
			_bHaveMouse = true;
			// keep the starting point of the rectangular rubber band
			this._ptRBOriginal.X = e.X;
			this._ptRBOriginal.Y = e.Y;
			// -1 indicates no previous rubber band
			this._ptRBLast.X = this._ptRBLast.Y = -1;
			this.Cursor = Cursors.Cross;		// use cross hair to indicate drawing

			return true;
		}

		private bool DrawPanelMouseDownTableColumnResize(object sender, MouseEventArgs e)
		{
			if (_MouseDownNode == null ||							
				_MouseDownLoc != HitLocationEnum.TableColumnResize)
				return false;
			this.Cursor = Cursors.VSplit;
			return true;
		}

		private bool DrawPanelMouseDownTableRowResize(object sender, MouseEventArgs e)
		{
			if (_MouseDownNode == null ||							
				_MouseDownLoc != HitLocationEnum.TableRowResize)
				return false;
			this.Cursor = Cursors.HSplit;
			return true;
		}

		private bool DrawPanelMouseDownInsert(HitLocation hl, object sender, MouseEventArgs e)
		{
			if (!(_CurrentInsert != null &&		// should we be inserting?
				_MouseDownNode != null &&			
				(_MouseDownNode.Name == "List" ||
				_MouseDownNode.Name == "Rectangle" ||
				_MouseDownNode.Name == "Body" ||
				_MouseDownNode.Name == "PageHeader" ||
				_MouseDownNode.Name == "PageFooter")))
			{
				if (_CurrentInsert == null || _CurrentInsert == "Line" || hl == null || 
						hl.HitContainer == null || (!(hl.HitContainer.Name == "Table" || hl.HitContainer.Name=="fyi:Grid")))
				   return false;
				
				if (MessageBox.Show("Do you want to replace contents of TableCell?", "Insert", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return false;
			}
			switch (_CurrentInsert)
			{
				case "Textbox":
					menuInsertTextbox_Click(sender, e);
					break;
				case "Chart":
					menuInsertChart_Click(sender, e);
					break;
				case "Rectangle":
					menuInsertRectangle_Click(sender, e);
					break;
				case "Table":
                case "fyi:Grid":
                    menuInsertTable_Click(sender, e);
					break;
                case "Matrix":
					menuInsertMatrix_Click(sender, e);
					break;
				case "List":
					menuInsertList_Click(sender, e);
					break;
				case "Line":
					menuInsertLine_Click(sender, e);
					break;
				case "Image":
					menuInsertImage_Click(sender, e);
					break;
				case "Subreport":
					menuInsertSubreport_Click(sender, e);
					break;
				default:
					break;
			}
			return true;
		}

		private void DrawPanelDoubleClick(object sender, EventArgs e)
		{
			menuProperties_Click();		// treat double click like a property menu click
		}

		private void DrawPanelRubberBand(Point p1, Point p2)
		{
			// Convert the points to screen coordinates
			p1 = PointToScreen(p1);
			p2 = PointToScreen(p2);
			
			// Get a rectangle from the two points
			Rectangle rc = DrawPanelRectFromPoints(p1, p2);

			// Draw reversibleFrame
			ControlPaint.DrawReversibleFrame(rc, Color.Red,	FrameStyle.Dashed);

			return;
		}

		private Rectangle DrawPanelRectFromPoints(Point p1, Point p2)
		{
			Rectangle r = new Rectangle();
			// set the width and x of rectangle
			if (p1.X < p2.X)
			{
				r.X = p1.X;
				r.Width = p2.X - p1.X;
			}
			else
			{
				r.X = p2.X;
				r.Width = p1.X - p2.X;
			}
			// set the height and y of rectangle
			if (p1.Y < p2.Y)
			{
				r.Y = p1.Y;
				r.Height = p2.Y - p1.Y;
			}
			else
			{
				r.Y = p2.Y;
				r.Height = p1.Y - p2.Y;
			}
			return r;
		}

		private void DrawPanelSetCursor(XmlNode node, HitLocationEnum hle)
		{
			Cursor c;
			if (node == null)
				c = Cursors.Arrow;
			else if (node.Name == "Height")
				c = Cursors.SizeNS;
			else if (hle == HitLocationEnum.TableColumnResize)	// doesn't need to be selected
				c = Cursors.VSplit;
			else if (hle == HitLocationEnum.TableRowResize)		// doesn't need to be selected 
				c = Cursors.HSplit;
			else if (this._DrawPanel.IsNodeSelected(node))
			{
				switch (hle)
				{
					case HitLocationEnum.BottomLeft:
					case HitLocationEnum.TopRight:
						c = Cursors.SizeNESW;
						break;
					case HitLocationEnum.LeftMiddle:
					case HitLocationEnum.RightMiddle:
						c = Cursors.SizeWE;
						break;
					case HitLocationEnum.BottomRight:
					case HitLocationEnum.TopLeft:
						c = Cursors.SizeNWSE;
						break;
					case HitLocationEnum.TopMiddle:
					case HitLocationEnum.BottomMiddle:
						c = Cursors.SizeNS;
						break;
					case HitLocationEnum.Move:
						c = Cursors.SizeAll;
						break;
					case HitLocationEnum.TableColumnResize:
						c = Cursors.VSplit;
						break;
					case HitLocationEnum.TableRowResize:
						c = Cursors.HSplit;
						break;
					case HitLocationEnum.LineLeft:
					case HitLocationEnum.LineRight:
				//		c = Cursors.Cross;			bug in C# runtime? Cross doesn't work!
						c = Cursors.Hand;
						break;
					case HitLocationEnum.Inside:
					default:
						c = Cursors.Arrow;
						break;
				}
			}
			else
				c = Cursors.Arrow;

			if (c != this.Cursor)
				this.Cursor = c;
		}

		private void DrawPanelMouseWheel(object sender, MouseEventArgs e)
		{
            if (!_vScroll.Enabled)
                return;                 // scroll not enabled
			int wvalue;
            bool bNotify = false;
			if (e.Delta < 0)
			{
				if (_vScroll.Value < _vScroll.Maximum)
				{
					wvalue = _vScroll.Value + _vScroll.SmallChange;

					_vScroll.Value = Math.Min(_vScroll.Maximum, wvalue);
                    bNotify = true;
				}
			}
			else 
			{
				if (_vScroll.Value > _vScroll.Minimum)
				{
					wvalue = _vScroll.Value - _vScroll.SmallChange;

					_vScroll.Value = Math.Max(_vScroll.Minimum, wvalue);
                    bNotify = true;
				}
			}
            if (bNotify)
            {
                if (VerticalScrollChanged != null)
                    VerticalScrollChanged(this, new EventArgs());
                _DrawPanel.Refresh();
            }
		}

		private void DrawPanelKeyDown(object sender, KeyEventArgs e)
		{
			int incX=0;
			int incY=0;
			int vScroll=_vScroll.Value;
			int hScroll=_hScroll.Value;

			// Force scroll up and down
			if (e.KeyCode == Keys.Down)  
			{
				incY=1;
			}
			else if (e.KeyCode == Keys.Up)
			{
				incY=-1;
			}
			else if (e.KeyCode == Keys.Left)
			{
				incX=-1;
			}
			else if (e.KeyCode == Keys.Right)
			{
				incX=1;
			}
			else if (e.KeyCode == Keys.PageDown)
			{
				vScroll = Math.Min(_vScroll.Value + _vScroll.LargeChange, _vScroll.Maximum);
			}
			else if (e.KeyCode == Keys.PageUp)
			{
				vScroll = Math.Max(_vScroll.Value - _vScroll.LargeChange, 0);
			}
			else if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				menuProperties_Click();
				return;
			}
			else if (e.KeyCode == Keys.Tab)
			{
				if (_DrawPanel.SelectNext((Control.ModifierKeys & Keys.Shift) == Keys.Shift))
				{
					RectangleF r = _DrawPanel.GetRectangle(_DrawPanel.SelectedList[0]);
					Rectangle nr = new Rectangle(PixelsX(r.X), PixelsY(r.Y), PixelsX(r.Width), PixelsY(r.Height));
					if (nr.Right > _hScroll.Value + Width - _vScroll.Width ||
						nr.Left < _hScroll.Value - _vScroll.Width)
						hScroll = Math.Min(nr.Left, _hScroll.Maximum);
					if (nr.Bottom > _vScroll.Value + Height - _hScroll.Height ||
						nr.Top < _vScroll.Value - _hScroll.Height)
						vScroll = Math.Min(nr.Top, _vScroll.Maximum);
					this.SelectionChanged(this, new EventArgs());
					_DrawPanel.Invalidate();   
				}
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Delete)
			{
				this.Delete();
				e.Handled = true;
			}
			
			bool bRefresh = false;
			if (vScroll != _vScroll.Value && _vScroll.Enabled)
			{
				_vScroll.Value = Math.Max(vScroll, 0);
				bRefresh = true;
				e.Handled = true;
			}

			if (hScroll != _hScroll.Value && _hScroll.Enabled)
			{
				_hScroll.Value = Math.Max(hScroll,0);
				bRefresh = true;
				e.Handled = true;
			}

			if (incX != 0 || incY != 0)
			{
				HitLocationEnum hle = HitLocationEnum.Move;
				if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)	// if shift key on resize
				{
					hle = incX != 0? HitLocationEnum.RightMiddle: HitLocationEnum.BottomMiddle;
				}
				_Undo.StartUndoGroup("Move");
				if (_DrawPanel.MoveSelectedItems(incX, incY, hle))
				{
					_Undo.EndUndoGroup(true);
					SelectionMoved(this, new EventArgs());
					ReportChanged(this, new EventArgs());
					_DrawPanel.Invalidate();   
				}
				else
					_Undo.EndUndoGroup(false);
				e.Handled = true;
			}

			if (bRefresh)
				_DrawPanel.Refresh();
		}

		private void DesignCtl_Layout(object sender, LayoutEventArgs e)
		{
			_DrawPanel.Location = new Point(0, 0);
			_DrawPanel.Width = this.Width - _vScroll.Width;
			_DrawPanel.Height = this.Height - _hScroll.Height;
			_hScroll.Location = new Point(0, this.Height - _hScroll.Height);
			_hScroll.Width = _DrawPanel.Width;
			_vScroll.Location = new Point(this.Width - _vScroll.Width, _DrawPanel.Location.Y);
			_vScroll.Height = _DrawPanel.Height;

		}

		
		private void menuCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetDataObject(GetCopy(), true);
		}
		
		private string GetCopy()
		{
            // When copying multiple report items that are in the same table/matrix 
            //   ask if you want to instead copy the table/matrix
            if (_DrawPanel.SelectedCount > 1)
            {
                XmlNode tn = _DrawPanel.TMParent(_DrawPanel.SelectedList[0]);
                bool bTorM = tn != null;

                for (int i=1; i < _DrawPanel.SelectedCount && bTorM; i++)
                {
                    if (tn != _DrawPanel.TMParent(_DrawPanel.SelectedList[i]))
                        bTorM = false;
                }
                if (bTorM)
                {   // all selected items are in the same table or matrix
                    if (MessageBox.Show(string.Format("Do you want to select the {0}?", tn.Name),
                        "Copy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        StringBuilder tb = new StringBuilder();
                        // Build XML representing the selected objects
                        tb.Append("<ReportItems>");		// surround by ReportItems element
                        tb.Append(tn.OuterXml);
                        tb.Append("</ReportItems>");
                        return tb.ToString();
                    }
                }
            }


			StringBuilder sb = new StringBuilder();
			// Build XML representing the selected objects
			sb.Append("<ReportItems>");		// surround by ReportItems element
			foreach (XmlNode riNode in _DrawPanel.SelectedList)
			{
				sb.Append(riNode.OuterXml);	
			}
			sb.Append("</ReportItems>");
			return sb.ToString();
		}
				
		private void menuPaste_Click(object sender, EventArgs e)
		{
			HitLocation hl = _DrawPanel.HitNode(_MousePosition, PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			XmlNode lNode = hl == null? null: hl.HitNode;

			if (lNode == null)
				return;
			if (!(lNode.Name == "List" ||
				lNode.Name == "Body" ||
				lNode.Name == "Rectangle" ||
				lNode.Name == "PageHeader" ||
				lNode.Name == "PageFooter"))
			{
				if (hl.HitContainer != null && (hl.HitContainer.Name == "Table" || hl.HitContainer.Name == "fyi:Grid"))
				{
				//	When table we need to replace the tablecell contents; ask first
					if (MessageBox.Show("Do you want to replace contents of TableCell?", "Paste", MessageBoxButtons.YesNo) != DialogResult.Yes)
						return;

					XmlNode repItems = lNode.ParentNode;
					if (repItems.Name != "ReportItems")
						return;

					XmlNode p = repItems.ParentNode;
					p.RemoveChild(repItems);

					DoPaste(p, hl.HitRelative);
				}
				return;
			}

			DoPaste(lNode, hl.HitRelative);
		}

		private void DoPaste(XmlNode lNode, PointF p)
		{
			IDataObject iData = Clipboard.GetDataObject();
			if (iData == null)
				return;
			if (!(iData.GetDataPresent(DataFormats.Text) ||
				  iData.GetDataPresent(DataFormats.Bitmap))) 
				return;

			if (lNode == null)
			{
				lNode = _DrawPanel.Body;
				if (lNode == null)
					return;
			}

			_Undo.StartUndoGroup("Paste");
			if (iData.GetDataPresent(DataFormats.Text))
			{
				// Build the xml string in case it is a straight pasting of text
				string t = (string) iData.GetData(DataFormats.Text);
				if (!(t.Length >=	27 && t.Substring(0, 13) == "<ReportItems>"))
				{
					t = t.Replace("&", "&amp;");
					t = t.Replace("<", "&lt;");
					t = string.Format("<ReportItems><Textbox><Height>12pt</Height><Width>1in</Width><Value>{0}</Value></Textbox></ReportItems>", t);
				}
				// PasteReportItems throws exception when you try to paste an illegal object
				try	
				{
					_DrawPanel.PasteReportItems(lNode, t, p);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Paste");
				}
			}
			else
			{
				// Build the xml string for an image; but we also need to put an
				//   embedded image into the report.
				System.Drawing.Bitmap bo = (System.Drawing.Bitmap) iData.GetData(DataFormats.Bitmap);

				_DrawPanel.PasteImage(lNode, bo, p);
			}
			_Undo.EndUndoGroup();
			_DrawPanel.Invalidate();
			ReportChanged(this, new EventArgs());
			SelectionChanged(this, new EventArgs());
		}
				
		private void menuDelete_Click(object sender, EventArgs e)
		{
			Delete();
		}
	
		private void menuInsertChart_Click(object sender, EventArgs e)
		{
			HitLocation hl = _DrawPanel.HitContainer(_MousePosition, 
				PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			if (hl == null || hl.HitContainer == null)
				return;

			// Charts aren't allowed in PageHeader or PageFooter
			if (_DrawPanel.InPageHeaderOrFooter(hl.HitContainer))
			{
				MessageBox.Show("Charts can only be inserted in the body of the report.", "Insert");
				return;
			}

			_Undo.StartUndoGroup("Insert Chart");
			DialogNewChart dnc = new DialogNewChart(this._DrawPanel, hl.HitContainer);
            try
            {
                DialogResult dr = dnc.ShowDialog(this);
                if (dr != DialogResult.OK)
                {
                    _Undo.EndUndoGroup(false);
                    return;
                }
            }
            finally
            {
                dnc.Dispose();
            }
			XmlNode chart;
			if (hl.HitContainer.Name == "Table" || hl.HitContainer.Name == "fyi:Grid")
			{
				chart = _DrawPanel.ReplaceTableMatrixOrChart(hl, dnc.ChartXml);
			}
			else
				chart = _DrawPanel.PasteTableMatrixOrChart(hl.HitContainer, dnc.ChartXml, hl.HitRelative);

			if (chart == null)
			{
				_Undo.EndUndoGroup(false);
				return;
			}
			_Undo.EndUndoGroup(true);
			ReportChanged(this, new EventArgs());
			SelectionChanged(this, new EventArgs());
			ReportItemInserted(this, new EventArgs());
			_DrawPanel.Invalidate();   

			// Now bring up the property dialog
            //List<XmlNode> ar = new List<XmlNode>();
            //ar.Add(chart);
            //_Undo.StartUndoGroup("Dialog");
            //PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.ReportItems);
            //dr = pd.ShowDialog(this);
            //if (pd.Changed || dr == DialogResult.OK)
            //{
            //    _Undo.EndUndoGroup(true);
            //    ReportChanged(this, new EventArgs());
            //    _DrawPanel.Invalidate();   
            //}
            //else
            //    _Undo.EndUndoGroup(false);

			SetFocus();
		}
	
		private void menuInsertCustomReportItem_Click(object sender, EventArgs e)
		{
			string ri;
            ICustomReportItem cri = null;
            try
            {
                MenuItem mi = sender as MenuItem;

                cri = RdlEngineConfig.CreateCustomReportItem((string) mi.Tag);

                string criXml = cri.GetCustomReportItemXml().Trim();
                if (!(criXml.StartsWith("<CustomReportItem>") && criXml.EndsWith("</CustomReportItem>")))
                {
                    MessageBox.Show(
                        string.Format("CustomReportItem {0} method GetCustomReportItemXml must return XML enclosed by <CustomReportItem> and </CustomReportItem>\r\n\r\nXML in error:\r\n{1}", mi.Tag.ToString(), criXml), "Insert");
                    return;
                }
                ri = "<ReportItems>" +
                    string.Format(criXml, mi.Tag) +     // substitute the type of custom report item
                    "</ReportItems>";
            }
            catch (Exception ex)
            {
                MessageBox.Show(string .Format("Exception building CustomReportItem insert: {0}", ex.Message), "Insert");
                return;
            }
            finally
            {
                if (cri != null)
                    cri.Dispose();
            }
            menuInsertReportItem(sender, e, ri);
		}

        private void menuInsertGrid_Click(object sender, EventArgs e)
        {
            string ri = @"<ReportItems>"+
"<fyi:Grid xmlns:fyi=\"http://www.fyireporting.com/schemas\" Name=\"Grid1\">" +
        @"<Style>
          <BorderStyle>
            <Default>Solid</Default>
          </BorderStyle>
          <BorderColor />
          <BorderWidth />
        </Style>
        <TableColumns>
          <TableColumn>
            <Width>83.7pt</Width>
          </TableColumn>
          <TableColumn>
            <Width>92.7pt</Width>
          </TableColumn>
          <TableColumn>
            <Width>93.7pt</Width>
          </TableColumn>
        </TableColumns>
        <Header>
          <TableRows>
            <TableRow>
              <Height>12 pt</Height>
              <TableCells>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox3'>
                      <Value>Grid Column 1</Value>
                      <Style>
                        <TextAlign>Center</TextAlign>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                        <FontWeight>Bold</FontWeight>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox4'>
                      <Value>Grid Column 2</Value>
                      <Style>
                        <TextAlign>Center</TextAlign>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                        <FontWeight>Bold</FontWeight>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox9'>
                      <Value>Grid Column 3</Value>
                      <Style>
                        <TextAlign>Center</TextAlign>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                        <FontWeight>Bold</FontWeight>
                      </Style>
                    </Textbox>
                  </ReportItems>
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
                  <ReportItems>
                    <Textbox Name='Textbox10'>
                      <Value>r1c1</Value>
                      <CanGrow>true</CanGrow>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox11'>
                      <Value>r1c2</Value>
                      <CanGrow>true</CanGrow>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox12'>
                      <Value>r1c3</Value>
                      <CanGrow>true</CanGrow>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
              </TableCells>
            </TableRow>
            <TableRow>
              <Height>12 pt</Height>
              <TableCells>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox13'>
                      <Value>r2c1</Value>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox14'>
                      <Value>r2c2</Value>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                        <BorderColor />
                        <BorderWidth />
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox15'>
                      <Value>r2c3</Value>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                    </Textbox>
                  </ReportItems>
                </TableCell>
              </TableCells>
            </TableRow>
          </TableRows>
        </Details>
        <Left>515.68pt</Left>
        <Top>11.34pt</Top>
        <Footer>
          <TableRows>
            <TableRow>
              <Height>.2in</Height>
              <TableCells>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox16'>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                      <Value>Footer 1</Value>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox17'>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                      <Value>
                      </Value>
                    </Textbox>
                  </ReportItems>
                </TableCell>
                <TableCell>
                  <ReportItems>
                    <Textbox Name='Textbox18'>
                      <Style>
                        <BorderStyle>
                          <Default>Solid</Default>
                        </BorderStyle>
                      </Style>
                      <Value>
                      </Value>
                    </Textbox>
                  </ReportItems>
                </TableCell>
              </TableCells>
            </TableRow>
          </TableRows>
        </Footer>
      </fyi:Grid>
                </ReportItems>";
            menuInsertReportItem(sender, e, ri);
        }

		private void menuInsertLine_Click(object sender, EventArgs e)
		{
			string ri = "<ReportItems><Line><Height>0in</Height><Width>1in</Width><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></Line></ReportItems>";
			menuInsertReportItem(sender, e, ri);
		}
	
		private void menuInsertList_Click(object sender, EventArgs e)
		{
			string ri = "<ReportItems><List><Height>1.5in</Height><Width>1.5in</Width></List></ReportItems>";

			// Lists aren't allowed in PageHeader or PageFooter
			HitLocation hl = _DrawPanel.HitContainer(_MousePosition, 
				PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			if (hl == null || hl.HitContainer == null)
				return;
			if (_DrawPanel.InPageHeaderOrFooter(hl.HitContainer))
			{
				MessageBox.Show("Lists can only be inserted in the body of the report.", "Insert");
				return;
			}
			menuInsertReportItem(hl, ri);
		}
	
		private void menuInsertImage_Click(object sender, EventArgs e)
		{
			string ri = "<ReportItems><Image><Height>1.5in</Height><Width>1.5in</Width></Image></ReportItems>";
			menuInsertReportItem(sender, e, ri);
		}
			
		private void menuInsertMatrix_Click(object sender, EventArgs e)
		{
			HitLocation hl = _DrawPanel.HitContainer(_MousePosition, 
				PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			if (hl == null || hl.HitContainer == null)
				return;

			// Matrixs aren't allowed in PageHeader or PageFooter
			if (_DrawPanel.InPageHeaderOrFooter(hl.HitContainer))
			{
				MessageBox.Show("Matrixs can only be inserted in the body of the report.", "Insert");
				return;
			}

			_Undo.StartUndoGroup("Insert Matrix");
			DialogNewMatrix dnm = new DialogNewMatrix(this._DrawPanel, hl.HitContainer);
            try
            {
                DialogResult dr = dnm.ShowDialog(this);
                if (dr != DialogResult.OK)
                {
                    _Undo.EndUndoGroup(false);
                    return;
                }
            }
            finally
            {
                dnm.Dispose();
            }
			XmlNode matrix;
			if (hl.HitContainer.Name == "Table" || hl.HitContainer.Name == "fyi:Grid")
				matrix = _DrawPanel.ReplaceTableMatrixOrChart(hl, dnm.MatrixXml);
			else
				matrix = _DrawPanel.PasteTableMatrixOrChart(hl.HitContainer, dnm.MatrixXml, hl.HitRelative);
			if (matrix == null)
			{
				_Undo.EndUndoGroup(false);
				return;
			}
			_Undo.EndUndoGroup(true);
			ReportChanged(this, new EventArgs());
			SelectionChanged(this, new EventArgs());
			ReportItemInserted(this, new EventArgs());
			_DrawPanel.Invalidate();   

			// Now bring up the property dialog
            //List<XmlNode> ar = new List<XmlNode>();
            //ar.Add(matrix);
            //_Undo.StartUndoGroup("Dialog");
            //PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.ReportItems);
            //dr = pd.ShowDialog(this);
            //_Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
            //if (pd.Changed || dr == DialogResult.OK)
            //{
            //    ReportChanged(this, new EventArgs());
            //    _DrawPanel.Invalidate();   
            //}
			SetFocus();
		}
			
		private void menuInsertRectangle_Click(object sender, EventArgs e)
		{
			string ri = "<ReportItems><Rectangle><Height>1.5in</Height><Width>1.5in</Width></Rectangle></ReportItems>";
			menuInsertReportItem(sender, e, ri);
		}

		private void menuInsertReportItem(object sender, EventArgs e, string reportItem)
		{
			HitLocation hl = _DrawPanel.HitContainer(_MousePosition, 
				PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			if (hl == null || hl.HitContainer == null)
				return;

			menuInsertReportItem(hl, reportItem);
		}	
		
		private void menuInsertReportItem(HitLocation hl, string reportItem)
		{
			_Undo.StartUndoGroup("Insert");
            try
            {
                if (hl.HitContainer.Name == "Table" || hl.HitContainer.Name == "fyi:Grid")
                {
                    _DrawPanel.ReplaceReportItems(hl, reportItem);
                }
                else
                    _DrawPanel.PasteReportItems(hl.HitContainer, reportItem, hl.HitRelative);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal error: illegal insert syntax:" + Environment.NewLine + 
                    reportItem + Environment.NewLine + ex.Message);
                return;
            }
            finally
            {
                _Undo.EndUndoGroup(true);
            }
			_DrawPanel.Invalidate();
			ReportChanged(this, new EventArgs());
			SelectionChanged(this, new EventArgs());
			ReportItemInserted(this, new EventArgs());

            if (reportItem.Contains("<Image") || reportItem.Contains("<Subreport"))
	    		menuProperties_Click();     // bring up report dialog for images and subreports
		}
			
		private void menuInsertSubreport_Click(object sender, EventArgs e)
		{
			// Subreports aren't allowed in PageHeader or PageFooter
			HitLocation hl = _DrawPanel.HitContainer(_MousePosition, 
				PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			if (hl == null || hl.HitContainer == null)
				return;
			if (_DrawPanel.InPageHeaderOrFooter(hl.HitContainer))
			{
				MessageBox.Show("Subreports can only be inserted in the body of the report.", "Insert");
				return;
			}

			string ri = "<ReportItems><Subreport><Height>1.5in</Height><Width>1.5in</Width></Subreport></ReportItems>";
			menuInsertReportItem(hl, ri);
		}

		private void menuInsertTable_Click(object sender, EventArgs e)
		{
			HitLocation hl = _DrawPanel.HitContainer(_MousePosition, 
				PointsX(_hScroll.Value), PointsY(_vScroll.Value));
			if (hl == null || hl.HitContainer == null)
				return;
			
			// Tables aren't allowed in PageHeader or PageFooter
			if (_DrawPanel.InPageHeaderOrFooter(hl.HitContainer))
			{
				MessageBox.Show("Tables can only be inserted in the body of the report.", "Insert");
				return;
			}

			_Undo.StartUndoGroup("Insert Table");
			DialogNewTable dnt = new DialogNewTable(this._DrawPanel, hl.HitContainer);
            try
            {
                DialogResult dr = dnt.ShowDialog(this);
                if (dr != DialogResult.OK)
                {
                    _Undo.EndUndoGroup(false);
                    return;
                }
            }
            finally
            {
                dnt.Dispose();
            }
			XmlNode table;
			if (hl.HitContainer.Name == "Table" || hl.HitContainer.Name == "fyi:Grid")
				table = _DrawPanel.ReplaceTableMatrixOrChart(hl, dnt.TableXml);
			else
				table = _DrawPanel.PasteTableMatrixOrChart(hl.HitContainer, dnt.TableXml, hl.HitRelative);
			if (table == null)
			{
				_Undo.EndUndoGroup(false);
				return;
			}
			_Undo.EndUndoGroup(true);
			ReportChanged(this, new EventArgs());
			SelectionChanged(this, new EventArgs());
			ReportItemInserted(this, new EventArgs());
			_DrawPanel.Invalidate();   

			// Now bring up the property dialog
            //List<XmlNode> ar = new List<XmlNode>();
            //ar.Add(table);
            //_Undo.StartUndoGroup("Dialog");
            //PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.ReportItems);
            //dr = pd.ShowDialog(this);
            //_Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
            //if (pd.Changed || dr == DialogResult.OK)
            //{
            //    ReportChanged(this, new EventArgs());
            //    _DrawPanel.Invalidate();   
            //}
			SetFocus();
		}
	
		private void menuInsertTextbox_Click(object sender, EventArgs e)
		{
			string ri = "<ReportItems><Textbox><Height>12pt</Height><Width>1in</Width><Value>Text</Value></Textbox></ReportItems>";
			menuInsertReportItem(sender, e, ri);
		}

		private void menuOpenSubreport_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedList.Count == 0)
				return;

			XmlNode sr = _DrawPanel.SelectedList[0];
			if (sr.Name != "Subreport")
				return;

			string name = _DrawPanel.GetElementValue(sr, "ReportName", "");
			if (name == null || name.Length == 0)
				return;
			
			if (OpenSubreport != null)
				OpenSubreport(this, new SubReportEventArgs(name));
		}

		private void menuSelectAll_Click(object sender, EventArgs e)
		{
			doSelectAll();
		}

		private void doSelectAll()
		{
			IEnumerable list = _DrawPanel.GetReportItems(null);		// get all the report items
			if (list == null)
				return;
			_DrawPanel.ClearSelected();

			foreach (XmlNode riNode in list)
			{
				if (riNode.Name == "Table" || riNode.Name == "fyi:Grid" || riNode.Name == "List" || riNode.Name == "Rectangle")
					continue;	// we'll just select all the sub objects in these containers
				
				_DrawPanel.AddSelection(riNode);
				SelectionChanged(this, new EventArgs());
			}
			_DrawPanel.Invalidate();
			return;
		}

		internal void menuProperties_Click(object sender, EventArgs e)
		{
			menuProperties_Click();
		}
		
		internal void menuProperties_Click()
		{
			if (_DrawPanel.SelectedCount > 0)
			{
				DoPropertyDialog(PropertyTypeEnum.ReportItems);
			}
			else
			{	// Put up the Report Property sheets
				DoPropertyDialog(PropertyTypeEnum.Report);
			}
			SelectionChanged(this, new EventArgs());
		}

		private void menuChartDeleteGrouping_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;
			string gname = menu.Text;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			_Undo.StartUndoGroup("Delete Grouping");
			bool bSuccess=false;
			if (_DrawPanel.DeleteChartGrouping(cNode, gname))
			{
				bSuccess = true;
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			_Undo.EndUndoGroup(bSuccess);
		}

		private void menuChartEditGrouping_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;
			string gname = menu.Text;
			XmlNode cNode = _DrawPanel.SelectedList[0];

			XmlNode group = _DrawPanel.GetChartGrouping(cNode, gname);

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(group.ParentNode);
			_Undo.StartUndoGroup("Dialog Grouping");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                _Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuChartInsertCategoryGrouping_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;

			_Undo.StartUndoGroup("Insert Category Grouping");

			XmlNode cNode = _DrawPanel.SelectedList[0];
			XmlNode colGroup = _DrawPanel.InsertChartCategoryGrouping(cNode);
			if (colGroup == null)
			{
				_Undo.EndUndoGroup(false);
				return;
			}
			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(colGroup);
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    _Undo.EndUndoGroup(true);
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
                else
                {
                    _DrawPanel.DeleteChartGrouping(colGroup);
                    _Undo.EndUndoGroup(false);
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuChartInsertSeriesGrouping_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			_Undo.StartUndoGroup("Insert Series Grouping");
			XmlNode cNode = _DrawPanel.SelectedList[0];
			XmlNode colGroup = _DrawPanel.InsertChartSeriesGrouping(cNode);
			if (colGroup == null)
			{
				_Undo.EndUndoGroup(false);
				return;
			}
			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(colGroup);
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    _Undo.EndUndoGroup(true);
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
                else
                {
                    _DrawPanel.DeleteChartGrouping(colGroup);
                    _Undo.EndUndoGroup(false);
                }
            }
            finally
            {
                pd.Dispose();
            }
		}
				
		private void menuPropertiesLegend_Click(object sender, EventArgs e)
		{
			DoPropertyDialog(PropertyTypeEnum.ChartLegend);
		}
				
		private void menuPropertiesCategoryAxis_Click(object sender, EventArgs e)
		{
			DoPropertyDialog(PropertyTypeEnum.CategoryAxis);
		}
				
		private void menuPropertiesValueAxis_Click(object sender, EventArgs e)
		{
			DoPropertyDialog(PropertyTypeEnum.ValueAxis);
		}
				
		private void menuPropertiesCategoryAxisTitle_Click(object sender, EventArgs e)
		{
			DoPropertyDialog(PropertyTypeEnum.CategoryAxisTitle);
		}
				
		private void menuPropertiesValueAxisTitle_Click(object sender, EventArgs e)
		{
			DoPropertyDialog(PropertyTypeEnum.ValueAxisTitle);
		}
		// 20022008 AJM GJL - Second Y axis support
        private void menuPropertiesValueAxis2Title_Click(object sender, EventArgs e)
        {
            DoPropertyDialog(PropertyTypeEnum.ValueAxis2Title);
        }
				
		private void menuPropertiesChartTitle_Click(object sender, EventArgs e)
		{
			DoPropertyDialog(PropertyTypeEnum.ChartTitle);
		}

		private void menuMatrixProperties_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode riNode = _DrawPanel.SelectedList[0];
			XmlNode table = _DrawPanel.GetMatrixFromReportItem(riNode);
			if (table == null)
				return;

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(table);
			_Undo.StartUndoGroup("Matrix Dialog");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.ReportItems);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                _Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuMatrixDelete_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			_Undo.StartUndoGroup("Matrix Delete");
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			if (_DrawPanel.DeleteMatrix(cNode))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}

		private void menuMatrixDeleteGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;
			_Undo.StartUndoGroup("Matrix Delete Group");
			string gname = menu.Text;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			if (_DrawPanel.DeleteMatrixGroup(cNode, gname))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}

		private void menuMatrixEditGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;
			string gname = menu.Text;
			XmlNode cNode = _DrawPanel.SelectedList[0];

			XmlNode group = _DrawPanel.GetMatrixGroup(cNode, gname);

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(group.ParentNode);
			_Undo.StartUndoGroup("Matrix Edit");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                _Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuMatrixInsertColumnGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			XmlNode colGroup = _DrawPanel.InsertMatrixColumnGroup(cNode);
			if (colGroup == null)
				return;

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(colGroup);
			_Undo.StartUndoGroup("Matrix Insert Column Group");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    _Undo.EndUndoGroup(true);
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
                else
                {
                    _DrawPanel.DeleteMatrixGroup(colGroup);
                    _Undo.EndUndoGroup(false);
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuMatrixInsertRowGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode =  _DrawPanel.SelectedList[0];
			XmlNode rowGroup = _DrawPanel.InsertMatrixRowGroup(cNode);
			if (rowGroup == null)
				return;

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(rowGroup);
			_Undo.StartUndoGroup("Matrix Insert Row Group");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    _Undo.EndUndoGroup(true);
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
                else
                {
                    _DrawPanel.DeleteMatrixGroup(rowGroup);
                    _Undo.EndUndoGroup(false);
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuTableDeleteColumn_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode =  _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Delete Table Column");

			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			if (_DrawPanel.DeleteTableColumn(cNode))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}

		private void menuTableDelete_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode =  _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Delete Table");
			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			if (_DrawPanel.DeleteTable(cNode))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}

		private void menuTableDeleteRow_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Delete Table Row");
			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			if (_DrawPanel.DeleteTableRow(cNode))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}

		private void menuTableDeleteGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;
			string gname = menu.Text;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Delete Table Group");
			_DrawPanel.ClearSelected();
			this.SelectionChanged(this, new EventArgs());
			if (_DrawPanel.DeleteTableGroup(cNode, gname))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}

		private void menuTableEditGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;
			string gname = menu.Text;
			XmlNode cNode = _DrawPanel.SelectedList[0];

			_Undo.StartUndoGroup("Dialog Table Group Edit");
			XmlNode tblGroup = _DrawPanel.GetTableGroup(cNode, gname);

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(tblGroup);
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                _Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuTableInsertColumnBefore_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Insert Table Column");
			if (_DrawPanel.InsertTableColumn(cNode, true))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}
				
		private void menuTableInsertColumnAfter_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Insert Table Column");
			if (_DrawPanel.InsertTableColumn(cNode, false))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}
				
		private void menuTableInsertGroup_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Insert Table Group");
			XmlNode tblGroup = _DrawPanel.InsertTableGroup(cNode);
			if (tblGroup == null)
			{
				_Undo.EndUndoGroup(false);
				return;
			}

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(tblGroup);
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    _Undo.EndUndoGroup(true);
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
                else
                {
                    _DrawPanel.DeleteTableGroup(tblGroup);
                    _Undo.EndUndoGroup(false);
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void menuTableInsertRowBefore_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode =  _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Insert Table Row");

			if (_DrawPanel.InsertTableRow(cNode, true))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}
				
		private void menuTableInsertRowAfter_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode cNode = _DrawPanel.SelectedList[0];
			_Undo.StartUndoGroup("Insert Table Row");
			if (_DrawPanel.InsertTableRow(cNode, false))
			{
				_Undo.EndUndoGroup(true);
				ReportChanged(this, new EventArgs());
				_DrawPanel.Invalidate();   
			}
			else
				_Undo.EndUndoGroup(false);
		}
				
		private void menuTableProperties_Click(object sender, EventArgs e)
		{
			if (_DrawPanel.SelectedCount != 1)
				return;
			XmlNode riNode = _DrawPanel.SelectedList[0];
			XmlNode table = _DrawPanel.GetTableFromReportItem(riNode);
			if (table == null)
				return;
			XmlNode tc = _DrawPanel.GetTableColumn(riNode);
			XmlNode tr = _DrawPanel.GetTableRow(riNode);

			List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
			ar.Add(table);
			_Undo.StartUndoGroup("Table Dialog");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, ar, PropertyTypeEnum.ReportItems, tc, tr);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                _Undo.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void DoPropertyDialog(PropertyTypeEnum type)
		{
			this.StartUndoGroup("Dialog");
			PropertyDialog pd = new PropertyDialog(_DrawPanel, _DrawPanel.SelectedList, type);
            try
            {
                DialogResult dr = pd.ShowDialog(this);
                this.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    ReportChanged(this, new EventArgs());
                    _DrawPanel.Invalidate();
                }
            }
            finally
            {
                pd.Dispose();
            }
			SetFocus();
		}

		internal void SetFocus()
		{
			this._DrawPanel.Focus();
		}
				
		private void menuContext_Popup(object sender, EventArgs e)
		{
			bool bEnable = _DrawPanel.SelectedCount <= 0? false: true;

			menuCopy.Enabled = bEnable;
			menuDelete.Enabled = bEnable;

			List<XmlNode> al=new List<XmlNode>();
			IDataObject iData = Clipboard.GetDataObject();
			if (iData == null)
				bEnable = false;
			else if (iData.GetDataPresent(al.GetType()))
				bEnable = true;
			else if (iData.GetDataPresent(DataFormats.Text)) 
				bEnable = true;
			else if (iData.GetDataPresent(DataFormats.Bitmap)) 
				bEnable = true;
			else
				bEnable = false;
			menuPaste.Enabled = bEnable;

			return;
		}
	}

	public class SubReportEventArgs : EventArgs
	{
		string _name;			// name of subreport user has requested be opened
		public SubReportEventArgs(string name) : base()
		{
			_name = name;
		}

		public string SubReportName
		{
			get {return _name;}
		}
	}

    public class HeightEventArgs : EventArgs
    {
        XmlNode _node;          // current node
        string _height;			// current height
        public HeightEventArgs(XmlNode node, string height)
            : base()
        {
            _node = node;
            _height = height;
        }

        public XmlNode Node
        {
            get { return _node; }
        }
        public string Height
        {
            get { return _height; }
        }
    }
}
