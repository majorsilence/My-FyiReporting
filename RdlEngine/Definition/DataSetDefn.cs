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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace fyiReporting.RDL
{
	///<summary>
	/// Information about a set of data that will be retrieved when report data is requested.
	///</summary>
	[Serializable]
	internal class DataSetDefn : ReportLink
	{
		Name _Name;			// Name of the data set
							// Cannot be the same name as any data region or grouping
		Fields _Fields;		// The fields in the data set
		Query _Query;		// Information about the data source, including
							//  connection information, query, etc. required to
							//  get the data from the data source.
		string _XmlRowData;	// User specified data; instead of invoking query we use inline XML data
							//   This is particularlly useful for testing and reporting bugs when
							//   you don't have access to the datasource.
		string _XmlRowFile; //   - File should be loaded for user data; if not found use XmlRowData
		TrueFalseAutoEnum _CaseSensitivity;	// Indicates if the data is case sensitive; true/false/auto
							// if auto; should query data provider; Default false if data provider doesn't support.
		string _Collation;	// The locale to use for the collation sequence for sorting data.
							//  See Microsoft SQL Server collation codes (http://msdn.microsoft.com/library/enus/tsqlref/ts_ca-co_2e95.asp).
							// If no Collation is specified, the application
							// should attempt to derive the collation setting by
							// querying the data provider.
							// Defaults to the application�s locale settings if
							// the data provider does not support that method
							// or returns an unsupported or invalid value
		TrueFalseAutoEnum _AccentSensitivity;	// Indicates whether the data is accent sensitive
							// True | False | Auto (Default)
							// If Auto is specified, the application should
							// attempt to derive the accent sensitivity setting
							// by querying the data provider. Defaults to False
							// if the data provider does not support that method.
		TrueFalseAutoEnum _KanatypeSensitivity;	// Indicates if the data is kanatype sensitive
							// True | False | Auto (Default)
							// If Auto is specified, the Application should
							// attempt to derive the kanatype sensitivity
							// setting by querying the data provider. Defaults
							// to False if the data provider does not support
							// that method.
		TrueFalseAutoEnum _WidthSensitivity;	// Indicates if the data is width sensitive
							// True | False | Auto (Default)
							// If Auto is specified, the Application should
							// attempt to derive the width sensitivity setting by
							// querying the data provider. Defaults to False if
							// the data provider does not support that method.
		Filters _Filters;	// Filters to apply to each row of data in the data set
        List<Textbox> _HideDuplicates;	// holds any textboxes that use this as a hideduplicate scope
	
		internal DataSetDefn(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Name=null;
			_Fields=null;
			_Query=null;
			_CaseSensitivity=TrueFalseAutoEnum.True;	
			_Collation=null;
			_AccentSensitivity=TrueFalseAutoEnum.False;
			_KanatypeSensitivity=TrueFalseAutoEnum.False;
			_WidthSensitivity=TrueFalseAutoEnum.False;
			_Filters=null;
			_HideDuplicates=null;
			// Run thru the attributes
			foreach(XmlAttribute xAttr in xNode.Attributes)
			{
				switch (xAttr.Name)
				{
					case "Name":
						_Name = new Name(xAttr.Value);
						break;
				}
			}

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Fields":
						_Fields = new Fields(r, this, xNodeLoop);
						break;
					case "Query":
						_Query = new Query(r, this, xNodeLoop);
						break;
					case "Rows":	// Extension !!!!!!!!!!!!!!!!!!!!!!!
					case "fyi:Rows":
						_XmlRowData = "<?xml version='1.0' encoding='UTF-8'?><Rows>" + xNodeLoop.InnerXml + "</Rows>";
						foreach(XmlAttribute xA in xNodeLoop.Attributes)
						{
							if (xA.Name == "File")
								_XmlRowFile = xA.Value;
						}
						break;
					case "CaseSensitivity":
						_CaseSensitivity = TrueFalseAuto.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Collation":
						_Collation = xNodeLoop.InnerText;
						break;
					case "AccentSensitivity":
						_AccentSensitivity = TrueFalseAuto.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "KanatypeSensitivity":
						_KanatypeSensitivity = TrueFalseAuto.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "WidthSensitivity":
						_WidthSensitivity = TrueFalseAuto.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Filters":
						_Filters = new Filters(r, this, xNodeLoop);
						break;
					default:
						OwnerReport.rl.LogError(4, "Unknown DataSet element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (this.Name != null)
				OwnerReport.LUAggrScope.Add(this.Name.Nm, this);		// add to referenceable TextBoxes
			else
				OwnerReport.rl.LogError(4, "Name attribute must be specified in a DataSet.");

			if (_Query == null)
				OwnerReport.rl.LogError(8, "Query element must be specified in a DataSet.");

		}
		
		override internal void FinalPass()
		{
			if (_Query != null)			// query must be resolved before fields
				_Query.FinalPass();
			if (_Fields != null)
				_Fields.FinalPass();
			if (_Filters != null)
				_Filters.FinalPass();
			return;
		}

		internal void AddHideDuplicates(Textbox tb)
		{
			if (_HideDuplicates == null)
				_HideDuplicates = new List<Textbox>();
			_HideDuplicates.Add(tb);
		}

		internal bool GetData(Report rpt)
		{
			ResetHideDuplicates(rpt);

            bool bRows = false;
			if (_XmlRowData != null)
			{		// Override the query and provide data from XML
				string xdata = GetDataFile(rpt, _XmlRowFile);
                if (xdata == null)
                {
                    xdata = _XmlRowData;					// didn't find any data
                }

				bRows = _Query.GetData(rpt, xdata, _Fields, _Filters);	// get the data (and apply the filters
				return bRows;
			}

            if (_Query == null)
            {
                return bRows;
            }

			bRows = _Query.GetData(rpt, this._Fields, _Filters);	// get the data (and apply the filters
            return bRows;
		}

		private string GetDataFile(Report rpt, string file)
		{
            if (file == null)		// no file no data
            {
                return null;
            }

			StreamReader fs=null;
			string d=null;
			string fullpath;
			string folder = rpt.Folder;
            if (folder == null || folder.Length == 0)
            {
                fullpath = file;
            }
            else
            {
                fullpath = folder + Path.DirectorySeparatorChar + file;
            }

			try
			{
				fs = new StreamReader(fullpath);
				d = fs.ReadToEnd();
			}
			catch (FileNotFoundException fe)
			{
				rpt.rl.LogError(4, string.Format("XML data file {0} not found.\n{1}", fullpath, fe.StackTrace));
				d = null;
			}
			catch (Exception ge)
			{
				rpt.rl.LogError(4, string.Format("XML data file error {0}\n{1}\n{2}", fullpath, ge.Message, ge.StackTrace));
				d = null;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			return d;
		}

		internal void SetData(Report rpt, IDataReader dr)
		{
			Query.SetData(rpt, dr, _Fields, _Filters);		// get the data (and apply the filters
		}

		internal void SetData(Report rpt, DataTable dt)
		{
			Query.SetData(rpt, dt, _Fields, _Filters);
		}

		internal void SetData(Report rpt, XmlDocument xmlDoc)
		{
			Query.SetData(rpt, xmlDoc, _Fields, _Filters);
		}

		internal void SetData(Report rpt, IEnumerable ie)
		{
			Query.SetData(rpt, ie, _Fields, _Filters);
		}

		internal void ResetHideDuplicates(Report rpt)
		{
            if (_HideDuplicates == null)
            {
                return;
            }

            foreach (Textbox tb in _HideDuplicates)
            {
                tb.ResetPrevious(rpt);
            }
		}

		internal Name Name
		{
			get { return  _Name; }
			set {  _Name = value; }
		}

		internal Fields Fields
		{
			get { return  _Fields; }
			set {  _Fields = value; }
		}

		internal Query Query
		{
			get { return  _Query; }
			set {  _Query = value; }
		}

		internal TrueFalseAutoEnum CaseSensitivity
		{
			get { return  _CaseSensitivity; }
			set {  _CaseSensitivity = value; }
		}

		internal string Collation
		{
			get { return  _Collation; }
			set {  _Collation = value; }
		}

		internal TrueFalseAutoEnum AccentSensitivity
		{
			get { return  _AccentSensitivity; }
			set {  _AccentSensitivity = value; }
		}

		internal TrueFalseAutoEnum KanatypeSensitivity
		{
			get { return  _KanatypeSensitivity; }
			set {  _KanatypeSensitivity = value; }
		}

		internal TrueFalseAutoEnum WidthSensitivity
		{
			get { return  _WidthSensitivity; }
			set {  _WidthSensitivity = value; }
		}

		internal Filters Filters
		{
			get { return  _Filters; }
			set {  _Filters = value; }
		}
	}
}
