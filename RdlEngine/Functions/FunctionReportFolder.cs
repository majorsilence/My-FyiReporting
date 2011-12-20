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

using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Report Folder
	/// </summary>
	[Serializable]
	internal class FunctionReportFolder : IExpr
	{
		/// <summary>
		/// Folder that holds Report
		/// </summary>
		public FunctionReportFolder() 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		public bool IsConstant()
		{
			return false;
		}

		public IExpr ConstantOptimization()
		{	// not a constant expression
			return this;
		}

		public object Evaluate(Report rpt, Row row)
		{
			return rpt.Folder;
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{	
			return double.NaN;
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return Decimal.MinValue;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return int.MinValue;
        }

		public string EvaluateString(Report rpt, Row row)
		{
			return rpt == null? "": rpt.Folder;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			return false;
		}
	}
}
