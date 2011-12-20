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
using System.Collections.Specialized;
using System.Xml;


namespace fyiReporting.RDL
{
	///<summary>
	/// The sets of data (defined by DataSet) that are retrieved as part of the Report.
	///</summary>
	[Serializable]
    public class DataSets : IEnumerable
	{
		Report _rpt;				// runtime report
		IDictionary _Items;			// list of report items

		internal DataSets(Report rpt, DataSetsDefn dsn)
		{
			_rpt = rpt;

			if (dsn.Items.Count < 10)
				_Items = new ListDictionary();	// Hashtable is overkill for small lists
			else
				_Items = new Hashtable(dsn.Items.Count);

			// Loop thru all the child nodes
			foreach(DataSetDefn dsd in dsn.Items.Values)
			{
				DataSet ds = new DataSet(rpt, dsd);
				_Items.Add(dsd.Name.Nm, ds);
			}
		}
		
		public DataSet this[string name]
		{
			get 
			{
				return _Items[name] as DataSet;
			}
		}

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _Items.Values.GetEnumerator();
        }

        #endregion
    }
}
