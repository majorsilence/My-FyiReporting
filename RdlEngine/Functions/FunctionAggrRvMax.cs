
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Aggregate function: RunningValue max
	/// </summary>
	[Serializable]
	internal class FunctionAggrRvMax : FunctionAggr, IExpr, ICacheData
	{
		private TypeCode _tc;		// type of result: decimal or double
		string _key;				// key for cache
		/// <summary>
		/// Aggregate function: Running Max returns the highest value to date within a group
		///	Return type is same as input expression	
		/// </summary>
        public FunctionAggrRvMax(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrrvmax" + Interlocked.Increment(ref Parser.Counter).ToString();

			// Determine the result
			_tc = e.GetTypeCode();
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return _tc;
		}

		public async Task<object> Evaluate(Report rpt, Row row)
		{
			bool bSave=true;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return null;

			Row startrow=null;
			foreach (Row r in re)
			{
				startrow = r;			// We just want the first row
				break;
			}

			object current_value = await _Expr.Evaluate(rpt, row);
			if (row == startrow)
			{}
			else
			{
				object v = GetValue(rpt);
				if (current_value == null)
					return v;
				else if (v == null)
				{}	// use the current_value
				else if (Filter.ApplyCompare(_tc, v, current_value) < 0)
				{}	// use the current_value
				else
					return v;
			}
			SetValue(rpt, current_value);
			return current_value;
		}
		
		public async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDouble(result);
		}
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDecimal(result);
		}

        public async Task<int> EvaluateInt32(Report rpt, Row row)
        {
            object result = await Evaluate(rpt, row);
            return Convert.ToInt32(result);
        }

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToString(result);
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}

		private object GetValue(Report rpt)
		{
			return rpt.Cache.Get(_key);
		}

		private void SetValue(Report rpt, object o)
		{
			if (o == null)
				rpt.Cache.Remove(_key);
			else
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
