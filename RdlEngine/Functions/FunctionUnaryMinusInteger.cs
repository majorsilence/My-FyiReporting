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
	/// Unary minus operator for a integer operand
	/// </summary>
	[Serializable]
	internal class FunctionUnaryMinusInteger : IExpr
	{
		IExpr _rhs;			// rhs

		/// <summary>
		/// Do minus on decimal data type
		/// </summary>
		public FunctionUnaryMinusInteger() 
		{
			_rhs = null;
		}

		public FunctionUnaryMinusInteger(IExpr r) 
		{
			_rhs = r;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
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
				double d = EvaluateDouble(null, null);
				return new ConstantInteger((int) d);
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return (int) EvaluateInt32(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			double result = _rhs.EvaluateDouble(rpt, row);

			return -result;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            int result = _rhs.EvaluateInt32(rpt, row);

            return -result;
        }
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			int result = EvaluateInt32(rpt, row);

			return Convert.ToDecimal(result);
		}

		public string EvaluateString(Report rpt, Row row)
		{
			int result = (int) EvaluateDouble(rpt, row);
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			int result = (int) EvaluateDouble(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			int result = (int) EvaluateDouble(rpt, row);
			return result == 0? false:true;
		}

		public IExpr Rhs
		{
			get { return  _rhs; }
			set {  _rhs = value; }
		}

	}
}
