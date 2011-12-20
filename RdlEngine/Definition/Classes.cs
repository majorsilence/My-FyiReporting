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
	/// Contains information about which classes to instantiate during report initialization.
	/// These instances can then be used in expressions throughout the report.
	///</summary>
	[Serializable]
	internal class Classes : ReportLink, IEnumerable
	{
        List<ReportClass> _Items;			// list of report class

		internal Classes(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
            _Items = new List<ReportClass>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "Class")
				{
					ReportClass rc = new ReportClass(r, this, xNodeLoop);
					_Items.Add(rc);
				}
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For Classes at least one Class is required.");
			else
                _Items.TrimExcess();
		}
		
		internal ReportClass this[string s]
		{
			get 
			{
				foreach (ReportClass rc in _Items)
				{
					if (rc.InstanceName.Nm == s)
						return rc;
				}
				return null;
			}
		}

		override internal void FinalPass()
		{
			foreach (ReportClass rc in _Items)
			{
				rc.FinalPass();
			}
			return;
		}

		internal void Load(Report rpt)
		{
			foreach (ReportClass rc in _Items)
			{
				rc.Load(rpt);
			}
			return;
		}

        internal List<ReportClass> Items
		{
			get { return  _Items; }
		}
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _Items.GetEnumerator();
		}

		#endregion
	}
}
