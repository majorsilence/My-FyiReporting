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
using System.IO;
using System.Threading.Tasks;
using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Page number operator.   Relies on render to set the proper page #.
	/// </summary>
	[Serializable]
	internal class FunctionPageNumber : IExpr
	{
		/// <summary>
		/// Current page number
		/// </summary>
		public FunctionPageNumber() 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public Task<IExpr> ConstantOptimization()
		{	// not a constant expression
			return Task.FromResult(this as IExpr);
		}

		// Evaluate is for interpretation  
		public Task<object> Evaluate(Report rpt, Row row)
		{
            return rpt == null ? Task.FromResult((int) 0 as object) : Task.FromResult((int) rpt.PageNumber as object);
		}
		
		public Task<double> EvaluateDouble(Report rpt, Row row)
		{	
			return rpt == null? Task.FromResult(0d): Task.FromResult((double)rpt.PageNumber);
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return rpt == null ? Task.FromResult(0) : Task.FromResult(rpt.PageNumber);
        }
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);

			return Convert.ToDecimal(result);
		}

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);
			return result.ToString();
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);
			return Convert.ToDateTime(result);
		}

		public Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Task.FromResult(false);
		}
	}
}
