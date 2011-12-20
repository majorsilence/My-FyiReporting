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
	/// iif function of the form iif(boolean, expr1, expr2)
	/// </summary>
	[Serializable]
	internal class FunctionIif : IExpr
	{
		IExpr _If;		// boolean expression
		IExpr _IfTrue;		// result if true
		IExpr _IfFalse;		// result if false

		/// <summary>
		/// Handle iif operator
		/// </summary>
		public FunctionIif(IExpr ife, IExpr ifTrue, IExpr ifFalse) 
		{
			_If = ife;
			_IfTrue = ifTrue;
			_IfFalse = ifFalse;
		}

		public TypeCode GetTypeCode()
		{
			return _IfTrue.GetTypeCode();
		}

		public bool IsConstant()
		{
			return _If.IsConstant() && _IfTrue.IsConstant() && _IfFalse.IsConstant();
		}

		public IExpr ConstantOptimization()
		{
			_If = _If.ConstantOptimization();
			_IfTrue = _IfTrue.ConstantOptimization();
			_IfFalse = _IfFalse.ConstantOptimization();

			if (_If.IsConstant())
			{
				bool result = _If.EvaluateBoolean(null, null);
				return result? _IfTrue: _IfFalse;
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			bool result = _If.EvaluateBoolean(rpt, row);
			if (result)
				return _IfTrue.Evaluate(rpt, row);

			object o = _IfFalse.Evaluate(rpt, row);
			// We may need to convert IfFalse to same type as IfTrue
			if (_IfTrue.GetTypeCode() == _IfFalse.GetTypeCode())
				return o;

			return Convert.ChangeType(o, _IfTrue.GetTypeCode());
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
