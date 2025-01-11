/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.IO;
using System.Threading.Tasks;
using fyiReporting.RDL;


namespace fyiReporting.RDL
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
