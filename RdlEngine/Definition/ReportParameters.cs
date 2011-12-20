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
	/// Collection of report parameters.
	///</summary>
	[Serializable]
	internal class ReportParameters : ReportLink, ICollection
	{
		IDictionary _Items;			// list of report items

		internal ReportParameters(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			if (xNode.ChildNodes.Count < 10)
				_Items = new ListDictionary();	// Hashtable is overkill for small lists
			else
				_Items = new Hashtable(xNode.ChildNodes.Count);

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "ReportParameter")
				{
					ReportParameter rp = new ReportParameter(r, this, xNodeLoop);
                    if (rp.Name != null)
					    _Items.Add(rp.Name.Nm, rp);
				}
				else
					OwnerReport.rl.LogError(4, "Unknown ReportParameters element '" + xNodeLoop.Name + "' ignored.");
			}
		}
		
		internal void SetRuntimeValues(Report rpt, IDictionary parms)
		{
			// Fill the values to use in the report parameters
			foreach (string pname in parms.Keys)	// Loop thru the passed parameters
			{
				ReportParameter rp = (ReportParameter) _Items[pname];
				if (rp == null)
				{	// When not found treat it as a warning message
					if (!pname.StartsWith("rs:"))	// don't care about report server parameters
						rpt.rl.LogError(4, "Unknown ReportParameter passed '" + pname + "' ignored.");
					continue;
				}

                // Search for the valid values
                object parmValue = parms[pname];
                if (parmValue is string && rp.ValidValues != null)
                {
                    string[] dvs = rp.ValidValues.DisplayValues(rpt);
                    if (dvs != null && dvs.Length > 0)
                    {
                        for (int i = 0; i < dvs.Length; i++)
                        {
                            if (dvs[i] == (string) parmValue)
                            {
                                object[] dv = rp.ValidValues.DataValues(rpt);
                                parmValue = dv[i];
                                break;
                            }
                        }
                    }
                }
				rp.SetRuntimeValue(rpt, parmValue);
			}

			return;
		}

		override internal void FinalPass()
		{
			foreach (ReportParameter rp in _Items.Values)
			{
				rp.FinalPass();
			}
			return;
		}

		internal IDictionary Items
		{
			get { return  _Items; }
		}
		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return _Items.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return _Items.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_Items.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _Items.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _Items.Values.GetEnumerator();
		}

		#endregion
	}
}
