
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Minus operator  of form lhs - rhs where both operands are decimal.
	/// </summary>
	[Serializable]
	internal class FunctionMinusDecimal : FunctionBinary, IExpr
	{

		/// <summary>
		/// Do minus on decimal data types
		/// </summary>
		public FunctionMinusDecimal() 
		{
			_lhs = null;
			_rhs = null;
		}

		public FunctionMinusDecimal(IExpr lhs, IExpr rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		public async Task<IExpr> ConstantOptimization()
		{
			_lhs = await _lhs.ConstantOptimization();
			_rhs = await _rhs.ConstantOptimization();
			bool bLeftConst = await _lhs.IsConstant();
			bool bRightConst = await _rhs.IsConstant();
			if (bLeftConst && bRightConst)
			{
				decimal d = await EvaluateDecimal(null, null);
				return new ConstantDecimal(d);
			}
			else if (bRightConst)
			{
				decimal d = await _rhs.EvaluateDecimal(null, null);
				if (d == 0m)
					return _lhs;
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateDecimal(rpt, row);
		}
		
		public async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			decimal result = await EvaluateDecimal(rpt, row);

			return Convert.ToDouble(result);
		}

        public async Task<int> EvaluateInt32(Report rpt, Row row)
        {
            decimal result = await EvaluateDecimal(rpt, row);

            return Convert.ToInt32(result);
        }
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			decimal lhs = await _lhs.EvaluateDecimal(rpt, row);
			decimal rhs = await _rhs.EvaluateDecimal(rpt, row);

			return (decimal) (lhs-rhs);
		}

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			decimal result = await EvaluateDecimal(rpt, row);
			return result.ToString();
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			decimal result = await EvaluateDecimal(rpt, row);
			return Convert.ToDateTime(result);
		}

		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			decimal result = await EvaluateDecimal(rpt, row);
			return Convert.ToBoolean(result);
		}
	}
}
