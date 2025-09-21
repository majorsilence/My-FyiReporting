

using System;
using System.Collections;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Runtime data structure representing the group hierarchy
	///</summary>
	internal class MatrixCellEntry
	{
		Rows _Data;						// Rows matching this cell entry
		ReportItem _DisplayItem;		// Report item to display in report
		double _Value=double.MinValue;	// used by Charts to optimize data request
		float _XPosition;				// position of cell
		float _Height;					// height of cell
		float _Width;					// width of cell
		MatrixEntry _RowME;				// MatrixEntry for the row that made cell entry
		MatrixEntry _ColumnME;			// MatrixEntry for the column that made cell entry
		int _ColSpan;					// # of columns spanned
	
		internal MatrixCellEntry(Rows d, ReportItem ri)
		{
			_Data = d;
			_DisplayItem = ri;
			_ColSpan = 1;
		}

		internal Rows Data
		{
			get { return  _Data; }
		}

		internal int ColSpan
		{
			get { return _ColSpan;}
			set { _ColSpan = value; }
		}

		internal ReportItem DisplayItem
		{
			get { return  _DisplayItem; }
		}

		internal float Height
		{
			get { return _Height; }
			set { _Height = value; }
		}

		internal MatrixEntry ColumnME
		{
			get { return _ColumnME; }
			set { _ColumnME = value; }
		}

		internal MatrixEntry RowME
		{
			get { return _RowME; }
			set { _RowME = value; }
		}

		internal double Value
		{
			get { return  _Value; }
			set { _Value = value; }
		}

		internal float Width
		{
			get { return _Width; }
			set { _Width = value; }
		}

		internal float XPosition
		{
			get { return _XPosition; }
			set { _XPosition = value; }
		}
	}
}
