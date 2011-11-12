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
using System.Xml;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace fyiReporting.RDL
{
	///<summary>
	/// The Textbox definition.  Inherits from ReportItem.
	///</summary>
	[Serializable]
	internal class Textbox : ReportItem
	{
		Expression _Value;	// (Variant) An expression, the value of which is
							// displayed in the text-box.
							// This can be a constant expression for constant labels.
		bool _CanGrow;		// Indicates the Textbox size can
							// increase to accommodate the contents
		bool _CanShrink;	// Indicates the Textbox size can
							// decrease to match the contents
		string _HideDuplicates;	// Indicates the item should be hidden
							//when the value of the expression
							//associated with the report item is the
							//same as the preceding instance. The
							//value of HideDuplicates is the name
							//of a grouping or data set over which
							//to apply the hiding. Each time a
							//new instance of that group is
							//encountered, the first instance of
							//this report item will not be hidden.
							//Rows on a previous page are
							//ignored for the purposes of hiding
							//duplicates. If the textbox is in a
							//table or matrix cell, only the text
							//will be hidden. The textbox will
							//remain to provide background and
							//border for the cell.
							//Ignored in matrix subtotals.
		ToggleImage _ToggleImage;	// Indicates the initial state of a
								// toggling image should one be
								// displayed as a part of the textbox.
		DataElementStyleEnum _DataElementStyle;	// Indicates whether textbox value
								// should render as an element or attribute: Auto (Default)
								// Auto uses the setting on the Report element.
		bool _IsToggle;		// Textbox is used to toggle a detail row
		List<string> _ExprReferences;	// array of names of expressions that reference this Textbox;
								//  only needed for page header/footer references 

        static readonly Regex HTMLEXPR = new Regex("(<expr>.+</expr>)");     // Split on all expressions.

		internal Textbox(ReportDefn r, ReportLink p, XmlNode xNode):base(r,p,xNode)
		{
			_Value=null;
			_CanGrow=false;
			_CanShrink=false;
			_HideDuplicates=null;
			_ToggleImage=null;
			_DataElementStyle=DataElementStyleEnum.Auto;
		
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Value":
						_Value = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					case "CanGrow":
						_CanGrow = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "CanShrink":
						_CanShrink = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "HideDuplicates":
						_HideDuplicates = xNodeLoop.InnerText;
						break;
					case "ToggleImage":
						_ToggleImage = new ToggleImage(r, this, xNodeLoop);
						break;
					case "DataElementStyle":
						_DataElementStyle = fyiReporting.RDL.DataElementStyle.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						if (ReportItemElement(xNodeLoop))	// try at ReportItem level
							break;
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Textbox element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			
			if (_Value == null)
				OwnerReport.rl.LogError(8, "Textbox value not specified for " + (this.Name == null? "'name not specified'": this.Name.Nm));

			if (this.Name != null)
			{
				try
				{
					OwnerReport.LUReportItems.Add(this.Name.Nm, this);		// add to referenceable TextBoxes
				}
				catch		// Duplicate name
				{
					OwnerReport.rl.LogError(4, "Duplicate Textbox name '" + this.Name.Nm + "' ignored.");
				}
			}
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			base.FinalPass();
			_Value.FinalPass();

			if (this.DataElementName == null && this.Name == null)
			{
				// no name or dataelementname; try using expression
				FunctionField ff = _Value.Expr as FunctionField;
				if (ff != null && ff.Fld != null)
				{
					this.DataElementName = ff.Fld.DataField;
				}
			}

			if (_ToggleImage != null)
				_ToggleImage.FinalPass();

			if (_HideDuplicates != null)
			{
				object o = OwnerReport.LUAggrScope[_HideDuplicates];
				if (o == null)
				{
					OwnerReport.rl.LogError(4, "HideDuplicate '" +_HideDuplicates + "' is not a Group or DataSet name.   It will be ignored.");
					_HideDuplicates=null;
				}
				else if (o is Grouping)
				{	
					Grouping g = o as Grouping;
					g.AddHideDuplicates(this);
				}
				else if (o is DataSetDefn)
				{
					DataSetDefn ds = o as DataSetDefn;
					ds.AddHideDuplicates(this);
				}
			}
			return;
		}

		internal void AddExpressionReference(string name)
		{
			if (_ExprReferences == null)
				_ExprReferences = new List<string>();
			_ExprReferences.Add(name);
		}

		internal void RecordPageReference(Report rpt, Page p, Row r)
		{
			if (_ExprReferences == null)
				return;
			foreach (string refr in _ExprReferences)
			{
				p.AddPageExpressionRow(rpt, refr, r);
			}
		}

		internal void ResetPrevious(Report rpt)
		{
			TextboxRuntime tbr = TextboxRuntime.GetTextboxRuntime(rpt, this);
			ResetPrevious(tbr);
		}

		void ResetPrevious(TextboxRuntime tbr)
		{
			tbr.PreviousText=null;	
			tbr.PreviousPage=null;	
		}

		override internal void Run(IPresent ip, Row row)
		{
			Report rpt = ip.Report();
			base.Run(ip, row);

			TextboxRuntime tbr = TextboxRuntime.GetTextboxRuntime(rpt, this);

			tbr.RunCount++;		// Increment the run count
			string t = RunText(rpt, row);
			bool bDup =	RunTextIsDuplicate(tbr, t, null);
			if (bDup)
			{
				if (!(this.IsTableOrMatrixCell(rpt)))	// don't put out anything if not in Table or Matrix
					return;
				t = "";		// still need to put out the cell
			}
			ip.Textbox(this, t, row);

			if (!bDup)
				tbr.PreviousText=t;	// set for next time
		}

		override internal void RunPage(Pages pgs, Row row)
		{
			Report r = pgs.Report;
			TextboxRuntime tbr = TextboxRuntime.GetTextboxRuntime(r, this);

			tbr.RunCount++;		// Increment the run count

            bool bHidden = IsHidden(r, row);

			SetPagePositionBegin(pgs);

			string t;
            if (bHidden)
                t = "";
            else
                t = RunText(r, row);	// get the text

			bool bDup =	RunTextIsDuplicate(tbr, t, pgs.CurrentPage);
			if (bDup)
			{
				if (!(this.IsTableOrMatrixCell(r)))	// don't put out anything if not in Table or Matrix
					bHidden = true;
				t = "";		// still need to put out the cell
			}
			PageText pt;
			PageTextHtml pth=null;
			if (IsHtml(r, row))
				pt = pth = new PageTextHtml(t);
			else
				pt = new PageText(t);
			SetPagePositionAndStyle(r, pt, row);
			if (this.CanGrow && tbr.RunHeight == 0)	// when textbox is in a DataRegion this will already be called
			{
				this.RunTextCalcHeight(r, pgs.G, row, pt is PageTextHtml? pt as PageTextHtml: null);
			}
			pt.H = Math.Max(pt.H, tbr.RunHeight);		// reset height
			if (pt.SI.BackgroundImage != null)
				pt.SI.BackgroundImage.H = pt.H;		//   and in the background image
			pt.CanGrow = this.CanGrow;

            // check TextAlign: if General then correct based on data type
            if (pt.SI.TextAlign == TextAlignEnum.General)
            {
                if (DataType.IsNumeric(this.Value.GetTypeCode() ))
                    pt.SI.TextAlign = TextAlignEnum.Right;
            }

            // Hidden objects don't affect the current page?
            if (!bHidden)
            {
                // Force page break if it doesn't fit on a page
                if (this.IsInBody &&                         // Only force page when object directly in body
                    pgs.CurrentPage.YOffset + pt.Y + pt.H >= pgs.BottomOfPage && // running off end of page
                    !pgs.CurrentPage.IsEmpty())                             // if page is already empty don't force new
                {	// force page break if it doesn't fit on the page
                    pgs.NextOrNew();
                    pgs.CurrentPage.YOffset = OwnerReport.TopOfPage;
                    if (this.YParents != null)
                        pt.Y = 0;
                }

                Page p = pgs.CurrentPage;
                RecordPageReference(r, p, row);			// save information for late page header/footer references
                p.AddObject(pt);
                if (!bDup)
                {
                    tbr.PreviousText = t;	// previous text displayed
                    tbr.PreviousPage = p;	//  page previous text was shown on
                }
            }

			SetPagePositionEnd(pgs, pt.Y+pt.H);
			if (pth != null)
				pth.Reset();
			if (this.CanGrow && !Value.IsConstant())
			{
				tbr.RunHeight = 0;					// need to recalculate
			}
		}

		// routine to determine if text is considered to be a duplicate;
		//  ie: same as previous text and on same page
		private bool RunTextIsDuplicate(TextboxRuntime tbr, string t, Page p)
		{
			if (this._HideDuplicates == null)
				return false;
			if (t == tbr.PreviousText && p == tbr.PreviousPage)
				return true;

			return false;
		}

		internal string RunText(Report rpt, Row row)
		{
			object o = _Value.Evaluate(rpt, row);
            // AJM 15082008: Suppress NaN from appearing in a textbox
            if (o is double) 
            {
                if (Double.IsNaN((double)o)) 
                { 
                    o = null; 
                } 
            }   
			string t = Style.GetFormatedString(rpt, this.Style, row, o, _Value.GetTypeCode());
            if (IsHtml(rpt, row) && t != null && t.Contains("<expr>"))
            {
                string[] parts = HTMLEXPR.Split(t);
                StringBuilder sb = new StringBuilder(t.Length);
                foreach (string s in parts)
                {
                    if (s.StartsWith("<expr>") && s.EndsWith("</expr>"))
                    {
                        string expr = s.Substring(6, s.Length - 13);
                        DynamicExpression de = new DynamicExpression(rpt, this, expr, row);
                        sb.Append(de.Evaluate(rpt, row));
                    }
                    else
                        sb.Append(s);
                }
                t = sb.ToString();
            }
			return t;
		}

		internal float RunTextCalcHeight(Report rpt, Graphics g, Row row)
		{
			return RunTextCalcHeight(rpt, g, row, null);
		}
		
		internal float RunTextCalcHeight(Report rpt, Graphics g, Row row, PageTextHtml pth)
		{	// normally only called when CanGrow is true
			Size s = Size.Empty;

			if (IsHidden(rpt, row))
				return 0;

			object o = _Value.Evaluate(rpt, row);

			TypeCode tc = _Value.GetTypeCode();
			int width = this.WidthCalc(rpt, g);

			if (this.Style != null)
			{
				width -= (Style.EvalPaddingLeftPx(rpt, row) + Style.EvalPaddingRightPx(rpt, row));

				if (this.IsHtml(rpt, row))
				{
					if (pth == null)
					{
						pth = new PageTextHtml(o==null? "": o.ToString());
						SetPagePositionAndStyle(rpt, pth, row);
					}
					pth.Build(g);
					s.Height = RSize.PixelsFromPoints(pth.TotalHeight);
				}
				else
					s = Style.MeasureString(rpt, g, o, tc, row, width);
			}
			else	// call the class static method
				s = Style.MeasureStringDefaults(rpt, g, o, tc, row, width);

			TextboxRuntime tbr = TextboxRuntime.GetTextboxRuntime(rpt, this);
			tbr.RunHeight = RSize.PointsFromPixels(g, s.Height);
			if (Style != null)
				tbr.RunHeight += (Style.EvalPaddingBottom(rpt, row) + Style.EvalPaddingTop(rpt, row));
			return tbr.RunHeight;
		}

		internal object Evaluate(Report rpt, Row r)
		{
			object o = _Value.Evaluate(rpt, r);
			return o;
		}

		internal Expression Value
		{
			get { return  _Value; }
			set {  _Value = value; }
		}

		internal bool CanGrow
		{
			get { return  _CanGrow; }
			set {  _CanGrow = value; }
		}

		internal bool CanShrink
		{
			get { return  _CanShrink; }
			set {  _CanShrink = value; }
		}

		internal string HideDuplicates
		{
			get { return  _HideDuplicates; }
			set {  _HideDuplicates = value; }
		}

		internal bool IsHtml(Report rpt, Row row)
		{
			if (this.Style == null || this.Style.Format == null)
				return false;
			string format = Style.Format.EvaluateString(rpt, row);
			if (format == null)
				return false;
			return format.ToLower() == "html";
		}

		internal ToggleImage ToggleImage
		{
			get { return  _ToggleImage; }
			set {  _ToggleImage = value; }
		}

		internal bool IsToggle
		{
			get { return  _IsToggle; }
			set {  _IsToggle = value; }
		}

		internal int RunCount(Report rpt)
		{
			TextboxRuntime tbr = TextboxRuntime.GetTextboxRuntime(rpt, this);

			return  tbr.RunCount;
		}

		internal DataElementStyleEnum DataElementStyle
		{
			get 
			{
				if (_DataElementStyle == DataElementStyleEnum.Auto)	// auto means use report
					return OwnerReport.DataElementStyle;
				else
					return  _DataElementStyle; 
			}
			set {  _DataElementStyle = value; }
		}

	}

	class TextboxRuntime
	{
		internal int RunCount=0;			// number of times TextBox is rendered at runtime;
											//    used to generate unique names for toggling visibility
		internal float RunHeight=0;			// the runtime height (in points)
		internal string PreviousText=null;	// previous text displayed
		internal Page PreviousPage=null;	//  page previous text was shown on
		internal object LastObject=null;	// last object calculated

		static internal TextboxRuntime GetTextboxRuntime(Report rpt, Textbox tb)
		{
			TextboxRuntime tbr = rpt.Cache.Get(tb, "txtbox") as TextboxRuntime;
			if (tbr != null)
				return tbr;
			tbr = new TextboxRuntime();
			rpt.Cache.Add(tb, "txtbox", tbr);
			return tbr;
		}
	}
}
