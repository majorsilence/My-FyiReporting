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
using Majorsilence.Reporting.RdlEngine.Resources;
using Majorsilence.Reporting.Rdl;
using System.Threading.Tasks;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Fields referenced dynamically (e.g. Fields(expr) are handled by this class.
	/// </summary>
	[Serializable]
	internal class FunctionFieldCollection : IExpr
	{
		private IDictionary _Fields;
		private IExpr _ArgExpr;

		/// <summary>
		/// obtain value of Field
		/// </summary>
		public FunctionFieldCollection(IDictionary fields, IExpr arg) 
		{
			_Fields = fields;
			_ArgExpr = arg;
		}

		public virtual TypeCode GetTypeCode()
		{
			return TypeCode.Object;		// we don't know the typecode until we run the function
		}

		public virtual Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public virtual async Task<IExpr> ConstantOptimization()
		{	
			_ArgExpr = await _ArgExpr.ConstantOptimization();

			if (await _ArgExpr.IsConstant())
			{
				string o = await _ArgExpr.EvaluateString(null, null);
				if (o == null)
					throw new Exception(Strings.FunctionFieldCollection_Error_FieldCollectionNull); 
				Field f = _Fields[o] as Field;
				if (f == null)
					throw new Exception(string.Format(Strings.FunctionFieldCollection_Error_FieldCollectionInvalid, o)); 
				return new FunctionField(f);
			}

			return this;
		}

		// 
		public virtual async Task<object> Evaluate(Report rpt, Row row)
		{
			if (row == null)
				return null;
			Field f;
			string field = await _ArgExpr.EvaluateString(rpt, row);
			if (field == null)
				return null;
			f = _Fields[field] as Field;
			if (f == null)
				return null;

			object o;
			if (f.Value != null)
				o = await f.Value.Evaluate(rpt, row);
			else
				o = row.Data[f.ColumnNumber];

			if (o == DBNull.Value)
				return null;

			if (f.RunType == TypeCode.String && o is char)	// work around; mono odbc driver confuses string and char
				o = Convert.ChangeType(o, TypeCode.String);
			
			return o;
		}
		
		public virtual async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			if (row == null)
				return Double.NaN;
			return Convert.ToDouble(await Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
		}
		
		public virtual async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			if (row == null)
				return decimal.MinValue;
			return Convert.ToDecimal(await Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
		}

        public virtual async Task<int> EvaluateInt32(Report rpt, Row row)
        {
            if (row == null)
                return int.MinValue;
            return Convert.ToInt32(await Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
        }

		public virtual async Task<string> EvaluateString(Report rpt, Row row)
		{
			if (row == null)
				return null;
			return Convert.ToString(await Evaluate(rpt, row));
		}

		public virtual async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			if (row == null)
				return DateTime.MinValue;
			return Convert.ToDateTime(await Evaluate(rpt, row));
		}

		public virtual async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			if (row == null)
				return false;
			return Convert.ToBoolean(await Evaluate(rpt, row));
		}
	}
}
