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
	/// Handle a Matrix Row: i.e. height and matrix cells that make up the row.
	///</summary>
	[Serializable]
	internal class MatrixRow : ReportLink
	{
		RSize _Height;	// Height of each detail cell in this row.
		MatrixCells _MatrixCells;	// The set of cells in a row in the detail section of the Matrix.		
	
		internal MatrixRow(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Height=null;
			_MatrixCells=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Height":
						_Height = new RSize(r, xNodeLoop);
						break;
					case "MatrixCells":
						_MatrixCells = new MatrixCells(r, this, xNodeLoop);
						break;
					default:
						break;
				}
			}
			if (_MatrixCells == null)
				OwnerReport.rl.LogError(8, "MatrixRow requires the MatrixCells element.");
		}
		
		override internal void FinalPass()
		{
			if (_MatrixCells != null)
				_MatrixCells.FinalPass();
			return;
		}

		internal RSize Height
		{
			get { return  _Height; }
			set {  _Height = value; }
		}

		internal MatrixCells MatrixCells
		{
			get { return  _MatrixCells; }
			set {  _MatrixCells = value; }
		}
	}
}
