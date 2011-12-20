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
