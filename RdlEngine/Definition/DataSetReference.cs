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
	/// The query to use to obtain a list of values for a parameter.   See ValidValues.
	///</summary>
	[Serializable]
	internal class DataSetReference : ReportLink
	{
		string _DataSetName;	//Name of the data set to use.
		DataSetDefn _ds;		// DataSet that matches the name
		string _ValueField;		//Name of the field to use for the values/defaults for the parameter
		string _LabelField;		//Name of the field to use for the value to display to the		
								// user for the selection.  If not supplied or the returned
								// value is null, the value in the ValueField is used.
								//  not used for DefaultValue.
		Field _vField;			// resolved value name
		Field _lField;			// resolved label name
		internal DataSetReference(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_DataSetName=null;
			_ValueField=null;
			_LabelField=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DataSetName":
						_DataSetName = xNodeLoop.InnerText;
						break;
					case "ValueField":
						_ValueField = xNodeLoop.InnerText;
						break;
					case "LabelField":
						_LabelField = xNodeLoop.InnerText;
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown DataSetReference element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_DataSetName == null)
				OwnerReport.rl.LogError(8, "DataSetReference DataSetName is required but not specified.");
			if (_ValueField == null)
				OwnerReport.rl.LogError(8, "DataSetReference ValueField is required but not specified for" + _DataSetName==null? "<unknown name>": _DataSetName);
		}
		
		override internal void FinalPass()
		{
			_ds = OwnerReport.DataSetsDefn[this._DataSetName];
			if (_ds == null)
				OwnerReport.rl.LogError(8, "DataSetReference refers to unknown data set '" + _DataSetName + "'");
			else
			{
				_vField = _ds.Fields[_ValueField];
				if (_vField == null)
					OwnerReport.rl.LogError(8, "ValueField refers to unknown field '" + _ValueField + "'");
				else
				{
					if (_LabelField == null)
						_lField = _vField;
					else
					{
						_lField = _ds.Fields[_LabelField];
						if (_lField == null)
							OwnerReport.rl.LogError(8, "LabelField refers to unknown field '" + _LabelField + "'");
					}
				}
			}

			return;
		}

		internal string DataSetName
		{
			get { return  _DataSetName; }
			set {  _DataSetName = value; }
		}

		internal string ValueField
		{
			get { return  _ValueField; }
			set {  _ValueField = value; }
		}

		internal string LabelField
		{
			get { return  _LabelField; }
			set {  _LabelField = value; }
		}

		internal void SupplyValues(Report rpt, out string[] displayValues, out object[] dataValues)
		{
			displayValues = null;
			dataValues = null;
			Rows rows = _ds.Query.GetMyData(rpt);
			if (rows == null)		// do we already have data?
			{
				// TODO:  this is wasteful;  likely to reretrieve the data again when report run with parameters
				//   should mark a dataset as only having one retrieval???
				bool lConnect = _ds.Query.DataSourceDefn.IsConnected(rpt);
				if (!lConnect)
					_ds.Query.DataSourceDefn.ConnectDataSource(rpt);	// connect; since not already connected
				_ds.GetData(rpt);									// get the data
				if (!lConnect)										// if we connected; then
					_ds.Query.DataSourceDefn.CleanUp(rpt);				//   we cleanup
				rows = _ds.Query.GetMyData(rpt);
				if (rows == null)			// any data now?
					return;					// no out of luck
			}

			displayValues = new string[rows.Data.Count];
			dataValues = new object[displayValues.Length];
			int index=0;
			object o;
			foreach (Row r in rows.Data)
			{
				dataValues[index] = r.Data[_vField.ColumnNumber];
				o = r.Data[_lField.ColumnNumber];
				if (o == null || o == DBNull.Value)
					displayValues[index] = "";
				else
					displayValues[index] = o.ToString();
				index++;
			}
		}
	}
}
