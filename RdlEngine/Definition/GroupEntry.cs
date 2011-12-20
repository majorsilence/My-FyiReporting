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
using System.Collections.Generic;

namespace fyiReporting.RDL
{
	///<summary>
	/// Runtime data structure representing the group hierarchy
	///</summary>
	internal class GroupEntry
	{
		Grouping _Group;		// Group this represents
		Sorting _Sort;			// Sort cooresponding to this group
		int _StartRow;			// Starting row of the group (inclusive)
		int _EndRow;			// Endding row of the group (inclusive)
		List<GroupEntry> _NestedGroup;	// group one hierarchy below
	
		internal GroupEntry(Grouping g, Sorting s, int start)
		{
			_Group = g;
			_Sort = s;
			_StartRow = start;
			_EndRow = -1;
            _NestedGroup = new List<GroupEntry>();

			// Check to see if grouping and sorting are the same
			if (g == null || s == null)
				return;			// nothing to check if either is null

			if (s.Items.Count != g.GroupExpressions.Items.Count)
				return;

			for (int i = 0; i < s.Items.Count; i++)
			{
				SortBy sb = s.Items[i] as SortBy;

				if (sb.Direction == SortDirectionEnum.Descending)
					return;			// TODO we could optimize this 
				
				FunctionField ff = sb.SortExpression.Expr as FunctionField;
				if (ff == null || ff.GetTypeCode() != TypeCode.String)
					return;

				GroupExpression ge = g.GroupExpressions.Items[i] as GroupExpression;
				FunctionField ff2 = ge.Expression.Expr as FunctionField;
				if (ff2 == null || ff.Fld != ff2.Fld)
					return;
			}
			_Sort = null;		// we won't need to sort since the groupby will handle it correctly
		}

		internal int StartRow
		{
			get { return  _StartRow; }
			set {  _StartRow = value; }
		}

		internal int EndRow
		{
			get { return  _EndRow; }
			set {  _EndRow = value; }
		}

		internal List<GroupEntry> NestedGroup
		{
			get { return  _NestedGroup; }
			set {  _NestedGroup = value; }
		}

		internal Grouping Group
		{
			get { return  _Group; }
		}

		internal Sorting Sort
		{
			get { return  _Sort; }
		}
	}

}
