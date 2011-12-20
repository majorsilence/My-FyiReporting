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

namespace fyiReporting.RDL
{
	///<summary>
	/// A Row in a data set.
	///</summary>
	internal class Row
	{
		int _RowNumber;		// Original row #
		int _Level;			// Usually 0; set when row is part of group with ParentGroup (ie recursive hierarchy)
		GroupEntry _GroupEntry;		//   like level; 
		Rows _R;			// Owner of row collection
		object[] _Data;		// Row of data
	
		internal Row(Rows r, Row rd)			// Constructor that uses existing Row data
		{
			_R = r;
			_Data = rd.Data;
			_Level = rd.Level;
		}

		internal Row(Rows r, int columnCount)
		{
			_R = r;
			_Data = new object[columnCount];
			_Level=0;
		}

		internal object[] Data
		{
			get { return  _Data; }
			set { _Data = value; }
		}

		internal Rows R
		{
			get { return  _R; }
			set { _R = value; }
		}

		internal GroupEntry GroupEntry
		{
			get { return  _GroupEntry; }
			set {  _GroupEntry = value; }
		}

		internal int Level
		{
			get { return  _Level; }
			set {  _Level = value; }
		}

		internal int RowNumber
		{
			get { return  _RowNumber; }
			set {  _RowNumber = value; }
		}
	}
}
