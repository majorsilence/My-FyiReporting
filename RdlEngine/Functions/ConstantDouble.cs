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
using System.Globalization;


using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Constant Double
	/// </summary>
	[Serializable]
	internal class ConstantDouble : IExpr
	{
		double _Value;		// value of the constant

		/// <summary>
		/// passed class name, function name, and args for evaluation
		/// </summary>
		public ConstantDouble(string v) 
		{
			_Value = Convert.ToDouble(v, NumberFormatInfo.InvariantInfo);
		}

		public ConstantDouble(double v) 
		{
			_Value = v;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		public bool IsConstant()
		{
			return true;
		}

		public IExpr ConstantOptimization()
		{	// already constant expression
			return this;
		}

		public object Evaluate(Report rpt, Row row)
		{
			return _Value;
		}

		public string EvaluateString(Report rpt, Row row)
		{
			return Convert.ToString(_Value);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			return _Value;
		}

        public decimal EvaluateDecimal(Report rpt, Row row)
        {
            return Convert.ToDecimal(_Value);
        }

        public int EvaluateInt32(Report rpt, Row row)
        {
            return Convert.ToInt32(_Value);
        }

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			return Convert.ToDateTime(_Value);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			return Convert.ToBoolean(_Value);
		}
	}
}
