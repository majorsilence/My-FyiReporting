using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

using fyiReporting.RDL;

namespace LibRdlCrossPlatformViewer
{
    public class RenderXwt
    {
        Xwt.Drawing.Context g;
        Xwt.Drawing.TextLayout layout;

        float dpiX = 96;
        float dpiY = 96;

        public RenderXwt(Xwt.Drawing.Context g) : this(g, 1.0f)
        {
        }

        public RenderXwt(Xwt.Drawing.Context g, float scale)
		{
			this.g = g;
            this.layout = new Xwt.Drawing.TextLayout(this.g);
			
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

        private void ProcessPage(Xwt.Drawing.Context g, IEnumerable p)
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
               
                    Xwt.Drawing.Color bcolorleft = XwtColor.SystemColorToXwtColor(pl.SI.BColorLeft);

                    DrawLine(bcolorleft, pl.SI.BStyleLeft, pl.SI.BWidthLeft,
                        g, PixelsX(pl.X), PixelsY(pl.Y), PixelsX(pl.X2), PixelsY(pl.Y2)
                    );
                    continue;
                }

                Xwt.Rectangle rect = new Xwt.Rectangle(PixelsX(pi.X), PixelsY(pi.Y), PixelsX(pi.W), PixelsY(pi.H));
            

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
               
                DrawBorder(pi, g, rect);
            }
        }

        private void ProcessHtml(PageTextHtml pth, Xwt.Drawing.Context g)
        {
            
            //            pth.Build(g);            // Builds the subobjects that make up the html 
            this.ProcessPage(g, pth);
        }

        private void DrawLine(Xwt.Drawing.Color c, BorderStyleEnum bs, float w, Xwt.Drawing.Context g, double x, double y, double x2, double y2)
        {
            if (bs == BorderStyleEnum.None //|| c.IsEmpty 
                || w <= 0)   // nothing to draw 
                return;

            g.Save();
            //          Pen p = null;  
            //          p = new Pen(c, w);
            g.SetColor(c);
            g.SetLineWidth(w);
            switch (bs)
            {
                case BorderStyleEnum.Dashed:
                    //	                p.DashStyle = DashStyle.Dash;
                    g.SetLineDash(0.0, new double[] { 2, 1 });
                    break;
                case BorderStyleEnum.Dotted:
                    //                        p.DashStyle = DashStyle.Dot;
                    g.SetLineDash(0.0, new double[] { 1 });
                    break;
                case BorderStyleEnum.Double:
                case BorderStyleEnum.Groove:
                case BorderStyleEnum.Inset:
                case BorderStyleEnum.Solid:
                case BorderStyleEnum.Outset:
                case BorderStyleEnum.Ridge:
                case BorderStyleEnum.WindowInset:
                default:
                    g.SetLineDash(0.0, new double[] { });
                    break;
            }

            g.MoveTo(x, y);
            g.LineTo(x2, y2);
            g.Stroke();

            g.Restore();
        }

        private void DrawImage(PageImage pi, Xwt.Drawing.Context g, Xwt.Rectangle r)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(pi.ImageData);
            Xwt.Drawing.Image im = null;
            try
            {
                im = Xwt.Drawing.Image.FromStream(ms);
                DrawImageSized(pi, im, g, r);
            }
            finally
            {

                if (im != null)
                    im.Dispose();
            }

        }

        private void DrawImageSized(PageImage pi, Xwt.Drawing.Image im, Xwt.Drawing.Context g, Xwt.Rectangle r)
        {
            double height, width;      // some work variables 
            StyleInfo si = pi.SI;

            Xwt.Rectangle r2 = new Xwt.Rectangle(r.X + PixelsX(si.PaddingLeft),
                                                      r.Y + PixelsY(si.PaddingTop),
                                                      r.Width - PixelsX(si.PaddingLeft + si.PaddingRight),
                                                      r.Height - PixelsY(si.PaddingTop + si.PaddingBottom));

            Xwt.Rectangle ir;   // int work rectangle 
            switch (pi.Sizing)
            {
                case ImageSizingEnum.AutoSize:

                    float imwidth = PixelsX( (float)im.Size.Width);
                    float imheight = PixelsX( (float)im.Size.Height);
                    ir = new Xwt.Rectangle(Convert.ToInt32(r2.X), Convert.ToInt32(r2.Y),
                                                    imwidth, imheight);

                    im.Scale((int)r2.Width, (int)r2.Height);
                    g.DrawImage(im, ir);
 
                    break;
                case ImageSizingEnum.Clip:
                    g.Save();
                    g.Rectangle(r2);
                    g.Clip();


                    ir = new Xwt.Rectangle(Convert.ToInt32(r2.X), Convert.ToInt32(r2.Y),
                                                    im.Size.Width, im.Size.Height);

                    g.DrawImage(im, ir);
                    g.Restore();
                    break;
                case ImageSizingEnum.FitProportional:
                    double ratioIm = (float)im.Size.Height / (float)im.Size.Width;
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
                    r2 = new Xwt.Rectangle(r2.X, r2.Y, width, height);
                    g.DrawImage(im, r2);
                    break;
                case ImageSizingEnum.Fit:
                default:
                    g.DrawImage(im, r2);
                    break;
            }
        }

        private void DrawString(PageText pt, Xwt.Drawing.Context g, Xwt.Rectangle r)
        {
            StyleInfo si = pt.SI;
            string s = pt.Text;
            g.Save();
            layout = new Xwt.Drawing.TextLayout(g);

            float fontsize = (si.FontSize * 72 / 96);

            layout.Font.WithFamily(si.GetFontFamily().Name);
            layout.Font.WithSize(fontsize * PixelsX(1));

            if (si.FontStyle == FontStyleEnum.Italic)
                layout.Font.WithStyle(Xwt.Drawing.FontStyle.Italic);
          
            switch (si.FontWeight)
            {
                case FontWeightEnum.Bold:
                case FontWeightEnum.Bolder:
                case FontWeightEnum.W500:
                case FontWeightEnum.W600:
                case FontWeightEnum.W700:
                case FontWeightEnum.W800:
                case FontWeightEnum.W900:
                    layout.Font.WithWeight(Xwt.Drawing.FontWeight.Bold);
                    break;
            }

            // TODO: Fix Alignment
            //switch (si.TextAlign)
            //{
            //    case TextAlignEnum.Right:
            //        layout.Alignment = Pango.Alignment.Right;
            //        break;
            //    case TextAlignEnum.Center:
            //        layout.Alignment = Pango.Alignment.Center;
            //        break;
            //    case TextAlignEnum.Left:
            //    default:
            //        layout.Alignment = Pango.Alignment.Left;
            //        break;
            //}

            // TODO: Fix with
            //layout.Width = Pango.Units.FromPixels((int)(r.Width - si.PaddingLeft - si.PaddingRight - 2));

            layout.Text = s;


                
            Xwt.Rectangle logical;
            Xwt.Rectangle ink;

            // TODO: Fix
            //layout.GetExtents(out ink, out logical);    
            //double height = logical.Height / Pango.Scale.PangoScale;

            double y = 0;
            //switch (si.VerticalAlign)
            //{
            //    case VerticalAlignEnum.Top:
            //        y = r.Y + si.PaddingTop;
            //        break;
            //    case VerticalAlignEnum.Middle:
            //        y = r.Y + (r.Height - height) / 2;
            //        break;
            //    case VerticalAlignEnum.Bottom:
            //        y = r.Y + (r.Height - height) - si.PaddingBottom;
            //        break;
            //}

            // draw the background 
            DrawBackground(g, r, si);

            Xwt.Rectangle box = new Xwt.Rectangle(
                                   r.X + si.PaddingLeft + 1,
                                   y,
                                   r.Width,
                                   r.Height);

            Xwt.Drawing.Color sicolor = XwtColor.SystemColorToXwtColor(si.Color);
            g.SetColor(sicolor);
           
            g.MoveTo(box.X, box.Y);
           
            g.Restore();
        }

        private void DrawBackground(Xwt.Drawing.Context g, Xwt.Rectangle rect, StyleInfo si)
        {
            //            LinearGradientBrush linGrBrush = null;
            //            SolidBrush sb = null;
            if (si.BackgroundColor.IsEmpty)
                return;

            g.Save();

            Xwt.Drawing.Color c = XwtColor.SystemColorToXwtColor(si.BackgroundColor);

            Xwt.Drawing.Gradient gradient = null;

            if (si.BackgroundGradientType != BackgroundGradientTypeEnum.None &&
                !si.BackgroundGradientEndColor.IsEmpty)
            {
                Xwt.Drawing.Color ec = XwtColor.SystemColorToXwtColor(si.BackgroundGradientEndColor);

                switch (si.BackgroundGradientType)
                {
                    case BackgroundGradientTypeEnum.LeftRight:
                        //                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                        gradient = new Xwt.Drawing.LinearGradient(rect.X, rect.Y, rect.X + rect.Width, rect.Y);
                        break;
                    case BackgroundGradientTypeEnum.TopBottom:
                        //                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Vertical);
                        gradient = new Xwt.Drawing.LinearGradient(rect.X, rect.Y, rect.X, rect.Y + rect.Height);
                        break;
                    case BackgroundGradientTypeEnum.Center:
                        //                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.Horizontal);
                        throw new NotSupportedException();
                    //                            break;
                    case BackgroundGradientTypeEnum.DiagonalLeft:
                        //                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.ForwardDiagonal);
                        gradient = new Xwt.Drawing.LinearGradient(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                        break;
                    case BackgroundGradientTypeEnum.DiagonalRight:
                        //                            linGrBrush = new LinearGradientBrush(rect, c, ec, LinearGradientMode.BackwardDiagonal);
                        gradient = new Xwt.Drawing.LinearGradient(rect.X + rect.Width, rect.Y + rect.Height, rect.X, rect.Y);
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
                // TODO: Fix
                //g.FillRectangle(rect, gradient);
            }
            else if (!si.BackgroundColor.IsEmpty)
            {
                // TODO: Fix
                //g.FillRectangle(rect, c);

            }
            g.Restore();
        }

        private void DrawBorder(PageItem pi, Xwt.Drawing.Context g, Xwt.Rectangle r)
        {
            if (r.Height <= 0 || r.Width <= 0)      // no bounding box to use 
                return;

            double right = r.X + r.Width;
            double bottom = r.Y + r.Height;
            StyleInfo si = pi.SI;

  
            Xwt.Drawing.Color ec = XwtColor.SystemColorToXwtColor(si.BackgroundGradientEndColor);

            DrawLine(XwtColor.SystemColorToXwtColor(si.BColorTop), si.BStyleTop, si.BWidthTop, g, r.X, r.Y, right, r.Y);
            DrawLine(XwtColor.SystemColorToXwtColor(si.BColorRight), si.BStyleRight, si.BWidthRight, g, right, r.Y, right, bottom);
            DrawLine(XwtColor.SystemColorToXwtColor(si.BColorLeft), si.BStyleLeft, si.BWidthLeft, g, r.X, r.Y, r.X, bottom);
            DrawLine(XwtColor.SystemColorToXwtColor(si.BColorBottom), si.BStyleBottom, si.BWidthBottom, g, r.X, bottom, right, bottom);

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
