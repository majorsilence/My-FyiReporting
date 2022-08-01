
/*
 * 
 Copyright (C) 2004-2008  fyiReporting Software, LLC
 Copyright (C) 2011  Peter Gill <peter@majorsilence.com>
 Copyright (c) 2010 devFU Pty Ltd, Josh Wilson and Others (http://reportfu.org)



 This file has been modified with suggestiong from forum users.
 *Obtained from Forum, User: sinnovasoft http://www.fyireporting.com/forum/viewtopic.php?t=1049

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
*/

using System;
using fyiReporting.RDL;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using fyiReporting.RDL.Utility;
using System.Security;

namespace fyiReporting.RDL
{


    ///<summary>
    /// Renders a report to PDF.   This is a page oriented formatting renderer.
    ///</summary>
    [SecuritySafeCritical]
    internal class RenderPdf : IPresent
    {
        Report r;					// report
        Stream tw;					// where the output is going
        PdfAnchor anchor;			// anchor tieing together all pdf objects
        PdfCatalog catalog;
        PdfPageTree pageTree;
        PdfInfo info;
        PdfFonts fonts;
        PdfPattern patterns;
        PdfImages images;
        PdfOutline outline;		 // holds the bookmarks (if any)
        PdfUtility pdfUtility;
        int filesize;
        PdfPage page;
        private PdfPageSize _pSize;
        PdfContent content;
        PdfElements elements;
        static readonly char[] lineBreak = new char[] { '\n' };
        static readonly char[] wordBreak = new char[] { ' ' };
        //		static readonly int MEASUREMAX = int.MaxValue;  //  .Net 2 doesn't seem to have a limit; 1.1 limit was 32
        static readonly int MEASUREMAX = 32;  //  guess I'm wrong -- .Net 2 doesn't seem to have a limit; 1.1 limit was 32

        #region PdfElements Rewrite in iTextSharp (Sinnovasoft add April 14 2010)
        Document pdfdocument = new Document();
        MemoryStream ms = new MemoryStream();
        PdfContentByte cb;
        /// <summary>
        /// List itextSharp Basefont added
        /// </summary>
        private List<BaseFont> BaseFonts = new List<BaseFont>();
        /// <summary>
        /// List font name
        /// </summary>
        private List<string> BaseFontsname = new List<string>();
        /// <summary>
        /// Page line element at the X Y to X2 Y2 position
        /// </summary>
        /// <returns></returns>
        private void iAddLine(float x, float y, float x2, float y2, StyleInfo si)
        {
            iAddLine(x, y, x2, y2, si.BWidthTop, si.BColorTop, si.BStyleTop);
        }
        /// <summary>
        /// Page line element at the X Y to X2 Y2 position
        /// </summary>
        /// <returns></returns>
        private void iAddLine(float x, float y, float x2, float y2, float width, System.Drawing.Color c, BorderStyleEnum ls)
        {
            // Get the line color			
            cb.SetRGBColorStroke(c.R, c.G, c.B);
            cb.SetLineWidth(width);
            // Get the line style Dotted - Dashed - Solid
            
            switch (ls)
            {
                case BorderStyleEnum.Dashed:
                    cb.SetLineDash(new float[] { width*3, width  }, 0);
                    break;
                case BorderStyleEnum.Dotted:
                    cb.SetLineDash(new float[] { width },0);
                    break;
                case BorderStyleEnum.Solid:
                default:
                    cb.SetLineDash(new float[] { }, 0);
                    break;
            }
            cb.MoveTo(x, _pSize.yHeight - y);
            cb.LineTo(x2, _pSize.yHeight - y2);
            cb.Stroke();
        }
        /// <summary>
        /// Add a filled rectangle
        /// </summary>
        /// <returns></returns>
        private void iAddFillRect(float x, float y, float width, float height, System.Drawing.Color c)
        {
            // Get the fill color
            cb.SetRGBColorFill(c.R, c.G, c.B);
            cb.Rectangle(x, _pSize.yHeight - y - height, width, height);
            //cb.ClosePathFillStroke();
            cb.Fill();
        }

        private void iAddFillRect(float x, float y, float width, float height, StyleInfo si, PdfPattern patterns)
        {
            System.Drawing.Color c;
            //Not sure about iTextSharp Pattern => Need check
            if (si.PatternType != patternTypeEnum.None)
            {
                string p = patterns.GetPdfPattern(si.PatternType.ToString());
                c = si.Color;
                double red = Math.Round((c.R / 255.0), 3);
                double green = Math.Round((c.G / 255.0), 3);
                double blue = Math.Round((c.B / 255.0), 3);
                StringBuilder elements = new StringBuilder();
                elements.AppendFormat("\r\nq");
                elements.AppendFormat("\r\n /CS1 cs");
                elements.AppendFormat("\r\n {0} {1} {2} /{3} scn", red, green, blue, p);
                elements.AppendFormat("\r\n {0} {1} {2} RG", red, green, blue);
                elements.AppendFormat("\r\n {0} {1} {2} {3} re\tf", x, _pSize.yHeight - y - height, width, height);
                elements.AppendFormat("\tQ");
                PdfPatternPainter pdfp = cb.CreatePattern(60f, 60f, 60f, 60f);
                pdfp.SetLiteral(elements.ToString());
                cb.SetPatternFill(pdfp);
            }
            // Get the fill color - could be a gradient or pattern etc...
            c = si.BackgroundColor;
            cb.SetRGBColorFill(c.R, c.G, c.B);
            cb.Rectangle(x, _pSize.yHeight - y - height, width, height);
            //cb.ClosePathFillStroke();
            cb.Fill();

        }
        /// <summary>
        /// Add border
        /// </summary>
        private void iAddBorder(StyleInfo si, float x, float y, float height, float width)
        {
            // Handle any border required   TODO: optimize border by drawing a rect when possible
            if (height <= 0 || width <= 0)		// no bounding box to use
                return;

            float ybottom = (y + height);
            float xright = x + width;
            if (si.BStyleTop != BorderStyleEnum.None && si.BWidthTop > 0)
                iAddLine(x, y, xright, y, si.BWidthTop, si.BColorTop, si.BStyleTop);

            if (si.BStyleRight != BorderStyleEnum.None && si.BWidthRight > 0)
                iAddLine(xright, y, xright, ybottom, si.BWidthRight, si.BColorRight, si.BStyleRight);

            if (si.BStyleLeft != BorderStyleEnum.None && si.BWidthLeft > 0)
                iAddLine(x, y, x, ybottom, si.BWidthLeft, si.BColorLeft, si.BStyleLeft);

            if (si.BStyleBottom != BorderStyleEnum.None && si.BWidthBottom > 0)
                iAddLine(x, ybottom, xright, ybottom, si.BWidthBottom, si.BColorBottom, si.BStyleBottom);

            return;
        }
        /// <summary>
        /// Add image to the page.
        /// </summary>
        /// <returns>string Image name</returns>
        private void iAddImage(PdfImages images, string name, int contentRef, StyleInfo si,
            ImageFormat imf, float x, float y, float width, float height, RectangleF clipRect,
            byte[] im, int samplesW, int samplesH, string url, string tooltip)
        {

            iTextSharp.text.Image pdfImg = iTextSharp.text.Image.GetInstance(im);
            pdfImg.ScaleAbsolute(width, height); //zoom		  
            pdfImg.SetAbsolutePosition(x, _pSize.yHeight - y - height);//Set position
            pdfdocument.Add(pdfImg);
            //add url
            if (url != null)
                pdfdocument.Add(new Annotation(x, _pSize.yHeight - y - _pSize.topMargin, width + x, height, url));
            //add tooltip
            if (!string.IsNullOrEmpty(tooltip))
                pdfdocument.Add(new Annotation(x, _pSize.yHeight - y - _pSize.topMargin, width + x, height, tooltip));
            iAddBorder(si, x - si.PaddingLeft, y - si.PaddingTop,
                height + si.PaddingTop + si.PaddingBottom,
                width + si.PaddingLeft + si.PaddingRight);			// add any required border
        }
        /// <summary>
        /// Page Polygon
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="si"></param>
        /// <param name="url"></param>
        /// <param name="patterns"></param>
        internal void iAddPolygon(PointF[] pts, StyleInfo si, string url, PdfPattern patterns)
        {
            if (si.BackgroundColor.IsEmpty)
                return;		 // nothing to do

            // Get the fill color - could be a gradient or pattern etc...
            System.Drawing.Color c = si.BackgroundColor;
            iAddPoints(pts);
            cb.SetRGBColorFill(c.R, c.G, c.B);
            cb.ClosePathFillStroke();

            //Not sure about iTextSharp Pattern => Need check
            if (si.PatternType != patternTypeEnum.None)
            {
                string p = patterns.GetPdfPattern(si.PatternType.ToString());
                StringBuilder elements = new StringBuilder();
                c = si.Color;
                double red = Math.Round((c.R / 255.0), 3);
                double green = Math.Round((c.G / 255.0), 3);
                double blue = Math.Round((c.B / 255.0), 3);
                elements.AppendFormat("\r\nq");
                elements.AppendFormat("\r\n /CS1 cs");
                elements.AppendFormat("\r\n {0} {1} {2} /{3} scn", red, green, blue, p);
                elements.AppendFormat("\r\n {0} {1} {2} RG", red, green, blue);
                elements.AppendFormat("\tQ");
                PdfPatternPainter pdfp = cb.CreatePattern(60f, 60f, 60f, 60f);
                pdfp.SetLiteral(elements.ToString());
                cb.SetPatternFill(pdfp);
                iAddPoints(pts);
                cb.ClosePathFillStroke();
            }
        }

        private void iAddPoints(PointF[] pts)
        {
            if (pts.Length > 0)
            {
                cb.MoveTo(pts[0].X, _pSize.yHeight - pts[0].Y);
                for (int pi = 1; pi < pts.Length; pi++)
                {
                    cb.LineTo(pts[pi].X, _pSize.yHeight - pts[pi].Y);
                }
            }
            return;
        }
        /// <summary>
        /// Page Rectangle element at the X Y position
        /// </summary>
        /// <returns></returns>
        private void iAddRectangle(float x, float y, float height, float width, StyleInfo si, string url, PdfPattern patterns, string tooltip)
        {
            // Draw background rectangle if needed
            if (!si.BackgroundColor.IsEmpty && height > 0 && width > 0)
            {	// background color, height and width are specified
                iAddFillRect(x, y, width, height, si, patterns);
            }

            iAddBorder(si, x, y, height, width);			// add any required border

            if (url != null)
                pdfdocument.Add(new Annotation(x, _pSize.yHeight - y, width + x, height, url));
            if (!string.IsNullOrEmpty(tooltip))
                pdfdocument.Add(new Annotation(x, _pSize.yHeight - y, width + x, height, tooltip));

            return;
        }

        /// <summary>
        /// Draw a pie
        /// </summary>
        /// <returns></returns>
        private void iAddPie(float x, float y, float height, float width, StyleInfo si, string url, PdfPattern patterns, string tooltip)
        {
            // Draw background rectangle if needed
            if (!si.BackgroundColor.IsEmpty && height > 0 && width > 0)
            {	// background color, height and width are specified
                iAddFillRect(x, y, width, height, si, patterns);
            }
            iAddBorder(si, x, y, height, width);			// add any required border

            //add url
            if (url != null)
                pdfdocument.Add(new Annotation(x, _pSize.yHeight - y, width + x, height, url));
            //add tooltip
            if (!string.IsNullOrEmpty(tooltip))
                pdfdocument.Add(new Annotation(x, _pSize.yHeight - y, width + x, height, tooltip));
            return;
        }

        /// <summary>
        /// Draw a curve
        /// </summary>
        /// <returns></returns>
        private void iAddCurve(PointF[] pts, StyleInfo si)
        {
            if (pts.Length > 2)
            {   // do a spline curve
                PointF[] tangents = iGetCurveTangents(pts);
                iDoCurve(pts, tangents, si);
            }
            else
            {   // we only have two points; just do a line segment
                iAddLine(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y, si);
            }
        }

        private void iDoCurve(PointF[] points, PointF[] tangents, StyleInfo si)
        {
            int i;

            for (i = 0; i < points.Length - 1; i++)
            {
                int j = i + 1;

                float x0 = points[i].X;
                float y0 = points[i].Y;

                float x1 = points[i].X + tangents[i].X;
                float y1 = points[i].Y + tangents[i].Y;

                float x2 = points[j].X - tangents[j].X;
                float y2 = points[j].Y - tangents[j].Y;

                float x3 = points[j].X;
                float y3 = points[j].Y;
                iAddCurve(x0, y0, x1, y1, x2, y2, x3, y3, si, null);
            }
        }

        private PointF[] iGetCurveTangents(PointF[] points)
        {
            float tension = .5f;				 // This  is the tension used on the DrawCurve GDI call.
            float coefficient = tension / 3.0f;
            int i;

            PointF[] tangents = new PointF[points.Length];

            // initialize everything to zero to begin with
            for (i = 0; i < tangents.Length; i++)
            {
                tangents[i].X = 0;
                tangents[i].Y = 0;
            }

            if (tangents.Length <= 2)
                return tangents;
            int count = tangents.Length;
            for (i = 0; i < count; i++)
            {
                int r = i + 1;
                int s = i - 1;

                if (r >= points.Length)
                    r = points.Length - 1;
                if (s < 0)
                    s = 0;

                tangents[i].X += (coefficient * (points[r].X - points[s].X));
                tangents[i].Y += (coefficient * (points[r].Y - points[s].Y));
            }

            return tangents;
        }

        //25072008 GJL Draw a bezier curve
        private void iAddCurve(float X1, float Y1, float X2, float Y2, float X3, float Y3, float X4, float Y4, StyleInfo si, string url)
        {

            if (si.BStyleTop != BorderStyleEnum.None)
            {
                switch (si.BStyleTop)
                {
                    case BorderStyleEnum.Dashed:
                        cb.SetLineDash(new float[] { '3', '2' }, 0);
                        break;
                    case BorderStyleEnum.Dotted:
                        cb.SetLineDash(new float[] { '2' }, 0);
                        break;
                    case BorderStyleEnum.Solid:
                    default:
                        cb.SetLineDash(new float[] { }, 0);
                        break;
                }
                cb.SetRGBColorStroke(si.BColorTop.R, si.BColorTop.G, si.BColorTop.B);
            }

            if (!si.BackgroundColor.IsEmpty)
            {
                cb.SetRGBColorStrokeF(si.BackgroundColor.R, si.BackgroundColor.G, si.BackgroundColor.B);
            }
            cb.CurveTo(X1, _pSize.yHeight - Y1, X2, _pSize.yHeight - Y1, X3, _pSize.yHeight - Y3);
            if (si.BackgroundColor.IsEmpty)
                cb.ClosePathStroke();
            else
                cb.ClosePathFillStroke();
        }

        //25072008 GJL Draw 4 bezier curves to approximate a circle
        private void iAddEllipse(float x, float y, float height, float width, StyleInfo si, string url)
        {
            if (si.BStyleTop != BorderStyleEnum.None)
            {
                switch (si.BStyleTop)
                {
                    case BorderStyleEnum.Dashed:
                        cb.SetLineDash(new float[] { '3', '2' }, 0);
                        break;
                    case BorderStyleEnum.Dotted:
                        cb.SetLineDash(new float[] { '2' }, 0);
                        break;
                    case BorderStyleEnum.Solid:
                    default:
                        cb.SetLineDash(new float[] { }, 0);
                        break;
                }
                cb.SetRGBColorStroke(si.BColorTop.R, si.BColorTop.G, si.BColorTop.B);
            }
            float RadiusX = (width / 2.0f);
            float RadiusY = (height / 2.0f);
            cb.Ellipse(x, _pSize.yHeight - y, x + RadiusX, y + RadiusY);
            if (!si.BackgroundColor.IsEmpty)
            {
                cb.SetRGBColorStrokeF(si.BackgroundColor.R, si.BackgroundColor.G, si.BackgroundColor.B);
            }
            if (si.BackgroundColor.IsEmpty)
                cb.ClosePathStroke();
            else
                cb.ClosePathFillStroke();
        }
        /// <summary>
        /// Font name , for my application almost fonts  will be unicode and embedded
        /// </summary>
        /// <returns></returns>
        private string iFontNameNormalize(string face)
        {
            string faceName;
            switch (face.ToLower())
            {
                case "times":
                case "times-roman":
                case "times roman":
                case "timesnewroman":
                case "times new roman":
                case "timesnewromanps":
                case "timesnewromanpsmt":
                case "serif":
                    faceName = "Times-Roman";
                    break;
                case "helvetica":
                case "arial":
                case "arialmt":
                case "sans-serif":
                case "sans serif":
                default:
                    faceName = "Arial";
                    break;
                case "courier":
                case "couriernew":
                case "courier new":
                case "couriernewpsmt":
                case "monospace":
                    faceName = "Courier New";
                    break;
                case "symbol":
                    faceName = "Symbol";
                    break;
                case "zapfdingbats":
                case "wingdings":
                case "wingding":
                    faceName = "ZapfDingbats";
                    break;
            }
            return faceName;
        }


        bool dejavuFonts = false;
        /// <summary> 
        /// Default I get embedded fonts in Fonts folder in current 
        /// folder RdlEngine.dll in, can set font folder here 
        /// </summary> 
		private string FontFolder {
			get {
				int platform = (int)Environment.OSVersion.Platform;
				int version = (int)Environment.OSVersion.Version.Major;

				//Kind of MacOSX
				if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
					return "/Library/Fonts";
				}
				if (System.Environment.OSVersion.Platform == PlatformID.Unix) {
					if (System.IO.Directory.Exists ("/usr/share/fonts/truetype/msttcorefonts")) {
						return "/usr/share/fonts/truetype/msttcorefonts";
					} else if (System.IO.Directory.Exists ("/usr/share/fonts/truetype")) {
						dejavuFonts = true;
						return "/usr/share/fonts/truetype";
					} else {
						dejavuFonts = true;
						return Environment.GetFolderPath (Environment.SpecialFolder.Fonts);
					}
				}

#if NET_4
         return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);
#else
                // get parent of System folder to have Windows folder
                DirectoryInfo dirWindowsFolder = Directory.GetParent (Environment.GetFolderPath (Environment.SpecialFolder.System));
                // Concatenate Fonts folder onto Windows folder.
                return Path.Combine (dirWindowsFolder.FullName, "Fonts");
                // Results in full path e.g. "C:\Windows\Fonts" 
#endif
            }
        }

        /// <summary>
        /// Page Text element at the X Y position; multiple lines handled
        /// </summary>
        /// <returns></returns>
		private void iAddText (float x, float y, float height, float width, string[] sa,
			StyleInfo si, PdfFonts fonts, float[] tw, bool bWrap, string url, bool bNoClip, string tooltip)
		{
			int platform = (int)Environment.OSVersion.Platform;
			int version = (int)Environment.OSVersion.Version.Major;

			BaseFont bf;
			string face = iFontNameNormalize (si.FontFamily);
			string fontname = "";
			bool fonttype1 = true;
			var folder = FontFolder; //Call to determine folder and set value of dejavuFonts;
			if (face == "Times-Roman") {
				if (si.IsFontBold () && si.FontStyle == FontStyleEnum.Italic) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "TimesNewRomanPS-BoldItalicMT";
						fontname = "Times New Roman Bold Italic.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Serif Condensed Bold Italic" : "Times-BoldItalic";
						fontname = (dejavuFonts ? "DejaVuSerifCondensed-BoldItalic.ttf" : "timesbi.ttf");
					}
				} else if (si.IsFontBold ()) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "TimesNewRomanPS-BoldMT";
						fontname = "Times New Roman Bold.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Serif Condensed Bold" : "Times-Bold";
						fontname = (dejavuFonts ? "DejaVuSerifCondensed-Bold.ttf" : "timesbd.ttf");
					}
				} else if (si.FontStyle == FontStyleEnum.Italic) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "TimesNewRomanPS-ItalicMT";
						fontname = "Times New Roman Italic.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Serif Condensed Italic" : "Times-Italic";
						fontname = (dejavuFonts ? "DejaVuSerifCondensed-Italic.ttf" : "timesi.ttf");
					}
				} else {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "TimesNewRomanPSMT";
						fontname = "Times New Roman.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Serif Condensed" : face;
						fontname = (dejavuFonts ? "DejaVuSerifCondensed.ttf" : "times.ttf");
					}
				}
				fonttype1 = false;
			} else if (face == "Arial") {
				if (si.IsFontBold () && si.FontStyle == FontStyleEnum.Italic) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "Arial BoldItalicMT";
						fontname = "Arial Bold Italic.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Condensed Bold Oblique" : "Arial-BoldItalic";
						fontname = (dejavuFonts ? "DejaVuSansCondensed-BoldOblique.ttf" : "arialbi.ttf");
					}
				} else if (si.IsFontBold ()) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "Arial-BoldMT";
						fontname = "Arial Bold.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Condensed Bold" : "Arial-Bold";
						fontname = (dejavuFonts ? "DejaVuSansCondensed-Bold.ttf" : "arialbd.ttf");
					}
				} else if (si.FontStyle == FontStyleEnum.Italic) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "Arial-ItalicMT";
						fontname = "Arial Italic.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Condensed Oblique" : "Arial-Italic";
						fontname = (dejavuFonts ? "DejaVuSansCondensed-Oblique.ttf" : "ariali.ttf");
					}
				} else {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "ArialMT";
						fontname = "Arial.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Condensed" : face;
						fontname = (dejavuFonts ? "DejaVuSansCondensed.ttf" : "arial.ttf");
					}
				}
				fonttype1 = false;
			} else if (face == "Courier New") {
				if (si.IsFontBold () && si.FontStyle == FontStyleEnum.Italic) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "CourierNewPS-BoldItalicMT";
						fontname = "Courier New Bold Italic.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Mono Bold Oblique" : "Courier New-BoldItalic";
						fontname = (dejavuFonts ? "DejaVuSansMono-BoldOblique.ttf" : "courbi.ttf");
					}
				} else if (si.IsFontBold ()) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "CourierNewPS-BoldMT";
						fontname = "Courier New Bold.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Mono Bold" : "Courier New-Bold";
						fontname = (dejavuFonts ? "DejaVuSansMono-Oblique.ttf" : "courbd.ttf");
					}
				} else if (si.FontStyle == FontStyleEnum.Italic) {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "CourierNewPS-ItalicMT";
						fontname = "Courier New Italic.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Mono Oblique" : "Courier New-Italic";
						fontname = (dejavuFonts ? "DejaVuSansMono-Oblique.ttf" : "couri.ttf");
					}
				} else {
					//OSX
					if ((platform == 4 || platform == 6 || platform == 128) && version > 8) {
						face = "CourierNewPSMT";
						fontname = "Courier New.ttf";
					} else {
						face = dejavuFonts ? "DejaVu Sans Mono" : face;
						fontname = (dejavuFonts ? "DejaVuSansMono.ttf" : "cour.ttf");
					}
				}
				fonttype1 = false;
			} else {
                        if (si.IsFontBold() &&
                    si.FontStyle == FontStyleEnum.Italic)   // bold and italic?
                            face = face + "-BoldOblique";
                        else if (si.IsFontBold())           // just bold?
                            face = face + "-Bold";
                        else if (si.FontStyle == FontStyleEnum.Italic)
                            face = face + "-Oblique";
                        fonttype1 = true;
                    }
            //Get index of fontname in List font name
            int indexbf = BaseFontsname.FindIndex(delegate(string _fontname) { return _fontname == face; });
            //If not found then add new BaseFont
            if (indexbf == -1)
            {
                BaseFontsname.Add(face);
                if (fonttype1)
                {
                    bf = BaseFont.CreateFont(face, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
                }
                else
                {
                    string path = System.IO.Path.Combine(folder, fontname);
                    bf = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                BaseFonts.Add(bf);
            }
            else
            //Get from List
            {
                bf = BaseFonts[indexbf];
            }

            // Loop thru the lines of text
            for (int i = 0; i < sa.Length; i++)
            {
                string text = sa[i];
                float textwidth = tw[i];
                // Calculate the x positino
                float startX = x + si.PaddingLeft;						// TODO: handle tb_rl
                float startY = y + si.PaddingTop + (i * si.FontSize);	// TODO: handle tb_rl
                int align = 0;
                if (si.WritingMode == WritingModeEnum.lr_tb)
                {	// TODO: not sure what alignment means with tb_lr so I'll leave it out for now
                    switch (si.TextAlign)
                    {
                        case TextAlignEnum.Center:
                            if (width > 0)
                            {
                                startX = x + si.PaddingLeft + ((width - si.PaddingLeft - si.PaddingRight) / 2) - (textwidth / 2);
                                align = Element.ALIGN_CENTER;
                            }
                            break;
                        case TextAlignEnum.Right:
                            if (width > 0)
                            {
                                startX = x + width - textwidth - si.PaddingRight;
                                align = Element.ALIGN_RIGHT;
                            }
                            break;
                        case TextAlignEnum.Left:
                        default:
                            align = Element.ALIGN_LEFT;
                            break;
                    }

                    // Calculate the y position
                    switch (si.VerticalAlign)
                    {
                        case VerticalAlignEnum.Middle:
                            if (height <= 0)
                                break;

                            // calculate the middle of the region
                            startY = y + si.PaddingTop + ((height - si.PaddingTop - si.PaddingBottom) / 2) - (si.FontSize / 2);
                            // now go up or down depending on which line
                            if (sa.Length == 1)
                                break;
                            if (sa.Length % 2 == 0)	// even number
                            {
                                startY = startY - (((sa.Length / 2) - i) * si.FontSize) + (si.FontSize / 2);
                            }
                            else
                            {
                                startY = startY - (((sa.Length / 2) - i) * si.FontSize);
                            }
                            break;
                        case VerticalAlignEnum.Bottom:
                            if (height <= 0)
                                break;

                            startY = y + height - si.PaddingBottom - (si.FontSize * (sa.Length - i));
                            break;
                        case VerticalAlignEnum.Top:
                        default:
                            break;
                    }
                }
                else
                {
                    //25072008 GJL - Move x in a little - it draws to close to the edge of the rectangle (25% of the font size seems to work!) and Center or right align vertical text
                    startX += si.FontSize / 4;

                    switch (si.TextAlign)
                    {
                        case TextAlignEnum.Center:
                            if (height > 0)
                                startY = y + si.PaddingLeft + ((height - si.PaddingLeft - si.PaddingRight) / 2) - (textwidth / 2);
                            break;
                        case TextAlignEnum.Right:
                            if (width > 0)
                                startY = y + height - textwidth - si.PaddingRight;
                            break;
                        case TextAlignEnum.Left:
                        default:
                            break;
                    }

                }

                // Draw background rectangle if needed (only put out on the first line, since we do whole rectangle)
                if (!si.BackgroundColor.IsEmpty && height > 0 && width > 0 && i == 0)
                {	// background color, height and width are specified
                    iAddFillRect(x, y, width, height, si.BackgroundColor);
                }

                // Set the clipping path, (Itext have no clip)
                if (height > 0 && width > 0)
                {
                    cb.SetRGBColorFill(si.Color.R, si.Color.G, si.Color.B);

                    if (si.WritingMode == WritingModeEnum.lr_tb)
                    {
                        //If textline after measure with word break can fit just simple show Text
                        if (width >= textwidth)
                        {
                            cb.BeginText();
                            cb.SetFontAndSize(bf, si.FontSize);
                            cb.SetTextMatrix(startX, (_pSize.yHeight - startY - si.FontSize));
                            cb.ShowText(text);
                            cb.EndText();
                        }
                        else
                        {
                            //else use Column text to wrap or clip (wrap: for example a text like an URL so word break is not working here, itextsharp ColumnText do the work for us)
                            ColumnText ct = new ColumnText(cb);
                            Phrase myPhrase = new Phrase(text, new iTextSharp.text.Font(bf, si.FontSize));
                            ct.SetSimpleColumn(myPhrase, x + si.PaddingLeft, _pSize.yHeight - startY, x + width - si.PaddingRight, _pSize.yHeight - y - si.PaddingBottom - height, 10f, align);
                            ct.Go();
                        }
                    }
                    else
                    {
                        //Not checked
                        double rads = -283.0 / 180.0;
                        double radsCos = Math.Cos(rads);
                        double radsSin = Math.Sin(rads);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, si.FontSize);
                        cb.SetTextMatrix((float)radsCos, (float)radsSin, (float)-radsSin, (float)radsCos, startX, _pSize.yHeight - startY);
                        cb.ShowText(text);
                        cb.EndText();
                    }

                    //add URL
                    if (url != null)
						pdfdocument.Add(new Annotation(x, _pSize.yHeight - (y + height), width + x, _pSize.yHeight - y, url));
                    //add tooltip
                    if (tooltip != null)
						pdfdocument.Add(new Annotation(x, _pSize.yHeight - (y + height), width + x, _pSize.yHeight - y, tooltip));

                }

                // Handle underlining etc.
                float maxX;
                switch (si.TextDecoration)
                {
                    case TextDecorationEnum.Underline:
                        maxX = width > 0 ? Math.Min(x + width, startX + textwidth) : startX + textwidth;
                        iAddLine(startX, startY + si.FontSize + 1, maxX, startY + si.FontSize + 1, 1, si.Color, BorderStyleEnum.Solid);
                        break;
                    case TextDecorationEnum.LineThrough:
                        maxX = width > 0 ? Math.Min(x + width, startX + textwidth) : startX + textwidth;
                        iAddLine(startX, startY + (si.FontSize / 2) + 1, maxX, startY + (si.FontSize / 2) + 1, 1, si.Color, BorderStyleEnum.Solid);
                        break;
                    case TextDecorationEnum.Overline:
                        maxX = width > 0 ? Math.Min(x + width, startX + textwidth) : startX + textwidth;
                        iAddLine(startX, startY + 1, maxX, startY + 1, 1, si.Color, BorderStyleEnum.Solid);
                        break;
                    case TextDecorationEnum.None:
                    default:
                        break;
                }
            }

            iAddBorder(si, x, y, height, width);			// add any required border

            return;
        }
        #endregion

        //Replaced from forum, User: Aulofee http://www.fyireporting.com/forum/viewtopic.php?t=793
        public void Dispose() { }

        public RenderPdf(Report rep, IStreamGen sg)
        {
            r = rep;
            tw = sg.GetStream();
        }

        public Report Report()
        {
            return r;
        }

        public bool IsPagingNeeded()
        {
            return true;
        }

        public void Start()
        {
            // Create the anchor for all pdf objects
            CompressionConfig cc = RdlEngineConfig.GetCompression();
            anchor = new PdfAnchor(cc != null);

            //Create a PdfCatalog
            string lang;
            if (r.ReportDefinition.Language != null)
                lang = r.ReportDefinition.Language.EvaluateString(this.r, null);
            else
                lang = null;
            catalog = new PdfCatalog(anchor, lang);

            //Create a Page Tree Dictionary
            pageTree = new PdfPageTree(anchor);

            //Create a Font Dictionary
            fonts = new PdfFonts(anchor);

            //Create a Pattern Dictionary
            patterns = new PdfPattern(anchor);

            //Create an Image Dictionary
            images = new PdfImages(anchor);

            //Create an Outline Dictionary
            outline = new PdfOutline(anchor);

            //Create the info Dictionary
            info = new PdfInfo(anchor);

            //Set the info Dictionary. 
            info.SetInfo(r.Name, r.Author, r.Description, "");	// title, author, subject, company

            //Create a utility object
            pdfUtility = new PdfUtility(anchor);

            //write out the header
            int size = 0;

            if (r.ItextPDF)
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfdocument, ms);
                pdfdocument.Open();
                cb = writer.DirectContent;
                pdfdocument.AddAuthor(r.Author);
                pdfdocument.AddCreationDate();
                pdfdocument.AddCreator("Majorsilence Reporting");
                pdfdocument.AddSubject(r.Description);
                pdfdocument.AddTitle(r.Name);

            }
            else
            {
                tw.Write(pdfUtility.GetHeader("1.5", out size), 0, size);
            }

            //
            filesize = size;
        }

        public void End()
        {
            //Write everything 
            int size = 0;
            #region ItextsharpPDF handler
            if (r.ItextPDF)
            {
                if (outline.Bookmarks.Count > 0)
                {
                    //Not handler TODO
                }
                pdfdocument.Close();
                //write out ItextSharp pdf stream to RDL stream
                byte[] contentbyte = ms.ToArray();
                tw.Write(contentbyte, 0, contentbyte.Length);
                ms.Dispose();
                BaseFonts.Clear();
                BaseFontsname.Clear();
            }
            else
            {
                tw.Write(catalog.GetCatalogDict(outline.GetObjectNumber(), pageTree.objectNum,
                    filesize, out size), 0, size);
                filesize += size;
                tw.Write(pageTree.GetPageTree(filesize, out size), 0, size);
                filesize += size;
                tw.Write(fonts.GetFontDict(filesize, out size), 0, size);
                filesize += size;
                tw.Write(patterns.GetPatternDict(filesize, out size), 0, size);
                filesize += size;

                if (images.Images.Count > 0)
                {
                    tw.Write(images.GetImageDict(filesize, out size), 0, size);
                    filesize += size;
                }
                if (outline.Bookmarks.Count > 0)
                {
                    tw.Write(outline.GetOutlineDict(filesize, out size), 0, size);
                    filesize += size;
                }

                tw.Write(info.GetInfoDict(filesize, out size), 0, size);
                filesize += size;

                tw.Write(pdfUtility.CreateXrefTable(filesize, out size), 0, size);
                filesize += size;

                tw.Write(pdfUtility.GetTrailer(catalog.objectNum,
                    info.objectNum, out size), 0, size);
                filesize += size;
            }
            #endregion
            return;
        }

        public void RunPages(Pages pgs)	// this does all the work
        {
            foreach (Page p in pgs)
            {
                //Create a Page Dictionary representing a visible page
                page = new PdfPage(anchor);
                content = new PdfContent(anchor);

                PdfPageSize pSize = new PdfPageSize((int)r.ReportDefinition.PageWidth.ToPoints(),
                                    (int)r.ReportDefinition.PageHeight.ToPoints());
                _pSize = pSize;
                page.CreatePage(pageTree.objectNum, pSize);
                pageTree.AddPage(page.objectNum);
                if (r.ItextPDF)
                {
                    //Itextsharp pdf handler, set pagesize
                    pdfdocument.SetPageSize(new iTextSharp.text.Rectangle(r.ReportDefinition.PageWidth.ToPoints(), r.ReportDefinition.PageHeight.ToPoints()));
                    pdfdocument.NewPage();
                }
                //Create object that presents the elements in the page
                elements = new PdfElements(page, pSize);

                ProcessPage(pgs, p);

                // after a page
                content.SetStream(elements.EndElements());

                page.AddResource(fonts, content.objectNum);
                page.AddResource(patterns, content.objectNum);
                //get the pattern colorspace...
                PatternObj po = new PatternObj(anchor);
                page.AddResource(po, content.objectNum);
                if (r.ItextPDF == false)
                {
                    int size = 0;
                    tw.Write(page.GetPageDict(filesize, out size), 0, size);
                    filesize += size;

                    tw.Write(content.GetContentDict(filesize, out size), 0, size);
                    filesize += size;

                    tw.Write(po.GetPatternObj(filesize, out size), 0, size);
                    filesize += size;
                }
            }
            return;
        }
        // render all the objects in a page in PDF
        private void ProcessPage(Pages pgs, IEnumerable items)
        {
            foreach (PageItem pi in items)
            {
                if (pi.SI.BackgroundImage != null)
                {	// put out any background image
                    PageImage bgImg = pi.SI.BackgroundImage;
                    //					elements.AddImage(images, i.Name, content.objectNum, i.SI, i.ImgFormat, 
                    //						pi.X, pi.Y, pi.W, pi.H, i.ImageData,i.SamplesW, i.SamplesH, null);				   
                    //Duc Phan modified 10 Dec, 2007 to support on background image 
                    float imW = Measurement.PointsFromPixels(bgImg.SamplesW, pgs.G.DpiX);
                    float imH = Measurement.PointsFromPixels(bgImg.SamplesH, pgs.G.DpiY);
                    int repeatX = 0;
                    int repeatY = 0;
                    float itemW = pi.W - (pi.SI.PaddingLeft + pi.SI.PaddingRight);
                    float itemH = pi.H - (pi.SI.PaddingTop + pi.SI.PaddingBottom);
                    switch (bgImg.Repeat)
                    {
                        case ImageRepeat.Repeat:
                            repeatX = (int)Math.Floor(itemW / imW);
                            repeatY = (int)Math.Floor(itemH / imH);
                            break;
                        case ImageRepeat.RepeatX:
                            repeatX = (int)Math.Floor(itemW / imW);
                            repeatY = 1;
                            break;
                        case ImageRepeat.RepeatY:
                            repeatY = (int)Math.Floor(itemH / imH);
                            repeatX = 1;
                            break;
                        case ImageRepeat.NoRepeat:
                        default:
                            repeatX = repeatY = 1;
                            break;
                    }

                    //make sure the image is drawn at least 1 times 
                    repeatX = Math.Max(repeatX, 1);
                    repeatY = Math.Max(repeatY, 1);

                    float currX = pi.X + pi.SI.PaddingLeft;
                    float currY = pi.Y + pi.SI.PaddingTop;
                    float startX = currX;
                    float startY = currY;
                    for (int i = 0; i < repeatX; i++)
                    {
                        for (int j = 0; j < repeatY; j++)
                        {
                            currX = startX + (i * imW);
                            currY = startY + (j * imH);
                       
                            if (r.ItextPDF)
                            {

                                iAddImage(images, bgImg.Name,
                                                content.objectNum, bgImg.SI, bgImg.ImgFormat,
                                                currX, currY, imW, imH, RectangleF.Empty, bgImg.ImageData, bgImg.SamplesW, bgImg.SamplesH, null, pi.Tooltip);
                            }
                            else
                            {
                                elements.AddImage(images, bgImg.Name,
                                           content.objectNum, bgImg.SI, bgImg.ImgFormat,
                                           currX, currY, imW, imH, RectangleF.Empty, bgImg.ImageData, bgImg.SamplesW, bgImg.SamplesH, null, pi.Tooltip);

                            }
                           
                        }
                    }
                }

                if (pi is PageTextHtml)
                {
                    PageTextHtml pth = pi as PageTextHtml;
                    pth.Build(pgs.G);
                    ProcessPage(pgs, pth);
                    continue;
                }

                if (pi is PageText)
                {
                    PageText pt = pi as PageText;
                    float[] textwidth;
                    string[] sa = MeasureString(pt, pgs.G, out textwidth);
                   

                    if (r.ItextPDF)
                    {
                        iAddText(pt.X, pt.Y, pt.H, pt.W, sa, pt.SI,
                        fonts, textwidth, pt.CanGrow, pt.HyperLink, pt.NoClip, pt.Tooltip);
                    }
                    else
                    {
                        elements.AddText(pt.X, pt.Y, pt.H, pt.W, sa, pt.SI,
                       fonts, textwidth, pt.CanGrow, pt.HyperLink, pt.NoClip, pt.Tooltip);
                    }
                    
                    if (pt.Bookmark != null)
                    {
                        outline.Bookmarks.Add(new PdfOutlineEntry(anchor, page.objectNum, pt.Bookmark, pt.X, elements.PageSize.yHeight - pt.Y));
                    }
                    continue;
                }

                if (pi is PageLine)
                {
                    PageLine pl = pi as PageLine;
                    

                    if (r.ItextPDF)
                    {
                        iAddLine(pl.X, pl.Y, pl.X2, pl.Y2, pl.SI);
                    }
                    else
                    {
                        elements.AddLine(pl.X, pl.Y, pl.X2, pl.Y2, pl.SI);
                    }
                   
                    continue;
                }

                if (pi is PageEllipse)
                {
                    PageEllipse pe = pi as PageEllipse;


                    if (r.ItextPDF)
                    {
                        iAddEllipse(pe.X, pe.Y, pe.H, pe.W, pe.SI, pe.HyperLink);
                    }
                    else
                    {
                        elements.AddEllipse(pe.X, pe.Y, pe.H, pe.W, pe.SI, pe.HyperLink);
                    }

                    
                    continue;
                }



                if (pi is PageImage)
                {
                    //PageImage i = pi as PageImage;
                    //float x = i.X + i.SI.PaddingLeft;
                    //float y = i.Y + i.SI.PaddingTop;
                    //float w = i.W - i.SI.PaddingLeft - i.SI.PaddingRight;
                    //float h = i.H - i.SI.PaddingTop - i.SI.PaddingBottom;
                    //elements.AddImage(images, i.Name, content.objectNum, i.SI, i.ImgFormat, 
                    //	x, y, w, h, i.ImageData,i.SamplesW, i.SamplesH, i.HyperLink);
                    //continue;
                    PageImage i = pi as PageImage;

                    //Duc Phan added 20 Dec, 2007 to support sized image 
                    RectangleF r2 = new RectangleF(i.X + i.SI.PaddingLeft, i.Y + i.SI.PaddingTop, i.W - i.SI.PaddingLeft - i.SI.PaddingRight, i.H - i.SI.PaddingTop - i.SI.PaddingBottom);

                    RectangleF adjustedRect;   // work rectangle 
                    RectangleF clipRect = RectangleF.Empty;
                    switch (i.Sizing)
                    {
                        case ImageSizingEnum.AutoSize:
                            adjustedRect = new RectangleF(r2.Left, r2.Top,
                                            r2.Width, r2.Height);
                            break;
                        case ImageSizingEnum.Clip:
                            adjustedRect = new RectangleF(r2.Left, r2.Top,
                                            Measurement.PointsFromPixels(i.SamplesW, pgs.G.DpiX), Measurement.PointsFromPixels(i.SamplesH, pgs.G.DpiY));
                            clipRect = new RectangleF(r2.Left, r2.Top,
                                            r2.Width, r2.Height);
                            break;
                        case ImageSizingEnum.FitProportional:
                            float height;
                            float width;
                            float ratioIm = (float)i.SamplesH / i.SamplesW;
                            float ratioR = r2.Height / r2.Width;
                            height = r2.Height;
                            width = r2.Width;
                            if (ratioIm > ratioR)
                            {   // this means the rectangle width must be corrected 
                                width = height * (1 / ratioIm);
                            }
                            else if (ratioIm < ratioR)
                            {   // this means the rectangle height must be corrected 
                                height = width * ratioIm;
                            }
                            adjustedRect = new RectangleF(r2.X, r2.Y, width, height);
                            break;
                        case ImageSizingEnum.Fit:
                        default:
                            adjustedRect = r2;
                            break;
                    }
                    if (i.ImgFormat == System.Drawing.Imaging.ImageFormat.Wmf || i.ImgFormat == System.Drawing.Imaging.ImageFormat.Emf)
                    {
                        //We dont want to add it - its already been broken down into page items;
                    }
                    else
                    {
                       
                        if (r.ItextPDF)
                        {
                            iAddImage(images, i.Name, content.objectNum, i.SI, i.ImgFormat,
                            adjustedRect.X, adjustedRect.Y, adjustedRect.Width, adjustedRect.Height, clipRect, i.ImageData, i.SamplesW, i.SamplesH, i.HyperLink, i.Tooltip);
                        }
                        else
                        {
                            elements.AddImage(images, i.Name, content.objectNum, i.SI, i.ImgFormat,
                           adjustedRect.X, adjustedRect.Y, adjustedRect.Width, adjustedRect.Height, clipRect, i.ImageData, i.SamplesW, i.SamplesH, i.HyperLink, i.Tooltip);

                        }
                       
                    }
                    continue;
                }

                if (pi is PageRectangle)
                {
                    PageRectangle pr = pi as PageRectangle;


                    if (r.ItextPDF)
                    {
                        iAddRectangle(pr.X, pr.Y, pr.H, pr.W, pi.SI, pi.HyperLink, patterns, pi.Tooltip);
                    }
                    else
                    {
                        elements.AddRectangle(pr.X, pr.Y, pr.H, pr.W, pi.SI, pi.HyperLink, patterns, pi.Tooltip);
                    }
                    
                    continue;
                }
                if (pi is PagePie)
                {   // TODO
                    PagePie pp = pi as PagePie;
                    // 

                    if (r.ItextPDF)
                    {
                        iAddPie(pp.X, pp.Y, pp.H, pp.W, pi.SI, pi.HyperLink, patterns, pi.Tooltip);
                    }
                    else
                    {
                        elements.AddPie(pp.X, pp.Y, pp.H, pp.W, pi.SI, pi.HyperLink, patterns, pi.Tooltip);
                    }
                   
                    continue;
                }
                if (pi is PagePolygon)
                {
                    PagePolygon ppo = pi as PagePolygon;
                    

                    if (r.ItextPDF)
                    {
                        iAddPolygon(ppo.Points, pi.SI, pi.HyperLink, patterns);
                    }
                    else
                    {
                        elements.AddPolygon(ppo.Points, pi.SI, pi.HyperLink, patterns);
                    }
                    
                    continue;
                }
                if (pi is PageCurve)
                {
                    PageCurve pc = pi as PageCurve;


                    if (r.ItextPDF)
                    {
                        iAddCurve(pc.Points, pi.SI);
                    }
                    else
                    {
                        elements.AddCurve(pc.Points, pi.SI);
                    }
                    
                    continue;
                }

            }

        }

        private string[] MeasureString(PageText pt, Graphics g, out float[] width)
        {
            StyleInfo si = pt.SI;
            string s = pt.Text;

            System.Drawing.Font drawFont = null;
            StringFormat drawFormat = null;
            SizeF ms;
            string[] sa = null;
            width = null;
            try
            {
                // STYLE
                System.Drawing.FontStyle fs = 0;
                if (si.FontStyle == FontStyleEnum.Italic)
                    fs |= System.Drawing.FontStyle.Italic;

                // WEIGHT
                switch (si.FontWeight)
                {
                    case FontWeightEnum.Bold:
                    case FontWeightEnum.Bolder:
                    case FontWeightEnum.W500:
                    case FontWeightEnum.W600:
                    case FontWeightEnum.W700:
                    case FontWeightEnum.W800:
                    case FontWeightEnum.W900:
                        fs |= System.Drawing.FontStyle.Bold;
                        break;
                    default:
                        break;
                }

                drawFont = new System.Drawing.Font(StyleInfo.GetFontFamily(si.FontFamilyFull), si.FontSize, fs);
                drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Near;

                // Measure string   
                //  pt.NoClip indicates that this was generated by PageTextHtml Build.  It has already word wrapped.
                if (pt.NoClip || pt.SI.WritingMode == WritingModeEnum.tb_rl)	// TODO: support multiple lines for vertical text
                {
                    ms = MeasureString(s, g, drawFont, drawFormat);
                    width = new float[1];
                    width[0] = Measurement.PointsFromPixels(ms.Width, g.DpiX);	// convert to points from pixels
                    sa = new string[1];
                    sa[0] = s;
                    return sa;
                }

                // handle multiple lines;
                //  1) split the string into the forced line breaks (ie "\n and \r")
                //  2) foreach of the forced line breaks; break these into words and recombine 
                s = s.Replace("\r\n", "\n");	// don't want this to result in double lines
                string[] flines = s.Split(lineBreak);
                List<string> lines = new List<string>();
                List<float> lineWidths = new List<float>();
                // remove the size reserved for left and right padding
                float ptWidth = pt.W - pt.SI.PaddingLeft - pt.SI.PaddingRight;
                if (ptWidth <= 0)
                    ptWidth = 1;
                foreach (string tfl in flines)
                {
                    string fl;
                    if (tfl.Length > 0 && tfl[tfl.Length - 1] == ' ')
                        fl = tfl.TrimEnd(' ');
                    else
                        fl = tfl;

                    // Check if entire string fits into a line
                    ms = MeasureString(fl, g, drawFont, drawFormat);
                    float tw = Measurement.PointsFromPixels(ms.Width, g.DpiX);
                    if (tw <= ptWidth)
                    {					   // line fits don't need to break it down further
                        lines.Add(fl);
                        lineWidths.Add(tw);
                        continue;
                    }

                    // Line too long; need to break into multiple lines
                    // 1) break line into parts; then build up again keeping track of word positions
                    string[] parts = fl.Split(wordBreak);	// this is the maximum split of lines
                    StringBuilder sb = new StringBuilder(fl.Length);
                    CharacterRange[] cra = new CharacterRange[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        int sc = sb.Length;	 // starting character
                        sb.Append(parts[i]);	// endding character
                        if (i != parts.Length - 1)  // last item doesn't need blank
                            sb.Append(" ");
                        int ec = sb.Length;
                        CharacterRange cr = new CharacterRange(sc, ec - sc);
                        cra[i] = cr;			// add to character array
                    }

                    // 2) Measure the word locations within the line
                    string wfl = sb.ToString();
                    WordStartFinish[] wordLocations = MeasureString(wfl, g, drawFont, drawFormat, cra);
                    if (wordLocations == null)
                        continue;

                    // 3) Loop thru creating new lines as needed
                    int startLoc = 0;
                    CharacterRange crs = cra[startLoc];
                    CharacterRange cre = cra[startLoc];
                    float cwidth = wordLocations[0].end;	// length of the first
                    float bwidth = wordLocations[0].start;  // characters need a little extra on start
                    string ts;
                    bool bLine = true;
                    for (int i = 1; i < cra.Length; i++)
                    {
                        cwidth = wordLocations[i].end - wordLocations[startLoc].start + bwidth;
                        if (cwidth > ptWidth)
                        {	// time for a new line
                            cre = cra[i - 1];
                            ts = wfl.Substring(crs.First, cre.First + cre.Length - crs.First);
                            lines.Add(ts);
                            lineWidths.Add(wordLocations[i - 1].end - wordLocations[startLoc].start + bwidth);

                            // Find the first non-blank character of the next line
                            while (i < cra.Length &&
                                    cra[i].Length == 1 &&
                                    fl[cra[i].First] == ' ')
                            {
                                i++;
                            }
                            if (i < cra.Length)   // any lines left?
                            {  // yes, continue on
                                startLoc = i;
                                crs = cre = cra[startLoc];
                                cwidth = wordLocations[i].end - wordLocations[startLoc].start + bwidth;
                            }
                            else  // no, we can stop
                                bLine = false;
                            //  bwidth = wordLocations[startLoc].start - wordLocations[startLoc - 1].end;
                        }
                        else
                            cre = cra[i];
                    }
                    if (bLine)
                    {
                        ts = fl.Substring(crs.First, cre.First + cre.Length - crs.First);
                        lines.Add(ts);
                        lineWidths.Add(cwidth);
                    }
                }
                // create the final array from the Lists
                string[] la = lines.ToArray();
                width = lineWidths.ToArray();
                return la;
            }
            finally
            {
                if (drawFont != null)
                    drawFont.Dispose();
                if (drawFormat != null)
                    drawFont.Dispose();
            }
        }

        /// <summary>
        /// Measures the location of an arbritrary # of words within a string
        /// </summary>
        private WordStartFinish[] MeasureString(string s, Graphics g, System.Drawing.Font drawFont, StringFormat drawFormat, CharacterRange[] cra)
        {
            if (cra.Length <= MEASUREMAX)		// handle the simple case of < MEASUREMAX words
                return MeasureString32(s, g, drawFont, drawFormat, cra);

            // Need to compensate for SetMeasurableCharacterRanges limitation of 32 (MEASUREMAX)
            int mcra = (cra.Length / MEASUREMAX);	// # of full 32 arrays we need
            int ip = cra.Length % MEASUREMAX;		// # of partial entries needed for last array (if any)
            WordStartFinish[] sz = new WordStartFinish[cra.Length];	// this is the final result;
            float startPos = 0;
            CharacterRange[] cra32 = new CharacterRange[MEASUREMAX];	// fill out			
            int icra = 0;						// index thru the cra 
            for (int i = 0; i < mcra; i++)
            {
                // fill out the new array
                int ticra = icra;
                for (int j = 0; j < cra32.Length; j++)
                {
                    cra32[j] = cra[ticra++];
                    cra32[j].First -= cra[icra].First;	// adjust relative offsets of strings
                }

                // measure the word locations (in the new string)
                // ???? should I put a blank in front of it?? 
                string ts = s.Substring(cra[icra].First,
                    cra[icra + cra32.Length - 1].First + cra[icra + cra32.Length - 1].Length - cra[icra].First);
                WordStartFinish[] pos = MeasureString32(ts, g, drawFont, drawFormat, cra32);

                // copy the values adding in the new starting positions
                for (int j = 0; j < pos.Length; j++)
                {
                    sz[icra].start = pos[j].start + startPos;
                    sz[icra++].end = pos[j].end + startPos;
                }
                startPos = sz[icra - 1].end;	// reset the start position for the next line
            }
            // handle the remaining character
            if (ip > 0)
            {
                // resize the range array
                cra32 = new CharacterRange[ip];
                // fill out the new array
                int ticra = icra;
                for (int j = 0; j < cra32.Length; j++)
                {
                    cra32[j] = cra[ticra++];
                    cra32[j].First -= cra[icra].First;	// adjust relative offsets of strings
                }
                // measure the word locations (in the new string)
                // ???? should I put a blank in front of it?? 
                string ts = s.Substring(cra[icra].First,
                    cra[icra + cra32.Length - 1].First + cra[icra + cra32.Length - 1].Length - cra[icra].First);
                WordStartFinish[] pos = MeasureString32(ts, g, drawFont, drawFormat, cra32);

                // copy the values adding in the new starting positions
                for (int j = 0; j < pos.Length; j++)
                {
                    sz[icra].start = pos[j].start + startPos;
                    sz[icra++].end = pos[j].end + startPos;
                }
            }
            return sz;
        }

        /// <summary>
        /// Measures the location of words within a string;  limited by .Net 1.1 to 32 words
        ///	 MEASUREMAX is a constant that defines that limit
        /// </summary>
        /// <param name="s"></param>
        /// <param name="g"></param>
        /// <param name="drawFont"></param>
        /// <param name="drawFormat"></param>
        /// <param name="cra"></param>
        /// <returns></returns>
        private WordStartFinish[] MeasureString32(string s, Graphics g, System.Drawing.Font drawFont, StringFormat drawFormat, CharacterRange[] cra)
        {
            if (s == null || s.Length == 0)
                return null;

            drawFormat.SetMeasurableCharacterRanges(cra);
            Region[] rs = new Region[cra.Length];
            rs = g.MeasureCharacterRanges(s, drawFont, new RectangleF(0, 0, float.MaxValue, float.MaxValue),
                drawFormat);
            WordStartFinish[] sz = new WordStartFinish[cra.Length];
            int isz = 0;
            foreach (Region r in rs)
            {
                RectangleF mr = r.GetBounds(g);
                sz[isz].start = Measurement.PointsFromPixels(mr.Left, g.DpiX);
                sz[isz].end = Measurement.PointsFromPixels(mr.Right, g.DpiX);
                isz++;
            }
            return sz;
        }

        struct WordStartFinish
        {
            internal float start;
            internal float end;
        }

        private SizeF MeasureString(string s, Graphics g, System.Drawing.Font drawFont, StringFormat drawFormat)
        {
            if (s == null || s.Length == 0)
                return SizeF.Empty;

            CharacterRange[] cr = { new CharacterRange(0, s.Length) };
            drawFormat.SetMeasurableCharacterRanges(cr);
            Region[] rs = new Region[1];
            rs = g.MeasureCharacterRanges(s, drawFont, new RectangleF(0, 0, float.MaxValue, float.MaxValue),
                drawFormat);
            RectangleF mr = rs[0].GetBounds(g);

            return new SizeF(mr.Width, mr.Height);
        }

        private float MeasureStringBlank(Graphics g, System.Drawing.Font drawFont, StringFormat drawFormat)
        {
            SizeF ms = MeasureString(" ", g, drawFont, drawFormat);
            float width = Measurement.PointsFromPixels(ms.Width, g.DpiX);	// convert to points from pixels
            return width * 2;
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
        }

        public void DataRegionNoRows(DataRegion d, string noRowsMsg)
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
            return true;
        }

        public void TableEnd(Table t, Row row)
        {
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

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)				// called first
        {
            return true;
        }

        public void MatrixColumns(Matrix m, MatrixColumns mc)	// called just after MatrixStart
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

        public void MatrixEnd(Matrix m, Row r)				// called last
        {
        }

        public void Chart(Chart c, Row r, ChartBase cb)
        {
        }

        public void Image(fyiReporting.RDL.Image i, Row r, string mimeType, Stream ior)
        {
        }

        public void Line(Line l, Row r)
        {
            return;
        }

        public bool RectangleStart(fyiReporting.RDL.Rectangle rect, Row r)
        {
            return true;
        }

        public void RectangleEnd(fyiReporting.RDL.Rectangle rect, Row r)
        {
        }

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
    }
}