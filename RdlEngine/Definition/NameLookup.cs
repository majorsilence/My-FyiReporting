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

namespace fyiReporting.RDL
{
	///<summary>
	/// Parsing name lookup.  Fields, parameters, report items, globals, user, aggregate scopes, grouping,...
	///</summary>
	internal class NameLookup
	{
		IDictionary fields;
		IDictionary parameters;
		IDictionary reportitems;
		IDictionary globals;
		IDictionary user;
		IDictionary aggrScope;
		Grouping g;					// if expression in a table group or detail group
									//   used to default aggregates to the right group
		Matrix m;					// if expression used in a Matrix
									//   used to default aggregate to the right matrix
		Classes instances;
		CodeModules cms;
		Type codeType;
		DataSetsDefn dsets;
		ReportLink _PageFooterHeader;	// when expression is in page header or footer this is set
		string _ExprName;			// name of the expression; this isn't always set

		internal NameLookup(IDictionary f, IDictionary p, IDictionary r, 
			IDictionary gbl, IDictionary u, IDictionary ascope, 
			Grouping ag, Matrix mt, CodeModules cm, Classes i, DataSetsDefn ds, Type ct)
		{
			fields=f;
			parameters=p;
			reportitems=r;
			globals=gbl;
			user=u;
			aggrScope = ascope;
			g=ag;
			m = mt;
			cms = cm;
			instances = i;
			dsets = ds;
			codeType = ct;
		}

		internal ReportLink PageFooterHeader
		{
			get {return _PageFooterHeader;}
			set {_PageFooterHeader = value;}
		}
		
		internal bool IsPageScope
		{
			get {return _PageFooterHeader != null;}
		}

		internal string ExpressionName
		{
			get {return _ExprName;}
			set {_ExprName = value;}
		}

		internal IDictionary Fields
		{
			get { return fields; }
		}
		internal IDictionary Parameters
		{
			get { return parameters; }
		}
		internal IDictionary ReportItems
		{
			get { return reportitems; }
		}
		internal IDictionary User
		{
			get { return user; }
		}
		internal IDictionary Globals
		{
			get { return globals; }
		}

		internal ReportClass LookupInstance(string name)
		{
			if (instances == null)
				return null;
			return instances[name];
		}

		internal Field LookupField(string name)
		{	
			if (fields == null)
				return null;

			return (Field) fields[name];
		}

		internal ReportParameter LookupParameter(string name)
		{	
			if (parameters == null)
				return null;
			return (ReportParameter) parameters[name];
		}

		internal Textbox LookupReportItem(string name)
		{	
			if (reportitems == null)
				return null;
			return (Textbox) reportitems[name];
		}

		internal IExpr LookupGlobal(string name)
		{	
			if (globals == null)
				return null;
			return (IExpr) globals[name];
		}

		internal Type LookupType(string clsname)
		{
			if (cms == null || clsname == string.Empty)
				return null;
			return cms[clsname];
		}

		internal CodeModules CMS
		{
			get{return cms;}
		}

		internal Type CodeClassType
		{
			get{return codeType;}
		}

		internal IExpr LookupUser(string name)
		{	
			if (user == null)
				return null;
			return (IExpr) user[name];
		}

		internal Grouping LookupGrouping()
		{
			return g;
		}

		internal Matrix LookupMatrix()
		{
			return m;
		}

		internal object LookupScope(string name)
		{
			if (aggrScope == null)
				return null;
			return aggrScope[name];
		}

		internal DataSetDefn ScopeDataSet(string name)
		{
			if (name == null)
			{	// Only allowed when there is only one dataset
				if (dsets.Items.Count != 1)
					return null;
                
				foreach (DataSetDefn ds in dsets.Items.Values)	// No easy way to get the item by index
					return ds;
				return null;
			}
			return dsets[name];
		}

	}
}
