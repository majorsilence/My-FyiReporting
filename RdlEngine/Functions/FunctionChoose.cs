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
	/// Choose function of the form Choose(int, expr1, expr2, ...)
	/// 
	///	
	/// </summary>
	[Serializable]
	internal class FunctionChoose : IExpr
	{
		IExpr[] _expr;
		TypeCode _tc;

		/// <summary>
		/// Choose function of the form Choose(int, expr1, expr2, ...)
		/// </summary>
		public FunctionChoose(IExpr[] ie) 
		{
			_expr = ie;
			_tc = _expr[1].GetTypeCode();

		}

		public TypeCode GetTypeCode()
		{
			return _tc;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(false);		// we could be more sophisticated here; but not much benefit
		}

		public async Task<IExpr> ConstantOptimization()
		{
			// simplify all expression if possible
			for (int i=0; i < _expr.Length; i++)
			{
				_expr[i] = await _expr[i].ConstantOptimization();
			}

			return this;
		}

		// 
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			double di = await _expr[0].EvaluateDouble(rpt, row);
			int i = (int) di;		// force it to integer; we'll accept truncation
			if (i >= _expr.Length || i <= 0)
				return null;
			
			return await _expr[i].Evaluate(rpt, row);
		}

		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToBoolean(result);
		}
		
		public async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDouble(result);
		}
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDecimal(result);
		}

        public async Task<int> EvaluateInt32(Report rpt, Row row)
        {
            object result = await Evaluate(rpt, row);
            return Convert.ToInt32(result);
        }

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToString(result);
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}
	}
}
