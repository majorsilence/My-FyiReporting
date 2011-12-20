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
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Collection of parameter values.
	///</summary>
	[Serializable]
	internal class ParameterValues : ReportLink
	{
        List<ParameterValue> _Items;			// list of ParameterValue

		internal ParameterValues(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			ParameterValue pv;
            _Items = new List<ParameterValue>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ParameterValue":
						pv = new ParameterValue(r, this, xNodeLoop);
						break;
					default:
						pv=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ParameterValues element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (pv != null)
					_Items.Add(pv);
			}

			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For ParameterValues at least one ParameterValue is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (ParameterValue pv in _Items)
			{
				pv.FinalPass();
			}
			return;
		}

        internal List<ParameterValue> Items
		{
			get { return  _Items; }
		}

		internal void SupplyValues(Report rpt, out string[] displayValues, out object[] dataValues)
		{
			displayValues = new string[_Items.Count];
			dataValues = new object[_Items.Count];
			int index=0;
			// go thru the parameters extracting the data values
			foreach (ParameterValue pv in _Items)
			{
				if (pv.Value == null)
					dataValues[index] = null;
				else
					dataValues[index] = pv.Value.Evaluate(rpt, null);
				if (pv.Label == null)
				{	// if label is null use the data value; if not provided use ""
					if (dataValues[index] == null)
						displayValues[index] = "";
					else
						displayValues[index] = dataValues[index].ToString();
				}
				else
				{
					displayValues[index] = pv.Label.EvaluateString(rpt, null);
					if (displayValues[index] == null)
						displayValues[index] = "";
				}
				index++;
			}
			return;
		}
	}
}
