/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

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
using fyiReporting.RDL;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Drawing;

namespace fyiReporting.RDL
{
	
	///<summary>
	/// Renders a report to HTML.   This handles some page formating but does not do true page formatting.
	///</summary>
	internal class RenderExcel: IPresent
	{
        Report r;					// report
		IStreamGen _sg;				// stream generater
        Bitmap _bm=null;			// bm and
		Graphics _g=null;			//		  g are needed when calculating string heights

        // Excel currentcy 
        int _ExcelRow = -1;               // current row
        int _ExcelCol = -1;               // current col
        ExcelValet _Excel;
        string SheetName;                   // current sheetname

		public RenderExcel(Report rep, IStreamGen sg)
		{
			r = rep;
			_sg = sg;					// We need this in future

            _Excel = new ExcelValet();

        }
		~RenderExcel()
		{
			// These should already be cleaned up; but in case of an unexpected error 
			//   these still need to be disposed of
			if (_bm != null)
				_bm.Dispose();
			if (_g != null)
				_g.Dispose();
		}

		public Report Report()
		{
			return r;
		}

		public bool IsPagingNeeded()
		{
			return false;
		}

		public void Start()		
		{
			return;
		}

		private Graphics GetGraphics
		{
			get 
			{
				if (_g == null)
				{
					_bm = new Bitmap(10, 10);
					_g = Graphics.FromImage(_bm);
				}
				return _g;
			}
		}

		public void End()
		{
            Stream fs = _sg.GetStream();
            _Excel.WriteExcel(fs);

			if (_g != null)
			{
				_g.Dispose();
				_g = null;
			}
			if (_bm != null)
			{
				_bm.Dispose();
				_bm = null;
			}
			return;
		}

		// Body: main container for the report
		public void BodyStart(Body b)
		{
		}

		public void BodyEnd(Body b)
		{
		}
		
		public void PageHeaderStart(PageHeader ph)
		{
		}

		public void PageHeaderEnd(PageHeader ph)
		{
		}
		
		public void PageFooterStart(PageFooter pf)
		{
		}

		public void PageFooterEnd(PageFooter pf)
		{
		}

		public void Textbox(Textbox tb, string t, Row row)
		{
            if (InTable(tb))
                _Excel.SetCell(_ExcelRow, _ExcelCol, t, GetStyle(tb, row));
            else if (InList(tb))
            {
                _ExcelCol++;
                _Excel.SetCell(_ExcelRow, _ExcelCol, t, GetStyle(tb, row));
            }
        }
        
        private StyleInfo GetStyle(ReportItem ri, Row row)
        {
            if (ri.Style == null)
                return null;

            return ri.Style.GetStyleInfo(r, row);
        }

        private static bool InTable(ReportItem tb)
        {
            Type tp = tb.Parent.Parent.GetType();
            return  (tp == typeof(TableCell) ||
                     tp == typeof(Corner) ||
                     tp == typeof(DynamicColumns) ||
                     tp == typeof(DynamicRows) ||
                     tp == typeof(StaticRow) ||
                     tp == typeof(StaticColumn) ||
                     tp == typeof(Subtotal) ||
                     tp == typeof(MatrixCell));
        }

        private static bool InList(ReportItem tb)
        {
            Type tp = tb.Parent.Parent.GetType();
            return (tp == typeof(List));
        }

        public void DataRegionNoRows(DataRegion d, string noRowsMsg)			// no rows in table
		{
		}

		// Lists
		public bool ListStart(List l, Row r)
		{
            _Excel.AddSheet(l.Name.Nm);
            SheetName = l.Name.Nm;           //keep track of sheet name
            _ExcelRow = -1;

            int ci = 0;
            foreach (ReportItem ri in l.ReportItems)
            {
                if (ri is Textbox)
                {
                    if (ri.Width != null)
                        _Excel.SetColumnWidth(ci, ri.Width.Points);
                    ci++;
                }
            }

            return true;
		}

		public void ListEnd(List l, Row r)
		{
		}

		public void ListEntryBegin(List l, Row r)
		{
            _ExcelRow++;
            _ExcelCol = -1;

            // calc height of tallest Textbox
            float height = float.MinValue;
            foreach (ReportItem ri in l.ReportItems)
            {
                if (ri is Textbox)
                {
                    if (ri.Height != null)
                        height = Math.Max(height, ri.Height.Points); 
                }
            }
            if (height != float.MinValue)
                _Excel.SetRowHeight(_ExcelRow, height);
        }

		public void ListEntryEnd(List l, Row r)
		{
		}

		// Tables					// Report item table
		public bool TableStart(Table t, Row row)
		{
            _Excel.AddSheet(t.Name.Nm);
            SheetName = t.Name.Nm;           //keep track of sheet name

            _ExcelRow = -1;

            for (int ci = 0; ci < t.TableColumns.Items.Count; ci++)
            {
                TableColumn tc = t.TableColumns[ci];

                _Excel.SetColumnWidth(ci, tc.Width.Points);
            }
            return true;
		}

		public bool IsTableSortable(Table t)
		{
            return false;	// can't have tableGroups; must have 1 detail row
		}

		public void TableEnd(Table t, Row row)
		{
            _ExcelRow++;
            return;
		}
 
		public void TableBodyStart(Table t, Row row)
		{
		}

		public void TableBodyEnd(Table t, Row row)
		{
		}

		public void TableFooterStart(Footer f, Row row)
		{
		}

		public void TableFooterEnd(Footer f, Row row)
		{
		}

		public void TableHeaderStart(Header h, Row row)
		{
		}

		public void TableHeaderEnd(Header h, Row row)
		{
		}

		public void TableRowStart(TableRow tr, Row row)
		{
            _ExcelRow++;
            _Excel.SetRowHeight(_ExcelRow, tr.HeightOfRow(r, this.GetGraphics, row));
            _ExcelCol = -1;
		}

		public void TableRowEnd(TableRow tr, Row row)
		{
		}

		public void TableCellStart(TableCell t, Row row)
		{
            _ExcelCol++;
            if (t.ColSpan > 1) {
                _Excel.SetMerge(string.Format("{0}{1}:{2}{3}", (char)('A' + _ExcelCol), _ExcelRow + 1, (char)('A' + _ExcelCol + t.ColSpan - 1), _ExcelRow + 1), SheetName);
            }
            return;
		}

		public void TableCellEnd(TableCell t, Row row)
		{
            // ajm 20062008 need to increase to cover the merged cells, excel still defines every cell
            _ExcelCol += t.ColSpan - 1;
			return;
		}

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)				// called first
		{
            _Excel.AddSheet(m.Name.Nm);
            _ExcelRow = -1;
            // set the widths of the columns 
            float[] widths = m.ColumnWidths(matrix, maxCols);
            for (int i = 0; i < maxCols; i++)
            {
                _Excel.SetColumnWidth(i, widths[i]);
            }
            return true;
		}

		public void MatrixColumns(Matrix m, MatrixColumns mc)	// called just after MatrixStart
		{
		}

		public void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan)
		{
            _ExcelCol++;
        }

        public void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r)
		{
		}

		public void MatrixRowStart(Matrix m, int row, Row r)
		{
            // we handle RowStart when the column is 0 so that we have a ReportItem to figure out the border information
            _ExcelRow++;
            _ExcelCol = -1;
        }

		public void MatrixRowEnd(Matrix m, int row, Row r)
		{
		}

		public void MatrixEnd(Matrix m, Row r)				// called last
		{
            _ExcelRow++;
            return;
		}

		public void Chart(Chart c, Row row, ChartBase cb)
		{
        }
        public void Image(Image i, Row r, string mimeType, Stream ioin)
        {
        }

		public void Line(Line l, Row r)
		{
			return;
		}

		public bool RectangleStart(RDL.Rectangle rect, Row r)
		{
			return true;
		}

		public void RectangleEnd(RDL.Rectangle rect, Row r)
		{
		}

		// Subreport:  
		public void Subreport(Subreport s, Row r)
		{
		}
		public void GroupingStart(Grouping g)			// called at start of grouping
		{
		}
		public void GroupingInstanceStart(Grouping g)	// called at start for each grouping instance
		{
		}
		public void GroupingInstanceEnd(Grouping g)	// called at start for each grouping instance
		{
		}
		public void GroupingEnd(Grouping g)			// called at end of grouping
		{
		}
		public void RunPages(Pages pgs)	// we don't have paging turned on for html
		{
		}
	}
	
}
