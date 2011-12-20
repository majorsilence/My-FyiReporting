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
	/// Unary minus operator with a decimal operand
	/// </summary>
	[Serializable]
	internal class FunctionUnaryMinusDecimal : IExpr
	{
		IExpr _rhs;			// rhs

		/// <summary>
		/// Do minus on decimal data type
		/// </summary>
		public FunctionUnaryMinusDecimal() 
		{
			_rhs = null;
		}

		public FunctionUnaryMinusDecimal(IExpr r) 
		{
			_rhs = r;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		public bool IsConstant()
		{
			return _rhs.IsConstant();
		}

		public IExpr ConstantOptimization()
		{
			_rhs = _rhs.ConstantOptimization();
			if (_rhs.IsConstant())
			{
				decimal d = EvaluateDecimal(null, null);
				return new ConstantDecimal(d);
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
			decimal rhs = _rhs.EvaluateDecimal(rpt, row);

			return (decimal) (-rhs);
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
			return false;
		}

		public IExpr Rhs
		{
			get { return  _rhs; }
			set {  _rhs = value; }
		}

	}
}
