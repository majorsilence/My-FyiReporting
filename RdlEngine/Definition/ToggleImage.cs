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
	/// Toggle image definition.
	///</summary>
	[Serializable]
	internal class ToggleImage : ReportLink
	{
		Expression _InitialState;	//(Boolean)
					//A Boolean expression, the value of which
					//determines the initial state of the toggle image.
					//True = �expanded� (i.e. a minus sign). False =
					//�collapsed� (i.e. a plus sign)		
	
		internal ToggleImage(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_InitialState=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "InitialState":
						_InitialState = new Expression(r, this, xNodeLoop, ExpressionType.Boolean);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ToggleImage element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_InitialState == null)
				OwnerReport.rl.LogError(8, "ToggleImage requires the InitialState element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_InitialState != null)
				_InitialState.FinalPass();
			return;
		}

		internal Expression InitialState
		{
			get { return  _InitialState; }
			set {  _InitialState = value; }
		}
	}
}
