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
	/// Format function: Format(expr, string expr format)
	/// </summary>
	[Serializable]
	internal class FunctionFormat : IExpr
	{
		IExpr _Formatee;	// object to format
		IExpr _Format;		// format string
		/// <summary>
		/// Format an object
		/// </summary>
		public FunctionFormat(IExpr formatee, IExpr format) 
		{
			_Formatee = formatee;
			_Format= format;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		// 
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateString(rpt, row);
		}

		public bool IsConstant()
		{
			if (_Formatee.IsConstant())
				return _Format.IsConstant();

			return false;
		}
	
		public IExpr ConstantOptimization()
		{
			_Formatee = _Formatee.ConstantOptimization();
			_Format = _Format.ConstantOptimization();
			if (_Formatee.IsConstant() && _Format.IsConstant())
			{
				string s = EvaluateString(null, null);
				return new ConstantString(s);
			}

			return this;
		}
		
		public bool EvaluateBoolean(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);

			return Convert.ToBoolean(result);
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
			object o = _Formatee.Evaluate(rpt, row);
			if (o == null)
				return null;
			string format = _Format.EvaluateString(rpt, row);
			if (format == null)
				return o.ToString();	// just return string version of object

			string result=null;
			try
			{
				result = String.Format("{0:" + format + "}", o);
			}
			catch (Exception ex) 		// invalid format string specified
			{           //    treat as a weak error
				rpt.rl.LogError(2, String.Format("Format string:{1} Value Type:{2} Exception:{0}", ex.Message, format, o.GetType().Name));
				result = o.ToString();
			}
			return result;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToDateTime(result);
		}
	}
}
