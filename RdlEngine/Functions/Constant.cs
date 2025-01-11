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
	/// <p>Expression definition</p>
	/// </summary>
	[Serializable]
	internal class Constant : IExpr
	{
		string _Value;		// value of the constant

		/// <summary>
		/// Constant - as opposed to an expression
		/// </summary>
		public Constant(string v) 
		{
			_Value = v;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(true);
		}

		public Task<IExpr> ConstantOptimization()
		{	// already constant expression
			return Task.FromResult(this as IExpr);
		}

		public Task<object> Evaluate(Report rpt, Row row)
		{
			return Task.FromResult((object)_Value);
		}

		public Task<string> EvaluateString(Report rpt, Row row)
		{
			return Task.FromResult(_Value);
		}
		
		public Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToDouble(_Value));
		}
		
		public Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToDecimal(_Value));
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return Task.FromResult(Convert.ToInt32(_Value));
        }

		public Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToDateTime(_Value));
		}

		public Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToBoolean(_Value));
		}
	}
}
