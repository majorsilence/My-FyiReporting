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
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
	///<summary>
	/// A report expression: includes original source, parsed expression and type information.
	///</summary>
	[Serializable]
	internal class DynamicExpression: IExpr
	{
		string _Source;			// source of expression
		IExpr _Expr;			// expression after parse
        TypeCode _Type;
        ReportLink _rl;
	
		internal DynamicExpression(Report rpt, ReportLink p, string expr, Row row)
		{
			_Source=expr;
			_Expr = null;
            _rl = p;
            _Type = DoParse(rpt);
		}

		internal TypeCode DoParse(Report rpt)
		{
			// optimization: avoid expression overhead if this isn't really an expression
			if (_Source == null)
			{
				_Expr = new Constant("");
                return _Expr.GetTypeCode();
			}
			else if (_Source == string.Empty ||			// empty expression
				_Source[0] != '=')	// if 1st char not '='
			{
				_Expr = new Constant(_Source);	//   this is a constant value
                return _Expr.GetTypeCode();
			}

			Parser p = new Parser(new System.Collections.Generic.List<ICacheData>());

			// find the fields that are part of the DataRegion (if there is one)
			IDictionary fields=null;
			ReportLink dr = _rl.Parent;
			Grouping grp= null;		// remember if in a table group or detail group or list group
			Matrix m=null;

            while (dr != null)
			{
				if (dr is Grouping)
					p.NoAggregateFunctions = true;
				else if (dr is TableGroup)
					grp = ((TableGroup) dr).Grouping;
				else if (dr is Matrix)
				{
					m = (Matrix) dr;		// if matrix we need to pass special
					break;
				}
				else if (dr is Details)
				{
					grp = ((Details) dr).Grouping;
				}
				else if (dr is List)
				{
					grp = ((List) dr).Grouping;
					break;
				}
				else if (dr is DataRegion || dr is DataSetDefn)
					break;
				dr = dr.Parent;
			}
			if (dr != null)
			{
				if (dr is DataSetDefn)
				{
					DataSetDefn d = (DataSetDefn) dr;
					if (d.Fields != null)
						fields = d.Fields.Items;
				}
				else	// must be a DataRegion
				{
					DataRegion d = (DataRegion) dr;
					if (d.DataSetDefn != null &&
						d.DataSetDefn.Fields != null)
						fields = d.DataSetDefn.Fields.Items;
				}
			}

			NameLookup lu = new NameLookup(fields, rpt.ReportDefinition.LUReportParameters,
                rpt.ReportDefinition.LUReportItems, rpt.ReportDefinition.LUGlobals,
                rpt.ReportDefinition.LUUser, rpt.ReportDefinition.LUAggrScope,
                grp, m, rpt.ReportDefinition.CodeModules, rpt.ReportDefinition.Classes, rpt.ReportDefinition.DataSetsDefn,
                rpt.ReportDefinition.CodeType);

			try 
			{
				_Expr = p.Parse(lu, _Source);
			}
			catch (Exception e)
			{
				_Expr = new ConstantError(e.Message);
				// Invalid expression
				rpt.rl.LogError(8, ErrorText(e.Message));
			}

			// Optimize removing any expression that always result in a constant
			try
			{
				_Expr = _Expr.ConstantOptimization();
			}
			catch(Exception ex)
			{
				rpt.rl.LogError(4, "Expression:" + _Source + "\r\nConstant Optimization exception:\r\n" + ex.Message + "\r\nStack trace:\r\n" + ex.StackTrace );
			}

            return _Expr.GetTypeCode();
		}

        private string ErrorText(string msg)
        {
            ReportLink rl = _rl.Parent;
            while (rl != null)
            {
                if (rl is ReportItem)
                    break;
                rl = rl.Parent;
            }

            string prefix="Expression";
            if (rl != null)
            {          
                ReportItem ri = rl as ReportItem;
                if (ri.Name != null)
                    prefix = ri.Name.Nm + " expression";
            }
            return prefix + " '" + _Source + "' failed to parse: " + msg;
        }

		private void ReportError(Report rpt, int severity, string err)
		{
            rpt.rl.LogError(severity, err);
		}

		internal string Source
		{
			get { return  _Source; }
		}
		internal IExpr Expr
		{
			get { return  _Expr; }
		}
		internal TypeCode Type
		{
			get { return  _Type; }
		}
		#region IExpr Members

		public System.TypeCode GetTypeCode()
		{
			return _Expr.GetTypeCode();
		}

		public bool IsConstant()
		{
			return _Expr.IsConstant();
		}

		public IExpr ConstantOptimization()
		{
			return this;
		}

		public object Evaluate(Report rpt, Row row)
		{
			try 
			{
				return _Expr.Evaluate(rpt, row);
			}
			catch (Exception e)
			{
				string err;
				if (e.InnerException != null)
					err = String.Format("Exception evaluating {0}.  {1}.  {2}", _Source, e.Message, e.InnerException.Message);
				else
					err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);

				ReportError(rpt, 4, err);
				return null;
			}
		}

		public string EvaluateString(Report rpt, Row row)
		{
			try 
			{
				return _Expr.EvaluateString(rpt, row);
			}
			catch (Exception e)
			{	
				string err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);
				ReportError(rpt, 4, err);
				return null;
			}
		}

		public double EvaluateDouble(Report rpt, Row row)
		{
			try 
			{
				return _Expr.EvaluateDouble(rpt, row);
			}
			catch (Exception e)
			{	
				string err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);
				ReportError(rpt, 4, err);
				return double.NaN;
			}
		}

		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			try 
			{
				return _Expr.EvaluateDecimal(rpt, row);
			}
			catch (Exception e)
			{	
				string err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);
				ReportError(rpt, 4, err);
				return decimal.MinValue;
			}
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            try
            {
                return _Expr.EvaluateInt32(rpt, row);
            }
            catch (Exception e)
            {
                string err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);
                ReportError(rpt, 4, err);
                return int.MinValue;
            }
        }

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			try 
			{
				return _Expr.EvaluateDateTime(rpt, row);
			}
			catch (Exception e)
			{	
				string err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);
				ReportError(rpt, 4, err);
				return DateTime.MinValue;
			}
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			try 
			{
				return _Expr.EvaluateBoolean(rpt, row);
			}
			catch (Exception e)
			{	
				string err = String.Format("Exception evaluating {0}.  {1}", _Source, e.Message);
				ReportError(rpt, 4, err);
				return false;
			}
		}

		#endregion
	}
}
