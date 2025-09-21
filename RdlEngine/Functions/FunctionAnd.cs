
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// And operator of form lhs &amp;&amp; rhs
	/// </summary>
	[Serializable]
	internal class FunctionAnd : FunctionBinary, IExpr
	{

		/// <summary>
		/// And two boolean expressions together of the form a &amp;&amp; b
		/// </summary>
		public FunctionAnd(IExpr lhs, IExpr rhs) 
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
			bool bLeftConst = await _lhs.IsConstant();
			bool bRightConst = await _rhs.IsConstant();
			if (bLeftConst && bRightConst)
			{
				bool b = await EvaluateBoolean(null, null);
				return new ConstantBoolean(b);
			}
			else if (bRightConst)
			{
				bool b = await _rhs.EvaluateBoolean(null, null);
				if (b)
					return _lhs;
				else 
					return new ConstantBoolean(false);
			}
			else if (bLeftConst)
			{
				bool b = await _lhs.EvaluateBoolean(null, null);
				if (b)
					return _rhs;
				else
					return new ConstantBoolean(false);
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateBoolean(rpt, row);
		}
		
		public Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return Task.FromResult(Double.NaN);
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

		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			bool r = await _lhs.EvaluateBoolean(rpt, row);
			if (!r)
				return false;
			return await _rhs.EvaluateBoolean(rpt, row);
		}
	}
}
