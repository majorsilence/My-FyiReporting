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
	/// Aggregate function: RunningValue count
	/// </summary>
	[Serializable]
	internal class FunctionAggrRvCount : FunctionAggr, IExpr, ICacheData
	{
		string _key;			// key to cached between invocations
		/// <summary>
		/// Aggregate function: RunningValue Sum returns the sum of all values of the
		///		expression within the scope up to that row
		///	Return type is decimal for decimal expressions and double for all
		///	other expressions.	
		/// </summary>
        public FunctionAggrRvCount(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrcount" + Interlocked.Increment(ref Parser.Counter).ToString();
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return (object) EvaluateInt32(rpt, row);
		}
		
		public int EvaluateInt32(Report rpt, Row row)
		{
			bool bSave=true;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return int.MinValue;

			Row startrow=null;
			foreach (Row r in re)
			{
				startrow = r;			// We just want the first row
				break;
			}

			int count;

			object currentValue = _Expr.Evaluate(rpt, row);
			int incr = currentValue == null? 0: 1;
			if (row == startrow)
			{
				// must be the start of a new group
				count = incr;
			}
			else
			{
				count = GetValue(rpt) + incr;
			}

			SetValue(rpt, count);
			return count;
		}

        public double EvaluateDouble(Report rpt, Row row)
        {
            return (double)EvaluateInt32(rpt, row);
        }
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return (decimal) EvaluateInt32(rpt, row);
		}

		public string EvaluateString(Report rpt, Row row)
		{
			object result = EvaluateDouble(rpt, row);
			return Convert.ToString(result);
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}
		private int GetValue(Report rpt)
		{
			OInt oi = rpt.Cache.Get(_key) as OInt;
			return oi == null? 0: oi.i;
		}

		private void SetValue(Report rpt, int i)
		{
			rpt.Cache.AddReplace(_key, new OInt(i));
		}

		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(_key);
		}

		#endregion
	}
}
