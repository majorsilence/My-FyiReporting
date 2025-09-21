

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// A collection of rows.
	///</summary>
    internal class Rows : System.Collections.Generic.IComparer<Row>, IDisposable
	{
		List<Row> _Data;	// array of Row object;
        List<RowsSortExpression> _SortBy;	// array of expressions used to sort the data
		GroupEntry[] _CurrentGroups;	// group
		Report _Rpt;

		internal Rows(Report rpt)
		{
			_Rpt = rpt;
			_SortBy = null;
			_CurrentGroups = null;
		}

		// Constructor that takes existing Rows; a start, end and bitArray with the rows wanted
		internal Rows(Report rpt, Rows r, int start, int end, BitArray ba)
		{
			_Rpt = rpt;
			_SortBy = null;
			_CurrentGroups = null;
			if (end - start < 0)			// null set?
			{
				_Data = new List<Row>(1);
                _Data.TrimExcess();
				return;
			}
			_Data = new List<Row>(end - start + 1);

			for (int iRow = start; iRow <= end; iRow++)
			{
				if (ba == null || ba.Get(iRow))
				{
					Row or = r.Data[iRow]; 
					Row nr = new Row(this, or);
					nr.RowNumber = or.RowNumber;
					_Data.Add(nr);
				}
			}

            _Data.TrimExcess();
		}

		// Constructor that takes existing Rows
		internal Rows(Report rpt, Rows r)
		{
			_Rpt = rpt;
			_SortBy = null;
			_CurrentGroups = null;
			if (r.Data == null || r.Data.Count <= 0)			// null set?
			{
				_Data = new List<Row>(1);
                _Data.TrimExcess();
				return;
			}
			_Data = new List<Row>(r.Data.Count);

			for (int iRow = 0; iRow < r.Data.Count; iRow++)
			{
				Row or = r.Data[iRow]; 
				Row nr = new Row(this, or);
				nr.RowNumber = or.RowNumber;
				_Data.Add(nr);
			}

            _Data.TrimExcess();
		}

        // Constructor that creates exactly one row and one column
        static internal Rows CreateOneRow(Report rpt)
        {
            Rows or = new Rows(rpt);

            or._Data = new List<Row>(1);
            Row nr = new Row(or, 1);
            nr.RowNumber = 0;
            or._Data.Add(nr);
            or._Data.TrimExcess();
            return or;
        }

		internal Rows(Report rpt, TableGroups tg, Grouping g, Sorting s)
		{
			_Rpt = rpt;
            _SortBy = new List<RowsSortExpression>();
			// Pull all the sort expression together
			if (tg != null)
			{
				foreach(TableGroup t in tg.Items)
				{
					foreach(GroupExpression ge in t.Grouping.GroupExpressions.Items)
					{
						_SortBy.Add(new RowsSortExpression(ge.Expression));
					}
					// TODO what to do with the sort expressions!!!!
				}
			}
			if (g != null)
			{
				if (g.ParentGroup != null)
					_SortBy.Add(new RowsSortExpression(g.ParentGroup));
				else if (g.GroupExpressions != null)
				{
					foreach (GroupExpression ge in g.GroupExpressions.Items)
					{
						_SortBy.Add(new RowsSortExpression(ge.Expression));
					}
				}
			}
			if (s != null)
			{
				foreach (SortBy sb in s.Items)
				{
					_SortBy.Add(new RowsSortExpression(sb.SortExpression, sb.Direction == SortDirectionEnum.Ascending));
				}
			}
            if (_SortBy.Count > 0)
            {
                _SortBy.TrimExcess();
            }
            else
            {
                _SortBy = null;
            }
		}

		internal Report Report
		{
			get {return this._Rpt;}
		}

		internal void Sort()
		{
			// sort the data array by the data.
			_Data.Sort(this);
		}

		internal List<Row> Data
		{
			get { return  _Data; }
			set 
			{ 
				_Data = value;			// Assign the new value
				foreach(Row r in _Data)	// Updata all rows
				{
					r.R = this;
				}
		
			}
		}

        internal List<RowsSortExpression> SortBy
		{
			get { return  _SortBy; }
			set { _SortBy = value; }
		}

		internal GroupEntry[] CurrentGroups
		{
			get { return  _CurrentGroups; }
			set { _CurrentGroups = value; }
		}

		#region IComparer Members

		public int Compare(Row r1, Row r2)
		{
			if (r1 == r2)				// why does the sort routine do this??
				return 0;

			object o1=null,o2=null;
			TypeCode tc = TypeCode.Object;
			int rc;
			try 
			{
				foreach (RowsSortExpression se in _SortBy)
				{
					// HACK: async
					o1 = Task.Run(async () => await se.expr.Evaluate(this._Rpt, r1)).GetAwaiter().GetResult();
					o2 = Task.Run(async () => await se.expr.Evaluate(this._Rpt, r2)).GetAwaiter().GetResult();
					tc = se.expr.GetTypeCode();
					rc = Filter.ApplyCompare(tc, o1, o2);
					if (rc != 0)
						return se.bAscending? rc: -rc;
				}
			}
			catch (Exception e)		// this really shouldn't happen
			{
				_Rpt.rl.LogError(8, 
                    string.Format("Sort rows exception\r\nArguments: {0} {1}\r\nTypecode: {2}\r\n{3}\r\n{4}", 
                    o1, o2, tc.ToString(), e.Message, e.StackTrace));
			}
			return r1.RowNumber - r2.RowNumber;		// in case of tie use original row number
		}

		#endregion

		public void Dispose()
		{
			_Data.Clear();
		}
	}

	class RowsSortExpression
	{
		internal Expression expr;
		internal bool bAscending;

		internal RowsSortExpression(Expression e, bool asc)
		{
			expr = e;
			bAscending = asc;
		}

		internal RowsSortExpression(Expression e)
		{
			expr = e;
			bAscending = true;
		}
	}
}
