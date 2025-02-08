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
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Runtime Information about a set of data; public interface to the definition
	///</summary>
	[Serializable]
	public class DataSet
	{
		Report _rpt;		//	the runtime report
		DataSetDefn _dsd;	//  the true definition of the DataSet
	
		internal DataSet(Report rpt, DataSetDefn dsd)
		{
			_rpt = rpt;
			_dsd = dsd;
		}

		public async Task SetData(IDataReader dr)
		{
            await _dsd.Query.SetData(_rpt, dr, _dsd.Fields, _dsd.Filters);		// get the data (and apply the filters
		}

		public async Task SetData(DataTable dt)
		{
            await _dsd.Query.SetData(_rpt, dt, _dsd.Fields, _dsd.Filters);
		}

		public async Task SetData(XmlDocument xmlDoc)
		{
            await _dsd.Query.SetData(_rpt, xmlDoc, _dsd.Fields, _dsd.Filters);
		}

        /// <summary>
        /// Sets the data in the dataset from an IEnumerable. The content of the IEnumerable
        /// depends on the flag collection. If collection is false it will contain classes whose fields
        /// or properties will be matched to the dataset field names. If collection is true it may 
        /// contain IDictionary(s) that will be matched by key with the field name or IEnumerable(s) 
        /// that will be matched by column number. It is possible to have a mix of IDictionary and 
        /// IEnumerable when collection is true.
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="collection"></param>
		public async Task SetData(IEnumerable ie, bool collection = false)
		{
            await _dsd.Query.SetData(_rpt, ie, _dsd.Fields, _dsd.Filters, collection);
		}

        public async Task SetSource(string sql)
        {
            await _dsd.Query.CommandText.SetSource(sql);
        }


	}
}
