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
	/// In charts, the DataValue defines a single value for the DataPoint.
	///</summary>
	[Serializable]
	internal class DataValue : ReportLink
	{
		Expression _Value;	// (Variant) Value expression. Same restrictions as
							//  the expressions in a matrix cell		
		internal DataValue(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Value=null;

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
					default:
						break;
				}
			}
			if (_Value == null)
				OwnerReport.rl.LogError(8, "DataValue requires the Value element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Value != null)
				_Value.FinalPass();
			return;
		}


		internal Expression Value
		{
			get { return  _Value; }
			set {  _Value = value; }
		}
	}
}
