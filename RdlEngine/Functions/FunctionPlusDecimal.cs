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
	/// Plus operator  of form lhs + rhs where both operands are decimal
	/// </summary>
	[Serializable]
	internal class FunctionPlusDecimal : FunctionBinary , IExpr
	{
		/// <summary>
		/// Do plus on decimal data types
		/// </summary>
		public FunctionPlusDecimal() 
		{
		}

		public FunctionPlusDecimal(IExpr lhs, IExpr rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		public IExpr ConstantOptimization()
		{
			_lhs = _lhs.ConstantOptimization();
			_rhs = _rhs.ConstantOptimization();
			bool bLeftConst = _lhs.IsConstant();
			bool bRightConst = _rhs.IsConstant();
			if (bLeftConst && bRightConst)
			{
				decimal d = EvaluateDecimal(null, null);
				return new ConstantDecimal(d);
			}
			else if (bRightConst)
			{
				decimal d = _rhs.EvaluateDecimal(null, null);
				if (d == 0m)
					return _lhs;
			}
			else if (bLeftConst)
			{
				decimal d = _lhs.EvaluateDecimal(null, null);
				if (d == 0m)
					return _rhs;
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateDecimal(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			decimal result = EvaluateDecimal(rpt, row);

			return Convert.ToDouble(result);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            decimal result = EvaluateDecimal(rpt, row);

            return Convert.ToInt32(result);
        }

		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			decimal lhs = _lhs.EvaluateDecimal(rpt, row);
			decimal rhs = _rhs.EvaluateDecimal(rpt, row);

			return (decimal) (lhs+rhs);
		}

		public string EvaluateString(Report rpt, Row row)
		{
			decimal result = EvaluateDecimal(rpt, row);
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			decimal result = EvaluateDecimal(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			decimal result = EvaluateDecimal(rpt, row);
			return Convert.ToBoolean(result);
		}
	}
}
