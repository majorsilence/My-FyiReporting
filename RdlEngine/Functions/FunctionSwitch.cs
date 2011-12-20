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


using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// switch function like this example: Switch(a=1, "a1", a=2, "a2", true, "other")
	/// </summary>
	[Serializable]
	internal class FunctionSwitch : IExpr
	{
		IExpr[] _expr;		// boolean expression
		TypeCode _tc;

		/// <summary>
		/// Switch function.  Evaluates boolean expression in order and returns result of
		/// the first true.  For example, Switch(a=1, "a1", a=2, "a2", true, "other")
		/// </summary>
		public FunctionSwitch(IExpr[] expr) 
		{
			_expr = expr;
			_tc = _expr[1].GetTypeCode();
		}

		public TypeCode GetTypeCode()
		{
			return _tc;
		}

		public bool IsConstant()
		{
			return false;		// we could be more sophisticated here; but not much benefit
		}

		public IExpr ConstantOptimization()
		{
			// simplify all expression if possible
			for (int i=0; i < _expr.Length; i++)
			{
				_expr[i] = _expr[i].ConstantOptimization();
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			bool result;
			for (int i=0; i < _expr.Length; i = i+2)
			{
				result = _expr[i].EvaluateBoolean(rpt, row);
				if (result)
				{
					object o = _expr[i+1].Evaluate(rpt, row);
					// We may need to convert to same type as first type
					if (i == 0 || _tc == _expr[i+1].GetTypeCode())	// first typecode will always match 
						return o;

					return Convert.ChangeType(o, _tc);
				}
			}

			return null;
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToBoolean(result);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDouble(result);
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDecimal(result);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            object result = Evaluate(rpt, row);
            return Convert.ToInt32(result);
        }

        public string EvaluateString(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToString(result);
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}
	}
}
