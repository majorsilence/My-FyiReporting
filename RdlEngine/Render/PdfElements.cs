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
using System.Collections;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using fyiReporting.RDL;

namespace fyiReporting.RDL
{
	/// <summary>
	/// Different elements in the pdf file
	/// </summary>
	internal class PdfElements
	{
		private PdfPageSize pSize;
		private StringBuilder elements;
		private PdfPage p;

		// Below are used when rotating text 90% -  numbers are odd but by experimentation PDF readers liked them best
		const double rads = -283.0/180.0;
		static readonly double radsCos = Math.Cos(rads); 
		static readonly double radsSin = Math.Sin(rads); 

		internal PdfElements(PdfPage pg, PdfPageSize pageSize)
		{
			p = pg;
			pSize=pageSize;
			elements = new StringBuilder();
		}

        internal PdfPageSize PageSize
        {
            get { return pSize; }
        }

		/// <summary>
		/// Page line element at the X Y to X2 Y2 position
		/// </summary>
		/// <returns></returns>
		internal void AddLine(float x,float y, float x2, float y2, StyleInfo si)
		{
			AddLine(x, y, x2, y2, si.BWidthTop, si.BColorTop, si.BStyleTop);
		}

		/// <summary>
		/// Page line element at the X Y to X2 Y2 position
		/// </summary>
		/// <returns></returns>
		internal void AddLine(float x,float y, float x2, float y2, float width, System.Drawing.Color c, BorderStyleEnum ls)
		{
			// Get the line color
			double red=c.R; 
			double green=c.G; 
			double blue=c.B;
			red = Math.Round((red/255),3);
			green = Math.Round((green/255),3);
			blue = Math.Round((blue/255),3);
			// Get the line style Dotted - Dashed - Solid
			string linestyle;				
			switch (ls)
			{
				case BorderStyleEnum.Dashed:
					linestyle="[3 2] 0 d";
					break;
				case BorderStyleEnum.Dotted:
					linestyle="[2] 0 d";
					break;
				case BorderStyleEnum.Solid:
				default:
					linestyle="[] 0 d";
					break;
			}

			elements.AppendFormat(NumberFormatInfo.InvariantInfo,
				"\r\nq\t{0} w\t{1} {2} {3} RG\t{4}\t{5} {6} m\t{7} {8} l\tS\tQ\t",
				width,			// line width
				red, green, blue,		// line color
				linestyle,				// line style
				x, pSize.yHeight-y, x2, pSize.yHeight-y2);	// positioning
		}

		/// <summary>
		/// Add a filled rectangle
		/// </summary>
		/// <returns></returns>
		internal void AddFillRect(float x,float y, float width, float height, Color c)
		{
			// Get the fill color
			double red=c.R; 
			double green=c.G; 
			double blue=c.B;
			red = Math.Round((red/255),3);
			green = Math.Round((green/255),3);
			blue = Math.Round((blue/255),3);

			elements.AppendFormat(NumberFormatInfo.InvariantInfo,
				"\r\nq\t{0} {1} {2} RG\t{0} {1} {2} rg\t{3} {4} {5} {6} re\tf\tQ\t",
				red, green, blue,		// color
				x, pSize.yHeight-y-height, width, height);	// positioning
		}

		internal void AddFillRect(float x,float y, float width, float height,StyleInfo si,PdfPattern patterns)
		{
			// Get the fill color - could be a gradient or pattern etc...
			Color c = si.BackgroundColor;
			double red=c.R;
			double green=c.G; 
			double blue=c.B;
			red = Math.Round((red/255),3);
			green = Math.Round((green/255),3);
			blue = Math.Round((blue/255),3);
			
			
			//Fill the rectangle with the background color first...			
			elements.AppendFormat(NumberFormatInfo.InvariantInfo,
				"\r\nq\t{0} {1} {2} RG\t{0} {1} {2} rg\t{3} {4} {5} {6} re\tf\tQ\t",
				red, green, blue,		// color
				x, pSize.yHeight-y-height, width, height);	// positioning	
			
            //if we have a pattern paint it now...
            if (si.PatternType != patternTypeEnum.None)
			{			
				string p = patterns.GetPdfPattern(si.PatternType.ToString());                
				c = si.Color;              
				red = Math.Round((c.R/255.0),3);
				green = Math.Round((c.G/255.0),3);
				blue = Math.Round((c.B/255.0),3);
				elements.AppendFormat("\r\nq");			
				elements.AppendFormat("\r\n /CS1 cs");
				elements.AppendFormat("\r\n {0} {1} {2} /{3} scn",red,green,blue,p);
				elements.AppendFormat("\r\n {0} {1} {2} RG",red,green,blue);
				elements.AppendFormat("\r\n {0} {1} {2} {3} re\tf",x, pSize.yHeight-y-height, width, height);
				elements.AppendFormat("\tQ");
			}
		}

		/// <summary>
		/// Add image to the page.  Adds a new XObject Image and a reference to it.
		/// </summary>
		/// <returns>string Image name</returns>
		internal string AddImage(PdfImages images, string name, int contentRef, StyleInfo si,
            ImageFormat imf, float x, float y, float width, float height, RectangleF clipRect,
			byte[] im, int samplesW, int samplesH, string url,string tooltip)
		{
            //MITEK:START---------------------------------- 
            //Duc Phan added 20 Dec, 2007 clip image 
            if (!clipRect.IsEmpty)
            {
                elements.AppendFormat(NumberFormatInfo.InvariantInfo,
                                      "\r\nq\t{0} {1} {2} {3} re W n",
                                      clipRect.X + pSize.leftMargin, pSize.yHeight - clipRect.Y - clipRect.Height - pSize.topMargin, clipRect.Width, clipRect.Height);
            }
            //MITEK:END---------------------------------- 
            
            string imgname = images.GetPdfImage(this.p, name, contentRef, imf, im, samplesW, samplesH);
			
			elements.AppendFormat(NumberFormatInfo.InvariantInfo,
				"\r\nq\t{2} 0 0 {3} {0} {1} cm\t",
				x, pSize.yHeight-y-height, width, height);	// push graphics state, positioning

			elements.AppendFormat(NumberFormatInfo.InvariantInfo, "\t/{0} Do\tQ\t", imgname);	// do the image then pop graphics state
            //MITEK:START---------------------------------- 

            //Duc Phan added 20 Dec, 2007 clip image 
            if (!clipRect.IsEmpty)
            {
                elements.AppendFormat("\tQ\t");   //pop graphics state 
            }

            //MITEK:END---------------------------------- 


			if (url != null)
//				p.AddHyperlink(x, pSize.yHeight-y, height, width, url);
                p.AddHyperlink(x + pSize.leftMargin, 
                               pSize.yHeight - y - pSize.topMargin, 
                               height, width, url);// Duc Phan modified 4 Sep, 2007 to account for the page margin 

            if(tooltip != null)
                p.AddToolTip(x + pSize.leftMargin,
                              pSize.yHeight - y - pSize.topMargin,
                              height, width, tooltip);


            // Border goes around the image padding
			AddBorder(si, x-si.PaddingLeft, y-si.PaddingTop, 
                height + si.PaddingTop+ si.PaddingBottom, 
                width + si.PaddingLeft + si.PaddingRight);			// add any required border

			return imgname;
		}
/// <summary>
/// Page Polygon
/// </summary>
/// <param name="pts"></param>
/// <param name="si"></param>
/// <param name="url"></param>
/// <param name="patterns"></param>
        internal void AddPolygon(PointF[] pts, StyleInfo si, string url, PdfPattern patterns)
        {
            if (si.BackgroundColor.IsEmpty)
                return;         // nothing to do
            
            // Get the fill color - could be a gradient or pattern etc...
            Color c = si.BackgroundColor;
            double red = c.R;
            double green = c.G;
            double blue = c.B;
            red = Math.Round((red / 255), 3);
            green = Math.Round((green / 255), 3);
            blue = Math.Round((blue / 255), 3);

            //Fill the polygon with the background color first...			
            elements.AppendFormat(NumberFormatInfo.InvariantInfo,
                "\r\nq\t{0} {1} {2} RG\t{0} {1} {2} rg\t", 
                red, green, blue);		// color
            AddPoints(elements, pts);

            //if we have a pattern paint it now...
            if (si.PatternType != patternTypeEnum.None)
            {
                string p = patterns.GetPdfPattern(si.PatternType.ToString());
                c = si.Color;
                red = Math.Round((c.R / 255.0), 3);
                green = Math.Round((c.G / 255.0), 3);
                blue = Math.Round((c.B / 255.0), 3);
                elements.AppendFormat("\r\nq");
                elements.AppendFormat("\r\n /CS1 cs");
                elements.AppendFormat("\r\n {0} {1} {2} /{3} scn", red, green, blue, p);
                elements.AppendFormat("\r\n {0} {1} {2} RG", red, green, blue);
                AddPoints(elements, pts);
            }
            elements.AppendFormat("\tQ");
        }

        private void AddPoints(StringBuilder elements, PointF[] pts)
        {
            for (int pi = 0; pi < pts.Length; pi++)
            {
                elements.AppendFormat("\t{0} {1} {2}", pts[pi].X, pSize.yHeight - pts[pi].Y, pi == 0 ? "m" : "l");
            }
            elements.Append("\th\tf");
            return;
        }

		/// <summary>
		/// Page Rectangle element at the X Y position
		/// </summary>
		/// <returns></returns>
		internal void AddRectangle(float x, float y, float height, float width, StyleInfo si, string url, PdfPattern patterns, string tooltip)
		{
			// Draw background rectangle if needed
			if (!si.BackgroundColor.IsEmpty && height > 0 && width > 0)
			{	// background color, height and width are specified
				AddFillRect(x, y, width, height, si,patterns);
			}

			AddBorder(si, x, y, height, width);			// add any required border

			if (url != null)
				p.AddHyperlink(x, pSize.yHeight-y, height, width, url);

            if (!string.IsNullOrEmpty(tooltip))
                p.AddToolTip(x, pSize.yHeight - y, height, width, tooltip);
		
			return;
		}

        //25072008 GJL Draw a pie
        internal void AddPie(float x, float y, float height, float width, StyleInfo si, string url, PdfPattern patterns, string tooltip)
        {
            // Draw background rectangle if needed
            if (!si.BackgroundColor.IsEmpty && height > 0 && width > 0)
            {	// background color, height and width are specified
                AddFillRect(x, y, width, height, si, patterns);
            }

            AddBorder(si, x, y, height, width);			// add any required border

            //Do something...



            if (url != null)
                p.AddHyperlink(x, pSize.yHeight - y, height, width, url);

            if (!string.IsNullOrEmpty(tooltip))
                p.AddToolTip(x, pSize.yHeight - y, height, width, tooltip);

            return;
        }
        //
        internal void AddCurve(PointF[] pts, StyleInfo si)
        {
            if (pts.Length > 2)
            {   // do a spline curve
                PointF[] tangents = GetCurveTangents(pts);
                DoCurve(pts, tangents, si);
            }
            else
            {   // we only have two points; just do a line segment
                AddLine(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y, si);
            }
        }

        void DoCurve(PointF[] points, PointF[] tangents, StyleInfo si)
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
                AddCurve(x0,y0,x1,y1,x2,y2,x3,y3, si, null);
            }
        }

        PointF[] GetCurveTangents(PointF[] points)
        {
            float tension = .5f;                 // This  is the tension used on the DrawCurve GDI call.
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
        internal void AddCurve(float X1, float Y1, float X2, float Y2, float X3, float Y3, float X4, float Y4,StyleInfo si, string url)
		{
			string linestyle;				
			switch (si.BStyleTop)
			{
				case BorderStyleEnum.Dashed:
					linestyle="[3 2] 0 d";
					break;
				case BorderStyleEnum.Dotted:
					linestyle="[2] 0 d";
					break;
				case BorderStyleEnum.Solid:
				default:
					linestyle="[] 0 d";
					break;
			}
			
			elements.AppendFormat("\r\nq\t");			
			if (si.BStyleTop != BorderStyleEnum.None)
			{				
				elements.AppendFormat("{0} w\t {1} \t",si.BWidthTop,linestyle);
				elements.AppendFormat("{0} {1} {2} RG\t", Math.Round(si.BColorTop.R / 255.0, 3), Math.Round(si.BColorTop.G / 255.0, 3), Math.Round(si.BColorTop.B / 255.0, 3)); //Set Stroking colours
			}
			if (!si.BackgroundColor.IsEmpty)
			{
				elements.AppendFormat("{0} {1} {2} rg\t", Math.Round(si.BackgroundColor.R / 255.0, 3), Math.Round(si.BackgroundColor.G / 255.0, 3), Math.Round(si.BackgroundColor.B / 255.0, 3)); //Set Non Stroking colours
			}
			elements.AppendFormat("{0} {1} m\t", X1, pSize.yHeight-Y1);//FirstPoint..
			elements.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X2, pSize.yHeight -Y2, X3,pSize.yHeight-Y3, X4,pSize.yHeight- Y4);		
				if (!si.BackgroundColor.IsEmpty && si.BStyleTop != BorderStyleEnum.None)
				{
					//Line and fill
					elements.AppendFormat("B\t");
				}
				else if (si.BStyleTop != BorderStyleEnum.None)
				{
					//Line
					elements.AppendFormat("S\t");
				}
				else if (!si.BackgroundColor.IsEmpty)
				{
					//fill
					elements.AppendFormat("f\t");
				}				
	             elements.AppendFormat("Q\t");					
		}
		
		//25072008 GJL Draw 4 bezier curves to approximate a circle
		internal void AddEllipse(float x, float y, float height,float width, StyleInfo si, string url)
		{
			
				//Ok we need to draw 4 bezier curves - Unfortunately we cant call drawcurve 4 times because of the fill - we would end up drawing 4 filled arcs with an empty diamond in the middle
				//but we will still include a drawcurve function - it may be usefull one day
				float k = 0.5522847498f;
				float RadiusX = (width / 2.0f) ;
            	float RadiusY = (height / 2.0f) ;            	
				float kRy = k * RadiusY;
            	float kRx = k * RadiusX;
            	
            	float Y4 = y;
            	float X1 = x;
            	float Y1 = Y4 + RadiusY;
            	float X4 = X1 + RadiusX;            	
            	//Control Point 1 will be on the same X as point 1 and be -kRy Y            	
            	float X2 = X1;
            	float Y2 = Y1 - kRy;
            	float X3 = X4 - kRx;
            	float Y3 = Y4;
            	
            	elements.AppendFormat("\r\nq\t");    
            	elements.AppendFormat("{0} {1} m\t", X1, pSize.yHeight-Y1);//FirstPoint..
            	
            	if (si.BStyleTop != BorderStyleEnum.None)	
            	{
            		string linestyle;				
					switch (si.BStyleTop)
					{
						case BorderStyleEnum.Dashed:
							linestyle="[3 2] 0 d";
							break;
						case BorderStyleEnum.Dotted:
							linestyle="[2] 0 d";
							break;
						case BorderStyleEnum.Solid:
						default:
							linestyle="[] 0 d";
							break;
					}
					elements.AppendFormat("{0} w\t {1} \t",si.BWidthTop,linestyle);
					elements.AppendFormat("{0} {1} {2} RG\t", Math.Round(si.BColorTop.R / 255.0, 3), Math.Round(si.BColorTop.G / 255.0, 3), Math.Round(si.BColorTop.B / 255.0, 3)); //Set Stroking colours
				}
				if (!si.BackgroundColor.IsEmpty)
				{
					elements.AppendFormat("{0} {1} {2} rg\t", Math.Round(si.BackgroundColor.R / 255.0, 3), Math.Round(si.BackgroundColor.G / 255.0, 3), Math.Round(si.BackgroundColor.B / 255.0, 3)); //Set Non Stroking colours
				}
            	elements.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X2, pSize.yHeight -Y2, X3,pSize.yHeight-Y3, X4,pSize.yHeight- Y4);	

               
            	X1 += 2 * RadiusX;
            	X2 = X1;
            	X3 += 2 * kRx;
            	
            	elements.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X3, pSize.yHeight -Y3, X2,pSize.yHeight-Y2, X1,pSize.yHeight- Y1);	
             
				
				Y2 += 2 * kRy;
            	Y3 += 2 * RadiusY;
                Y4 = Y3;
                
                elements.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X2, pSize.yHeight -Y2, X3,pSize.yHeight-Y3, X4,pSize.yHeight- Y4);	
              
                
                X1 -= 2 * RadiusX;
            	X2 = X1;
                X3 -= 2 * kRx;
              
                elements.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X3, pSize.yHeight -Y3, X2,pSize.yHeight-Y2, X1,pSize.yHeight- Y1);


                if (!si.BackgroundColor.IsEmpty && si.BStyleTop != BorderStyleEnum.None)
                {
                    //Line and fill
                    elements.AppendFormat("B\t");
                }
                else if (si.BStyleTop != BorderStyleEnum.None)
                {
                    //Line
                    elements.AppendFormat("S\t");
                }
                else if (!si.BackgroundColor.IsEmpty)
                {
                    //fill
                    elements.AppendFormat("f\t");
                }

                elements.AppendFormat("Q\t");	
			
		}

		/// <summary>
		/// Page Text element at the X Y position; multiple lines handled
		/// </summary>
		/// <returns></returns>
		internal void AddText(float x, float y, float height, float width, string[] sa, 
			StyleInfo si, PdfFonts fonts, float[] tw, bool bWrap, string url, bool bNoClip,string tooltip)
		{
			// Calculate the RGB colors e.g. RGB(255, 0, 0) = red = 1 0 0 rg
			double r = si.Color.R; 
			double g = si.Color.G; 
			double b = si.Color.B;
			r = Math.Round((r/255),3);
			g = Math.Round((g/255),3);
			b = Math.Round((b/255),3);

			string pdfFont = fonts.GetPdfFont(si);	// get the pdf font resource name

			// Loop thru the lines of text
			for (int i=0; i < sa.Length; i++)
			{
				string text = sa[i];
				float textwidth = tw[i];
				// Calculate the x position
				float startX = x + si.PaddingLeft;						// TODO: handle tb_rl
				float startY = y + si.PaddingTop + (i * si.FontSize);	// TODO: handle tb_rl

				if (si.WritingMode == WritingModeEnum.lr_tb)
				{	// TODO: not sure what alignment means with tb_lr so I'll leave it out for now
					switch(si.TextAlign)
					{
						case TextAlignEnum.Center:
							if (width > 0)
								startX = x + si.PaddingLeft + (width - si.PaddingLeft - si.PaddingRight)/2 - textwidth/2;	 
							break;
						case TextAlignEnum.Right:
							if (width > 0)
								startX = x + width - textwidth - si.PaddingRight;	
							break;
						case TextAlignEnum.Left:
						default:
							break;
					}

					// Calculate the y position
					switch(si.VerticalAlign)
					{
						case VerticalAlignEnum.Middle:
							if (height <= 0)
								break;

							// calculate the middle of the region
							startY = y + si.PaddingTop + (height - si.PaddingTop - si.PaddingBottom)/2 - si.FontSize/2;	 
							// now go up or down depending on which line
							if (sa.Length == 1)
								break;
							if (sa.Length % 2 == 0)	// even number
							{
								startY = startY - ((sa.Length/2 - i) * si.FontSize) + si.FontSize/2;
							}
							else
							{
								startY = startY - ((sa.Length/2 - i) * si.FontSize);
							}
							break;
						case VerticalAlignEnum.Bottom:
							if (height <= 0)
								break;
							
							startY = y + height - si.PaddingBottom - (si.FontSize * (sa.Length-i));	
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
                                startY = y + si.PaddingLeft + (height - si.PaddingLeft - si.PaddingRight) / 2 - textwidth / 2;
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
					AddFillRect(x, y, width, height, si.BackgroundColor);
				}

				// Set the clipping path
				if (height > 0 && width > 0)
				{
					if (bNoClip)	// no clipping but we still want URL checking
						elements.Append("\r\nq\t");
					else
						elements.AppendFormat(NumberFormatInfo.InvariantInfo,
							"\r\nq\t{0} {1} {2} {3} re W n",
							x, pSize.yHeight-y-height, width, height);
					if (url != null)
						p.AddHyperlink(x, pSize.yHeight-y, height, width, url);
                    if (tooltip != null)
                        p.AddToolTip(x, pSize.yHeight - y, height, width, tooltip);

				}
				else
					elements.Append("\r\nq\t");

				// Escape the text
                string newtext = PdfUtility.UTF16StringQuoter(text);
                //string newtext = text.Replace("\\", "\\\\");
                //newtext = newtext.Replace("(", "\\(");
                //newtext = newtext.Replace(")", "\\)");
				if (si.WritingMode == WritingModeEnum.lr_tb)
				{
					elements.AppendFormat(NumberFormatInfo.InvariantInfo,
						"\r\nBT/{0} {1} Tf\t{5} {6} {7} rg\t{2} {3} Td \t({4}) Tj\tET\tQ\t",
						pdfFont,si.FontSize,startX,(pSize.yHeight-startY-si.FontSize),newtext, r, g, b);
				}
				else
				{	// Rotate text -90 degrees=-.5 radians (this works for english don't know about true tb-rl language)
					//   had to play with reader to find best approximation for this rotation; didn't do what I expected
					//    see pdf spec section 4.2.2 pg 141  "Common Transformations"

					elements.AppendFormat(NumberFormatInfo.InvariantInfo,
						"\r\nBT/{0} {1} Tf\t{5} {6} {7} rg\t{8} {9} {10} {11} {2} {3} Tm \t({4}) Tj\tET\tQ\t",
						pdfFont,si.FontSize,startX,(pSize.yHeight-startY),newtext, r, g, b,
						radsCos, radsSin, -radsSin, radsCos);
				}

				// Handle underlining etc.
				float maxX;
				switch (si.TextDecoration)
				{
					case TextDecorationEnum.Underline:
						maxX = width > 0? Math.Min(x + width, startX+textwidth): startX+textwidth;
						AddLine(startX, startY+si.FontSize+1, maxX, startY+si.FontSize+1, 1, si.Color, BorderStyleEnum.Solid);
						break;
					case TextDecorationEnum.LineThrough:
						maxX = width > 0? Math.Min(x + width, startX+textwidth): startX+textwidth;
						AddLine(startX, startY+(si.FontSize/2)+1, maxX, startY+(si.FontSize/2)+1, 1, si.Color, BorderStyleEnum.Solid);
						break;
					case TextDecorationEnum.Overline:
						maxX = width > 0? Math.Min(x + width, startX+textwidth): startX+textwidth;
						AddLine(startX, startY+1, maxX, startY+1, 1, si.Color, BorderStyleEnum.Solid);
						break;
					case TextDecorationEnum.None:
					default:
						break;
				}
			}

			AddBorder(si, x, y, height, width);			// add any required border
		
			return;
		}

		void AddBorder(StyleInfo si, float x, float y, float height, float width)
		{
			// Handle any border required   TODO: optimize border by drawing a rect when possible
			if (height <= 0 || width <= 0)		// no bounding box to use
				return;

			float ybottom = (y + height);
			float xright = x + width;
			if (si.BStyleTop != BorderStyleEnum.None && si.BWidthTop > 0)
				AddLine(x, y, xright, y, si.BWidthTop, si.BColorTop, si.BStyleTop);

			if (si.BStyleRight != BorderStyleEnum.None && si.BWidthRight > 0)
				AddLine(xright, y, xright, ybottom, si.BWidthRight, si.BColorRight, si.BStyleRight);
			
			if (si.BStyleLeft != BorderStyleEnum.None && si.BWidthLeft > 0)
				AddLine(x, y, x, ybottom, si.BWidthLeft, si.BColorLeft, si.BStyleLeft);
			
			if (si.BStyleBottom != BorderStyleEnum.None && si.BWidthBottom > 0)
				AddLine(x, ybottom, xright, ybottom, si.BWidthBottom, si.BColorBottom, si.BStyleBottom);

			return;
		}
		/// <summary>
		/// No more elements on the page.
		/// </summary>
		/// <returns></returns>
		internal string EndElements()
		{
			string r = elements.ToString();
			elements = new StringBuilder();		// restart it
			return r;
		}
	}
}
