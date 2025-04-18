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
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// The width of the border.  Expressions for all sides as well as default expression.
	///</summary>
	[Serializable]
	internal class StyleBorderWidth : ReportLink
	{
		Expression _Default;	//(Size) Width of the border (unless overridden for a specific side)
								// Borders are centered on the edge of the object
								// Default: 1 pt Max: 20 pt Min: 0.25 pt
		Expression _Left;	//(Size) Width of the left border. Max: 20 pt Min: 0.25 pt
		Expression _Right;	//(Size) Width of the right border. Max: 20 pt Min: 0.25 pt
		Expression _Top;	//(Size) Width of the top border. Max: 20 pt Min: 0.25 pt
		Expression _Bottom;	//(Size) Width of the bottom border. Max: 20 pt Min: 0.25 pt
	
		internal StyleBorderWidth(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Default=null;
			_Left=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Default":
						_Default = new Expression(r, this, xNodeLoop, ExpressionType.ReportUnit);
						break;
					case "Left":
						_Left = new Expression(r, this, xNodeLoop, ExpressionType.ReportUnit);
						break;
					case "Right":
						_Right = new Expression(r, this, xNodeLoop, ExpressionType.ReportUnit);
						break;
					case "Top":
						_Top = new Expression(r, this, xNodeLoop, ExpressionType.ReportUnit);
						break;
					case "Bottom":
						_Bottom = new Expression(r, this, xNodeLoop, ExpressionType.ReportUnit);
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown BorderWidth element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}

		// Handle parsing of function in final pass
		async override internal Task FinalPass()
		{
			if (_Default != null)
                await _Default.FinalPass();
			if (_Left != null)
                await _Left.FinalPass();
			if (_Right != null)
                await _Right.FinalPass();
			if (_Top != null)
                await _Top.FinalPass();
			if (_Bottom != null)
                await _Bottom.FinalPass();
			return;
		}

		// Generate a CSS string from the specified styles
		internal async Task<string> GetCSS(Report rpt, Row row, bool bDefaults)
		{
			StringBuilder sb = new StringBuilder();

			if (_Default != null)
				sb.AppendFormat("border-width:{0};", await _Default.EvaluateString(rpt, row));
			else if (bDefaults)
				sb.Append("border-width:1pt;");

			if (_Left != null)
				sb.AppendFormat("border-left-width:{0};", await _Left.EvaluateString(rpt, row));

			if (_Right != null)
				sb.AppendFormat("border-right-width:{0};", await _Right.EvaluateString(rpt, row));

			if (_Top != null)
				sb.AppendFormat("border-top-width:{0};", await _Top.EvaluateString(rpt, row));

			if (_Bottom != null)
				sb.AppendFormat("border-bottom-width:{0};", await _Bottom.EvaluateString(rpt, row));

			return sb.ToString();
		}

		internal async Task<bool> IsConstant()
		{
			bool rc = true;

			if (_Default != null)
				rc = await _Default.IsConstant();

			if (!rc)
				return false;

			if (_Left != null)
				rc = await _Left.IsConstant();

			if (!rc)
				return false;

			if (_Right != null)
				rc = await _Right.IsConstant();

			if (!rc)
				return false;

			if (_Top != null)
				rc = await _Top.IsConstant();

			if (!rc)
				return false;

			if (_Bottom != null)
				rc = await _Bottom.IsConstant();

			return rc;
		}

		static internal string GetCSSDefaults()
		{
			return "border-width:1pt;";
		}

		internal Expression Default
		{
			get { return  _Default; }
			set {  _Default = value; }
		}

		internal async Task<float> EvalDefault(Report rpt, Row r)	// return points
		{
			if (_Default == null)
				return 1;

			string sw;
			sw = await _Default.EvaluateString(rpt, r);

			RSize rs = new RSize(this.OwnerReport, sw);
			return rs.Points;
		}

		internal Expression Left
		{
			get { return  _Left; }
			set {  _Left = value; }
		}

		internal async Task<float> EvalLeft(Report rpt, Row r)	// return points
		{
			if (_Left == null)
				return await EvalDefault(rpt, r);

			string sw = await _Left.EvaluateString(rpt, r);
			RSize rs = new RSize(this.OwnerReport, sw);
			return rs.Points;
		}

		internal Expression Right
		{
			get { return  _Right; }
			set {  _Right = value; }
		}

		internal async Task<float> EvalRight(Report rpt, Row r)	// return points
		{
			if (_Right == null)
				return await EvalDefault(rpt, r);

			string sw = await _Right.EvaluateString(rpt, r);
			RSize rs = new RSize(this.OwnerReport, sw);
			return rs.Points;
		}

		internal Expression Top
		{
			get { return  _Top; }
			set {  _Top = value; }
		}

		internal async Task<float> EvalTop(Report rpt, Row r)	// return points
		{
			if (_Top == null)
				return await EvalDefault(rpt, r);

			string sw = await _Top.EvaluateString(rpt, r);
			RSize rs = new RSize(this.OwnerReport, sw);
			return rs.Points;
		}

		internal Expression Bottom
		{
			get { return  _Bottom; }
			set {  _Bottom = value; }
		}

		internal async Task<float> EvalBottom(Report rpt, Row r)	// return points
		{
			if (_Bottom == null)
				return await EvalDefault(rpt, r);

			string sw = await _Bottom.EvaluateString(rpt, r);
			RSize rs = new RSize(this.OwnerReport, sw);
			return rs.Points;
		}
	}
}
