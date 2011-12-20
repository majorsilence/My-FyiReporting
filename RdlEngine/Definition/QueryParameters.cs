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
	/// Collection of query parameters.
	///</summary>
	[Serializable]
	internal class QueryParameters : ReportLink
	{
        List<QueryParameter> _Items;			// list of QueryParameter
        bool _ContainsArray;                   // true if any of the parameters is an array reference

		internal QueryParameters(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
            _ContainsArray = false;
			QueryParameter q;
            _Items = new List<QueryParameter>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "QueryParameter":
						q = new QueryParameter(r, this, xNodeLoop);
						break;
					default:	
						q=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown QueryParameters element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (q != null)
					_Items.Add(q);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For QueryParameters at least one QueryParameter is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (QueryParameter q in _Items)
			{
				q.FinalPass();
                if (q.IsArray)
                    _ContainsArray = true;
			}
			return;
		}

        internal List<QueryParameter> Items
		{
			get { return  _Items; }
		}
        internal bool ContainsArray
        {
            get { return _ContainsArray; }
        }
	}
}
