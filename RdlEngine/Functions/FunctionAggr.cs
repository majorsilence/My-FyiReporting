
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
{
	/// <summary>
	/// Base class for all aggregate functions
	/// </summary>
	[Serializable]
	internal abstract class FunctionAggr 
	{
		public IExpr _Expr;			// aggregate expression
		public object _Scope;		// DataSet or Grouping or DataRegion that contains (directly or
										//  indirectly) the report item that the aggregate
										//  function is used in
										// Can also hold the Matrix object
		bool _LevelCheck;				// row processing requires level check
										//   i.e. simple specified on recursive row check
		/// <summary>
		/// Base class of all aggregate functions
		/// </summary>

		public FunctionAggr(IExpr e, object scp) 
		{
			_Expr = e;
			_Scope = scp;
			_LevelCheck = false;
		}

		public Task<bool> IsConstant()
		{
			return Task.FromResult(false);
		}

		public async Task<IExpr> ConstantOptimization()
		{
			if (_Expr != null)
				_Expr = await _Expr.ConstantOptimization();
			return (IExpr) this;
		}

		public virtual Task<bool> EvaluateBoolean(Report rpt, Row row)
		{
			return Task.FromResult(false);
		}

		public IExpr Expr
		{
			get { return  _Expr; }
		}

		public object Scope
		{
			get { return  _Scope; }
		}

		public bool LevelCheck
		{
			get { return  _LevelCheck; }
			set { _LevelCheck = value; }
		}

		// return an IEnumerable that represents the scope of the data
		protected RowEnumerable GetDataScope(Report rpt, Row row, out bool bSave)
			{
			bSave=true;
			RowEnumerable re=null;

			if (this._Scope != null)
			{
				Type t = this._Scope.GetType();

                //150208AJM GJL Trying - And Succeeding!!! to get data from chart
                if (t == typeof(Chart)) {
                    Chart c = (Chart)this._Scope;
                    this._Scope = c.ChartMatrix;
                    t = this._Scope.GetType();
                }

				if (t == typeof(Grouping))
				{
					bSave=false;
					Grouping g = (Grouping) (this._Scope);
					if (g.InMatrix)
					{
						Rows rows = g.GetRows(rpt);
						if (rows == null)
							return null;
						re = new RowEnumerable(0, rows.Data.Count-1, rows.Data, _LevelCheck);
					}
					else
					{
						if (row == null || row.R.CurrentGroups == null)     // currentGroups can be null when reference Textbox in header/footer that has a scoped aggr function reference (TODO: this is a problem!)
							return null;
						GroupEntry ge = row.R.CurrentGroups[g.GetIndex(rpt)];
						re = new RowEnumerable (ge.StartRow, ge.EndRow, row.R.Data, _LevelCheck);
					}
				}
				else if (t == typeof(Matrix))
				{
					bSave=false;
					Matrix m = (Matrix) (this._Scope);
					Rows mData = m.GetMyData(rpt);
					re = new RowEnumerable(0, mData.Data.Count-1, mData.Data, false);
				}
				else if (t == typeof(string))
				{	// happens on page header/footer scope
					if (row != null)
						re = new RowEnumerable (0, row.R.Data.Count-1, row.R.Data, false);
					bSave = false;
				}
                else if (t == typeof(DataSetDefn))
                {
                    DataSetDefn ds = this._Scope as DataSetDefn;
                    if (ds != null && ds.Query != null)
                    {
                        Rows rows = ds.Query.GetMyData(rpt);
                        if (rows != null)
                            re = new RowEnumerable(0, rows.Data.Count - 1, rows.Data, false);
                    }
                }
				else if (row != null)
				{
					re = new RowEnumerable (0, row.R.Data.Count-1, row.R.Data, false);
				}
                else
				{
					DataSetDefn ds = this._Scope as DataSetDefn;
					if (ds != null && ds.Query != null)
					{
						Rows rows = ds.Query.GetMyData(rpt);
						if (rows != null)
							re = new RowEnumerable(0, rows.Data.Count-1, rows.Data, false);
					}
				}
			}
			else if (row != null)
			{
				re = new RowEnumerable (0, row.R.Data.Count-1, row.R.Data, false);
			}

			return re;
		}
	}

	internal class RowEnumerable : IEnumerable
	{
		int startRow;
		int endRow;
		List<Row> data;
		bool _LevelCheck;
        public RowEnumerable(int start, int end, List<Row> d, bool levelCheck)
		{
			startRow = start;
			endRow = end;
			data = d;
			_LevelCheck = levelCheck;
		}

        public List<Row> Data
		{
			get{return data;}
		}

		public int FirstRow
		{
			get{return startRow;}
		}

		public int LastRow
		{
			get{return endRow;}
		}

		public bool LevelCheck
		{
			get{return _LevelCheck;}
		}

		// Methods
		public IEnumerator GetEnumerator()
		{
			return new RowEnumerator(this);
		}
	}

	internal class RowEnumerator : IEnumerator
	{
		private RowEnumerable re;
		private int index = -1;
		public RowEnumerator(RowEnumerable rea)
		{
			re = rea;
		}
		//Methods
		public bool MoveNext()
		{
			index++;
			while (true)
			{
				if (index + re.FirstRow > re.LastRow)
					return false;
				else
				{
					if (re.LevelCheck)
					{	//
						Row r1 = re.Data[re.FirstRow] as Row;
						Row r2 = re.Data[index + re.FirstRow] as Row;
						if (r1.Level == r1.Level)
							return true;
						index++;
					}
					else
						return true;
				}
			}
		}

		public void Reset()
		{
			index=-1;
		}

		public object Current
		{
			get{return(re.Data[index + re.FirstRow]);}
		}
	}
}
