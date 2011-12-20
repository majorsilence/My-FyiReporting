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
	/// Aggregate function: RunningValue var
	/// </summary>
	[Serializable]
	internal class FunctionAggrRvVar : FunctionAggr, IExpr, ICacheData
	{
		string _key;				// key for cache of data between invocations
		/// <summary>
		/// Aggregate function: RunningValue var returns the variance of all values of the
		///		expression within the scope up to that row
		///	Return type is double for all expressions.	
		/// </summary>
        public FunctionAggrRvVar(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrrvvar" + Interlocked.Increment(ref Parser.Counter).ToString();
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		public object Evaluate(Report rpt, Row row)
		{
			return (object) EvaluateDouble(rpt, row);
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

			WorkClass wc = GetValue(rpt);
			double currentValue = _Expr.EvaluateDouble(rpt, row);
			if (row == startrow)
			{
				// restart the group
				wc.Sum = wc.Sum2 = 0;
				wc.Count = 0;
			}
			
			if (currentValue.CompareTo(double.NaN) != 0)
			{
				wc.Sum += currentValue;
				wc.Sum2 += (currentValue*currentValue);
				wc.Count++;
			}

			double result;
			if (wc.Count > 1)
				result = (wc.Count * wc.Sum2 - wc.Sum*wc.Sum) / (wc.Count * (wc.Count-1));
			else		
				result = double.NaN;

			return result;
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			double d = EvaluateDouble(rpt, row);
			if (d.CompareTo(double.NaN) == 0)
				return decimal.MinValue;

			return Convert.ToDecimal(d);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            double d = EvaluateDouble(rpt, row);
            if (d.CompareTo(double.NaN) == 0)
                return int.MinValue;

            return Convert.ToInt32(d);
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
			internal double Sum;		
			internal double Sum2;		
			internal int Count;			
			internal WorkClass()
			{
				Sum = Sum2 = 0;
				Count = -1;
			}
		}
	}
}
