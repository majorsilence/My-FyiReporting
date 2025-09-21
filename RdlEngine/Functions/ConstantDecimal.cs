
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Globalization;

using Majorsilence.Reporting.Rdl;
using System.Threading.Tasks;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Constant decimal.
	/// </summary>
	[Serializable]
	internal class ConstantDecimal : IExpr
	{
		decimal _Value;		// value of the constant

		/// <summary>
		/// passed string of the number
		/// </summary>
		public ConstantDecimal(string v) 
		{
			_Value = Convert.ToDecimal(v, NumberFormatInfo.InvariantInfo);
		}

		public ConstantDecimal(decimal v) 
		{
			_Value = v;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(true);
		}

		public Task<IExpr> ConstantOptimization()
		{	// already constant expression
			return Task.FromResult(this as IExpr);
		}

		public Task<object> Evaluate(Report rpt, Row row)
		{
			return Task.FromResult((object)_Value);
		}

		public Task<string> EvaluateString(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToString(_Value));
		}
		
		public Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToDouble(_Value));
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return Task.FromResult(Convert.ToInt32(_Value));
        }
		
		public Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return Task.FromResult(_Value);
		}

		public Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToDateTime(_Value));
		}

		public Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToBoolean(_Value));
		}
	}
}
