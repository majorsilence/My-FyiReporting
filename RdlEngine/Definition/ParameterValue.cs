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

namespace fyiReporting.RDL
{
	///<summary>
	/// The value of a report paramenter.
	///</summary>
	[Serializable]
	internal class ParameterValue : ReportLink
	{
		Expression _Value;		// Possible value (variant) for the parameter
								// For Boolean: use "true" and "false"
								// For DateTime: use ISO 8601
								// For Float: use "." as the optional decimal separator.
		Expression _Label;		// Label (string) for the value to display in the UI
								// If not supplied, the _Value is used as the label. If
								// _Value not supplied, _Label is the empty string;
	
		internal ParameterValue(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Value=null;
			_Label=null;

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
					case "Label":
						_Label = new Expression(r, this, xNodeLoop, ExpressionType.String);
						break;
					default:
						break;
				}
			}
		

		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Value != null)
				_Value.FinalPass();
			if (_Label != null)
				_Label.FinalPass();
			return;
		}

		internal Expression Value
		{
			get { return  _Value; }
			set {  _Value = value; }
		}


		internal Expression Label
		{
			get { return  _Label; }
			set {  _Label = value; }
		}
	}
}
