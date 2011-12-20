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
	/// Query to execute for valid values of a parameter.
	///</summary>
	[Serializable]
	internal class ValidValues : ReportLink
	{
		DataSetReference _DataSetReference;	// The query to execute to obtain a list of
											// possible values for the parameter.
		ParameterValues _ParameterValues;	// Hardcoded values for the parameter	

		internal ValidValues(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_DataSetReference=null;
			_ParameterValues=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DataSetReference":
						_DataSetReference = new DataSetReference(r, this, xNodeLoop);
						break;
					case "ParameterValues":
						_ParameterValues = new ParameterValues(r, this, xNodeLoop);
						break;
					default:
						OwnerReport.rl.LogError(4, "Unknown ValidValues element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_DataSetReference == null)
			{
				if (_ParameterValues == null)				
				{
					OwnerReport.rl.LogError(8, "For ValidValues element either DataSetReference or ParameterValue must be specified, but not both.");
				}
			}
			else if (_ParameterValues != null)
			{
				OwnerReport.rl.LogError(8, "For ValidValues element either DataSetReference or ParameterValue must be specified, but not both.");
			}
		}
		
		override internal void FinalPass()
		{
			if (_DataSetReference != null)
				_DataSetReference.FinalPass();
			if (_ParameterValues != null)
				_ParameterValues.FinalPass();
			return;
		}

		internal DataSetReference DataSetReference
		{
			get { return  _DataSetReference; }
			set {  _DataSetReference = value; }
		}

		internal ParameterValues ParameterValues
		{
			get { return  _ParameterValues; }
			set {  _ParameterValues = value; }
		}

		internal string[] DisplayValues(Report rpt)
		{
			lock (this)
			{
				string[] dsplValues = rpt.Cache.Get(this, "displayvalues") as string[];
				object[] dataValues;

				if (dsplValues != null)
					return dsplValues;

				if (_DataSetReference != null)
					_DataSetReference.SupplyValues(rpt, out dsplValues, out dataValues);
				else
					_ParameterValues.SupplyValues(rpt, out dsplValues, out dataValues);

                if (dataValues == null)
                    dataValues = new object[0];
                if (dsplValues == null)
                    dsplValues = new string[0];

				// there shouldn't be a problem; but if there is it doesn't matter as values can be recreated
				try {rpt.Cache.Add(this, "datavalues", dataValues);} 
				catch (Exception e1)
				{
					rpt.rl.LogError(4, "Error caching data values.  " + e1.Message);
				}
				try {rpt.Cache.Add(this, "displayvalues", dsplValues);} 
				catch (Exception e2)
				{
					rpt.rl.LogError(4, "Error caching display values.  " + e2.Message);
				}

				return dsplValues;
			}
		}

		internal object[] DataValues(Report rpt)
		{
			lock (this)
			{
				string[] dsplValues;
				object[] dataValues = rpt.Cache.Get(this, "datavalues") as object[];

				if (dataValues != null)
					return dataValues;

				if (_DataSetReference != null)
					_DataSetReference.SupplyValues(rpt, out dsplValues, out dataValues);
				else
					_ParameterValues.SupplyValues(rpt, out dsplValues, out dataValues);

                if (dataValues == null)
                    dataValues = new object[0];
                if (dsplValues == null)
                    dsplValues = new string[0];

				// there shouldn't be a problem; but if there is it doesn't matter as values can be recreated
				try {rpt.Cache.Add(this, "datavalues", dataValues);} 
				catch (Exception e1)
				{
					rpt.rl.LogError(4, "Error caching data values.  " + e1.Message);
				}
				try {rpt.Cache.Add(this, "displayvalues", dsplValues);} 
				catch (Exception e2)
				{
					rpt.rl.LogError(4, "Error caching display values.  " + e2.Message);
				}
				return dataValues;
			}
		}
	}
}
