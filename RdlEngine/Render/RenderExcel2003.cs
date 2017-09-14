using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace fyiReporting.RDL
{

    ///<summary>
    /// Renders a report to HTML.   This handles some page formating but does not do true page formatting.
    ///</summary>
    internal class RenderExcel2003 : RenderExcel
    {
        public RenderExcel2003(Report rep, IStreamGen sg) : base(rep, sg)
        { }


        public override void End()
        {
            base.End();

            Byte[] byteArray = ((MemoryStream)base.StreamGen.GetStream()).ToArray();
            MemoryStream xlsFile = this.ConvertXSLXToXLS(byteArray);

            ((MemoryStreamGen)base.StreamGen).SetStream(xlsFile);
        
            return;
        }

        // Mostly Grok'd from https://stackoverflow.com/questions/29542249/convert-xlsx-file-to-xls-using-npoi-in-c-sharp
        /// <summary>
        /// Converts Excel XML format (2007+) to BIFF format (Excel 1997-2003)
        /// </summary>
        //public MemoryStreamGen ConvertXSLXToXLS(MemoryStreamGen input)
        private MemoryStream ConvertXSLXToXLS(Byte[] byteArray)
        {
            MemoryStream file = new MemoryStream();

            XSSFWorkbook xlsxWb = new XSSFWorkbook(new MemoryStream(byteArray));
            HSSFWorkbook xlsWb = ConvertWorkbookXSSFToHSSF(xlsxWb);
            xlsWb.Write(file);
            return file;
        }

        private static HSSFWorkbook ConvertWorkbookXSSFToHSSF(XSSFWorkbook source)
        {
            //Install-Package NPOI -Version 2.0.6
            HSSFWorkbook retVal = new HSSFWorkbook();
            for (int i = 0; i < source.NumberOfSheets; i++)
            {
                HSSFSheet hssfSheet = (HSSFSheet)retVal.CreateSheet(source.GetSheetAt(i).SheetName);

                XSSFSheet xssfsheet = (XSSFSheet)source.GetSheetAt(i);
                CopySheets(xssfsheet, hssfSheet, retVal);
            }
            return retVal;
        }

        private static void CopySheets(XSSFSheet source, HSSFSheet destination, HSSFWorkbook retVal)
        {
            int maxColumnNum = 0;
            Dictionary<int, XSSFCellStyle> styleMap = new Dictionary<int, XSSFCellStyle>();
            for (int i = source.FirstRowNum; i <= source.LastRowNum; i++)
            {
                XSSFRow srcRow = (XSSFRow)source.GetRow(i);
                HSSFRow destRow = (HSSFRow)destination.CreateRow(i);
                if (srcRow != null)
                {
                    CopyRow(source, destination, srcRow, destRow, styleMap, retVal);
                    if (srcRow.LastCellNum > maxColumnNum)
                    {
                        maxColumnNum = srcRow.LastCellNum;
                    }
                }
            }
            for (int i = 0; i <= maxColumnNum; i++)
            {
                destination.SetColumnWidth(i, source.GetColumnWidth(i));
            }
        }

        private static void CopyRow(XSSFSheet srcSheet, HSSFSheet destSheet, XSSFRow srcRow, HSSFRow destRow,
                Dictionary<int, XSSFCellStyle> styleMap, HSSFWorkbook retVal)
        {
            // manage a list of merged zone in order to not insert two times a
            // merged zone
            List<CellRangeAddress> mergedRegions = new List<CellRangeAddress>();
            destRow.Height = srcRow.Height;
            // pour chaque row
            for (int j = srcRow.FirstCellNum; j <= srcRow.LastCellNum; j++)
            {
                XSSFCell oldCell = (XSSFCell)srcRow.GetCell(j); // ancienne cell
                HSSFCell newCell = (HSSFCell)destRow.GetCell(j); // new cell
                if (oldCell != null)
                {
                    if (newCell == null)
                    {
                        newCell = (HSSFCell)destRow.CreateCell(j);
                    }
                    // copy chaque cell
                    CopyCell(oldCell, newCell, styleMap, retVal);
                    // copy les informations de fusion entre les cellules
                    CellRangeAddress mergedRegion = GetMergedRegion(srcSheet, srcRow.RowNum,
                            (short)oldCell.ColumnIndex);

                    if (mergedRegion != null)
                    {
                        CellRangeAddress newMergedRegion = new CellRangeAddress(mergedRegion.FirstRow,
                                mergedRegion.LastRow, mergedRegion.FirstColumn, mergedRegion.LastColumn);
                        if (IsNewMergedRegion(newMergedRegion, mergedRegions))
                        {
                            mergedRegions.Add(newMergedRegion);
                            destSheet.AddMergedRegion(newMergedRegion);
                        }

                        if (newMergedRegion.FirstColumn == 0 && newMergedRegion.LastColumn == 6 && newMergedRegion.FirstRow == newMergedRegion.LastRow)
                        {
                            HSSFCellStyle style2 = (HSSFCellStyle)retVal.CreateCellStyle();
                            style2.VerticalAlignment = VerticalAlignment.Center;
                            style2.Alignment = HorizontalAlignment.Left;
                            style2.FillForegroundColor = HSSFColor.Teal.Index;
                            style2.FillPattern = FillPattern.SolidForeground;

                            for (int i = destRow.FirstCellNum; i <= destRow.LastCellNum; i++)
                            {
                                if (destRow.GetCell(i) != null)
                                    destRow.GetCell(i).CellStyle = style2;
                            }
                        }
                    }
                }
            }



        }

        private static void CopyCell(XSSFCell oldCell, HSSFCell newCell, Dictionary<int, XSSFCellStyle> styleMap, HSSFWorkbook retVal)
        {
            if (styleMap != null)
            {
                int stHashCode = oldCell.CellStyle.Index;
                XSSFCellStyle sourceCellStyle = null;
                if (styleMap.TryGetValue(stHashCode, out sourceCellStyle)) { }

                HSSFCellStyle destnCellStyle = (HSSFCellStyle)newCell.CellStyle;
                if (sourceCellStyle == null)
                {
                    sourceCellStyle = (XSSFCellStyle)oldCell.Sheet.Workbook.CreateCellStyle();
                }
                // destnCellStyle.CloneStyleFrom(oldCell.CellStyle);
                if (!styleMap.Any(p => p.Key == stHashCode))
                {
                    styleMap.Add(stHashCode, sourceCellStyle);
                }

                destnCellStyle.VerticalAlignment = VerticalAlignment.Top;
                newCell.CellStyle = (HSSFCellStyle)destnCellStyle;
            }
            switch (oldCell.CellType)
            {
                case CellType.String:
                    newCell.SetCellValue(oldCell.StringCellValue);
                    break;
                case CellType.Numeric:
                    newCell.SetCellValue(oldCell.NumericCellValue);
                    break;
                case CellType.Blank:
                    newCell.SetCellType(CellType.Blank);
                    break;
                case CellType.Boolean:
                    newCell.SetCellValue(oldCell.BooleanCellValue);
                    break;
                case CellType.Error:
                    newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                    break;
                case CellType.Formula:
                    newCell.SetCellFormula(oldCell.CellFormula);
                    break;
                default:
                    break;
            }

        }


        private static CellRangeAddress GetMergedRegion(XSSFSheet sheet, int rowNum, short cellNum)
        {
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress merged = sheet.GetMergedRegion(i);
                if (merged.IsInRange(rowNum, cellNum))
                {
                    return merged;
                }
            }
            return null;
        }

        private static bool IsNewMergedRegion(CellRangeAddress newMergedRegion,
                List<CellRangeAddress> mergedRegions)
        {
            return !mergedRegions.Contains(newMergedRegion);
        }
    }


}