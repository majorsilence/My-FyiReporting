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
	internal class MatrixEntry
	{
        Row _r;                 // needed for sort
		Dictionary<string, MatrixEntry> _HashData;	// Hash table of data values
		List<MatrixEntry> _SortedData;	//  Sorted List version of the data 
        string _hash;           // item that is used for hash
		BitArray _Rows;			// rows 
		MatrixEntry _Parent;	// parent
		ColumnGrouping _ColumnGroup;	//   Column grouping
		RowGrouping _RowGroup;	// Row grouping
		int _FirstRow;			// First row in _Rows marked true
		int _LastRow;			// Last row in _Rows marked true
		int _rowCount;			//   we save the rowCount so we can delay creating bitArray
		int _StaticColumn=0;	// this is the index to which column to use (always 0 when dynamic)
		int _StaticRow=0;		// this is the index to which row to use (always 0 when dynamic)
		Rows _Data;				// set dynamically when needed
	
		internal MatrixEntry(Row r, string hash, MatrixEntry p, int rowCount)
		{
            _r = r;
            _hash = hash;
			_HashData = new Dictionary<string, MatrixEntry>();
			_ColumnGroup = null;
			_RowGroup = null;
			_SortedData = null;
			_Data = null;
			_rowCount = rowCount;
			_Rows = null;
			_Parent = p;
			_FirstRow = -1;
			_LastRow = -1;
		}

		internal Dictionary<string, MatrixEntry> HashData
		{
			get { return  _HashData; }
		}

		internal Rows Data
		{
			get { return _Data; }
			set { _Data = value; }
		}

        internal string HashItem
        {
            get { return _hash; }
        }
		internal List<MatrixEntry> GetSortedData(Report rpt)
		{
			if (_SortedData == null && _HashData != null && _HashData.Count > 0)
			{
				_SortedData = new List<MatrixEntry>(_HashData.Values);
                _SortedData.Sort(new MatrixEntryComparer(rpt, GetSortBy()));
				_HashData = null;		// we only keep one
			}

			return  _SortedData; 
		}

		internal MatrixEntry Parent
		{
			get { return  _Parent; }
		}

		internal ColumnGrouping ColumnGroup
		{
			get { return  _ColumnGroup; }
			set {  _ColumnGroup = value; }
		}

		internal int StaticRow
		{
			get { return  _StaticRow; }
			set {  _StaticRow = value; }
		}

		internal int StaticColumn
		{
			get { return  _StaticColumn; }
			set {  _StaticColumn = value; }
		}

		internal RowGrouping RowGroup
		{
			get { return  _RowGroup; }
			set {  _RowGroup = value; }
		}

		internal int FirstRow
		{
			get { return  _FirstRow; }
			set 
			{
				if (_FirstRow == -1)
					_FirstRow = value; 
			}
		}

		internal int LastRow
		{
			get { return  _LastRow; }
			set 
			{
				if (value >= _LastRow)
					_LastRow = value; 
			}
		}

		internal BitArray Rows
		{
			get 
			{
				if (_Rows == null)
					_Rows = new BitArray(_rowCount);

				return  _Rows; 
			}
			set {  _Rows = value; }
		}

        internal Row R
        {
            get { return _r; }
        }

        Sorting GetSortBy()
        {
            MatrixEntry me=null;
            foreach (MatrixEntry m in this.HashData.Values)
            {   // just get the first one
                me = m;
                break;
            }
            if (me == null)
                return null;

            Sorting sb = null;
            if (me.RowGroup != null)
            {
                if (me.RowGroup.DynamicRows != null)
                    sb = me.RowGroup.DynamicRows.Sorting;
            }
            else if (me.ColumnGroup != null)
            {
                if (me.ColumnGroup.DynamicColumns != null)
                    sb = me.ColumnGroup.DynamicColumns.Sorting;
            }
            return sb;
        }

    }

    class MatrixEntryComparer : System.Collections.Generic.IComparer<MatrixEntry>
    {
        Report _rpt;
        Sorting _Sorting;

        public MatrixEntryComparer(Report rpt, Sorting s)
        {
            _rpt = rpt;
            _Sorting = s;
        }

		#region IComparer Members

        public int Compare(MatrixEntry m1, MatrixEntry m2)
        {
            if (_Sorting == null)
                return m1.HashItem.CompareTo(m2.HashItem);

            object o1 = null, o2 = null;
            TypeCode tc = TypeCode.Object;
            int rc;
            try
            {
                foreach (SortBy sb in _Sorting.Items)
                {
                    IExpr e = sb.SortExpression.Expr;
                    o1 = e.Evaluate(_rpt, m1.R);
                    o2 = e.Evaluate(_rpt, m2.R);
                    tc = e.GetTypeCode();
                    rc = Filter.ApplyCompare(tc, o1, o2);
                    if (rc != 0)
                        return sb.Direction == SortDirectionEnum.Ascending? rc : -rc;
                }
            }
            catch (Exception e)		// this really shouldn't happen
            {
                _rpt.rl.LogError(8,
                    string.Format("Matrix Sort rows exception\r\nArguments: {0} {1}\r\nTypecode: {2}\r\n{3}\r\n{4}",
                    o1, o2, tc.ToString(), e.Message, e.StackTrace));
                return m1.HashItem.CompareTo(m2.HashItem);
            }
            return 0;		    // treat as the same

        }

        #endregion
    }
}
