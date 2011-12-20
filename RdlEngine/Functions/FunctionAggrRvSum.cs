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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Aggregate function: RunningValue sum
	/// </summary>
	[Serializable]
	internal class FunctionAggrRvSum : FunctionAggr, IExpr, ICacheData
	{
		private TypeCode _tc;		// type of result: decimal or double
		private string _key;		// key for cache of data between invocations
		/// <summary>
		/// Aggregate function: RunningValue Sum returns the sum of all values of the
		///		expression within the scope up to that row
		///	Return type is decimal for decimal expressions and double for all
		///	other expressions.	
		/// </summary>
        public FunctionAggrRvSum(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrrvsum" + Interlocked.Increment(ref Parser.Counter).ToString();

			// Determine the result
			_tc = e.GetTypeCode();
			if (_tc != TypeCode.Decimal)	// if not decimal
				_tc = TypeCode.Double;		// force result to double
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return _tc;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return _tc==TypeCode.Decimal? (object) EvaluateDecimal(rpt, row): (object) EvaluateDouble(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			bool bSave=true;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return double.NaN;

			Row startrow=null;
			foreach (Row r in re)
			{
				startrow = r;			// We just want the first row
				break;
			}

			double currentValue = _Expr.EvaluateDouble(rpt, row);
			WorkClass wc = GetValue(rpt);
			if (row == startrow)
			{
				// must be the start of a new group
				wc.Value = currentValue;
			}
			else
				wc.Value = ((double) wc.Value + currentValue);

			return (double) wc.Value;
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			bool bSave;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return decimal.MinValue;

			Row startrow=null;
			foreach (Row r in re)
			{
				startrow = r;			// We just want the first row
				break;
			}

			WorkClass wc = GetValue(rpt);
			decimal currentValue = _Expr.EvaluateDecimal(rpt, row);
			if (row == startrow)
			{
				// must be the start of a new group
				wc.Value = currentValue;
			}
			else
				wc.Value = ((decimal) wc.Value + currentValue);

			return (decimal) wc.Value;
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
		private WorkClass GetValue(Report rpt)
		{
			WorkClass wc = rpt.Cache.Get(_key) as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(_key, wc);
			}
			return wc;
		}

		private void SetValue(Report rpt, WorkClass w)
		{
			rpt.Cache.AddReplace(_key, w);
		}
		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(_key);
		}

		#endregion
		class WorkClass
		{
			internal object Value;		
			internal WorkClass()
			{
				Value = null;
			}
		}
	}
}
