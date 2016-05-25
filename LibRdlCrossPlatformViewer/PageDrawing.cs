/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2012  Peter Gill <peter@majorsilence.com>

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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using fyiReporting.RDL;

namespace LibRdlCrossPlatformViewer
{
    class PageDrawing : Xwt.Canvas
    {
  
        // During drawing these are set
        float _left;
        float _top;
        float _vScroll;
        float _hScroll;
        float DpiX = 96;
        float DpiY = 96;

        Xwt.Drawing.Context xwtContext;
        System.Drawing.Graphics g;
        System.Drawing.Image gImg;

        public PageDrawing(Xwt.Drawing.Context g, float scale, int width, int height)
        {

            DpiX *= scale;
            DpiY *= scale;

            this.xwtContext = g;

            System.Drawing.Bitmap bm = new Bitmap(width, height);
            gImg= (Image)bm;
            this.g = Graphics.FromImage(gImg);
        }

      
        protected bool IsInputKey(Xwt.KeyEventArgs keyData)
        {
            if (keyData.Key == Xwt.Key.Left ||
                keyData.Key == Xwt.Key.Right ||
                keyData.Key == Xwt.Key.Up ||
                keyData.Key == Xwt.Key.Down ||
                keyData.Key == Xwt.Key.Home ||
                keyData.Key == Xwt.Key.End ||
                keyData.Key == Xwt.Key.PageDown ||
                keyData.Key == Xwt.Key.PageUp)
            {
                return true;
            }

            return false;
        }

          /// <summary>
        /// Draw: accounting for scrolling and zoom factors
        /// </summary>
        /// <param name="_pgs"></param>

        /// <remarks>This the equivalent of RdlViewer.PageDrawing.Draw</remarks>
        public void RunPage(Page _pgs)
        {
                ProcessPage(g, _pgs);
        }

    
        private Rectangle RectFromPoints(Point p1, Point p2)
        {
            Rectangle r = new Rectangle();
            // set the width and x of rectangle
            if (p1.X < p2.X)
            {
                r.X = p1.X;
                r.Width = p2.X - p1.X;
            }
            else
            {
                r.X = p2.X;
                r.Width = p1.X - p2.X;
            }
            // set the height and y of rectangle
            if (p1.Y < p2.Y)
            {
                r.Y = p1.Y;
                r.Height = p2.Y - p1.Y;
            }
            else
            {
                r.Y = p2.Y;
                r.Height = p1.Y - p2.Y;
            }
            return r;
        }


        internal float PixelsX(float x)
        {
            return (float)(x * DpiX / 72.0f);
        }

        internal float PixelsY(float y)
        {
            return (float)(y * DpiY / 72.0f);
        }

        // render all the objects in a page (or any composite object
        private void ProcessPage(System.Drawing.Graphics g, IEnumerable p)
        {
            // TODO: (Peter) Support can grow and can shrink
            foreach (PageItem pi in p)
            {
                if (pi is PageTextHtml)
                {	// PageTextHtml is actually a composite object (just like a page)
                    ProcessHtml(pi as PageTextHtml, g);
                    continue;
                }

                if (pi is PageLine)
                {
                    PageLine pl = pi as PageLine;
                    DrawLine(pl.SI.BColorLeft, pl.SI.BStyleLeft, pl.SI.BWidthLeft,
                        g, PixelsX(pl.X + _left - _hScroll), PixelsY(pl.Y + _top - _vScroll),
                        PixelsX(pl.X2 + _left - _hScroll), PixelsY(pl.Y2 + _top - _vScroll));
                    continue;
                }


                RectangleF rect = new RectangleF(PixelsX(pi.X + _left - _hScroll), PixelsY(pi.Y + _top - _vScroll),
                                                                    PixelsX(pi.W), PixelsY(pi.H));


                if ((pi is PagePolygon) || (pi is PageCurve))
                { // intentionally empty; polygon's rectangles aren't calculated
                }

                if (pi.SI.BackgroundImage != null)
                {	// put out any background image
                    PageImage i = pi.SI.BackgroundImage;
                    DrawImageBackground(i, pi.SI, g, rect);
                }

                if (pi is PageText)
                {
                    // TODO: enable can shrink, can grow
                    // 2005 spec file, page 9, in the text box has
                    // CanGrow and CanShrink
                    PageText pt = pi as PageText;
                    DrawString(pt, g, rect);
                }
                else if (pi is PageImage)
                {
                    PageImage i = pi as PageImage;
                    DrawImage(i, g, rect);
                }
                else if (pi is PageRectangle)
                {
                    this.DrawBackground(g, rect, pi.SI);
                }
                else if (pi is PageEllipse)
                {
                    PageEllipse pe = pi as PageEllipse;
                    DrawEllipse(pe, g, rect);
                }
                else if (pi is PagePie)
                {
                    PagePie pp = pi as PagePie;
                    DrawPie(pp, g, rect);
                }
                else if (pi is PagePolygon)
                {
                    PagePolygon ppo = pi as PagePolygon;
                    FillPolygon(ppo, g, rect);
                }
                else if (pi is PageCurve)
                {
                    PageCurve pc = pi as PageCurve;
                    DrawCurve(pc.SI.BColorLeft, pc.SI.BStyleLeft, pc.SI.BWidthLeft,
                        g, pc.Points, pc.Offset, pc.Tension);
                }


                DrawBorder(pi, g, rect);
            }

            // TO: convert System.Drawing.Graphics to Xwt.Drawing.Context and draw it to this.g
           
            Bitmap bm = new Bitmap(gImg.Width, gImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            g.DrawImage(bm, gImg.Width, gImg.Height);


            // Xwt.Drawing.Image.FromStream does not work.  It crashes with both wpf and gtk
            // As a work around save the image to a temporary file and load it into xwt using the
            // FromFile method.

            System.IO.MemoryStream s = new System.IO.MemoryStream();
            gImg.Save(s, System.Drawing.Imaging.ImageFormat.Png);
            Xwt.Drawing.Image img = Xwt.Drawing.Image.FromStream(s);

            xwtContext.DrawImage(img, new Xwt.Rectangle(0, 0, gImg.Width, gImg.Height), new Xwt.Rectangle(0, 0, gImg.Width, gImg.Height));
            img.Dispose();
            
        }

        private void DrawBackground(Graphics g, System.Drawing.RectangleF rect, StyleInfo si)
        {
            LinearGradientBrush linGrBrush = null;
            SolidBrush sb = null;
            HatchBrush hb = null;
            try
            {
                if (si.BackgroundGradientType != BackgroundGradientTypeEnum.None &&
                    !si.BackgroundGradientEndColor.IsEmpty &&
                    !si.BackgroundColor.IsEmpty)
                {
                    Color c = si.BackgroundColor;
                    Color ec = si.BackgroundGradientEndColor;

                    switch (si.BackgroundGradientType)
                    {
                        case BackgroundGradientTypeEnum.LeftRight:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                            break;
                        case BackgroundGradientTypeEnum.TopBottom:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Vertical);
                            break;
                        case BackgroundGradientTypeEnum.Center:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                            break;
                        case BackgroundGradientTypeEnum.DiagonalLeft:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.ForwardDiagonal);
                            break;
                        case BackgroundGradientTypeEnum.DiagonalRight:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.BackwardDiagonal);
                            break;
                        case BackgroundGradientTypeEnum.HorizontalCenter:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                            break;
                        case BackgroundGradientTypeEnum.VerticalCenter:
                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Vertical);
                            break;
                        default:
                            break;
                    }
                }
                if (si.PatternType != patternTypeEnum.None)
                {
                    switch (si.PatternType)
                    {
                        case patternTypeEnum.BackwardDiagonal:
                            hb = new HatchBrush(HatchStyle.BackwardDiagonal, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.CheckerBoard:
                            hb = new HatchBrush(HatchStyle.LargeCheckerBoard, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.Cross:
                            hb = new HatchBrush(HatchStyle.Cross, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.DarkDownwardDiagonal:
                            hb = new HatchBrush(HatchStyle.DarkDownwardDiagonal, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.DarkHorizontal:
                            hb = new HatchBrush(HatchStyle.DarkHorizontal, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.DiagonalBrick:
                            hb = new HatchBrush(HatchStyle.DiagonalBrick, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.HorizontalBrick:
                            hb = new HatchBrush(HatchStyle.HorizontalBrick, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.LargeConfetti:
                            hb = new HatchBrush(HatchStyle.LargeConfetti, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.OutlinedDiamond:
                            hb = new HatchBrush(HatchStyle.OutlinedDiamond, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.SmallConfetti:
                            hb = new HatchBrush(HatchStyle.SmallConfetti, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.SolidDiamond:
                            hb = new HatchBrush(HatchStyle.SolidDiamond, si.Color, si.BackgroundColor);
                            break;
                        case patternTypeEnum.Vertical:
                            hb = new HatchBrush(HatchStyle.Vertical, si.Color, si.BackgroundColor);
                            break;
                    }
                }

                if (linGrBrush != null)
                {
                    g.FillRectangle(linGrBrush, rect);
                    linGrBrush.Dispose();
                }
                else if (hb != null)
                {
                    g.FillRectangle(hb, rect);
                    hb.Dispose();
                }
                else if (!si.BackgroundColor.IsEmpty)
                {
                    sb = new SolidBrush(si.BackgroundColor);
                    g.FillRectangle(sb, rect);
                    sb.Dispose();
                }
            }
            finally
            {
                if (linGrBrush != null)
                    linGrBrush.Dispose();
                if (sb != null)
                    sb.Dispose();
            }
            return;
        }

        private void DrawBorder(PageItem pi, Graphics g, RectangleF r)
        {
            if (pi.GetType().Name.Equals("PagePie")) return;
            if (r.Height <= 0 || r.Width <= 0)		// no bounding box to use
                return;

            StyleInfo si = pi.SI;

            DrawLine(si.BColorTop, si.BStyleTop, si.BWidthTop, g, r.X, r.Y, r.Right, r.Y);

            DrawLine(si.BColorRight, si.BStyleRight, si.BWidthRight, g, r.Right, r.Y, r.Right, r.Bottom);

            DrawLine(si.BColorLeft, si.BStyleLeft, si.BWidthLeft, g, r.X, r.Y, r.X, r.Bottom);

            DrawLine(si.BColorBottom, si.BStyleBottom, si.BWidthBottom, g, r.X, r.Bottom, r.Right, r.Bottom);

            return;

        }

        private void DrawImage(PageImage pi, Graphics g, RectangleF r)
        {
            Stream strm = null;
            System.Drawing.Image im = null;
            try
            {
                strm = new MemoryStream(pi.ImageData);
                im = System.Drawing.Image.FromStream(strm);
                DrawImageSized(pi, im, g, r);
            }
            finally
            {
                if (strm != null)
                    strm.Close();
                if (im != null)
                    im.Dispose();
            }

        }
        private void DrawImageBackground(PageImage pi, StyleInfo si, Graphics g, RectangleF r)
        {
            Stream strm = null;
            System.Drawing.Image im = null;
            try
            {
                strm = new MemoryStream(pi.ImageData);
                im = System.Drawing.Image.FromStream(strm);

                // http://www.fyireporting.com/forum/viewtopic.php?t=892
                //A.S.> convert pt to px if needed(when printing we need px, when draw preview - pt) 

                RectangleF r2;
                if (g.PageUnit == GraphicsUnit.Pixel)
                {
                    r2 = new RectangleF(r.Left + (si.PaddingLeft * g.DpiX) / 72,
                    r.Top + (si.PaddingTop * g.DpiX) / 72,
                    r.Width - ((si.PaddingLeft + si.PaddingRight) * g.DpiX) / 72,
                    r.Height - ((si.PaddingTop + si.PaddingBottom) * g.DpiX) / 72);
                }
                else
                {
                    // adjust drawing rectangle based on padding
                    r2 = new RectangleF(r.Left + si.PaddingLeft,
                    r.Top + si.PaddingTop,
                    r.Width - si.PaddingLeft - si.PaddingRight,
                    r.Height - si.PaddingTop - si.PaddingBottom);
                }


                int repeatX = 0;
                int repeatY = 0;
                switch (pi.Repeat)
                {
                    case ImageRepeat.Repeat:
                        repeatX = (int)Math.Floor(r2.Width / pi.SamplesW);
                        repeatY = (int)Math.Floor(r2.Height / pi.SamplesH);
                        break;
                    case ImageRepeat.RepeatX:
                        repeatX = (int)Math.Floor(r2.Width / pi.SamplesW);
                        repeatY = 1;
                        break;
                    case ImageRepeat.RepeatY:
                        repeatY = (int)Math.Floor(r2.Height / pi.SamplesH);
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

                float startX = r2.Left;
                float startY = r2.Top;

                Region saveRegion = g.Clip;
                Region clipRegion = new Region(g.Clip.GetRegionData());
                clipRegion.Intersect(r2);
                g.Clip = clipRegion;

                for (int i = 0; i < repeatX; i++)
                {
                    for (int j = 0; j < repeatY; j++)
                    {
                        float currX = startX + i * pi.SamplesW;
                        float currY = startY + j * pi.SamplesH;
                        g.DrawImage(im, new RectangleF(currX, currY, pi.SamplesW, pi.SamplesH));
                    }
                }
                g.Clip = saveRegion;
            }
            finally
            {
                if (strm != null)
                    strm.Close();
                if (im != null)
                    im.Dispose();
            }
        }
        private void DrawImageSized(PageImage pi, Image im, Graphics g, RectangleF r)
        {
            float height, width;		// some work variables
            StyleInfo si = pi.SI;

            // adjust drawing rectangle based on padding

            // http://www.fyireporting.com/forum/viewtopic.php?t=892
            //A.S.> convert pt to px if needed(when printing we need px, when draw preview - pt) 

            RectangleF r2;
            if (g.PageUnit == GraphicsUnit.Pixel)
            {
                r2 = new RectangleF(r.Left + (si.PaddingLeft * g.DpiX) / 72,
                r.Top + (si.PaddingTop * g.DpiX) / 72,
                r.Width - ((si.PaddingLeft + si.PaddingRight) * g.DpiX) / 72,
                r.Height - ((si.PaddingTop + si.PaddingBottom) * g.DpiX) / 72);
            }
            else
            {
                // adjust drawing rectangle based on padding
                r2 = new RectangleF(r.Left + si.PaddingLeft,
                r.Top + si.PaddingTop,
                r.Width - si.PaddingLeft - si.PaddingRight,
                r.Height - si.PaddingTop - si.PaddingBottom);
            }

            Rectangle ir;	// int work rectangle
            ir = new Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                               Convert.ToInt32(r2.Width), Convert.ToInt32(r2.Height));
            switch (pi.Sizing)
            {
                case ImageSizingEnum.AutoSize:
                    // Note: GDI+ will stretch an image when you only provide
                    //  the left/top coordinates.  This seems pretty stupid since
                    //  it results in the image being out of focus even though
                    //  you don't want the image resized.
                    if (g.DpiX == im.HorizontalResolution &&
                        g.DpiY == im.VerticalResolution)
                    {
                        ir = new Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                                                        im.Width, im.Height);
                    }
                    g.DrawImage(im, ir);

                    break;
                case ImageSizingEnum.Clip:
                    Region saveRegion = g.Clip;
                    Region clipRegion = new Region(g.Clip.GetRegionData());
                    clipRegion.Intersect(r2);
                    g.Clip = clipRegion;
                    if (g.DpiX == im.HorizontalResolution &&
                        g.DpiY == im.VerticalResolution)
                    {
                        ir = new Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                                                        im.Width, im.Height);
                    }
                    g.DrawImage(im, ir);
                    g.Clip = saveRegion;
                    break;
                case ImageSizingEnum.FitProportional:
                    float ratioIm = (float)im.Height / (float)im.Width;
                    float ratioR = r2.Height / r2.Width;
                    height = r2.Height;
                    width = r2.Width;
                    if (ratioIm > ratioR)
                    {	// this means the rectangle width must be corrected
                        width = height * (1 / ratioIm);
                    }
                    else if (ratioIm < ratioR)
                    {	// this means the ractangle height must be corrected
                        height = width * ratioIm;
                    }
                    r2 = new RectangleF(r2.X, r2.Y, width, height);
                    g.DrawImage(im, r2);
                    break;
                case ImageSizingEnum.Fit:
                default:
                    g.DrawImage(im, r2);
                    break;
            }

            return;
        }

        private void DrawLine(Color c, BorderStyleEnum bs, float w, Graphics g,
                                float x, float y, float x2, float y2)
        {
            if (bs == BorderStyleEnum.None || c.IsEmpty || w <= 0)	// nothing to draw
                return;

            float tmpW = w;
            if (g.PageUnit == GraphicsUnit.Pixel)
                tmpW = (tmpW * g.DpiX) / 72;
            Pen p = new Pen(c, tmpW);
            try
            {
                p = new Pen(c, w);
                switch (bs)
                {
                    case BorderStyleEnum.Dashed:
                        p.DashStyle = DashStyle.Dash;
                        break;
                    case BorderStyleEnum.Dotted:
                        p.DashStyle = DashStyle.Dot;
                        break;
                    case BorderStyleEnum.Double:
                    case BorderStyleEnum.Groove:
                    case BorderStyleEnum.Inset:
                    case BorderStyleEnum.Solid:
                    case BorderStyleEnum.Outset:
                    case BorderStyleEnum.Ridge:
                    case BorderStyleEnum.WindowInset:
                    default:
                        p.DashStyle = DashStyle.Solid;
                        break;
                }

                g.DrawLine(p, x, y, x2, y2);
            }
            finally
            {
                if (p != null)
                    p.Dispose();
            }

        }

        private void DrawCurve(Color c, BorderStyleEnum bs, float w, Graphics g,
                                PointF[] points, int Offset, float Tension)
        {
            if (bs == BorderStyleEnum.None || c.IsEmpty || w <= 0)	// nothing to draw
                return;

            Pen p = null;
            try
            {
                p = new Pen(c, w);
                switch (bs)
                {
                    case BorderStyleEnum.Dashed:
                        p.DashStyle = DashStyle.Dash;
                        break;
                    case BorderStyleEnum.Dotted:
                        p.DashStyle = DashStyle.Dot;
                        break;
                    case BorderStyleEnum.Double:
                    case BorderStyleEnum.Groove:
                    case BorderStyleEnum.Inset:
                    case BorderStyleEnum.Solid:
                    case BorderStyleEnum.Outset:
                    case BorderStyleEnum.Ridge:
                    case BorderStyleEnum.WindowInset:
                    default:
                        p.DashStyle = DashStyle.Solid;
                        break;
                }
                PointF[] tmp = new PointF[points.Length];
                for (int i = 0; i < points.Length; i++)
                {

                    tmp[i].X = PixelsX(points[i].X + _left - _hScroll);
                    tmp[i].Y = PixelsY(points[i].Y + _top - _vScroll);
                }

                g.DrawCurve(p, tmp, Offset, tmp.Length - 1, Tension);
            }
            finally
            {
                if (p != null)
                    p.Dispose();
            }

        }


        private void ProcessHtml(PageTextHtml pth, System.Drawing.Graphics g)
        {
            pth.Build(g);				// Builds the subobjects that make up the html
            this.ProcessPage(g, pth);
        }

        private void DrawEllipse(PageEllipse pe, Graphics g, RectangleF r)
        {
            StyleInfo si = pe.SI;
            if (!si.BackgroundColor.IsEmpty)
            {
                g.FillEllipse(new SolidBrush(si.BackgroundColor), r);
            }
            if (si.BStyleTop != BorderStyleEnum.None)
            {
                Pen p = new Pen(si.BColorTop, si.BWidthTop);
                switch (si.BStyleTop)
                {
                    case BorderStyleEnum.Dashed:
                        p.DashStyle = DashStyle.Dash;
                        break;
                    case BorderStyleEnum.Dotted:
                        p.DashStyle = DashStyle.Dot;
                        break;
                    case BorderStyleEnum.Double:
                    case BorderStyleEnum.Groove:
                    case BorderStyleEnum.Inset:
                    case BorderStyleEnum.Solid:
                    case BorderStyleEnum.Outset:
                    case BorderStyleEnum.Ridge:
                    case BorderStyleEnum.WindowInset:
                    default:
                        p.DashStyle = DashStyle.Solid;
                        break;
                }
                g.DrawEllipse(p, r);
            }
        }

        private void FillPolygon(PagePolygon pp, Graphics g, RectangleF r)
        {

            StyleInfo si = pp.SI;
            PointF[] tmp = new PointF[pp.Points.Length];
            if (!si.BackgroundColor.IsEmpty)
            //RectangleF(PixelsX(pi.X + _left - _hScroll), PixelsY(pi.Y + _top - _vScroll), 
            //                                                                    PixelsX(pi.W), PixelsY(pi.H))           
            {
                for (int i = 0; i < pp.Points.Length; i++)
                {

                    tmp[i].X = PixelsX(pp.Points[i].X + _left - _hScroll);
                    tmp[i].Y = PixelsY(pp.Points[i].Y + _top - _vScroll);
                }
                g.FillPolygon(new SolidBrush(si.BackgroundColor), tmp);
            }
        }

        private void DrawPie(PagePie pp, Graphics g, RectangleF r)
        {
            StyleInfo si = pp.SI;
            if (!si.BackgroundColor.IsEmpty)
            {
                g.FillPie(new SolidBrush(si.BackgroundColor), (int)r.X, (int)r.Y, (int)r.Width, (int)r.Height, (float)pp.StartAngle, (float)pp.SweepAngle);
            }

            if (si.BStyleTop != BorderStyleEnum.None)
            {
                Pen p = new Pen(si.BColorTop, si.BWidthTop);
                switch (si.BStyleTop)
                {
                    case BorderStyleEnum.Dashed:
                        p.DashStyle = DashStyle.Dash;
                        break;
                    case BorderStyleEnum.Dotted:
                        p.DashStyle = DashStyle.Dot;
                        break;
                    case BorderStyleEnum.Double:
                    case BorderStyleEnum.Groove:
                    case BorderStyleEnum.Inset:
                    case BorderStyleEnum.Solid:
                    case BorderStyleEnum.Outset:
                    case BorderStyleEnum.Ridge:
                    case BorderStyleEnum.WindowInset:
                    default:
                        p.DashStyle = DashStyle.Solid;
                        break;
                }
                g.DrawPie(p, r, pp.StartAngle, pp.SweepAngle);
            }
        }

        private void DrawString(PageText pt, Graphics g, RectangleF r)
        {
            StyleInfo si = pt.SI;
            string s = pt.Text;

            Font drawFont = null;
            StringFormat drawFormat = null;
            Brush drawBrush = null;
            try
            {
                // STYLE
                System.Drawing.FontStyle fs = 0;
                if (si.FontStyle == FontStyleEnum.Italic)
                    fs |= System.Drawing.FontStyle.Italic;

                switch (si.TextDecoration)
                {
                    case TextDecorationEnum.Underline:
                        fs |= System.Drawing.FontStyle.Underline;
                        break;
                    case TextDecorationEnum.LineThrough:
                        fs |= System.Drawing.FontStyle.Strikeout;
                        break;
                    case TextDecorationEnum.Overline:
                    case TextDecorationEnum.None:
                        break;
                }

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
                try
                {
                    drawFont = new Font(si.GetFontFamily(), si.FontSize, fs);	// si.FontSize already in points
                }
                catch (ArgumentException)
                {
                    drawFont = new Font("Arial", si.FontSize, fs);	// if this fails we'll let the error pass thru
                }
                // ALIGNMENT
                drawFormat = new StringFormat();
                switch (si.TextAlign)
                {
                    case TextAlignEnum.Right:
                        drawFormat.Alignment = StringAlignment.Far;
                        break;
                    case TextAlignEnum.Center:
                        drawFormat.Alignment = StringAlignment.Center;
                        break;
                    case TextAlignEnum.Left:
                    default:
                        drawFormat.Alignment = StringAlignment.Near;
                        break;
                }
                if (pt.SI.WritingMode == WritingModeEnum.tb_rl)
                {
                    drawFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    drawFormat.FormatFlags |= StringFormatFlags.DirectionVertical;
                }
                switch (si.VerticalAlign)
                {
                    case VerticalAlignEnum.Bottom:
                        drawFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case VerticalAlignEnum.Middle:
                        drawFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case VerticalAlignEnum.Top:
                    default:
                        drawFormat.LineAlignment = StringAlignment.Near;
                        break;
                }
                // draw the background 
                DrawBackground(g, r, si);

                // adjust drawing rectangle based on padding
                // http://www.fyireporting.com/forum/viewtopic.php?t=892
                //A.S.> convert pt to px if needed(when printing we need px, when draw preview - pt) 
                RectangleF r2;
                if (g.PageUnit == GraphicsUnit.Pixel)
                {
                    r2 = new RectangleF(r.Left + (si.PaddingLeft * g.DpiX) / 72,
                    r.Top + (si.PaddingTop * g.DpiX) / 72,
                    r.Width - ((si.PaddingLeft + si.PaddingRight) * g.DpiX) / 72,
                    r.Height - ((si.PaddingTop + si.PaddingBottom) * g.DpiX) / 72);
                }
                else
                {
                    // adjust drawing rectangle based on padding
                    r2 = new RectangleF(r.Left + si.PaddingLeft,
                    r.Top + si.PaddingTop,
                    r.Width - si.PaddingLeft - si.PaddingRight,
                    r.Height - si.PaddingTop - si.PaddingBottom);
                }

                drawBrush = new SolidBrush(si.Color);
                if (si.TextAlign == TextAlignEnum.Justified)
                {
                    GraphicsExtended.DrawStringJustified(g, pt.Text, drawFont, drawBrush, r2);
                }
                else if (pt.NoClip)	// request not to clip text
                {
                    g.DrawString(pt.Text, drawFont, drawBrush, new PointF(r.Left, r.Top), drawFormat);
                }
                else
                {
                    g.DrawString(pt.Text, drawFont, drawBrush, r2, drawFormat);
                }

            }
            finally
            {
                if (drawFont != null)
                    drawFont.Dispose();
                if (drawFormat != null)
                    drawFont.Dispose();
                if (drawBrush != null)
                    drawBrush.Dispose();
            }
        }

    }
}
