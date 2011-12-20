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
	/// CategoryGroupings definition and processing.
	///</summary>
	[Serializable]
	internal class CategoryGroupings : ReportLink
	{
        List<CategoryGrouping> _Items;			// list of category groupings

		internal CategoryGroupings(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			CategoryGrouping cg;
            _Items = new List<CategoryGrouping>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "CategoryGrouping":
						cg = new CategoryGrouping(r, this, xNodeLoop);
						break;
					default:
						cg=null;		// don't know what this is
						break;
				}
				if (cg != null)
					_Items.Add(cg);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For CategoryGroupings at least one CategoryGrouping is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (CategoryGrouping cg in _Items)
			{
				cg.FinalPass();
			}
			return;
		}

        internal List<CategoryGrouping> Items
		{
			get { return  _Items; }
		}
	}
}
