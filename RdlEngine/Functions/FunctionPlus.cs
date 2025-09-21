
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Plus operator  of form lhs + rhs
	/// </summary>
	[Serializable]
	internal class FunctionPlus : FunctionBinary , IExpr
	{
		/// <summary>
		/// Do plus on double data types
		/// </summary>
		public FunctionPlus() 
		{
		}

		public FunctionPlus(IExpr lhs, IExpr rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		public async Task<IExpr> ConstantOptimization()
		{
			_lhs = await _lhs.ConstantOptimization();
			_rhs = await _rhs.ConstantOptimization();
			bool bLeftConst = await _lhs.IsConstant();
			bool bRightConst = await _rhs.IsConstant();
			if (bLeftConst && bRightConst)
			{
				double d = await EvaluateDouble(null, null);
				return new ConstantDouble(d);
			}
			else if (bRightConst)
			{
				double d = await _rhs.EvaluateDouble(null, null);
				if (d == 0)
					return _lhs;
			}
			else if (bLeftConst)
			{
				double d = await _lhs.EvaluateDouble(null, null);
				if (d == 0)
					return _rhs;
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateDouble(rpt, row);
		}
		
		public async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			double lhs = await _lhs.EvaluateDouble(rpt, row);
			double rhs = await _rhs.EvaluateDouble(rpt, row);

			return lhs+rhs;
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

		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			double result = await EvaluateDouble(rpt, row);
			return Convert.ToBoolean(result);
		}
	}
}
