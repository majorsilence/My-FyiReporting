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
	/// Chart static member.
	///</summary>
	[Serializable]
	internal class StaticMember : ReportLink
	{
		Expression _Label;	//(Variant) The label for the static member (displayed either on
							// the category axis or legend, as appropriate).		
	
		internal StaticMember(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Label=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Label":
						_Label = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					default:
						break;
				}
			}
			if (_Label == null)
				OwnerReport.rl.LogError(8, "StaticMember requires the Label element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Label != null)
				_Label.FinalPass();
			return;
		}

		internal Expression Label
		{
			get { return  _Label; }
			set {  _Label = value; }
		}
	}
}
