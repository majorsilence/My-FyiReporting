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
using System.Globalization;
using Majorsilence.Reporting.RdlEngine.Resources;
using Majorsilence.Reporting.Rdl;
using System.Threading.Tasks;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// The Language field in the User collection.
	/// </summary>
	[Serializable]
	internal class FunctionUserLanguage : IExpr
	{
		/// <summary>
		/// Client user language
		/// </summary>
		public FunctionUserLanguage() 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public Task<IExpr> ConstantOptimization()
		{	
			return Task.FromResult(this as IExpr);
		}

		// Evaluate is for interpretation  
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			return await EvaluateString(rpt, row);
		}
		
		public Task<double> EvaluateDouble(Report rpt, Row row)
		{	
			throw new Exception(Strings.FunctionUserLanguage_Error_ConvertToDouble);
		}
		
		public Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			throw new Exception(Strings.FunctionUserLanguage_Error_ConvertToDecimal);
		}

        public Task<int> EvaluateInt32(Report rpt, Row row)
        {
            throw new Exception(Strings.FunctionUserLanguage_Error_ConvertToInt32);
        }
		public Task<string> EvaluateString(Report rpt, Row row)
		{
			if (rpt == null || rpt.ClientLanguage == null)
				return Task.FromResult(CultureInfo.CurrentCulture.ThreeLetterISOLanguageName);
			else
				return Task.FromResult(rpt.ClientLanguage);
		}

		public Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			throw new Exception(Strings.FunctionUserLanguage_Error_ConvertToDateTime);
		}

		public Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			throw new Exception(Strings.FunctionUserLanguage_Error_ConvertToBoolean);
		}
	}
}
