
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Constant Integer
	/// </summary>
	[Serializable]
	internal class ConstantInteger : IExpr
	{
		int _Value;		// value of the constant

		/// <summary>
		/// passed class name, function name, and args for evaluation
		/// </summary>
		public ConstantInteger(string v) 
		{
			_Value = Convert.ToInt32(v);
		}

		public ConstantInteger(int v) 
		{
			_Value = v;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
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
			return Task.FromResult((double)_Value);
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return Task.FromResult(_Value);
        }
		
		public Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return Task.FromResult(Convert.ToDecimal(_Value));
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
