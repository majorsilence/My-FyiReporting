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
	/// Mult operator  of form lhs * rhs
	/// </summary>
	[Serializable]
	internal class FunctionMult : FunctionBinary, IExpr
	{

		/// <summary>
		/// Do multiplication on double data types
		/// </summary>
		public FunctionMult() 
		{
			_lhs = null;
			_rhs = null;
		}

		public FunctionMult(IExpr lhs, IExpr rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		public IExpr ConstantOptimization()
		{
			_lhs = _lhs.ConstantOptimization();
			_rhs = _rhs.ConstantOptimization();
			bool bLeftConst = _lhs.IsConstant();
			bool bRightConst = _rhs.IsConstant();
			if (bLeftConst && bRightConst)
			{
				double d = EvaluateDouble(null, null);
				return new ConstantDouble(d);
			}
			else if (bLeftConst)
			{
				double d = _lhs.EvaluateDouble(null, null);
				if (d == 1)
					return _rhs;
				else if (d == 0)
					return new ConstantDouble(0);
			}
			else if (bRightConst)
			{
				double d = _rhs.EvaluateDouble(null, null);
				if (d == 1)
					return _lhs;
				else if (d == 0)
					return new ConstantDouble(0);
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateDouble(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			double lhs = _lhs.EvaluateDouble(rpt, row);
			double rhs = _rhs.EvaluateDouble(rpt, row);

			return lhs*rhs;
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			double result = EvaluateDouble(rpt, row);

			return Convert.ToDecimal(result);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            double result = EvaluateDouble(rpt, row);

            return Convert.ToInt32(result);
        }

		public string EvaluateString(Report rpt, Row row)
		{
			double result = EvaluateDouble(rpt, row);
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			double result = EvaluateDouble(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			double result = EvaluateDouble(rpt, row);
			return Convert.ToBoolean(result);
		}
	}
}
