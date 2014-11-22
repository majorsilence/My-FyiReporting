// 
//  RenderCairo.cs
//  
//  Author:
//       Krzysztof Marecki 
//
// Copyright (c) 2010 Krzysztof Marecki 
//
// This file is part of the NReports project
// This file is part of the My-FyiReporting project 
//	
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections;
using System.Globalization;
using System.Threading;

using fyiReporting.RDL;

namespace fyiReporting.RdlGtkViewer
{
    public class RenderCairo
    {
        Cairo.Context g;
        Pango.Layout layout;
		
        float dpiX = 96;
        float dpiY = 96;

        public RenderCairo(Cairo.Context g)
            : this(g, 1.0f)
        {
        }

        public RenderCairo(Cairo.Context g, float scale)
        {
            this.g = g;
            this.layout = Pango.CairoHelper.CreateLayout(g);
			
            dpiX *= scale;
            dpiY *= scale;
        }

        internal float PixelsX(float x)
        {
            return (x * dpiX / 96.0f);
        }

        internal float PixelsY(float y)
        {
            return (y * dpiY / 96.0f);
        }

        private void ProcessPage(Cairo.Context g, IEnumerable p)
        {
            foreach (PageItem pi in p)
            {
				
                if (pi is PageTextHtml)
                {   // PageTextHtml is actually a composite object (just like a page) 
                    ProcessHtml(pi as PageTextHtml, g);
                    continue;
                }

                if (pi is PageLine)
                {
                    PageLine pl = pi as PageLine;
                    DrawLine(
                        pl.SI.BColorLeft.ToCairoColor(), pl.SI.BStyleLeft, pl.SI.BWidthLeft,
                        g, PixelsX(pl.X), PixelsY(pl.Y), PixelsX(pl.X2), PixelsY(pl.Y2)
                    );
                    continue;
                }

//                RectangleF rect = new RectangleF(PixelsX(pi.X), PixelsY(pi.Y), PixelsX(pi.W), PixelsY(pi.H));
                Cairo.Rectangle rect = new Cairo.Rectangle(PixelsX(pi.X), PixelsY(pi.Y), PixelsX(pi.W), PixelsY(pi.H));

                if (pi.SI.BackgroundImage != null)
                {
                    // put out any background image 
                    PageImage i = pi.SI.BackgroundImage;
                    DrawImage(i, g, rect);
                    continue;
                }

                if (pi is PageText)
                {
                    PageText pt = pi as PageText;
                    DrawString(pt, g, rect);
                }
                
                if (pi is PageImage)
                {
                    PageImage i = pi as PageImage;
                    DrawImage(i, g, rect);
                }
                
                if (pi is PageRectangle)
                {
                    //DrawBackground(g, rect, pi.SI);
                }
                //                else if (pi is PageEllipse)
                //                {
                //                    PageEllipse pe = pi as PageEllipse;
                //                    DrawEllipse(pe, g, rect);
                //                }
                //                else if (pi is PagePie)
                //                {
                //                    PagePie pp = pi as PagePie;
                //                    DrawPie(pp, g, rect);
                //                }
                //                else if (pi is PagePolygon)
                //                {
                //                    PagePolygon ppo = pi as PagePolygon;
                //                    FillPolygon(ppo, g, rect);
                //                }
                //                else if (pi is PageCurve)
                //                {
                //                    PageCurve pc = pi as PageCurve;
                //                    DrawCurve(pc.SI.BColorLeft, pc.SI.BStyleLeft, pc.SI.BWidthLeft,
                //                        g, pc.Points, pc.Offset, pc.Tension);
                //                }
                //
                DrawBorder(pi, g, rect);
            }
        }

        private void ProcessHtml(PageTextHtml pth, Cairo.Context g)
        {
			
//            pth.Build(g);            // Builds the subobjects that make up the html 
            this.ProcessPage(g, pth);
        }

        private void DrawLine(Cairo.Color c, BorderStyleEnum bs, float w, Cairo.Context g, double x, double y, double x2, double y2)
        {
            if (bs == BorderStyleEnum.None//|| c.IsEmpty 
                || w <= 0)   // nothing to draw 
                return;

            g.Save();
//          Pen p = null;  
//          p = new Pen(c, w);
            g.Color = c;
            g.LineWidth = w;
            switch (bs)
            {
                case BorderStyleEnum.Dashed:
//	                p.DashStyle = DashStyle.Dash;
                    g.SetDash(new double[] { 2, 1 }, 0.0);
                    break;
                case BorderStyleEnum.Dotted:
//                        p.DashStyle = DashStyle.Dot;
                    g.SetDash(new double[] { 1 }, 0.0);
                    break;
                case BorderStyleEnum.Double:
                case BorderStyleEnum.Groove:
                case BorderStyleEnum.Inset:
                case BorderStyleEnum.Solid:
                case BorderStyleEnum.Outset:
                case BorderStyleEnum.Ridge:
                case BorderStyleEnum.WindowInset:
                default:
                    g.SetDash(new double[] { }, 0.0);
                    break;
            }
	
//  	    g.DrawLine(p, x, y, x2, y2);
            g.MoveTo(x, y);
            g.LineTo(x2, y2);
            g.Stroke();
            
            g.Restore();
        }

        private void DrawImage(PageImage pi, Cairo.Context g, Cairo.Rectangle r)
        {
//            Stream strm = null;
//            System.Drawing.Image im = null;
            Gdk.Pixbuf im = null;
            try
            {
//                strm = new MemoryStream (pi.ImageData);
//                im = System.Drawing.Image.FromStream (strm);
                im = new Gdk.Pixbuf(pi.ImageData);
                DrawImageSized(pi, im, g, r);
            }
            finally
            {
//                if (strm != null)
//                    strm.Close();
                if (im != null)
                    im.Dispose();
            }

        }

        private void DrawImageSized(PageImage pi, Gdk.Pixbuf im, Cairo.Context g, Cairo.Rectangle r)
        {
            double height, width;      // some work variables 
            StyleInfo si = pi.SI;

            // adjust drawing rectangle based on padding 
//            System.Drawing.RectangleF r2 = new System.Drawing.RectangleF(r.Left + PixelsX(si.PaddingLeft),
//                r.Top + PixelsY(si.PaddingTop),
//                r.Width - PixelsX(si.PaddingLeft + si.PaddingRight),
//                r.Height - PixelsY(si.PaddingTop + si.PaddingBottom));
            Cairo.Rectangle r2 = new Cairo.Rectangle(r.X + PixelsX(si.PaddingLeft),
                            r.Y + PixelsY(si.PaddingTop),
                            r.Width - PixelsX(si.PaddingLeft + si.PaddingRight),
                            r.Height - PixelsY(si.PaddingTop + si.PaddingBottom));

            Cairo.Rectangle ir;   // int work rectangle 
            switch (pi.Sizing)
            {
                case ImageSizingEnum.AutoSize:
//                    // Note: GDI+ will stretch an image when you only provide 
//                    //  the left/top coordinates.  This seems pretty stupid since 
//                    //  it results in the image being out of focus even though 
//                    //  you don't want the image resized. 
//                    if (g.DpiX == im.HorizontalResolution &&
//                        g.DpiY == im.VerticalResolution)
                    float imwidth = PixelsX(im.Width);
                    float imheight = PixelsX(im.Height);
                    ir = new Cairo.Rectangle(Convert.ToInt32(r2.X), Convert.ToInt32(r2.Y),
                        imwidth, imheight);
//                    else
//                        ir = new Cairo.Rectangle(Convert.ToInt32(r2.X), Convert.ToInt32(r2.Y),
//                                           Convert.ToInt32(r2.Width), Convert.ToInt32(r2.Height));
                    //g.DrawImage(im, ir);
                    im = im.ScaleSimple((int)r2.Width, (int)r2.Height, Gdk.InterpType.Hyper);
                    g.DrawPixbufRect(im, ir);
                    break;
                case ImageSizingEnum.Clip:
//                    Region saveRegion = g.Clip;
                    g.Save();
//                    Region clipRegion = new Region(g.Clip.GetRegionData());
//                    clipRegion.Intersect(r2);
//                    g.Clip = clipRegion;
                    g.Rectangle(r2);
                    g.Clip();
				
//                    if (dpiX == im.HorizontalResolution &&
//                        dpiY == im.VerticalResolution) 
                    ir = new Cairo.Rectangle(Convert.ToInt32(r2.X), Convert.ToInt32(r2.Y),
                        im.Width, im.Height);
//                    else
//                        ir = new Cairo.Rectangle(Convert.ToInt32(r2.X), Convert.ToInt32(r2.Y),
//                                           Convert.ToInt32(r2.Width), Convert.ToInt32(r2.Height));
//                    g.DrawImage(im, ir);
                    g.DrawPixbufRect(im, ir);					
//                    g.Clip = saveRegion;
                    g.Restore();
                    break;
                case ImageSizingEnum.FitProportional:
                    double ratioIm = (float)im.Height / (float)im.Width;
                    double ratioR = r2.Height / r2.Width;
                    height = r2.Height;
                    width = r2.Width;
                    if (ratioIm > ratioR)
                    { 
                        // this means the rectangle width must be corrected 
                        width = height * (1 / ratioIm);
                    }
                    else if (ratioIm < ratioR)
                    {  
                        // this means the ractangle height must be corrected 
                        height = width * ratioIm;
                    }
                    r2 = new Cairo.Rectangle(r2.X, r2.Y, width, height);
                    g.DrawPixbufRect(im, r2);
                    break;
                case ImageSizingEnum.Fit:
                default:
                    g.DrawPixbufRect(im, r2);
                    break;
            }
        }

        private void DrawString(PageText pt, Cairo.Context g, Cairo.Rectangle r)
        {
            StyleInfo si = pt.SI;
            string s = pt.Text;
            g.Save();
            layout = Pango.CairoHelper.CreateLayout(g);

//            Font drawFont = null;
//            StringFormat drawFormat = null;
//            Brush drawBrush = null;
			
           
            // STYLE 
//                System.Drawing.FontStyle fs = 0;
//                if (si.FontStyle == FontStyleEnum.Italic)
//                    fs |= System.Drawing.FontStyle.Italic;
            //Pango fonts are scaled to 72dpi, Windows fonts uses 96dpi
            float fontsize = (si.FontSize * 72 / 96);
            var font = Pango.FontDescription.FromString(string.Format("{0} {1}", si.GetFontFamily().Name,  
                       fontsize * PixelsX(1)));
            if (si.FontStyle == FontStyleEnum.Italic)
                font.Style = Pango.Style.Italic;	
//
//                switch (si.TextDecoration)
//                {
//                    case TextDecorationEnum.Underline:
//                        fs |= System.Drawing.FontStyle.Underline;
//                        break;
//                    case TextDecorationEnum.LineThrough:
//                        fs |= System.Drawing.FontStyle.Strikeout;
//                        break;
//                    case TextDecorationEnum.Overline:
//                    case TextDecorationEnum.None:
//                        break;
//                }
				

            // WEIGHT 
//                switch (si.FontWeight)
//                {
//                    case FontWeightEnum.Bold:
//                    case FontWeightEnum.Bolder:
//                    case FontWeightEnum.W500:
//                    case FontWeightEnum.W600:
//                    case FontWeightEnum.W700:
//                    case FontWeightEnum.W800:
//                    case FontWeightEnum.W900:
//                        fs |= System.Drawing.FontStyle.Bold;
//                        break;
//                    default:
//                        break;
//                }
//                try
//                {
//                    drawFont = new Font(si.GetFontFamily(), si.FontSize, fs);   // si.FontSize already in points 
//                }
//                catch (ArgumentException)
//                {
//                    drawFont = new Font("Arial", si.FontSize, fs);   // if this fails we'll let the error pass thru 
//                }
            //font.AbsoluteSize = (int)(PixelsX (si.FontSize));
				
            switch (si.FontWeight)
            {
                case FontWeightEnum.Bold:
                case FontWeightEnum.Bolder:
                case FontWeightEnum.W500:
                case FontWeightEnum.W600:
                case FontWeightEnum.W700:
                case FontWeightEnum.W800:
                case FontWeightEnum.W900:
                    font.Weight = Pango.Weight.Bold;
                    break;
            }
				
            Pango.FontDescription oldfont = layout.FontDescription;
            layout.FontDescription = font;
				
            // ALIGNMENT 
//                drawFormat = new StringFormat();
//                switch (si.TextAlign)
//                {
//                    case TextAlignEnum.Right:
//                        drawFormat.Alignment = StringAlignment.Far;
//                        break;
//                    case TextAlignEnum.Center:
//                        drawFormat.Alignment = StringAlignment.Center;
//                        break;
//                    case TextAlignEnum.Left:
//                    default:
//                        drawFormat.Alignment = StringAlignment.Near;
//                        break;
//                }
				
            switch (si.TextAlign)
            {
                case TextAlignEnum.Right:
                    layout.Alignment = Pango.Alignment.Right;
                    break;
                case TextAlignEnum.Center:
                    layout.Alignment = Pango.Alignment.Center;
                    break;
                case TextAlignEnum.Left:
                default:
                    layout.Alignment = Pango.Alignment.Left;
                    break;
            }
				
            layout.Width = Pango.Units.FromPixels((int)(r.Width - si.PaddingLeft - si.PaddingRight - 2));
//				layout.Width = 	(int)Pango.Units.FromPixels((int)r.Width);
				
            layout.SetText(s);
			
				
//                if (pt.SI.WritingMode == WritingModeEnum.tb_rl)
//                {
//                    drawFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
//                    drawFormat.FormatFlags |= StringFormatFlags.DirectionVertical;
//                }
//                switch (si.VerticalAlign)
//                {
//                    case VerticalAlignEnum.Bottom:
//                        drawFormat.LineAlignment = StringAlignment.Far;
//                        break;
//                    case VerticalAlignEnum.Middle:
//                        drawFormat.LineAlignment = StringAlignment.Center;
//                        break;
//                    case VerticalAlignEnum.Top:
//                    default:
//                        drawFormat.LineAlignment = StringAlignment.Near;
//                        break;
//                }
//               
            Pango.Rectangle logical;
            Pango.Rectangle ink;
            layout.GetExtents(out ink, out logical);
            double height = logical.Height / Pango.Scale.PangoScale;
            double y = 0;
            switch (si.VerticalAlign)
            {
                case VerticalAlignEnum.Top: 
                    y = r.Y + si.PaddingTop;
                    break;
                case VerticalAlignEnum.Middle:
                    y = r.Y + (r.Height - height) / 2;
                    break;
                case VerticalAlignEnum.Bottom:
                    y = r.Y + (r.Height - height) - si.PaddingBottom;
                    break;
            }
            // draw the background 
            DrawBackground(g, r, si);

            // adjust drawing rectangle based on padding 
//                Cairo.Rectangle r2 = new Cairo.Rectangle(r.X + si.PaddingLeft,
//                                               r.Y + si.PaddingTop,
//                                               r.Width - si.PaddingLeft - si.PaddingRight,
//                                               r.Height - si.PaddingTop - si.PaddingBottom);
            Cairo.Rectangle box = new Cairo.Rectangle(
                              r.X + si.PaddingLeft + 1,
                              y,
                              r.Width,
                              r.Height);

            //drawBrush = new SolidBrush(si.Color);
            g.Color = si.Color.ToCairoColor();
//                if (pt.NoClip)   // request not to clip text 
//                {
//                    g.DrawString(pt.Text, drawFont, drawBrush, new PointF(r.Left, r.Top), drawFormat);
//                    //HighlightString(g, pt, new RectangleF(r.Left, r.Top, float.MaxValue, float.MaxValue),drawFont, drawFormat); 
//                }
//                else
//                {
//                    g.DrawString(pt.Text, drawFont, drawBrush, r2, drawFormat);
//                    //HighlightString(g, pt, r2, drawFont, drawFormat); 
//                }
			
            g.MoveTo(box.X, box.Y);
			
            Pango.CairoHelper.ShowLayout(g, layout);
			
            layout.FontDescription = oldfont;
            g.Restore();
        }

        private void DrawBackground(Cairo.Context g, Cairo.Rectangle rect, StyleInfo si)
        {
//            LinearGradientBrush linGrBrush = null;
//            SolidBrush sb = null;
            if (si.BackgroundColor.IsEmpty)
                return;
			
            g.Save();
            Cairo.Color c = si.BackgroundColor.ToCairoColor();
            Cairo.Gradient gradient = null;
			
            if (si.BackgroundGradientType != BackgroundGradientTypeEnum.None &&
                    !si.BackgroundGradientEndColor.IsEmpty)
            {
                Cairo.Color ec = si.BackgroundGradientEndColor.ToCairoColor();

                switch (si.BackgroundGradientType)
                {
                    case BackgroundGradientTypeEnum.LeftRight:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                        gradient = new Cairo.LinearGradient(rect.X, rect.Y, rect.X + rect.Width, rect.Y);
                        break;
                    case BackgroundGradientTypeEnum.TopBottom:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Vertical);
                        gradient = new Cairo.LinearGradient(rect.X, rect.Y, rect.X, rect.Y + rect.Height);
                        break;
                    case BackgroundGradientTypeEnum.Center:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                        throw new NotSupportedException();
//                            break;
                    case BackgroundGradientTypeEnum.DiagonalLeft:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.ForwardDiagonal);
                        gradient = new Cairo.LinearGradient(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                        break;
                    case BackgroundGradientTypeEnum.DiagonalRight:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.BackwardDiagonal);
                        gradient = new Cairo.LinearGradient(rect.X + rect.Width, rect.Y + rect.Height, rect.X, rect.Y);
                        break;
                    case BackgroundGradientTypeEnum.HorizontalCenter:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                        throw new NotSupportedException();
//							break;
                    case BackgroundGradientTypeEnum.VerticalCenter:
//                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Vertical);
                        throw new NotSupportedException();
//							break;
                    default:
                        break;
                }
					
                gradient.AddColorStop(0, c);
                gradient.AddColorStop(1, ec);
            }

            if (gradient != null)
            {
////                    g.FillRectangle(linGrBrush, rect);
                g.FillRectangle(rect, gradient);
                gradient.Destroy();	
            }
            else if (!si.BackgroundColor.IsEmpty)
            {
                g.FillRectangle(rect, c);
//					g.DrawRoundedRectangle (rect, 2, c, 1);
					
//					g.FillRoundedRectangle (rect, 8, c);
            }
            g.Restore();
        }

        private void DrawBorder(PageItem pi, Cairo.Context g, Cairo.Rectangle r)
        {
            if (r.Height <= 0 || r.Width <= 0)      // no bounding box to use 
                return;

            double right = r.X + r.Width;
            double bottom = r.Y + r.Height;
            StyleInfo si = pi.SI;
			
            DrawLine(si.BColorTop.ToCairoColor(), si.BStyleTop, si.BWidthTop, g, r.X, r.Y, right, r.Y);
            DrawLine(si.BColorRight.ToCairoColor(), si.BStyleRight, si.BWidthRight, g, right, r.Y, right, bottom);
            DrawLine(si.BColorLeft.ToCairoColor(), si.BStyleLeft, si.BWidthLeft, g, r.X, r.Y, r.X, bottom);
            DrawLine(si.BColorBottom.ToCairoColor(), si.BStyleBottom, si.BWidthBottom, g, r.X, bottom, right, bottom);
            //if (si.) {
//				g.DrawRoundedRectangle (r, 8, si.BColorTop.ToCairoColor (), 1);
            //}
        }

        #region IRender implementation

        public void RunPages(Pages pgs)
        {
            //TODO : Why Cairo is broken when CurrentThread.CurrentCulture is set to local ?
            //At Linux when CurrentCulture is set to local culture, Cairo rendering is serious broken
            CultureInfo oldci = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			
            try
            {
                foreach (Page p in pgs)
                {
                    ProcessPage(g, p);
                    break;
                }
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldci;
            }
        }

        public void RunPage(Page pgs)
        {
            //TODO : Why Cairo is broken when CurrentThread.CurrentCulture is set to local ?
            //At Linux when CurrentCulture is set to local culture, Cairo rendering is serious broken
            CultureInfo oldci = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			
            try
            {
                ProcessPage(g, pgs);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldci;
            }
        }

		
        #endregion

    }
}

