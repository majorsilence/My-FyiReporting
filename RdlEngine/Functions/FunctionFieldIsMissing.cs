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
using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// IsMissing attribute
	/// </summary>
	[Serializable]
	internal class FunctionFieldIsMissing : FunctionField
	{
		/// <summary>
		/// Determine if value of Field is available
		/// </summary>
		public FunctionFieldIsMissing(Field fld) : base(fld)
		{
		}
		public FunctionFieldIsMissing(string method) : base(method)
		{
		}

		public override TypeCode GetTypeCode()
		{
			return TypeCode.Boolean;
		}

		public override Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public override Task<IExpr> ConstantOptimization()
		{	
			return Task.FromResult(this as IExpr);	// not a constant
		}

		// 
		public override async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateBoolean(rpt, row);
		}
		
		public override async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return await EvaluateBoolean(rpt, row)? 1: 0;
		}
		
		public override async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return await EvaluateBoolean(rpt, row)? 1m: 0m;
		}

		public override async Task<string> EvaluateString(Report rpt, Row row)
		{
			return await EvaluateBoolean(rpt, row)? "True": "False";
		}

		public override Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			return Task.FromResult(DateTime.MinValue);
		}

		public override async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			object o = await base.Evaluate(rpt, row);
			if(o is double)
				return double.IsNaN((double)o) ? true : false;
			else
				return o == null? true: false;
		}
	}
}
