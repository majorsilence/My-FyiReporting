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
using System.IO;
using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Presentation: generation of presentation; e.g. html, pdf, xml, ...
	/// </summary>

    //Changed from forum, User: Aulofee http://www.fyireporting.com/forum/viewtopic.php?t=793
    internal interface IPresent : IDisposable 
	{
		// Meta Information: can be called at any time
		bool IsPagingNeeded();						// should report engine perform paging

		// General
		void Start();								// called first
		void End();									// called last

		// 
		void RunPages(Pages pgs);					// only called if IsPagingNeeded - 

		// Body: main container for the report
		void BodyStart(Body b);						// called right before body processing  
		void BodyEnd(Body b);						// called 

		// PageHeader: 
		void PageHeaderStart(PageHeader ph);
		void PageHeaderEnd(PageHeader ph);

		// PageFooter: 
		void PageFooterStart(PageFooter pf);
		void PageFooterEnd(PageFooter pf);

		// ReportItems
		void Textbox(Textbox tb, string t, Row r);	// encountered a textbox
		void DataRegionNoRows(DataRegion d, string noRowsMsg);	// no rows in DataRegion
		
		// Lists
		bool ListStart(List l, Row r);				// called first in list
		void ListEnd(List l, Row r);				// called last in list
		void ListEntryBegin(List l, Row r);			// called to begin each list entry
		void ListEntryEnd(List l, Row r);			// called to end each list entry

		// Tables					// Report item table
		bool TableStart(Table t, Row r);			// called first in table
		void TableEnd(Table t, Row r);				// called last in table
		void TableBodyStart(Table t, Row r);		// table body
		void TableBodyEnd(Table t, Row r);			// 
		void TableFooterStart(Footer f, Row r);		// footer row(s)
		void TableFooterEnd(Footer f, Row r);		// 
		void TableHeaderStart(Header h, Row r);		// header row(s)
		void TableHeaderEnd(Header h, Row r);		// 
		void TableRowStart(TableRow tr, Row r);		// row
		void TableRowEnd(TableRow tr, Row r);		// 
		void TableCellStart(TableCell t, Row r);	// report item will be called after
		void TableCellEnd(TableCell t, Row r);		// report item will be called before

		// Matrix					// Report item matrix
		bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols);				// called first
		void MatrixColumns(Matrix m, MatrixColumns mc);	// called just after MatrixStart
		void MatrixRowStart(Matrix m, int row, Row r);	// row
		void MatrixRowEnd(Matrix m, int row, Row r);	// 
		void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan);
		void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r);
		void MatrixEnd(Matrix m, Row r);				// called last

		// Chart
		void Chart(Chart c, Row r, ChartBase cb);

		// Image
		void Image(Image i, Row r, string mimeType, Stream io);

		// Line
		void Line(Line l, Row r);

		// Rectangle
		bool RectangleStart(Rectangle rect, Row r);				// called before any reportitems
		void RectangleEnd(Rectangle rect, Row r);				// called after any reportitems

		// Subreport
		void Subreport(Subreport s, Row r);

		// Grouping
		void GroupingStart(Grouping g);			// called at start of grouping
		void GroupingInstanceStart(Grouping g);	// called at start for each grouping instance
		void GroupingInstanceEnd(Grouping g);	// called at start for each grouping instance
		void GroupingEnd(Grouping g);			// called at end of grouping

		Report Report();
	}
}
