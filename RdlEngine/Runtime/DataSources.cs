

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Contains list of DataSource about how to connect to sources of data used by the DataSets.
	///</summary>
	[Serializable]
    public class DataSources : IEnumerable
	{
		Report _rpt;				// Runtime report
		ListDictionary _Items;		// list of report items

		internal DataSources(Report rpt, DataSourcesDefn dsds)
		{
			_rpt = rpt;
			_Items = new ListDictionary();

			// Loop thru all the child nodes
			foreach(DataSourceDefn dsd in dsds.Items.Values)
			{
				DataSource ds = new DataSource(rpt, dsd);
				_Items.Add(dsd.Name.Nm,	ds);
			}
		}

		public DataSource this[string name]
		{
			get 
			{
				return _Items[name] as DataSource;
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
