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
using System.Collections.Generic;
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// In Charts, the collection of data values for a single data point.
	///</summary>
	[Serializable]
	internal class DataValues : ReportLink
	{
        List<DataValue> _Items;			// list of DataValue

		internal DataValues(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			DataValue dv;
            _Items = new List<DataValue>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DataValue":
						dv = new DataValue(r, this, xNodeLoop);
						break;
					default:	
						dv=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown DataValues element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (dv != null)
					_Items.Add(dv);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For DataValues at least one DataValue is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (DataValue dv in _Items)
			{
				dv.FinalPass();
			}
			return;
		}

        internal List<DataValue> Items
		{
			get { return  _Items; }
		}
	}
}
