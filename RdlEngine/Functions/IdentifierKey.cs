
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// IdentifierKey
	/// </summary>
	public enum IdentifierKeyEnum
	{
		/// <summary>
		/// Recursive
		/// </summary>
		Recursive,
		/// <summary>
		/// Simple
		/// </summary>
		Simple	
	}

	[Serializable]
	internal class IdentifierKey : IExpr
	{
		IdentifierKeyEnum _Value;		// value of the identifier

		/// <summary>
		/// 
		/// </summary>
		public IdentifierKey(IdentifierKeyEnum v) 
		{
			_Value = v;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;			
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public IdentifierKeyEnum Value
		{
			get {return _Value;}
		}

		public Task<IExpr> ConstantOptimization()
		{	
			return Task.FromResult(this as IExpr);
		}

		public Task<object> Evaluate(Report rpt, Row row)
		{	
			return Task.FromResult(_Value as object);
		}

		public Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return Task.FromResult(Double.NaN);
		}
		
		public Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return Task.FromResult(Decimal.MinValue);
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return Task.FromResult(int.MinValue);
        }

		public Task<string> EvaluateString(Report rpt, Row row)
		{
			return null;
		}

		public Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			return Task.FromResult(DateTime.MinValue);
		}

		public Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Task.FromResult(false);
		}
	}
}
