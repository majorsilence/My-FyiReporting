
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Relational operator GTE of form lhs >= rhs
	/// </summary>
	[Serializable]
	internal class FunctionRelopGTE : FunctionBinary, IExpr
	{
		/// <summary>
		/// Do relational equal operation
		/// </summary>
		public FunctionRelopGTE(IExpr lhs, IExpr rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Boolean;
		}

		public async Task<IExpr> ConstantOptimization()
		{
			_lhs = await _lhs.ConstantOptimization();
			_rhs = await _rhs.ConstantOptimization();
			if (await _lhs.IsConstant() && await _rhs.IsConstant())
			{
				bool b = await EvaluateBoolean(null, null);
				return new ConstantBoolean(b);
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateBoolean(rpt, row);
		}

		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			object left = await _lhs.Evaluate(rpt, row);
			object right = await _rhs.Evaluate(rpt, row);
			if (Filter.ApplyCompare(_lhs.GetTypeCode(), left, right) >= 0)
				return true;
			else
				return false;
		}
		
		public Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return Task.FromResult(double.NaN);
		}
		
		public Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return Task.FromResult(decimal.MinValue);
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return Task.FromResult(int.MinValue);
        }

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			bool result = await EvaluateBoolean(rpt, row);
			return result.ToString();
		}

		public Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			return Task.FromResult(DateTime.MinValue);
		}
	}
}
