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

using ClosedXML.Excel;
using RdlEngine.Render.ExcelConverter;
using System;
using System.IO;

namespace fyiReporting.RDL
{
    ///<summary>
    /// Renders a report to Excel 2007.   This handles some page formating but does not do true page formatting.
    ///</summary>
    internal class RenderExcel2007 : IPresent
    {
        Report report;                   // report
        IStreamGen _sg;             // stream generater

        ExcelCellsBuilder excelBuilder = new ExcelCellsBuilder();
        XLWorkbook workbook;
        IXLWorksheet worksheet;

        double k = 4.8;

        public RenderExcel2007(Report rep, IStreamGen sg)
        {
            report = rep;
            _sg = sg;

            excelBuilder.Report = report;

            workbook = new XLWorkbook();
            string sheetName = string.IsNullOrEmpty(rep.Name) ? "Report" : rep.Name;
            // Excel sheet name length limit
            if (sheetName.Length > 31) sheetName = sheetName.Substring(0, 31);

            worksheet = workbook.Worksheets.Add(sheetName);

            var pageSetup = worksheet.PageSetup;
            pageSetup.PaperSize = XLPaperSize.A4Paper;
            pageSetup.PageOrientation = XLPageOrientation.Landscape;

            // 1 point = 1/72 inch
            pageSetup.Margins.Left = rep.LeftMarginPoints / 72.0;
            pageSetup.Margins.Top = rep.TopMarginPoints / 72.0;
            pageSetup.Margins.Right = rep.RightMarginPoints / 72.0;
            pageSetup.Margins.Bottom = rep.BottomMarginPoints / 72.0;
        }

        protected IStreamGen StreamGen { get => _sg; set => _sg = value; }

        public void Dispose()
        {
            if (workbook != null)
                workbook.Dispose();
        }

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

        public virtual void End()
        {
            excelBuilder.CellsCorrection();
            for (int i = 0; i < excelBuilder.Columns.Count - 1; i++)
            {
                var widthInPoints = excelBuilder.Columns[i + 1].XPosition - excelBuilder.Columns[i].XPosition;
                var widthInSymbols = widthInPoints / k;
                if (widthInSymbols > 255) widthInSymbols = 255; // overflow protection

                worksheet.Column(i + 1).Width = widthInSymbols;
            }

            for (int i = 0; i < excelBuilder.Rows.Count - 1; i++)
            {
                var builderRow = excelBuilder.Rows[i];
                var nextRowY = excelBuilder.Rows[i + 1].YPosition;
                var heightInPoints = nextRowY - builderRow.YPosition;

                if (heightInPoints > 0)
                    worksheet.Row(i + 1).Height = Math.Min(heightInPoints, 409);
            }

            for (int i = 0; i < excelBuilder.Rows.Count - 1; i++)
            {
                var builderRow = excelBuilder.Rows[i];
                int rowIndex = i + 1; //ClosedXML 1-based

                for (int j = 0; j < builderRow.Cells.Count; j++)
                {
                    var builderCell = builderRow.Cells[j];
                    var columnIndex = excelBuilder.Columns.IndexOf(builderCell.Column);
                    if (columnIndex < 0) continue;
                    var exelColumnIndex = columnIndex + 1;
                    var cell = worksheet.Cell(rowIndex, exelColumnIndex);
                    if (builderCell.ReportItem != null)
                    {
                        SetValue(cell, builderCell.Value);

                        if (builderCell.Style != null)
                        {
                            ExcelCellStyle.ApplyStyle(cell, builderCell.Style);
                        }

                        var rightAttach = excelBuilder.GetRightAttachCells(builderCell);
                        var bottomAttach = excelBuilder.GetBottomAttachCells(builderCell);

                        if (rightAttach > 0 || bottomAttach > 0)
                        {
                            var lastRow = rowIndex + bottomAttach;
                            var lastCol = exelColumnIndex + rightAttach;

                            var range = worksheet.Range(rowIndex, exelColumnIndex, lastRow, lastCol);
                            range.Merge();

                            if (builderCell.Style != null)
                            {
                                ExcelCellStyle.ApplyBorderToRange(range, builderCell.Style);
                            }
                        }
                    }
                }
            }


            //Create images
            foreach (var image in excelBuilder.Images)
            {
                if (image.ImageStream == null) continue;

                const double pointsToPixels = 96.0 / 72.0;

                var pic = worksheet.AddPicture(image.ImageStream);

                var colIndex = excelBuilder.Columns.FindIndex(c => c.XPosition >= image.AbsoluteLeft) + 1;
                var rowIndex = excelBuilder.Rows.FindIndex(r => r.YPosition >= image.AbsoluteTop) + 1;

                if (colIndex < 1) colIndex = 1;
                if (rowIndex < 1) rowIndex = 1;

                pic.MoveTo(worksheet.Cell(rowIndex, colIndex));

                pic.Width = (int)(image.ImageWidth * pointsToPixels);
                pic.Height = (int)(image.ImageHeight * pointsToPixels);

                image.ImageStream.Dispose();

            }

            //Create lines
            foreach (var line in excelBuilder.Lines)
            {
                float width = Math.Abs(line.Right - line.AbsoluteLeft);
                float height = Math.Abs(line.Bottom - line.AbsoluteTop);
                float pointsHeight = Math.Abs(line.Line.Height.Points);
                float pointsWidth = Math.Abs(line.Line.Width.Points);
                // ClosedXML can't handle them, only via borders
                bool isHorizontal = pointsHeight < 0.01 || width < 1.0 || pointsHeight < pointsWidth;
                bool isVertical = pointsWidth < 0.01 || height < 1.0 || pointsHeight > pointsWidth;

                var colIndex = excelBuilder.Columns.FindIndex(c => c.XPosition >= line.AbsoluteLeft) + 1;
                var rowIndex = excelBuilder.Rows.FindIndex(r => r.YPosition >= line.AbsoluteTop) + 1;

                if (colIndex < 1 || rowIndex < 1) continue;

                var borderStyle = XLBorderStyleValues.Thin;
                if (line.BorderWidth > 1.5) borderStyle = XLBorderStyleValues.Medium;
                if (line.BorderWidth > 2.5) borderStyle = XLBorderStyleValues.Thick;

                var cell = worksheet.Cell(rowIndex, colIndex);

                if (isHorizontal)
                {
                    var endColIndex = excelBuilder.Columns.FindIndex(c => c.XPosition >= line.Right) + 1;
                    if (endColIndex < colIndex) endColIndex = colIndex;

                    var range = worksheet.Range(rowIndex, colIndex, rowIndex, endColIndex - 1);
                    range.Style.Border.TopBorder = borderStyle;
                    range.Style.Border.TopBorderColor = XLColor.Black;
                }
                else if (isVertical)
                {
                    var endRowIndex = excelBuilder.Rows.FindIndex(r => r.YPosition >= line.Bottom) + 1;
                    if (endRowIndex < rowIndex) endRowIndex = rowIndex;

                    var range = worksheet.Range(rowIndex, colIndex, endRowIndex - 1, colIndex);
                    range.Style.Border.LeftBorder = borderStyle;
                    range.Style.Border.LeftBorderColor = XLColor.Black;
                }
            }

            workbook.SaveAs(_sg.GetStream());
            return;
        }
        private void SetValue(IXLCell cell, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (value.StartsWith("0") && value.Length > 1 && double.TryParse(value, out _))
            {
                cell.Value = value;
                //cell.DataType = XLDataType.Text; // Принудительно текст
            }
            else if (double.TryParse(value, out double dVal))
            {
                cell.Value = dVal;
            }
            else if (DateTime.TryParse(value, out DateTime dtVal))
            {
                cell.Value = dtVal;
            }
            else
            {
                cell.Value = value;
            }
            cell.Style.Alignment.WrapText = true;
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
            if (!tb.IsHidden(report, row))
            {
                excelBuilder.AddTextbox(tb, t, row);
            }
        }

        public void DataRegionNoRows(DataRegion d, string noRowsMsg)            // no rows in table
        {
        }

        // Lists
        public bool ListStart(List l, Row r)
        {
            return true;
        }

        public void ListEnd(List l, Row r)
        {
        }

        public void ListEntryBegin(List l, Row r)
        {
        }

        public void ListEntryEnd(List l, Row r)
        {
        }

        // Tables					// Report item table
        public bool TableStart(Table t, Row row)
        {
            if (t.Visibility == null || (t.Visibility != null && !t.Visibility.IsHidden(report, row)))
            {
                excelBuilder.AddTable(t);
                return true;
            }
            return false;
        }

        public bool IsTableSortable(Table t)
        {
            return false;   // can't have tableGroups; must have 1 detail row
        }

        public void TableEnd(Table t, Row row)
        {
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
            excelBuilder.AddRow(tr, row);
        }

        public void TableRowEnd(TableRow tr, Row row)
        {
        }

        public void TableCellStart(TableCell t, Row row)
        {
            return;
        }

        public void TableCellEnd(TableCell t, Row row)
        {
            return;
        }

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)               // called first
        {
            return true;
        }

        public void MatrixColumns(Matrix m, MatrixColumns mc)   // called just after MatrixStart
        {
        }

        public void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan)
        {
        }

        public void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r)
        {
        }

        public void MatrixRowStart(Matrix m, int row, Row r)
        {
        }

        public void MatrixRowEnd(Matrix m, int row, Row r)
        {
        }

        public void MatrixEnd(Matrix m, Row r)              // called last
        {
            return;
        }

        public void Chart(Chart c, Row row, ChartBase cb)
        {
        }
        public void Image(Image i, Row r, string mimeType, Stream ioin)
        {
            if (i.Visibility != null && i.Visibility.IsHidden(report, r))
                return;

            if (ioin == null) return;

            MemoryStream ms = new MemoryStream();
            ioin.CopyTo(ms);
            ms.Position = 0;

            ExcelImage excelImageObj = excelBuilder.AddImage(i, 0);

            excelImageObj.ImageStream = ms;
        }

        public void Line(Line l, Row row)
        {
            float borderWidth = 1;
            if (l.Style.BorderWidth != null)
            {
                borderWidth = l.Style.BorderWidth.EvalDefault(report, row);
            }
            else if (l.Style.BorderStyle != null)
            {
                borderWidth = (float)l.Style.BorderStyle.EvalDefault(report, row);
            }
            excelBuilder.AddLine(l, borderWidth);

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
        public void GroupingStart(Grouping g)           // called at start of grouping
        {
        }
        public void GroupingInstanceStart(Grouping g)   // called at start for each grouping instance
        {
        }
        public void GroupingInstanceEnd(Grouping g) // called at start for each grouping instance
        {
        }
        public void GroupingEnd(Grouping g)         // called at end of grouping
        {
        }
        public void RunPages(Pages pgs) // we don't have paging turned on for html
        {
        }
    }
}
