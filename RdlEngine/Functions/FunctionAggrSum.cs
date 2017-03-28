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
using System.Linq;
using System.Reflection;
using System.Threading;

namespace fyiReporting.RDL
{
	/// <summary>
	/// Aggregate function: sum
	/// </summary>
	[Serializable]
	internal class FunctionAggrSum : FunctionAggr, IExpr, ICacheData
	{
		private TypeCode _tc;		// type of result: decimal or double
		string _key;				// key for cache when scope is dataset we can cache the result
		/// <summary>
		/// Aggregate function: Sum returns the sum of all values of the
		///		expression within the scope
		///	Return type is decimal for decimal expressions and double for all
		///	other expressions.
		/// </summary>
        public FunctionAggrSum(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrsum" + Interlocked.Increment(ref Parser.Counter).ToString();

			// Determine the result
			_tc = e.GetTypeCode();
			if (_tc != TypeCode.Decimal && _tc != TypeCode.Object)
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
			switch (_tc)
			{
				case TypeCode.Decimal:
					return (object) EvaluateDecimal(rpt, row);
				case TypeCode.Object:
					return EvaluateObject(rpt, row);
				case TypeCode.Double:
				default:
					return (object) EvaluateDouble(rpt, row);
			}
		}

		public object EvaluateObject(Report rpt, Row row)
		{
			bool bSave;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return null;

			var od = GetValueObject(rpt);
			if (od != null)
				return od;

			object sum = null;
			object temp;

			foreach (Row r in re)
			{
				temp = _Expr.Evaluate(rpt, r);
				if (temp != null)
				{
					if (sum != null)
						sum = temp.GetType().GetMethod("op_Addition").Invoke(null, new []{ sum, temp});
					else
						sum = temp;
				}
			}
			if (bSave)
				SetValue(rpt, sum);

			return sum;

		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			bool bSave=true;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return double.NaN;

			ODouble v = GetValueDouble(rpt);
			if (v != null)
				return v.d;

			double sum=0;
			double temp;
			foreach (Row r in re)
			{
				temp = _Expr.EvaluateDouble(rpt, r);
				if (temp.CompareTo(double.NaN) != 0)
					sum += temp;
			}
			if (bSave)
				SetValue(rpt, sum);

			return sum;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            double result = EvaluateDouble(rpt, row);
            return Convert.ToInt32(result);
        }

		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			bool bSave;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return decimal.MinValue;

			ODecimal od = GetValueDecimal(rpt);
			if (od != null)
				return od.d;

			decimal sum=0;
			decimal temp;
			foreach (Row r in re)
			{
				temp = _Expr.EvaluateDecimal(rpt, r);
				if (temp != decimal.MinValue)		// indicate null value
					sum += temp;
			}
			if (bSave)
				SetValue(rpt,sum);

			return sum;
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

		private ODecimal GetValueDecimal(Report rpt)
		{
			return rpt.Cache.Get(_key) as ODecimal;
		}

		private ODouble GetValueDouble(Report rpt)
		{
			return rpt.Cache.Get(_key) as ODouble;
		}

		private object GetValueObject(Report rpt)
		{
			return rpt.Cache.Get(_key);
		}

		private void SetValue(Report rpt, double d)
		{
			rpt.Cache.AddReplace(_key, new ODouble(d));
		}

		private void SetValue(Report rpt, decimal d)
		{
			rpt.Cache.AddReplace(_key, new ODecimal(d));
		}

		private void SetValue(Report rpt, object d)
		{
			rpt.Cache.AddReplace(_key, d);
		}

		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(_key);
		}

		#endregion
	}
}
