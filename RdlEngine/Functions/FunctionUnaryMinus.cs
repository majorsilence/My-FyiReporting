
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Unary minus operator
	/// </summary>
	[Serializable]
	internal class FunctionUnaryMinus : IExpr
	{
		IExpr _rhs;			// rhs 

		/// <summary>
		/// Do division on double data types
		/// </summary>
		public FunctionUnaryMinus() 
		{
			_rhs = null;
		}

		public FunctionUnaryMinus(IExpr r) 
		{
			_rhs = r;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		public async Task<bool> IsConstant()
		{
			return await _rhs.IsConstant();
		}

		public async Task<IExpr> ConstantOptimization()
		{
            await _rhs.ConstantOptimization();
			if (await _rhs.IsConstant())
				return new ConstantDouble(await EvaluateDouble(null, null));
			else
				return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateDouble(rpt, row);
		}
		
		public async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			double rhs = await _rhs.EvaluateDouble(rpt, row);

			return -rhs;
		}
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);

			return Convert.ToDecimal(result);
		}

        public async Task<int> EvaluateInt32(Report rpt, Row row)
        {
            double result = await EvaluateDouble(rpt, row);

            return Convert.ToInt32(result);
        }

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);
			return result.ToString();
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);
			return Convert.ToDateTime(result);
		}

		public Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Task.FromResult(false);
		}

		public IExpr Rhs
		{
			get { return  _rhs; }
			set {  _rhs = value; }
		}

	}
}
