
using System;
using System.IO;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// The IExpr interface should be implemented when you want to create a built-in function.
	/// </summary>
	internal interface IExpr
	{
		TypeCode GetTypeCode();			// return the type of the expression
		Task<bool> IsConstant();				// expression returns a constant
		Task<IExpr> ConstantOptimization();	// constant optimization

		// Evaluate is for interpretation
		Task<object> Evaluate(Report r, Row row);				// return an object
		Task<string> EvaluateString(Report r, Row row);		// return a string
		Task<double> EvaluateDouble(Report r, Row row);		// return a double
		Task<decimal> EvaluateDecimal(Report r, Row row);		// return a decimal
		Task<int> EvaluateInt32(Report r, Row row);           // return an Int32
		Task<DateTime> EvaluateDateTime(Report r, Row row);	// return a DateTime
		Task<bool> EvaluateBoolean(Report r, Row row);		// return boolean
	}
}
