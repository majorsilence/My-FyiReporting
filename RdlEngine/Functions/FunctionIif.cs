
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// iif function of the form iif(boolean, expr1, expr2)
	/// </summary>
	[Serializable]
	internal class FunctionIif : IExpr
	{
		IExpr _If;		// boolean expression
		IExpr _IfTrue;		// result if true
		IExpr _IfFalse;		// result if false

		/// <summary>
		/// Handle iif operator
		/// </summary>
		public FunctionIif(IExpr ife, IExpr ifTrue, IExpr ifFalse) 
		{
			_If = ife;
			_IfTrue = ifTrue;
			_IfFalse = ifFalse;
		}

		public TypeCode GetTypeCode()
		{
			return _IfTrue.GetTypeCode();
		}

		public async Task<bool> IsConstant()
		{
			return await _If.IsConstant() && await _IfTrue.IsConstant() && await _IfFalse.IsConstant();
		}

		public async Task<IExpr> ConstantOptimization()
		{
			_If = await _If.ConstantOptimization();
			_IfTrue = await _IfTrue.ConstantOptimization();
			_IfFalse = await _IfFalse.ConstantOptimization();

			if (await _If.IsConstant())
			{
				bool result = await _If.EvaluateBoolean(null, null);
				return result? _IfTrue: _IfFalse;
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			bool result = await _If.EvaluateBoolean(rpt, row);
			if (result)
				return await _IfTrue.Evaluate(rpt, row);

			object o = await _IfFalse.Evaluate(rpt, row);
			// We may need to convert IfFalse to same type as IfTrue
			if (_IfTrue.GetTypeCode() == _IfFalse.GetTypeCode())
				return o;

			return Convert.ChangeType(o, _IfTrue.GetTypeCode());
		}

		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			object result = await Evaluate(rpt, row);
			return Convert.ToBoolean(result);
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
	}
}
