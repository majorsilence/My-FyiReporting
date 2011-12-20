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
using fyiReporting.RDL;
using System.IO;
using System.Collections;
using System.Text;

namespace fyiReporting.RDL
{

    ///<summary>
    ///The primary class to "run" a report to XML
    ///</summary>
    internal class RenderCsv : IPresent
    {
        Report report;					// report
        DelimitedTextWriter tw;				// where the output is going

        public RenderCsv(Report report, IStreamGen sg)
        {
            this.report = report;
            tw = new DelimitedTextWriter(sg.GetTextWriter(), ",");
        }

        public void Dispose() { } 

        public Report Report()
        {
            return report;
        }

        public bool IsPagingNeeded()
        {
            return false;
        }

        public void Start()
        {
        }

        public void End()
        {

        }									

        public void RunPages(Pages pgs)
        {
        }					

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
            if (ph.PrintOnFirstPage||ph.PrintOnLastPage) {tw.WriteLine();}           
        }

        public void PageFooterStart(PageFooter pf)
        {
        }

        public void PageFooterEnd(PageFooter pf)
        {
            if (pf.PrintOnLastPage || pf.PrintOnFirstPage) {tw.WriteLine();}
        }

        public void Textbox(Textbox tb, string t, Row r)
        {
            object value = tb.Evaluate(report, r);
            tw.Write(value);
        }	
        
        public void DataRegionNoRows(DataRegion d, string noRowsMsg)
        {
        }

        public bool ListStart(List l, Row r)
        {
            return true;
        }
        
        public void ListEnd(List l, Row r)
        {
            tw.WriteLine();
        }
        
        public void ListEntryBegin(List l, Row r)
        {
        }
        public void ListEntryEnd(List l, Row r)
        {
            tw.WriteLine();
        }

        public bool TableStart(Table t, Row r)
        {
            return true;
        }
        
        public void TableEnd(Table t, Row r)
        {
        }
        
        public void TableBodyStart(Table t, Row r)
        {
        }
        
        public void TableBodyEnd(Table t, Row r)
        {
        }
        
        public void TableFooterStart(Footer f, Row r)
        {
        }
        
        public void TableFooterEnd(Footer f, Row r)
        {
        }
        
        public void TableHeaderStart(Header h, Row r)
        {
        }
        
        public void TableHeaderEnd(Header h, Row r)
        {
        }
        
        public void TableRowStart(TableRow tr, Row r)
        {
        }
        
        public void TableRowEnd(TableRow tr, Row r)
        {
            tw.WriteLine();
        }
        
        public void TableCellStart(TableCell t, Row r)
        {
        }
        
        public void TableCellEnd(TableCell t, Row r)
        {
        }

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)
        {
            return true;
        }
        
        public void MatrixColumns(Matrix m, MatrixColumns mc)
        {
        }

        public void MatrixRowStart(Matrix m, int row, Row r)
        {
        }
        
        public void MatrixRowEnd(Matrix m, int row, Row r)
        {
            tw.WriteLine();
        }
        
        public void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan)
        {
        }

        public void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r)
        {
        }

        public void MatrixEnd(Matrix m, Row r)
        {
        }
        
        public void Chart(Chart c, Row r, ChartBase cb)
        {
        }

        public void Image(Image i, Row r, string mimeType, Stream io)
        {
        }

        public void Line(Line l, Row r)
        {
        }

        public bool RectangleStart(Rectangle rect, Row r)
        {
            return true;
        }
        
        public void RectangleEnd(Rectangle rect, Row r)
        {
        }	
        
        public void Subreport(Subreport s, Row r)
        {
        }

        public void GroupingStart(Grouping g)
        {
        }
        
        public void GroupingInstanceStart(Grouping g)
        {
        }
        
        public void GroupingInstanceEnd(Grouping g)
        {
        }
        
        public void GroupingEnd(Grouping g)
        {
        }
    }
}
