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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using RdlEngine.Resources;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Process a custom static method invokation.
	/// </summary>
	[Serializable]
	internal class FunctionCustomStatic : IExpr
	{
		string _Cls;		// class name
		string _Func;		// function/operator
		IExpr[] _Args;		// arguments 
		CodeModules _Cm;		// the loaded assemblies
		TypeCode _ReturnTypeCode;	// the return type
		Type[] _ArgTypes;	// argument types

		/// <summary>
		/// passed class name, function name, and args for evaluation
		/// </summary>
		public FunctionCustomStatic(CodeModules cm, string c, string f, IExpr[] a, TypeCode type) 
		{
			_Cls = c;
			_Func = f;
			_Args = a;
			_Cm = cm;
			_ReturnTypeCode = type;

			_ArgTypes = new Type[a.Length];
			int i=0;
			foreach (IExpr ex in a)
			{
				_ArgTypes[i++] = XmlUtil.GetTypeFromTypeCode(ex.GetTypeCode());
			}

		}

		public TypeCode GetTypeCode()
		{
			return _ReturnTypeCode;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(false);		// Can't know what the function does
		}

		public async Task<IExpr> ConstantOptimization()
		{
			// Do constant optimization on all the arguments
			for (int i=0; i < _Args.GetLength(0); i++)
			{
				IExpr e = (IExpr)_Args[i];
				_Args[i] = await e.ConstantOptimization();
			}

			// Can't assume that the function doesn't vary
			//   based on something other than the args e.g. Now()
			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public async Task<object> Evaluate(Report rpt, Row row)
		{
			// get the results
			object[] argResults = new object[_Args.Length];
			int i=0;
			bool bUseArg=true;
			foreach(IExpr a  in _Args)
			{
				argResults[i] = await a.Evaluate(rpt, row);
				if (argResults[i] != null && argResults[i].GetType() != _ArgTypes[i])
					bUseArg = false;
				i++;
			}
			// we build the arguments based on the type
			Type[] argTypes = bUseArg? _ArgTypes: Type.GetTypeArray(argResults);

			// We can definitely optimize this by caching some info TODO

			// Get ready to call the function
			Object returnVal;
			Type theClassType= _Cm[_Cls];
            MethodInfo mInfo = XmlUtil.GetMethod(theClassType, _Func, argTypes);
            if (mInfo == null)
            {
                throw new Exception(string.Format(Strings.FunctionCustomStatic_Error_MethodNotFoundInClass, _Func, _Cls));
            }

            returnVal = mInfo.Invoke(theClassType, argResults);

			return returnVal;
		}

		public async Task<double> EvaluateDouble(Report rpt, Row row)
		{
			return Convert.ToDouble(await Evaluate(rpt, row));
		}
		
		public async Task<decimal> EvaluateDecimal(Report rpt, Row row)
		{
			return Convert.ToDecimal(await Evaluate(rpt, row));
		}

        public async Task<int> EvaluateInt32(Report rpt, Row row)
        {
            return Convert.ToInt32(await Evaluate(rpt, row));
        }

		public async Task<string> EvaluateString(Report rpt, Row row)
		{
			return Convert.ToString(await Evaluate(rpt, row));
		}

		public async Task<DateTime> EvaluateDateTime(Report rpt, Row row)
		{
			return Convert.ToDateTime(await Evaluate(rpt, row));
		}


		public async Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Convert.ToBoolean(await Evaluate(rpt, row));
		}

		public string Cls
		{
			get { return  _Cls; }
			set {  _Cls = value; }
		}

		public string Func
		{
			get { return  _Func; }
			set {  _Func = value; }
		}

		public IExpr[] Args
		{
			get { return  _Args; }
			set {  _Args = value; }
		}
	}

}
