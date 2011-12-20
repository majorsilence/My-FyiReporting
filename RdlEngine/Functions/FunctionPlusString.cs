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
	/// Plus operator  of form lhs + rhs where operands are strings
	/// </summary>
	[Serializable]
	internal class FunctionPlusString : FunctionBinary, IExpr
	{

		/// <summary>
		/// append two strings together
		/// </summary>
		public FunctionPlusString(IExpr lhs, IExpr rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateString(rpt, row);
		}
	
		public IExpr ConstantOptimization()
		{
			_lhs = _lhs.ConstantOptimization();
			_rhs = _rhs.ConstantOptimization();
			if (_lhs.IsConstant() && _rhs.IsConstant())
			{
				string s = EvaluateString(null, null);
				return new ConstantString(s);
			}

			return this;
		}
	
		public double EvaluateDouble(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);

			return Convert.ToDouble(result);
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToDecimal(result);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            string result = EvaluateString(rpt, row);
            return Convert.ToInt32(result);
        }

		public string EvaluateString(Report rpt, Row row)
		{
			string lhs = _lhs.EvaluateString(rpt, row);
			string rhs = _rhs.EvaluateString(rpt, row);

			if (lhs != null && rhs != null)
				return lhs + rhs;
			else
				return null;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToBoolean(result);
		}
	}
}
