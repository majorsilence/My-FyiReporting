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
	/// The default value for a parameter.
	///</summary>
	[Serializable]
	internal class DefaultValue : ReportLink
	{
		// Only one of Values and DataSetReference can be specified.
		DataSetReference _DataSetReference;	// The query to execute to obtain the default value(s) for the parameter.
									// The default is the first value of the ValueField.
		Values _Values;		// The default values for the parameter

		internal DefaultValue(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_DataSetReference=null;
			_Values=null;

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
					case "Values":
						_Values = new Values(r, this, xNodeLoop);
						break;
					default:
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			if (_DataSetReference != null)
				_DataSetReference.FinalPass();
			if (_Values != null)
				_Values.FinalPass();
			return;
		}

		internal DataSetReference DataSetReference
		{
			get { return  _DataSetReference; }
			set {  _DataSetReference = value; }
		}

		internal object[] GetValue(Report rpt)
		{
			if (_Values != null)
				return ValuesCalc(rpt);
			object[] dValues = this.GetDataValues(rpt);
			if (dValues != null)
				return dValues;

			string[] dsValues;
			if (_DataSetReference != null)
				_DataSetReference.SupplyValues(rpt, out dsValues, out dValues);

			this.SetDataValues(rpt, dValues);
			return dValues;
		}

		internal Values Values
		{
			get { return  _Values; }
			set {  _Values = value; }
		}

		internal object[] ValuesCalc(Report rpt)
		{
			if (_Values == null)
				return null;
			object[] result = new object[_Values.Count];
			int index=0;
			foreach (Expression v in _Values)
			{
				result[index++] = v.Evaluate(rpt, null);
			}
			return result;
		}

		private object[] GetDataValues(Report rpt)
		{
			return rpt.Cache.Get(this, "datavalues") as object[];
		}

		private void SetDataValues(Report rpt, object[] vs)
		{
			if (vs == null)
				rpt.Cache.Remove(this, "datavalues");
			else
				rpt.Cache.AddReplace(this, "datavalues", vs);
		}
	}
}
