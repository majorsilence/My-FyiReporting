

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;


namespace Majorsilence.Reporting.Rdl
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
