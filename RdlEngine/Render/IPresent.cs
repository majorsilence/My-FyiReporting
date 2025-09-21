
using System;
using System.IO;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;


namespace Majorsilence.Reporting.Rdl
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
		Task End();									// called last

		// 
		Task RunPages(Pages pgs);					// only called if IsPagingNeeded - 

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
		Task Textbox(Textbox tb, string t, Row r);	// encountered a textbox
		Task DataRegionNoRows(DataRegion d, string noRowsMsg);	// no rows in DataRegion
		
		// Lists
		Task<bool> ListStart(List l, Row r);				// called first in list
		Task ListEnd(List l, Row r);				// called last in list
		Task ListEntryBegin(List l, Row r);			// called to begin each list entry
		void ListEntryEnd(List l, Row r);			// called to end each list entry

		// Tables					// Report item table
		Task<bool> TableStart(Table t, Row r);			// called first in table
		Task TableEnd(Table t, Row r);				// called last in table
		void TableBodyStart(Table t, Row r);		// table body
		void TableBodyEnd(Table t, Row r);			// 
		void TableFooterStart(Footer f, Row r);		// footer row(s)
		void TableFooterEnd(Footer f, Row r);		// 
		void TableHeaderStart(Header h, Row r);		// header row(s)
		void TableHeaderEnd(Header h, Row r);		// 
		Task TableRowStart(TableRow tr, Row r);		// row
		void TableRowEnd(TableRow tr, Row r);		// 
		Task TableCellStart(TableCell t, Row r);	// report item will be called after
		void TableCellEnd(TableCell t, Row r);		// report item will be called before

		// Matrix					// Report item matrix
		Task<bool> MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols);				// called first
		void MatrixColumns(Matrix m, MatrixColumns mc);	// called just after MatrixStart
		void MatrixRowStart(Matrix m, int row, Row r);	// row
		void MatrixRowEnd(Matrix m, int row, Row r);	// 
		Task MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan);
		Task MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r);
		Task MatrixEnd(Matrix m, Row r);				// called last

		// Chart
		Task Chart(Chart c, Row r, ChartBase cb);

		// Image
		Task Image(Image i, Row r, string mimeType, Stream io);

		// Line
		Task Line(Line l, Row r);

		// Rectangle
		Task<bool> RectangleStart(Rectangle rect, Row r);				// called before any reportitems
		Task RectangleEnd(Rectangle rect, Row r);				// called after any reportitems

		// Subreport
		Task Subreport(Subreport s, Row r);

		// Grouping
		void GroupingStart(Grouping g);			// called at start of grouping
		void GroupingInstanceStart(Grouping g);	// called at start for each grouping instance
		void GroupingInstanceEnd(Grouping g);	// called at start for each grouping instance
		void GroupingEnd(Grouping g);			// called at end of grouping

		Report Report();
	}
}
