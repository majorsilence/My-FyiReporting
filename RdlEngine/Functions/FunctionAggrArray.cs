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
	/// Aggregate function: Count
	/// </summary>
	[Serializable]
	internal class FunctionAggrArray : FunctionAggr, IExpr, ICacheData
	{
		string _key;
		/// <summary>
		/// Aggregate function: Count
		/// 
		///	Return type is double
		/// </summary>
        public FunctionAggrArray(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrarray" + Interlocked.Increment(ref Parser.Counter).ToString();
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
            bool bSave = true;
            RowEnumerable re = this.GetDataScope(rpt, row, out bSave);
            if (re == null)
                return null;

            object v = GetValue(rpt);
            if (v == null)
            {
                object temp;
                ArrayList ar = new ArrayList(Math.Max(1,re.LastRow - re.FirstRow + 1));
                foreach (Row r in re)
                {
                    temp = _Expr.Evaluate(rpt, r);
                    ar.Add(temp);
                }

                v = ar;
                if (bSave)
                    SetValue(rpt, v);
            }
            return v;
        }
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			return double.MinValue;
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return decimal.MinValue;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return int.MinValue;
        }

		public string EvaluateString(Report rpt, Row row)
		{
			return null;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
            return DateTime.MinValue;
		}

		private object GetValue(Report rpt)
		{
			object oi = rpt.Cache.Get(_key);
			return oi;
		}

		private void SetValue(Report rpt, object o)
		{
			rpt.Cache.AddReplace(_key, o);
		}

		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(_key);
		}

		#endregion
	}
}
