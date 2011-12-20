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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Globalization;

//using ICSharpCode.SharpZipLib.Zip;

namespace fyiReporting.RDL
{
    internal class ExcelValet
    {

        //  Excel file structure is a zip file with the following files and directories
        //dir      _rels
        //file          .rels
        //dir      docProps
        //file          app.xml
        //file          core.xml
        //dir      xl
        //file          sharedStrings.xml      -- Excel keeps only one copy of each string
        //file          styles.xml
        //file          workbook.xml
        //dir           _rels
        //file              workbook.xml.rels
        //dir           theme
        //file              theme1.xml
        //dir           worksheets
        //file              sheet1.xml
        //file              sheet2.xml
        //file              sheet3.xml
        //file     [Content_Types].xml

        static readonly string RELS_RELS =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
            "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
            "<Relationship Id=\"rId3\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties\" " +
            "Target=\"docProps/app.xml\"/>" +
            "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties\" " +
            "Target=\"docProps/core.xml\"/>" +
            "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" " +
            "Target=\"xl/workbook.xml\"/></Relationships>";
        static readonly string CONTENT_TYPES =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
        "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
        "<Override PartName=\"/xl/theme/theme1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.theme+xml\"/>" +
        "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
        "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
        "<Default Extension=\"xml\" ContentType=\"application/xml\"/><Override PartName=\"/xl/workbook.xml\" " +
        "ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
        "<Override PartName=\"/docProps/app.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.extended-properties+xml\"/>" +
        "<Override PartName=\"/xl/worksheets/sheet2.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
        "<Override PartName=\"/xl/worksheets/sheet3.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
        "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
        "<Override PartName=\"/xl/sharedStrings.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml\"/>" +
        "<Override PartName=\"/docProps/core.xml\" ContentType=\"application/vnd.openxmlformats-package.core-properties+xml\"/>" +
        "</Types>";
//
        static readonly string APPXML =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
        "<Properties xmlns=\"http://schemas.openxmlformats.org/officeDocument/2006/extended-properties\" " +
        "xmlns:vt=\"http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes\">" +
        "<Application>Microsoft Excel</Application><DocSecurity>0</DocSecurity><ScaleCrop>false</ScaleCrop>" +
        "<HeadingPairs><vt:vector size=\"2\" baseType=\"variant\"><vt:variant><vt:lpstr>Worksheets</vt:lpstr></vt:variant><vt:variant>" +
        "<vt:i4>3</vt:i4></vt:variant></vt:vector></HeadingPairs>" +
        "<TitlesOfParts><vt:vector size=\"3\" baseType=\"lpstr\"><vt:lpstr>Sheet1</vt:lpstr><vt:lpstr>Sheet2</vt:lpstr>" +
        "<vt:lpstr>Sheet3</vt:lpstr></vt:vector></TitlesOfParts>" +
        "<Company></Company><LinksUpToDate>false</LinksUpToDate><SharedDoc>false</SharedDoc>" +
        "<HyperlinksChanged>false</HyperlinksChanged><AppVersion>12.0000</AppVersion></Properties>";

        static readonly string COREXML =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
        "<cp:coreProperties xmlns:cp=\"http://schemas.openxmlformats.org/package/2006/metadata/core-properties\" " +
        "xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:dcterms=\"http://purl.org/dc/terms/\" " +
        "xmlns:dcmitype=\"http://purl.org/dc/dcmitype/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
        "<dc:creator></dc:creator><cp:lastModifiedBy></cp:lastModifiedBy>" +
        "<dcterms:created xsi:type=\"dcterms:W3CDTF\">2006-09-16T00:00:00Z</dcterms:created>" +
            "<dcterms:modified xsi:type=\"dcterms:W3CDTF\">2006-12-21T16:02:39Z</dcterms:modified>" +
        "</cp:coreProperties>";
        StringCache _StringCache;                               // holds cache of data strings
        StringCache _BorderCache;                               // holds cache of border strings
        StringCache _FontCache;                                 // holds cache of font strings
        StringCache _FillCache;                                 // holds cache of fill strings
        StringCache _MergeCells;                                // AJM GJL 130608 Merge Cells
        StringCache _StyleCache;                                // holds cache of Style strings
        StringCache _StyleXfsCache;                             //  Excel has two levels for styles
        List<SheetInfo> _Sheets;                                // Sheets in the spread sheet
        SparseMatrix _Grid;                                     //  the current grid
        StyleInfo _DefaultStyle;                                // Default styles

        internal ExcelValet()
        {
            _Sheets = new List<SheetInfo>();
            _Grid = null;
            _StringCache = new StringCache();
            _BorderCache = new StringCache();
            _FontCache = new StringCache();
            _FillCache = new StringCache();
            // work around so that the first 2 fill caches matches what Excel chooses for them
            _FillCache.GetIndex("<fill><patternFill patternType=\"none\"/></fill>"); //index 0 
            _FillCache.GetIndex("<fill><patternFill patternType=\"gray125\"/></fill>"); //index 1 

            //AJM GJL 130608 - Merge Cells
            _MergeCells = new StringCache();
            _StyleCache = new StringCache();
            _StyleXfsCache = new StringCache();
            _DefaultStyle = new StyleInfo();                    // use this when no style is specified

            GetStyleIndex(_DefaultStyle);                       // populates the default style entries as 0 
            ZipWrap.Init();                                     // intialize the zip utility (doesn't hurt to do if already done)
        }

        internal void AddSheet(string name)
        {
            SheetInfo si = new SheetInfo(name);
            _Sheets.Add(si);                                    // add the latest sheet
            _Grid = si.Grid;                                    // set the current grid
        }
        /// <summary>
        /// Set the value of a cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="val"></param>
        /// <param name="si"></param>
        /// Row 0 is reserved to hold the column width
        /// Col 0 is reserved to hold the row height
        internal void SetCell(int row, int col, string val, StyleInfo si)
        {
            if (_Grid == null)                                  // if no sheet yet, create one
                AddSheet("Sheet1");

            int gval = _StringCache.GetIndex(val);
            
            if (si == null)
                si = _DefaultStyle;

            _Grid[row+1, col+1] = new CellData(gval, GetStyleIndex(si));    
        }

        // AJM GJL 130608 Cell Merge
        public void SetMerge(String merge, string x)
        {
            _MergeCells.GetIndex(merge);
        }
        internal void SetColumnWidth(int col, float w)
        {
            if (_Grid == null)                                  // if no sheet yet, create one
                AddSheet("Sheet1");
            // convert points to Excel units: characters 
            //   Assume 11 characters to the inch
            float eu = (float) (w / 72f) * 11;               // 
                
            _Grid[0, col + 1] = new CellData(eu, -1);
        }
        internal void SetRowHeight(int row, float h)
        {
            if (_Grid == null)                                  // if no sheet yet, create one
                AddSheet("Sheet1");
            _Grid[row + 1, 0] = new CellData(h, -1);
        }

        int GetStyleIndex(StyleInfo si)
        {
            int fi = GetFontIndex(si);
            int filli = GetFillIndex(si);
            int bi = GetBorderIndex(si);
            // Do the cell style xfs first (because the xfid is needed in the cell style
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<xf numFmtId=\"0\" fontId=\"{0}\" fillId=\"{1}\" borderId=\"{2}\"" + 
                " applyFont=\"1\" applyAlignment=\"1\" applyFill=\"1\">", fi, filli, bi);
            sb.AppendFormat("<alignment horizontal=\"{0}\" wrapText=\"1\"/>", GetAlignment(si.TextAlign));
            sb.Append("</xf>");
            int cfxi = _StyleXfsCache.GetIndex(sb.ToString());

            sb.Length = 0;          // reset and build another one
            sb.AppendFormat("<xf numFmtId=\"0\" fontId=\"{0}\" fillId=\"{1}\" borderId=\"{2}\" xfId=\"{3}\"" + 
                " applyFont=\"1\" applyAlignment=\"1\" applyFill=\"1\">", fi, filli, bi, cfxi);
            sb.AppendFormat("<alignment horizontal=\"{0}\" wrapText=\"1\"/>", GetAlignment(si.TextAlign));
            sb.Append("</xf>");
            int i = _StyleCache.GetIndex(sb.ToString());
            return i;
        }

        string GetAlignment(TextAlignEnum ta)
        {
            switch (ta)
            {
                case TextAlignEnum.Center: return "center";
                case TextAlignEnum.Right: return "right";
                case TextAlignEnum.Left: return "left";
                default: return "general";
            }
        }

        int GetFillIndex(StyleInfo si)
        {
            string s;
            if (si.BackgroundColor.IsEmpty)
                s = "<fill><patternFill patternType=\"none\"/></fill>";
            else
            {//<fill><patternFill patternType="solid"><fgColor rgb="FFFFFF00"/><bgColor rgb="FFFFFF00"/></patternFill></fill>
                s = string.Format("<fill><patternFill patternType=\"solid\">{0}{1}</patternFill></fill>",
                    GetColor("fgColor", si.BackgroundColor), GetColor("bgColor", si.BackgroundColor));

            }
            int i = _FillCache.GetIndex(s);
            return i;
        }
        int GetFontIndex(StyleInfo si)
        {
            StringBuilder sb = new StringBuilder(150);
            sb.Append("<font>");
            sb.Append(GetColor(si.Color));
            sb.AppendFormat("<sz val=\"{0}\"/> ", si.FontSize);
            sb.AppendFormat("<name val=\"{0}\"/> ", si.FontFamily);
            if (si.IsFontBold())
                sb.Append("<b /> ");
            if (si.FontStyle == FontStyleEnum.Italic)
                sb.Append("<i /> ");
            sb.Append("</font>");
            int i = _FontCache.GetIndex(sb.ToString());
            return i;
        }

        string GetColor(Color c)
        {
            return GetColor("color", c);
        }

        string GetColor(string name, Color c)
        {
            string s = string.Format("<{0} rgb=\"FF{1}{2}{3}\"/> ", name, GetColor(c.R), GetColor(c.G), GetColor(c.B));
            return s;
        }

        string GetColor(byte b)
        {
            string sb = Convert.ToString(b, 16).ToUpperInvariant();
            
            return sb.Length > 1 ? sb : "0" + sb;
        }

        int GetBorderIndex(StyleInfo si)
        {
            StringBuilder sb = new StringBuilder(150);
            sb.Append("<border>");
            if (si.BStyleLeft == BorderStyleEnum.None &&
                si.BStyleRight == BorderStyleEnum.None &&
                si.BStyleBottom == BorderStyleEnum.None &&
                si.BStyleTop == BorderStyleEnum.None)
                sb.Append("<left /> <right /> <top /> <bottom /> <diagonal/>");
            else
            {
                sb.AppendFormat("<left style=\"{0}\">{1}</left>\r\n<right style=\"{2}\">{3}</right>" +
                        "\r\n<top style=\"{4}\">{5}</top>\r\n<bottom style=\"{6}\">{7}</bottom>\r\n<diagonal/>",
                    GetBorderName(si.BStyleLeft, si.BWidthLeft), GetColor(si.BColorLeft),
                    GetBorderName(si.BStyleRight, si.BWidthRight), GetColor(si.BColorRight),
                    GetBorderName(si.BStyleTop, si.BWidthTop), GetColor(si.BColorTop),
                    GetBorderName(si.BStyleBottom, si.BWidthBottom), GetColor(si.BColorBottom));
            }
            sb.Append("</border>");
//<w:top w:val="dashed" w:sz="24" …/>
            int i = _BorderCache.GetIndex(sb.ToString());
            return i;
        }

        string GetBorderName(BorderStyleEnum bs, float width)
        {
            string s;
            switch (bs)
            {
                case BorderStyleEnum.Dashed:
                    s = width < 1.2f? "dashed": "mediumDashed";
                    break;
                case BorderStyleEnum.Dotted:
                    s = "dotted";
                    break;
                case BorderStyleEnum.Double:
                    s = "double";
                    break;
                case BorderStyleEnum.None:
                    s = "none";
                    break;
                case BorderStyleEnum.Outset:
                case BorderStyleEnum.Inset:
                case BorderStyleEnum.Groove:
                case BorderStyleEnum.Ridge:
                case BorderStyleEnum.WindowInset:
                case BorderStyleEnum.Solid:
                default:
                    s = width < 1.2f? "thin": "medium";
                    break;
            }
            return s;
        }
        internal void WriteExcel(Stream str)
        {
            ZipOutputStream zip = null;
            try
            {
                zip = new ZipOutputStream(str);
                ZipEntry ze = new ZipEntry("[Content_Types].xml");
                zip.PutNextEntry(ze);
                zip.Write(CONTENT_TYPES);
                // docProps directory
                ze = new ZipEntry("docProps/");
                zip.PutNextEntry(ze);
                ze = new ZipEntry("docProps/app.xml");
                zip.PutNextEntry(ze);
                zip.Write(APPXML);
                ze = new ZipEntry("docProps/core.xml");
                zip.PutNextEntry(ze);
                zip.Write(COREXML);
        
                // xl directory
                ze = new ZipEntry("xl/");
                zip.PutNextEntry(ze);
                ze = new ZipEntry("xl/sharedStrings.xml");
                zip.PutNextEntry(ze);
                WriteStringCache(zip);
                ze = new ZipEntry("xl/styles.xml");
                zip.PutNextEntry(ze);
                WriteStyles(zip);
                ze = new ZipEntry("xl/workbook.xml");
                zip.PutNextEntry(ze);
                WriteWorkbook(zip);

                // xl/theme
                ze = new ZipEntry("xl/theme/");
                zip.PutNextEntry(ze);
                ze = new ZipEntry("xl/theme/theme1.xml");
                zip.PutNextEntry(ze);
                WriteTheme(zip);

                // xl/worksheets
                ze = new ZipEntry("xl/worksheets/");
                zip.PutNextEntry(ze);

                if (_Sheets.Count == 0)
                {   // output an empty work sheet
                    ze = new ZipEntry("xl/worksheets/sheet1.xml");
                    zip.PutNextEntry(ze);
                    WriteEmptyWorksheet(zip);
                }
                else
                {
                    // output the spreadsheets
                    foreach (SheetInfo sinfo in _Sheets)
                    {
                        string sname = string.Format("xl/worksheets/{0}.xml", sinfo.Name);
                        ze = new ZipEntry(sname);
                        zip.PutNextEntry(ze);
                        WriteData(zip, sinfo.Grid);                     // here's where the meat of the data goes
                    }
                }
                // xl/_rels
                ze = new ZipEntry("xl/_rels/");
                zip.PutNextEntry(ze);
                ze = new ZipEntry("xl/_rels/workbook.xml.rels");
                zip.PutNextEntry(ze);
                WriteWorkbookRels(zip);

                // _rels directory
                ze = new ZipEntry("_rels/");
                zip.PutNextEntry(ze);
                ze = new ZipEntry("_rels/.rels");
                zip.PutNextEntry(ze);
                zip.Write(RELS_RELS);

            }
            finally
            {
                if (zip != null)
                {
                    zip.Finish();
                }

            }
        }

        void WriteStringCache(ZipOutputStream zip)
        {
            MemoryStream ms = new MemoryStream();

            using (XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8))
            {
                xtw.WriteStartDocument(true);

                xtw.WriteStartElement("sst");
                xtw.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                int sCount = _StringCache.Count;
                xtw.WriteAttributeString("count", sCount.ToString());
                xtw.WriteAttributeString("uniqueCount", sCount.ToString());

                foreach (string s in _StringCache)
                {
                    xtw.WriteStartElement("si");
                    xtw.WriteElementString("t", s);
                    xtw.WriteEndElement();
                }

                xtw.WriteEndElement();
                xtw.Flush();
                byte[] ba = ms.ToArray();
                zip.Write(ba, 0, ba.Length);
            }
        }

        void WriteStyles(ZipOutputStream zip)
        {
            StringBuilder sb = new StringBuilder(500);

            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
                "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" );

            // output the font list
            sb.AppendFormat("<fonts count=\"{0}\">", _FontCache.Count);
            foreach (string f in _FontCache)
            {
                sb.AppendLine(f);
            }
            sb.AppendLine("</fonts>");

            // output the fills
            sb.AppendFormat("\r\n<fills count=\"{0}\">", _FillCache.Count);
            foreach (string fill in _FillCache)
            {
                sb.AppendLine(fill);
            }
            sb.AppendLine("</fills>");

            // output the borders
            sb.AppendFormat("\r\n<borders count=\"{0}\">", _BorderCache.Count);
            foreach (string b in _BorderCache)
            {
                sb.AppendLine(b);
            }
            sb.AppendLine("</borders>");

           // sb.AppendLine("<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>");
            // output the xfs styles
            sb.AppendFormat("<cellStyleXfs count=\"{0}\">", _StyleXfsCache.Count);
            foreach (string s in _StyleXfsCache)
            {
                sb.AppendLine(s);
            }
            sb.AppendLine("</cellStyleXfs>");

            // output the styles
            sb.AppendFormat("<cellXfs count=\"{0}\">", _StyleCache.Count);
            foreach (string s in _StyleCache)
            {
                sb.AppendLine(s);
            }
            sb.AppendLine("</cellXfs>");
            sb.AppendLine("<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>");
            sb.AppendLine("<dxfs count=\"0\"/><tableStyles count=\"0\" defaultTableStyle=\"TableStyleMedium9\" defaultPivotStyle=\"PivotStyleLight16\"/>");
            sb.AppendLine("</styleSheet>");

            zip.Write(sb.ToString());
        }
        void WriteTheme(ZipOutputStream zip)
        {
            const string theme =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
                "<a:theme xmlns:a=\"http://schemas.openxmlformats.org/drawingml/2006/main\" name=\"Office Theme\">" +
                "<a:themeElements><a:clrScheme name=\"Office\"><a:dk1><a:sysClr val=\"windowText\" lastClr=\"000000\"/></a:dk1>" +
                "<a:lt1><a:sysClr val=\"window\" lastClr=\"FFFFFF\"/></a:lt1><a:dk2><a:srgbClr val=\"1F497D\"/></a:dk2><a:lt2>" +
                "<a:srgbClr val=\"EEECE1\"/></a:lt2><a:accent1><a:srgbClr val=\"4F81BD\"/></a:accent1><a:accent2>" +
                "<a:srgbClr val=\"C0504D\"/></a:accent2><a:accent3><a:srgbClr val=\"9BBB59\"/></a:accent3><a:accent4>" +
                "<a:srgbClr val=\"8064A2\"/></a:accent4><a:accent5><a:srgbClr val=\"4BACC6\"/></a:accent5><a:accent6>" +
                "<a:srgbClr val=\"F79646\"/></a:accent6><a:hlink><a:srgbClr val=\"0000FF\"/></a:hlink><a:folHlink>" +
                "<a:srgbClr val=\"800080\"/></a:folHlink></a:clrScheme><a:fontScheme name=\"Office\"><a:majorFont>" +
                "<a:latin typeface=\"Cambria\"/><a:ea typeface=\"\"/><a:cs typeface=\"\"/>" +
                "<a:font script=\"Jpan\" typeface=\"ＭＳ Ｐゴシック\"/><a:font script=\"Hang\" typeface=\"맑은 고딕\"/>" +
                "<a:font script=\"Hans\" typeface=\"宋体\"/><a:font script=\"Hant\" typeface=\"新細明體\"/>" +
                "<a:font script=\"Arab\" typeface=\"Times New Roman\"/><a:font script=\"Hebr\" typeface=\"Times New Roman\"/>" +
                "<a:font script=\"Thai\" typeface=\"Tahoma\"/><a:font script=\"Ethi\" typeface=\"Nyala\"/>" +
                "<a:font script=\"Beng\" typeface=\"Vrinda\"/><a:font script=\"Gujr\" typeface=\"Shruti\"/>" +
                "<a:font script=\"Khmr\" typeface=\"MoolBoran\"/><a:font script=\"Knda\" typeface=\"Tunga\"/>" +
                "<a:font script=\"Guru\" typeface=\"Raavi\"/><a:font script=\"Cans\" typeface=\"Euphemia\"/>" +
                "<a:font script=\"Cher\" typeface=\"Plantagenet Cherokee\"/><a:font script=\"Yiii\" typeface=\"Microsoft Yi Baiti\"/>" +
                "<a:font script=\"Tibt\" typeface=\"Microsoft Himalaya\"/><a:font script=\"Thaa\" typeface=\"MV Boli\"/>" +
                "<a:font script=\"Deva\" typeface=\"Mangal\"/><a:font script=\"Telu\" typeface=\"Gautami\"/>" +
                "<a:font script=\"Taml\" typeface=\"Latha\"/><a:font script=\"Syrc\" typeface=\"Estrangelo Edessa\"/>" +
                "<a:font script=\"Orya\" typeface=\"Kalinga\"/><a:font script=\"Mlym\" typeface=\"Kartika\"/>" +
                "<a:font script=\"Laoo\" typeface=\"DokChampa\"/><a:font script=\"Sinh\" typeface=\"Iskoola Pota\"/>" +
                "<a:font script=\"Mong\" typeface=\"Mongolian Baiti\"/><a:font script=\"Viet\" typeface=\"Times New Roman\"/>" +
                "<a:font script=\"Uigh\" typeface=\"Microsoft Uighur\"/></a:majorFont><a:minorFont><a:latin typeface=\"Calibri\"/>" +
                "<a:ea typeface=\"\"/><a:cs typeface=\"\"/><a:font script=\"Jpan\" typeface=\"ＭＳ Ｐゴシック\"/>" +
                "<a:font script=\"Hang\" typeface=\"맑은 고딕\"/><a:font script=\"Hans\" typeface=\"宋体\"/>" +
                "<a:font script=\"Hant\" typeface=\"新細明體\"/><a:font script=\"Arab\" typeface=\"Arial\"/>" +
                "<a:font script=\"Hebr\" typeface=\"Arial\"/><a:font script=\"Thai\" typeface=\"Tahoma\"/>" +
                "<a:font script=\"Ethi\" typeface=\"Nyala\"/><a:font script=\"Beng\" typeface=\"Vrinda\"/>" +
                "<a:font script=\"Gujr\" typeface=\"Shruti\"/><a:font script=\"Khmr\" typeface=\"DaunPenh\"/>" +
                "<a:font script=\"Knda\" typeface=\"Tunga\"/><a:font script=\"Guru\" typeface=\"Raavi\"/>" +
                "<a:font script=\"Cans\" typeface=\"Euphemia\"/><a:font script=\"Cher\" typeface=\"Plantagenet Cherokee\"/>" +
                "<a:font script=\"Yiii\" typeface=\"Microsoft Yi Baiti\"/><a:font script=\"Tibt\" typeface=\"Microsoft Himalaya\"/>" +
                "<a:font script=\"Thaa\" typeface=\"MV Boli\"/><a:font script=\"Deva\" typeface=\"Mangal\"/>" +
                "<a:font script=\"Telu\" typeface=\"Gautami\"/><a:font script=\"Taml\" typeface=\"Latha\"/>" +
                "<a:font script=\"Syrc\" typeface=\"Estrangelo Edessa\"/><a:font script=\"Orya\" typeface=\"Kalinga\"/>" +
                "<a:font script=\"Mlym\" typeface=\"Kartika\"/><a:font script=\"Laoo\" typeface=\"DokChampa\"/>" +
                "<a:font script=\"Sinh\" typeface=\"Iskoola Pota\"/><a:font script=\"Mong\" typeface=\"Mongolian Baiti\"/>" +
                "<a:font script=\"Viet\" typeface=\"Arial\"/><a:font script=\"Uigh\" typeface=\"Microsoft Uighur\"/>" +
                "</a:minorFont></a:fontScheme><a:fmtScheme name=\"Office\"><a:fillStyleLst><a:solidFill><a:schemeClr val=\"phClr\"/>" +
                "</a:solidFill><a:gradFill rotWithShape=\"1\"><a:gsLst><a:gs pos=\"0\"><a:schemeClr val=\"phClr\">" +
                "<a:tint val=\"50000\"/><a:satMod val=\"300000\"/></a:schemeClr></a:gs><a:gs pos=\"35000\">" +
                "<a:schemeClr val=\"phClr\"><a:tint val=\"37000\"/><a:satMod val=\"300000\"/></a:schemeClr>" +
                "</a:gs><a:gs pos=\"100000\"><a:schemeClr val=\"phClr\"><a:tint val=\"15000\"/><a:satMod val=\"350000\"/></a:schemeClr>" +
                "</a:gs></a:gsLst><a:lin ang=\"16200000\" scaled=\"1\"/></a:gradFill><a:gradFill rotWithShape=\"1\"><a:gsLst>" +
                "<a:gs pos=\"0\"><a:schemeClr val=\"phClr\"><a:shade val=\"51000\"/><a:satMod val=\"130000\"/></a:schemeClr>" +
                "</a:gs><a:gs pos=\"80000\"><a:schemeClr val=\"phClr\"><a:shade val=\"93000\"/><a:satMod val=\"130000\"/>" +
                "</a:schemeClr></a:gs><a:gs pos=\"100000\"><a:schemeClr val=\"phClr\"><a:shade val=\"94000\"/><a:satMod val=\"135000\"/>" +
                "</a:schemeClr></a:gs></a:gsLst><a:lin ang=\"16200000\" scaled=\"0\"/></a:gradFill></a:fillStyleLst><a:lnStyleLst>" +
                "<a:ln w=\"9525\" cap=\"flat\" cmpd=\"sng\" algn=\"ctr\"><a:solidFill><a:schemeClr val=\"phClr\"><a:shade val=\"95000\"/>" +
                "<a:satMod val=\"105000\"/></a:schemeClr></a:solidFill><a:prstDash val=\"solid\"/></a:ln>" +
                "<a:ln w=\"25400\" cap=\"flat\" cmpd=\"sng\" algn=\"ctr\"><a:solidFill><a:schemeClr val=\"phClr\"/></a:solidFill>" +
                "<a:prstDash val=\"solid\"/></a:ln><a:ln w=\"38100\" cap=\"flat\" cmpd=\"sng\" algn=\"ctr\"><a:solidFill>" +
                "<a:schemeClr val=\"phClr\"/></a:solidFill><a:prstDash val=\"solid\"/></a:ln></a:lnStyleLst><a:effectStyleLst>" +
                "<a:effectStyle><a:effectLst><a:outerShdw blurRad=\"40000\" dist=\"20000\" dir=\"5400000\" rotWithShape=\"0\">" +
                "<a:srgbClr val=\"000000\"><a:alpha val=\"38000\"/></a:srgbClr></a:outerShdw></a:effectLst></a:effectStyle>" +
                "<a:effectStyle><a:effectLst><a:outerShdw blurRad=\"40000\" dist=\"23000\" dir=\"5400000\" rotWithShape=\"0\">" +
                "<a:srgbClr val=\"000000\"><a:alpha val=\"35000\"/></a:srgbClr></a:outerShdw></a:effectLst></a:effectStyle>" +
                "<a:effectStyle><a:effectLst><a:outerShdw blurRad=\"40000\" dist=\"23000\" dir=\"5400000\" rotWithShape=\"0\">" +
                "<a:srgbClr val=\"000000\"><a:alpha val=\"35000\"/></a:srgbClr></a:outerShdw></a:effectLst><a:scene3d>" +
                "<a:camera prst=\"orthographicFront\"><a:rot lat=\"0\" lon=\"0\" rev=\"0\"/></a:camera>" +
                "<a:lightRig rig=\"threePt\" dir=\"t\"><a:rot lat=\"0\" lon=\"0\" rev=\"1200000\"/></a:lightRig></a:scene3d>" +
                "<a:sp3d><a:bevelT w=\"63500\" h=\"25400\"/></a:sp3d></a:effectStyle></a:effectStyleLst><a:bgFillStyleLst>" +
                "<a:solidFill><a:schemeClr val=\"phClr\"/></a:solidFill><a:gradFill rotWithShape=\"1\"><a:gsLst><a:gs pos=\"0\">" +
                "<a:schemeClr val=\"phClr\"><a:tint val=\"40000\"/><a:satMod val=\"350000\"/></a:schemeClr></a:gs><a:gs pos=\"40000\">" +
                "<a:schemeClr val=\"phClr\"><a:tint val=\"45000\"/><a:shade val=\"99000\"/><a:satMod val=\"350000\"/></a:schemeClr>" +
                "</a:gs><a:gs pos=\"100000\"><a:schemeClr val=\"phClr\"><a:shade val=\"20000\"/><a:satMod val=\"255000\"/></a:schemeClr>" +
                "</a:gs></a:gsLst><a:path path=\"circle\"><a:fillToRect l=\"50000\" t=\"-80000\" r=\"50000\" b=\"180000\"/></a:path>" +
                "</a:gradFill><a:gradFill rotWithShape=\"1\"><a:gsLst><a:gs pos=\"0\"><a:schemeClr val=\"phClr\"><a:tint val=\"80000\"/>" +
                "<a:satMod val=\"300000\"/></a:schemeClr></a:gs><a:gs pos=\"100000\"><a:schemeClr val=\"phClr\"><a:shade val=\"30000\"/>" +
                "<a:satMod val=\"200000\"/></a:schemeClr></a:gs></a:gsLst><a:path path=\"circle\">" +
                "<a:fillToRect l=\"50000\" t=\"50000\" r=\"50000\" b=\"50000\"/></a:path></a:gradFill></a:bgFillStyleLst></a:fmtScheme>" +
                "</a:themeElements><a:objectDefaults/><a:extraClrSchemeLst/></a:theme>";
            zip.Write(theme);
        }
        void WriteWorkbook(ZipOutputStream zip)
        {
            //const string wb =
            //    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
            //    "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
            //    "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
            //    "<fileVersion appName=\"xl\" lastEdited=\"4\" lowestEdited=\"4\" rupBuild=\"4505\"/>" +
            //    "<workbookPr filterPrivacy=\"1\" defaultThemeVersion=\"124226\"/><bookViews>" +
            //    "<workbookView xWindow=\"240\" yWindow=\"105\" windowWidth=\"14805\" windowHeight=\"8010\"/></bookViews>" +
            //    "<sheets><sheet name=\"Sheet1\" sheetId=\"1\" r:id=\"rId1\"/><sheet name=\"Sheet2\" sheetId=\"2\" r:id=\"rId2\"/>" +
            //    "<sheet name=\"Sheet3\" sheetId=\"3\" r:id=\"rId3\"/></sheets><calcPr calcId=\"124519\"/>" +
            //    "</workbook>";
            StringBuilder sb = new StringBuilder(400);
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
                "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
                "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                "<fileVersion appName=\"xl\" lastEdited=\"4\" lowestEdited=\"4\" rupBuild=\"4505\"/>" +
                "<workbookPr filterPrivacy=\"1\" defaultThemeVersion=\"124226\"/><bookViews>" +
                "<workbookView xWindow=\"240\" yWindow=\"105\" windowWidth=\"14805\" windowHeight=\"8010\"/></bookViews>" +
                "<sheets>");
            if (_Sheets.Count == 0)
            {   // we'll output an empty sheet in this case; just so we have a valid file
                sb.Append("<sheet name=\"Sheet1\" sheetId=\"1\" r:id=\"rId1\"/>");
            }
            else
            {
                int id = 1;
                foreach (SheetInfo sinfo in _Sheets)
                {
                    sb.AppendFormat("<sheet name=\"{0}\" sheetId=\"{1}\" r:id=\"rId{1}\"/>", sinfo.Name, id);
                    id++;
                }
            }
            sb.Append("</sheets><calcPr calcId=\"124519\"/></workbook>");
            zip.Write(sb.ToString());
        }

        void WriteWorkbookRels(ZipOutputStream zip)
        {
            //const string wbr =
            //    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
            //    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
            //    "<Relationship Id=\"rId3\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" " +
            //    "Target=\"worksheets/sheet3.xml\"/><Relationship Id=\"rId2\" " +
            //    "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" " +
            //    "Target=\"worksheets/sheet2.xml\"/><Relationship Id=\"rId1\" " +
            //    "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" " +
            //    "Target=\"worksheets/sheet1.xml\"/><Relationship Id=\"rId6\" " +
            //    "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings\" " +
            //    "Target=\"sharedStrings.xml\"/><Relationship Id=\"rId5\" " +
            //    "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" " +
            //    "Target=\"styles.xml\"/><Relationship Id=\"rId4\" " +
            //    "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme\" " +
            //    "Target=\"theme/theme1.xml\"/></Relationships>";
            StringBuilder sb = new StringBuilder(400);
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
                "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
            int id = 1;
            foreach (SheetInfo sinfo in _Sheets)
            {
                sb.AppendFormat("<Relationship Id=\"rId{0}\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" " +
                "Target=\"worksheets/{1}.xml\"/>", id, sinfo.Name);
                id++;
            }
            sb.AppendFormat("<Relationship Id=\"rId{0}\" " +
                "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings\" " +
                "Target=\"sharedStrings.xml\"/>", id++);

            sb.AppendFormat("<Relationship Id=\"rId{0}\" " +
                "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" " +
                "Target=\"styles.xml\"/>", id++);

            sb.AppendFormat("<Relationship Id=\"rId{0}\" " +
                "Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme\" " +
                "Target=\"theme/theme1.xml\"/>", id++);

            sb.Append("</Relationships>");

            zip.Write(sb.ToString());
        }
        void WriteEmptyWorksheet(ZipOutputStream zip)
        {
            const string ws =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "\r\n" +
                "<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
                "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                "<dimension ref=\"A1\"/><sheetViews><sheetView workbookViewId=\"0\"/></sheetViews>" +
                "<sheetFormatPr defaultRowHeight=\"15\"/><sheetData/>" +
                "<pageMargins left=\"0.7\" right=\"0.7\" top=\"0.75\" bottom=\"0.75\" header=\"0.3\" footer=\"0.3\"/></worksheet>";
            zip.Write(ws);
        }
        string RowColumnToPosition(int row, int column)
        {
            return ColumnIndexToName(column) + RowIndexToName(row);
        }

        string ColumnIndexToName(int columnIndex)
        {
            char second = (char)(((int)'A') + columnIndex % 26);

            columnIndex /= 26;

            if (columnIndex == 0)
                return second.ToString();
            else
                return ((char)(((int)'A') - 1 + columnIndex)).ToString() + second.ToString();
        }

        string RowIndexToName(int rowIndex)
        {
            return (rowIndex + 1).ToString();
        }

        void WriteData(ZipOutputStream zip, SparseMatrix grid)
        {
            MemoryStream ms = new MemoryStream();
            using (XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8))
            {
                xtw.WriteStartDocument(true);

                xtw.WriteStartElement("worksheet");
                xtw.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                xtw.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

                xtw.WriteStartElement("dimension");
                string lastCell = RowColumnToPosition(grid.Count - 2, grid.GetColumnCount(grid.Count-2) - 1);
                xtw.WriteAttributeString("ref", "A1:" + lastCell);
                xtw.WriteEndElement();

                xtw.WriteStartElement("sheetViews");
                xtw.WriteStartElement("sheetView");
                xtw.WriteAttributeString("tabSelected", "1");
                xtw.WriteAttributeString("workbookViewId", "0");
                xtw.WriteEndElement(); 
                xtw.WriteEndElement();

                xtw.WriteStartElement("sheetFormatPr");
                xtw.WriteAttributeString("defaultRowHeight", "15");
                xtw.WriteEndElement();

                // column width information e.g.   //<cols><col min="1" max="1" width="18.5703125" customWidth="1"/></cols>
                xtw.WriteStartElement("cols");
                int cols = grid.GetColumnCount(0);

                for (int ci = 1; ci < cols; ci++)
                {
                    CellData cd = grid[0, ci] as CellData;
                    if (cd == null)                // skip empty grid items
                        continue;
                    xtw.WriteStartElement("col");
                    string sc = ci.ToString();
                    xtw.WriteAttributeString("min", sc);
                    xtw.WriteAttributeString("max", sc);
                    xtw.WriteAttributeString("width", cd.Val.ToString());
                    xtw.WriteAttributeString("customWidth", "1");
                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();

                // sheet data
                xtw.WriteStartElement("sheetData");
                WriteGridData(xtw, grid);
                xtw.WriteEndElement();

                // Merge Cells AJM 130608
                xtw.WriteStartElement("mergeCells");
                xtw.WriteAttributeString("count", _MergeCells.Count.ToString());
                WriteMergeCells(xtw, _MergeCells);
                xtw.WriteEndElement();
                xtw.WriteStartElement("pageMargins");
                xtw.WriteAttributeString("left", "0.7");
                xtw.WriteAttributeString("right", "0.7");
                xtw.WriteAttributeString("top", "0.75");
                xtw.WriteAttributeString("bottom", "0.75");
                xtw.WriteAttributeString("header", "0.3");
                xtw.WriteAttributeString("footer", "0.3");
                xtw.WriteEndElement();

                xtw.WriteEndElement();
                xtw.Flush();
                byte[] ba = ms.ToArray();
                zip.Write(ba, 0, ba.Length);
            }
        }

        // AJM GJL 130608 Merge Cells
        void WriteMergeCells(XmlTextWriter xtw, StringCache MC)
        {
            foreach (string mc in MC)
            {
                xtw.WriteStartElement("mergeCell");
                xtw.WriteAttributeString("ref", mc);
                xtw.WriteEndElement();
            }
        }
        void WriteGridData(XmlTextWriter xtw, SparseMatrix grid)
        {
            int rows = grid.Count;
            string relPos;
            for (int ri = 1; ri < rows; ri++)
            {
                xtw.WriteStartElement("row");
                relPos = RowIndexToName(ri-1);
                xtw.WriteAttributeString("r", relPos);
                int cols = grid.GetColumnCount(ri);
                xtw.WriteAttributeString("spans", "1:" + (cols-1).ToString());

                CellData cd = grid[ri, 0] as CellData;
                if (cd != null)                // skip empty grid items
                {
                    xtw.WriteAttributeString("ht", Convert.ToString(cd.Val, NumberFormatInfo.InvariantInfo));
                    xtw.WriteAttributeString("customHeight", "1");
                }
                for (int ci = 1; ci < cols; ci++)
                {
                    cd = grid[ri, ci] as CellData;
                    if (cd == null)                // skip empty grid items
                        continue;

                    xtw.WriteStartElement("c");
                    relPos = RowColumnToPosition(ri-1, ci-1);
                    xtw.WriteAttributeString("r", relPos);
                    xtw.WriteAttributeString("t", "s");
                    if (cd.StyleIndex >= 0)
                        xtw.WriteAttributeString("s", cd.StyleIndex.ToString());

                    xtw.WriteElementString("v", cd.Val.ToString());

                    xtw.WriteEndElement();
                }

                xtw.WriteEndElement();
            }
        }
    }

    class StringCache:IEnumerable<string>
    {
        Dictionary<string, int> _StringCache;                   // holds the cache of all strings
        List<string> _StringList;

        public StringCache()
        {
            _StringCache = new Dictionary<string, int>();
            _StringList = new List<string>();
        }

        public int GetIndex(string val) 
        {
            int gval;
            if (!_StringCache.TryGetValue(val, out gval))
            {
                gval = _StringList.Count;
                _StringCache.Add(val, gval);
                _StringList.Add(val);
            }
            return gval;
        }

        public int Count
        {
            get { return _StringList.Count; }
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return _StringList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _StringList.GetEnumerator();
        }

        #endregion
    }
/// <summary>
/// CellData represents the information in a cell.  It consists of the value and the style index.
/// </summary>
    class CellData
    {
        public object Val;
        public int StyleIndex;             // -1 means no style index has been defined
        
        public CellData(object val, int si)
        {
            Val = val;
            StyleIndex = si;
        }

    }
    /// <summary>
    /// SheetInfo holds information about the sheet; mainly the name and the data
    /// </summary>
    class SheetInfo
    {
        string _Name;                   // name of sheet
        SparseMatrix _Grid;             // holds the grid data for the sheet
        internal SheetInfo(string name)
        {
            _Name = name;
            _Grid = new SparseMatrix();
        }
        internal string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        internal SparseMatrix Grid
        {
            get { return _Grid; }
        }
    }

    /// <summary>
    /// SparseMatrix is an implementation of a sparse matrix.  However, the implementation leans towards being 
    /// a fairly dense grid where you don't need to know the row or column counts up front. 
    /// </summary>
    internal class SparseMatrix
    {
        List<List<object>> _data;
        
        internal SparseMatrix()
        {
            _data = new List<List<object>>();
        }

        internal int Count
        {
            get { return _data.Count; }
        }

        internal int GetColumnCount(int row)
        {
            if (row >= _data.Count)
                return 0;
            return _data[row].Count;
        }

        object GetCell(int row, int col)
        {
            if (row >= _data.Count)
                return null;
            List<object> l = _data[row];
            if (col >= l.Count)
                return null;
            return l[col];
        }
        void SetCell(int row, int col, object val)
        {
            while (row >= _data.Count)
            {
                // Add range of null so that we don't hit this everytime
                _data.Add(new List<object>());
            }
            List<object> l = _data[row];
            while (col >= l.Count)
                l.Add(null);

            l[col] = val;
            return;
        }
        internal object this[int i1, int i2]
        {
            get { return GetCell(i1, i2); }
            set { SetCell(i1, i2, value); }
        }
    }
}
