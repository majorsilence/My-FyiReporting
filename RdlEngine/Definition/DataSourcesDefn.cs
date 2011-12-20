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
using System.Collections.Specialized;
using System.Xml;


namespace fyiReporting.RDL
{
	///<summary>
	/// Contains list of DataSource about how to connect to sources of data used by the DataSets.
	///</summary>
	[Serializable]
	internal class DataSourcesDefn : ReportLink
	{
		ListDictionary _Items;			// list of report items

		internal DataSourcesDefn(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			// Run thru the attributes
//			foreach(XmlAttribute xAttr in xNode.Attributes)
//			{
//			}
			_Items = new ListDictionary();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "DataSource")
				{
					DataSourceDefn ds = new DataSourceDefn(r, this, xNodeLoop);
					if (ds.Name != null)
						_Items.Add(ds.Name.Nm, ds);
				}
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For DataSources at least one DataSource is required.");
		}

		public DataSourceDefn this[string name]
		{
			get 
			{
				return _Items[name] as DataSourceDefn;
			}
		}

		internal void CleanUp(Report rpt)		// closes any connections
		{
			foreach (DataSourceDefn ds in _Items.Values)
			{
				ds.CleanUp(rpt);
			}
		}
		
		override internal void FinalPass()
		{
			foreach (DataSourceDefn ds in _Items.Values)
			{
				ds.FinalPass();
			}
			return;
		}

		internal bool ConnectDataSources(Report rpt)
		{
			// Handle any parent connections if any	(ie we're in a subreport and want to use parent report connections
			if (rpt.ParentConnections != null && rpt.ParentConnections.Items != null)
			{	// we treat subreport merged transaction connections as set by the User 
				foreach (DataSourceDefn ds in _Items.Values)
				{
					foreach (DataSourceDefn dsp in rpt.ParentConnections.Items.Values)
					{
						if (ds.AreSameDataSource(dsp))
						{
							ds.SetUserConnection(rpt, dsp.GetConnection(rpt));
							break;
						}
					}
				}
			}

			foreach (DataSourceDefn ds in _Items.Values)
			{
				ds.ConnectDataSource(rpt);
			}
			return true;
		}


		internal ListDictionary Items
		{
			get { return  _Items; }
		}
	}
}
