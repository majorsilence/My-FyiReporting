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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Obtain the runtime value of a report parameter label.
	/// </summary>
	[Serializable]
	internal class FunctionReportParameterLabel : FunctionReportParameter
	{
		/// <summary>
		/// obtain value of ReportParameter
		/// </summary>
		public FunctionReportParameterLabel(ReportParameter parm): base(parm) 
		{
		}

		public override TypeCode GetTypeCode()
		{
            if (this.ParameterMethod == ReportParameterMethodEnum.Value)
                return TypeCode.String;
            else
                return base.GetTypeCode();
		}

		public override Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public override Task<IExpr> ConstantOptimization()
		{	// not a constant expression
			return Task.FromResult(this as IExpr);
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async override Task<object> Evaluate(Report rpt, Row row)
		{
			string v = await base.EvaluateString(rpt, row);

			if (p.ValidValues == null)
				return v;

			string[] displayValues = await p.ValidValues.DisplayValues(rpt);
			object[] dataValues = await p.ValidValues.DataValues(rpt);

			for (int i=0; i < dataValues.Length; i++)
			{
				if (dataValues[i].ToString() == v)
					return displayValues[i];
			}

			return v;
		}
		
		public async override Task<double> EvaluateDouble(Report rpt, Row row)
		{	
			string r = await EvaluateString(rpt, row);

			return r == null? double.MinValue: Convert.ToDouble(r);
		}
		
		public override async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			string r = await EvaluateString(rpt, row);

			return r == null? decimal.MinValue: Convert.ToDecimal(r);
		}

		public async override Task<string> EvaluateString(Report rpt, Row row)
		{
			return (string)await Evaluate(rpt, row);
		}

		public async override Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			string r = await EvaluateString(rpt, row);

			return r == null? DateTime.MinValue: Convert.ToDateTime(r);
		}

		public async override Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			string r = await EvaluateString(rpt, row);

			return r.ToLower() == "true"? true: false;
		}
	}
}
