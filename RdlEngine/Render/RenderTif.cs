/* ====================================================================
   Copyright (C) 2008  samuelchoi - donated to RDL Project
   Copyright (C) 2004-2008  fyiReporting Software, LLC  -- most of the drawing originally came from Viewer

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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Runtime.InteropServices;

namespace fyiReporting.RDL
{

    ///<summary> 
    /// Renders a report to TIF.   This is a page oriented formatting renderer. 
    ///</summary> 
    internal class RenderTif : IPresent
    {
        Report r;               // report 
        Stream tw;               // where the output is going 

        Bitmap _tif;

        float DpiX;
        float DpiY;

        bool _RenderColor;

        public RenderTif(Report rep, IStreamGen sg)
        {
            r = rep;
            tw = sg.GetStream();
            _RenderColor = true;
        }
        /// <summary>
        /// Set RenderColor to false if you want to create a fax compatible tiff in black and white
        /// </summary>
        internal bool RenderColor
        {
            get { return _RenderColor; }
            set { _RenderColor = value; }
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
        }

        public void End()
        {
        }

        public void RunPages(Pages pgs)   // this does all the work 
        {
            int pageNo = 1;

            // STEP: processing a page. 
            foreach (Page p in pgs)
            {
                System.Drawing.Bitmap bm = CreateObjectBitmap();
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);

                g.PageUnit = GraphicsUnit.Pixel;
                g.ScaleTransform(1, 1);

                DpiX = g.DpiX;
                DpiY = g.DpiY;

                // STEP: Fill backgroup 
                g.FillRectangle(Brushes.White, 0F, 0F, (float)bm.Width, (float)bm.Height);

                // STEP: draw page to bitmap 
                ProcessPage(g, p);

                // STEP: 
                System.Drawing.Bitmap bm2 = ConvertToBitonal(bm);

                if (pageNo == 1)
                    _tif = bm2;

                SaveBitmap(_tif, bm2, tw, pageNo);

                pageNo++;
            }

            if (_tif != null)
            {
                // STEP: prepare encoder parameters 
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(
                    System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush
                );

                // STEP: 
                _tif.SaveAdd(encoderParams);
            }

            return;
        }

        private void ProcessPage(Graphics g, IEnumerable p)
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
                        pl.SI.BColorLeft, pl.SI.BStyleLeft, pl.SI.BWidthLeft,
                        g, PixelsX(pl.X), PixelsY(pl.Y), PixelsX(pl.X2), PixelsY(pl.Y2)
                    );
                    continue;
                }

                RectangleF rect = new RectangleF(PixelsX(pi.X), PixelsY(pi.Y), PixelsX(pi.W), PixelsY(pi.H));

                if (pi.SI.BackgroundImage != null)
                {   // put out any background image 
                    PageImage i = pi.SI.BackgroundImage;
                    DrawImage(i, g, rect);
                }

                if (pi is PageText)
                {
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
        }

        private void ProcessHtml(PageTextHtml pth, System.Drawing.Graphics g)
        {
            pth.Build(g);            // Builds the subobjects that make up the html 
            this.ProcessPage(g, pth);
        }

        private void DrawLine(Color c, BorderStyleEnum bs, float w, Graphics g, float x, float y, float x2, float y2)
        {
            if (bs == BorderStyleEnum.None || c.IsEmpty || w <= 0)   // nothing to draw 
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

                    tmp[i].X = PixelsX(points[i].X);
                    tmp[i].Y = PixelsY(points[i].Y);
                }

                g.DrawCurve(p, tmp, Offset, tmp.Length - 1, Tension);
            }
            finally
            {
                if (p != null)
                    p.Dispose();
            }

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
            {
                for (int i = 0; i < pp.Points.Length; i++)
                {
                    tmp[i].X = PixelsX(pp.Points[i].X);
                    tmp[i].Y = PixelsY(pp.Points[i].Y);
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
                    drawFont = new Font(si.GetFontFamily(), si.FontSize, fs);   // si.FontSize already in points 
                }
                catch (ArgumentException)
                {
                    drawFont = new Font("Arial", si.FontSize, fs);   // if this fails we'll let the error pass thru 
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
                RectangleF r2 = new RectangleF(r.Left + si.PaddingLeft,
                                               r.Top + si.PaddingTop,
                                               r.Width - si.PaddingLeft - si.PaddingRight,
                                               r.Height - si.PaddingTop - si.PaddingBottom);

                drawBrush = new SolidBrush(si.Color);
                if (pt.NoClip)   // request not to clip text 
                {
                    g.DrawString(pt.Text, drawFont, drawBrush, new PointF(r.Left, r.Top), drawFormat);
                    //HighlightString(g, pt, new RectangleF(r.Left, r.Top, float.MaxValue, float.MaxValue),drawFont, drawFormat); 
                }
                else
                {
                    g.DrawString(pt.Text, drawFont, drawBrush, r2, drawFormat);
                    //HighlightString(g, pt, r2, drawFont, drawFormat); 
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

        private void DrawImageSized(PageImage pi, System.Drawing.Image im, System.Drawing.Graphics g, System.Drawing.RectangleF r)
        {
            float height, width;      // some work variables 
            StyleInfo si = pi.SI;

            // adjust drawing rectangle based on padding 
            System.Drawing.RectangleF r2 = new System.Drawing.RectangleF(r.Left + PixelsX(si.PaddingLeft),
                r.Top + PixelsY(si.PaddingTop),
                r.Width - PixelsX(si.PaddingLeft + si.PaddingRight),
                r.Height - PixelsY(si.PaddingTop + si.PaddingBottom));

            System.Drawing.Rectangle ir;   // int work rectangle 
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
                        ir = new System.Drawing.Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                                                        im.Width, im.Height);
                    }
                    else
                        ir = new System.Drawing.Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                                           Convert.ToInt32(r2.Width), Convert.ToInt32(r2.Height));
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
                        ir = new System.Drawing.Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                                                        im.Width, im.Height);
                    }
                    else
                        ir = new System.Drawing.Rectangle(Convert.ToInt32(r2.Left), Convert.ToInt32(r2.Top),
                                           Convert.ToInt32(r2.Width), Convert.ToInt32(r2.Height));
                    g.DrawImage(im, ir);
                    g.Clip = saveRegion;
                    break;
                case ImageSizingEnum.FitProportional:
                    float ratioIm = (float)im.Height / (float)im.Width;
                    float ratioR = r2.Height / r2.Width;
                    height = r2.Height;
                    width = r2.Width;
                    if (ratioIm > ratioR)
                    {   // this means the rectangle width must be corrected 
                        width = height * (1 / ratioIm);
                    }
                    else if (ratioIm < ratioR)
                    {   // this means the ractangle height must be corrected 
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

        private void DrawBackground(Graphics g, System.Drawing.RectangleF rect, StyleInfo si)
        {
            LinearGradientBrush linGrBrush = null;
            SolidBrush sb = null;
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

                if (linGrBrush != null)
                {
                    g.FillRectangle(linGrBrush, rect);
                    linGrBrush.Dispose();
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
            if (r.Height <= 0 || r.Width <= 0)      // no bounding box to use 
                return;

            StyleInfo si = pi.SI;

            DrawLine(si.BColorTop, si.BStyleTop, si.BWidthTop, g, r.X, r.Y, r.Right, r.Y);

            DrawLine(si.BColorRight, si.BStyleRight, si.BWidthRight, g, r.Right, r.Y, r.Right, r.Bottom);

            DrawLine(si.BColorLeft, si.BStyleLeft, si.BWidthLeft, g, r.X, r.Y, r.X, r.Bottom);

            DrawLine(si.BColorBottom, si.BStyleBottom, si.BWidthBottom, g, r.X, r.Bottom, r.Right, r.Bottom);

            return;

        }

        #region TIFF image handler
        private Bitmap CreateObjectBitmap()
        {
            float dpiX = 200F;
            float dpiY = 200F;

            Bitmap bm = new System.Drawing.Bitmap(
                Convert.ToInt32(r.ReportDefinition.PageWidth.Size / 2540F * dpiX),
                Convert.ToInt32(r.ReportDefinition.PageHeight.Size / 2540F * dpiY)
            );

            bm.MakeTransparent(Color.White);
            bm.SetResolution(dpiX, dpiY);

            return bm;
        }

        private Bitmap ConvertToBitonal(Bitmap original)
        {
            if (_RenderColor)
                return original;

            Bitmap source = null;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert 
            if (original.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(original, 0, 0);
                }
            }
            else
            {
                source = original;
            }

            // Lock source bitmap in memory 
            BitmapData sourceData = source.LockBits(new System.Drawing.Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array 
            int imageSize = sourceData.Stride * sourceData.Height;
            byte[] sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap 
            source.UnlockBits(sourceData);

            // Create destination bitmap 
            Bitmap destination = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);

            // Set resolution 
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            // Lock destination bitmap in memory 
            BitmapData destinationData = destination.LockBits(new System.Drawing.Rectangle(0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            // Create destination buffer 
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];

            int sourceIndex = 0;
            int destinationIndex = 0;
            int pixelTotal = 0;
            byte destinationValue = 0;
            int pixelValue = 128;
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;

            // Iterate lines 
            for (int y = 0; y < height; y++)
            {
                sourceIndex = y * sourceData.Stride;
                destinationIndex = y * destinationData.Stride;
                destinationValue = 0;
                pixelValue = 128;

                // Iterate pixels 
                for (int x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values) 
                    pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    sourceIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap 
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap 
            destination.UnlockBits(destinationData);

            // Return 
            return destination;
        }

        private void SaveBitmap(Bitmap tif, Bitmap bm, Stream st, int pageNo)
        {
            if (pageNo == 1)
            {
                // Handling saving first page 

                // STEP: Prepare ImageCodecInfo for saving 
                ImageCodecInfo info = null;

                foreach (ImageCodecInfo i in ImageCodecInfo.GetImageEncoders())
                {
                    if (i.MimeType == "image/tiff")
                    {
                        info = i;
                        break;
                    }
                }

                // STEP: Prepare parameters 
                EncoderParameters encoderParams = new EncoderParameters(2);

                encoderParams.Param[0] = new EncoderParameter(
                    System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame
                );

                encoderParams.Param[1] = new EncoderParameter(
                    System.Drawing.Imaging.Encoder.Compression, 
                    (long)(_RenderColor? EncoderValue.CompressionLZW: EncoderValue.CompressionCCITT3)
                );

                // STEP: Save bitmap 
                tif.Save(st, info, encoderParams);
            }
            else
            {
                // STEP: Prepare parameters 
                EncoderParameters encoderParams = new EncoderParameters(1);

                encoderParams.Param[0] = new EncoderParameter(
                    System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage
                );

                // STEP: Save bitmap 
                tif.SaveAdd(bm, encoderParams);
            }
        }
        #endregion

        internal float PixelsX(float x)
        {
            return (x * DpiX / 72.0f);
        }

        internal float PixelsY(float y)
        {
            return (y * DpiY / 72.0f);
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

        // Tables               // Report item table 
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

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)            // called first 
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

        public void MatrixEnd(Matrix m, Row r)            // called last 
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

        public bool RectangleStart(RDL.Rectangle rect, Row r)
        {
            return true;
        }

        public void RectangleEnd(RDL.Rectangle rect, Row r)
        {
        }

        public void Subreport(Subreport s, Row r)
        {
        }

        public void GroupingStart(Grouping g)         // called at start of grouping 
        {
        }
        public void GroupingInstanceStart(Grouping g)   // called at start for each grouping instance 
        {
        }
        public void GroupingInstanceEnd(Grouping g)   // called at start for each grouping instance 
        {
        }
        public void GroupingEnd(Grouping g)         // called at end of grouping 
        {
        }
    }
}
