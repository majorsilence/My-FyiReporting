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

namespace fyiReporting.RDL
{
	///<summary>
	/// The type (dotted, solid, ...) of border.  Expressions for all sides as well as default expression.
	///</summary>
	[Serializable]
	internal class StyleBorderStyle : ReportLink
	{
		Expression _Default;	// (Enum BorderStyle) Style of the border (unless overridden for a specific side)
		// Default: none		
		Expression _Left;		// (Enum BorderStyle) Style of the left border
		Expression _Right;		// (Enum BorderStyle) Style of the right border
		Expression _Top;		// (Enum BorderStyle) Style of the top border
		Expression _Bottom;		// (Enum BorderStyle) Style of the bottom border
	
		internal StyleBorderStyle(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
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
						_Default = new Expression(r, this, xNodeLoop, ExpressionType.Enum);
						break;
					case "Left":
						_Left = new Expression(r, this, xNodeLoop, ExpressionType.Enum);
						break;
					case "Right":
						_Right = new Expression(r, this, xNodeLoop, ExpressionType.Enum);
						break;
					case "Top":
						_Top = new Expression(r, this, xNodeLoop, ExpressionType.Enum);
						break;
					case "Bottom":
						_Bottom = new Expression(r, this, xNodeLoop, ExpressionType.Enum);
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown BorderStyle element '" + xNodeLoop.Name + "' ignored.");
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
				sb.AppendFormat("border-style:{0};",_Default.EvaluateString(rpt, row));
			else if (bDefaults)
				sb.Append("border-style:none;");

			if (_Left != null)
				sb.AppendFormat("border-left-style:{0};",_Left.EvaluateString(rpt, row));

			if (_Right != null)
				sb.AppendFormat("border-right-style:{0};",_Right.EvaluateString(rpt, row));

			if (_Top != null)
				sb.AppendFormat("border-top-style:{0};",_Top.EvaluateString(rpt, row));

			if (_Bottom != null)
				sb.AppendFormat("border-bottom-style:{0};",_Bottom.EvaluateString(rpt, row));

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
			return "border-style:none;";
		}

		internal Expression Default
		{
			get { return  _Default; }
			set {  _Default = value; }
		}
 
		internal BorderStyleEnum EvalDefault(Report rpt, Row r)
		{
			if (_Default == null)
				return BorderStyleEnum.None;

			string bs = _Default.EvaluateString(rpt, r);
			return GetBorderStyle(bs, BorderStyleEnum.Solid);
		}

		internal Expression Left
		{
			get { return  _Left; }
			set {  _Left = value; }
		}

		internal BorderStyleEnum EvalLeft(Report rpt, Row r)
		{
			if (_Left == null)
				return EvalDefault(rpt, r);

			string bs = _Left.EvaluateString(rpt, r);
			return GetBorderStyle(bs, BorderStyleEnum.Solid);
		}

		internal Expression Right
		{
			get { return  _Right; }
			set {  _Right = value; }
		}

		internal BorderStyleEnum EvalRight(Report rpt, Row r)
		{
			if (_Right == null)
				return EvalDefault(rpt, r);

			string bs = _Right.EvaluateString(rpt, r);
			return GetBorderStyle(bs, BorderStyleEnum.Solid);
		}

		internal Expression Top
		{
			get { return  _Top; }
			set {  _Top = value; }
		}

		internal BorderStyleEnum EvalTop(Report rpt, Row r)
		{
			if (_Top == null)
				return EvalDefault(rpt, r);

			string bs = _Top.EvaluateString(rpt, r);
			return GetBorderStyle(bs, BorderStyleEnum.Solid);
		}

		internal Expression Bottom
		{
			get { return  _Bottom; }
			set {  _Bottom = value; }
		}

		internal BorderStyleEnum EvalBottom(Report rpt, Row r)
		{
			if (_Bottom == null)
				return EvalDefault(rpt, r);

			string bs = _Bottom.EvaluateString(rpt, r);
			return GetBorderStyle(bs, BorderStyleEnum.Solid);
		}

		// return the BorderStyleEnum given a particular string value
		static internal BorderStyleEnum GetBorderStyle(string v, BorderStyleEnum def)
		{
			BorderStyleEnum bs;
			switch (v)
			{
				case "None":
					bs = BorderStyleEnum.None;
					break;
				case "Dotted":
					bs = BorderStyleEnum.Dotted;
					break;
				case "Dashed":
					bs = BorderStyleEnum.Dashed;
					break;
				case "Solid":
					bs = BorderStyleEnum.Solid;
					break;
				case "Double":
					bs = BorderStyleEnum.Double;
					break;
				case "Groove":
					bs = BorderStyleEnum.Groove;
					break;
				case "Ridge":
					bs = BorderStyleEnum.Ridge;
					break;
				case "Inset":
					bs = BorderStyleEnum.Inset;
					break;
				case "WindowInset":
					bs = BorderStyleEnum.WindowInset;
					break;
				case "Outset":
					bs = BorderStyleEnum.Outset;
					break;
				default:
					bs = def;
					break;
			}
			return bs;
		}
	}
	/// <summary>
	/// Allowed values for border styles.  Note: these may not be actually supported depending
	/// on the renderer used.
	/// </summary>
	public enum BorderStyleEnum
	{
		/// <summary>
		/// No border
		/// </summary>
		None,
		/// <summary>
		/// Dotted line border
		/// </summary>
		Dotted,
		/// <summary>
		/// Dashed lin border
		/// </summary>
		Dashed,
		/// <summary>
		/// Solid line border
		/// </summary>
		Solid,
		/// <summary>
		/// Double line border
		/// </summary>
		Double,
		/// <summary>
		/// Grooved border
		/// </summary>
		Groove,
		/// <summary>
		/// Ridge border
		/// </summary>
		Ridge,
		/// <summary>
		/// Inset border
		/// </summary>
		Inset,
		/// <summary>
		/// Windows Inset border
		/// </summary>
		WindowInset,
		/// <summary>
		/// Outset border
		/// </summary>
		Outset
	}
}
