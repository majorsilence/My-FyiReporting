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
using RdlEngine.Resources;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Class is used to evaluate static system classes.   System meaning
	/// any class that is part of this assembly.   The parser restricts this
	/// to Math, String, Convert, Financial, ...
	/// </summary>
	[Serializable]
	internal class FunctionSystem : IExpr
	{
		string _Cls;		// class name
		string _Func;		// function/operator
		IExpr[] _Args;		// arguments 
		TypeCode _ReturnTypeCode;	// the return type

		/// <summary>
		/// passed class name, function name, and args for evaluation
		/// </summary>
		public FunctionSystem(string c, string f, IExpr[] a, TypeCode type) 
		{
			_Cls = c;
			_Func = f;
			_Args = a;
			_ReturnTypeCode = type;
		}

		public TypeCode GetTypeCode()
		{
			return _ReturnTypeCode;
		}

		public bool IsConstant()
		{
			return false;		// Can't know what the function does
		}

		public IExpr ConstantOptimization()
		{
			// Do constant optimization on all the arguments
			for (int i=0; i < _Args.GetLength(0); i++)
			{
				IExpr e = (IExpr)_Args[i];
				_Args[i] = e.ConstantOptimization();
			}

			// Can't assume that the function doesn't vary
			//   based on something other than the args e.g. Now()
			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			// get the results
			object[] argResults = new object[_Args.Length];
			int i=0;
            bool bNull = false;
			foreach(IExpr a  in _Args)
			{
				argResults[i] = a.Evaluate(rpt, row);
                if (argResults[i] == null)
                    bNull = true;
                i++;
			}
			Type[] argTypes;
            if (bNull)
            {
                // Need to put fake values in that match the types
                object[] tempResults = new object[argResults.Length];
                for (int ix = 0; ix < argResults.Length; ix++)
                {
                    tempResults[ix] =
                        argResults[ix] == null?
                            XmlUtil.GetConstFromTypeCode(_Args[ix].GetTypeCode()):
                            argResults[ix];

                }
                argTypes = Type.GetTypeArray(tempResults);
            }
            else
                argTypes = Type.GetTypeArray(argResults);

			// We can definitely optimize this by caching some info TODO

			// Get ready to call the function
			object returnVal;
			Type theClassType= Type.GetType(_Cls, true, true);
            MethodInfo mInfo = XmlUtil.GetMethod(theClassType, _Func, argTypes);
            if (mInfo == null)
            {
                throw new Exception(string.Format(Strings.FunctionSystem_Error_MethodNotFound, _Func, _Cls));
            }

            returnVal = mInfo.Invoke(theClassType, argResults);

			return returnVal;
		}

		public double EvaluateDouble(Report rpt, Row row)
		{
			return Convert.ToDouble(Evaluate(rpt, row));
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return Convert.ToDecimal(Evaluate(rpt, row));
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return Convert.ToInt32(Evaluate(rpt, row));
        }

		public string EvaluateString(Report rpt, Row row)
		{
			return Convert.ToString(Evaluate(rpt, row));
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			return Convert.ToDateTime(Evaluate(rpt, row));
		}


		public bool EvaluateBoolean(Report rpt, Row row)
		{
			return Convert.ToBoolean(Evaluate(rpt, row));
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
