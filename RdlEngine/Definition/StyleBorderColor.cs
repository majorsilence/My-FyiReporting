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
using System.Text;
using System.Drawing;
using System.Globalization;

namespace fyiReporting.RDL
{
	///<summary>
	/// The style of the border colors.  Expressions for all sides as well as default expression.
	///</summary>
	[Serializable]
	internal class StyleBorderColor : ReportLink
	{
		Expression _Default;	// (Color) Color of the border (unless overridden for a specific
								//   side). Default: Black.
		Expression _Left;		// (Color) Color of the left border
		Expression _Right;		// (Color) Color of the right border
		Expression _Top;		// (Color) Color of the top border
		Expression _Bottom;		// (Color) Color of the bottom border
	
		internal StyleBorderColor(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Default=null;
			_Left=null;
			_Right=null;
			_Top=null;
			_Bottom=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Default":
						_Default = new Expression(r, this, xNodeLoop, ExpressionType.Color);
						break;
					case "Left":
						_Left = new Expression(r, this, xNodeLoop, ExpressionType.Color);
						break;
					case "Right":
						_Right = new Expression(r, this, xNodeLoop, ExpressionType.Color);
						break;
					case "Top":
						_Top = new Expression(r, this, xNodeLoop, ExpressionType.Color);
						break;
					case "Bottom":
						_Bottom = new Expression(r, this, xNodeLoop, ExpressionType.Color);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown BorderColor element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Default != null)
				_Default.FinalPass();
			if (_Left != null)
				_Left.FinalPass();
			if (_Right != null)
				_Right.FinalPass();
			if (_Top != null)
				_Top.FinalPass();
			if (_Bottom != null)
				_Bottom.FinalPass();
			return;
		}

		// Generate a CSS string from the specified styles
		internal string GetCSS(Report rpt, Row row, bool bDefaults)
		{
			StringBuilder sb = new StringBuilder();

			if (_Default != null)
				sb.AppendFormat(NumberFormatInfo.InvariantInfo, "border-color:{0};",_Default.EvaluateString(rpt, row));
			else if (bDefaults)
				sb.Append("border-color:black;");

			if (_Left != null)
				sb.AppendFormat(NumberFormatInfo.InvariantInfo, "border-left:{0};",_Left.EvaluateString(rpt, row));

			if (_Right != null)
				sb.AppendFormat(NumberFormatInfo.InvariantInfo, "border-right:{0};",_Right.EvaluateString(rpt, row));

			if (_Top != null)
				sb.AppendFormat(NumberFormatInfo.InvariantInfo, "border-top:{0};",_Top.EvaluateString(rpt, row));

			if (_Bottom != null)
				sb.AppendFormat(NumberFormatInfo.InvariantInfo, "border-bottom:{0};",_Bottom.EvaluateString(rpt, row));

			return sb.ToString();
		}

		internal bool IsConstant()
		{
			bool rc = true;

			if (_Default != null)
				rc = _Default.IsConstant();

			if (!rc)
				return false;

			if (_Left != null)
				rc = _Left.IsConstant();

			if (!rc)
				return false;

			if (_Right != null)
				rc = _Right.IsConstant();

			if (!rc)
				return false;

			if (_Top != null)
				rc = _Top.IsConstant();

			if (!rc)
				return false;

			if (_Bottom != null)
				rc = _Bottom.IsConstant();

			return rc;
		}

		static internal string GetCSSDefaults()
		{
			return "border-color:black;";
		}

		internal Expression Default
		{
			get { return  _Default; }
			set {  _Default = value; }
		}

		internal Color EvalDefault(Report rpt, Row r)
		{
			if (_Default == null)
				return System.Drawing.Color.Black;
			
			string c = _Default.EvaluateString(rpt, r);
			return XmlUtil.ColorFromHtml(c, System.Drawing.Color.Black, rpt);
		}

		internal Expression Left
		{
			get { return  _Left; }
			set {  _Left = value; }
		}

		internal Color EvalLeft(Report rpt, Row r)
		{
			if (_Left == null)
				return EvalDefault(rpt, r);
			
			string c = _Left.EvaluateString(rpt, r);
			return XmlUtil.ColorFromHtml(c, System.Drawing.Color.Black, rpt);
		}

		internal Expression Right
		{
			get { return  _Right; }
			set {  _Right = value; }
		}

		internal Color EvalRight(Report rpt, Row r)
		{
			if (_Right == null)
				return EvalDefault(rpt, r);
			
			string c = _Right.EvaluateString(rpt, r);
			return XmlUtil.ColorFromHtml(c, System.Drawing.Color.Black, rpt);
		}

		internal Expression Top
		{
			get { return  _Top; }
			set {  _Top = value; }
		}

		internal Color EvalTop(Report rpt, Row r)
		{
			if (_Top == null)
				return EvalDefault(rpt, r);
			
			string c = _Top.EvaluateString(rpt, r);
			return XmlUtil.ColorFromHtml(c, System.Drawing.Color.Black, rpt);
		}

		internal Expression Bottom
		{
			get { return  _Bottom; }
			set {  _Bottom = value; }
		}

		internal Color EvalBottom(Report rpt, Row r)
		{
			if (_Bottom == null)
				return EvalDefault(rpt, r);
			
			string c = _Bottom.EvaluateString(rpt, r);
			return XmlUtil.ColorFromHtml(c, System.Drawing.Color.Black, rpt);
		}
	}
}
