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
	/// Aggregate function: CountRows
	/// </summary>
	[Serializable]
	internal class FunctionAggrCountRows : FunctionAggr, IExpr
	{
		/// <summary>
		/// Aggregate function: CountRows
		/// 
		///	Return type is double
		/// </summary>
		public FunctionAggrCountRows(object scp):base(null, scp) 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		// Evaluate is for interpretation
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return (object)await EvaluateInt32(rpt, row);
		}
		
		public Task<int> EvaluateInt32(Report rpt, Row row)
		{
			bool bSave=true;
			RowEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return Task.FromResult(0);

			int count = re.LastRow - re.FirstRow + 1;

			return Task.FromResult(count);
		}

        public async Task<double> EvaluateDouble(Report rpt, Row row)
        {
            int d = await EvaluateInt32(rpt, row);

            return Convert.ToDouble(d);
        }
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			int d = await EvaluateInt32(rpt, row);

			return Convert.ToDecimal(d);
		}

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);
			return Convert.ToString(result);
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}
	}
}
