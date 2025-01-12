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
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#if DRAWINGCOMPAT
using Drawing = Majorsilence.Drawing;
#else
using Drawing = System.Drawing;
#endif


namespace fyiReporting.RDL
{
    ///<summary>
    /// Bar chart definition and processing.
    ///</summary>
    internal class ChartBar : ChartBase
    {
        int _GapSize = 6;	//  TODO: hard code for now

        internal ChartBar(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX, Expression _ToolTipYFormat, Expression _ToolTipXFormat)
            : base(r, row, c, m, showTooltips, showTooltipsX, _ToolTipYFormat, _ToolTipXFormat)
        {
        }

        override internal async Task Draw(Report rpt)
        {
            CreateSizedBitmap();

#if !DRAWINGCOMPAT
            //AJM GJL 14082008 Using Vector Graphics
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (Drawing.Graphics g1 = Drawing.Graphics.FromImage(_bm))
                {
                    _aStream = new System.IO.MemoryStream();
                    IntPtr HDC = g1.GetHdc();
                    _mf = new Drawing.Imaging.Metafile(_aStream, HDC,
                        new Drawing.RectangleF(0, 0, _bm.Width, _bm.Height), Drawing.Imaging.MetafileFrameUnit.Pixel);
                    g1.ReleaseHdc(HDC);
                }
            }


            using (Drawing.Graphics g = Drawing.Graphics.FromImage(_mf != null ? _mf : _bm))
#else
            using (Drawing.Graphics g = Drawing.Graphics.FromImage(_bm))
#endif
            {
                // 06122007AJM Used to Force Higher Quality
                g.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.None;
                g.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality;

                // Adjust the top margin to depend on the title height
                Drawing.Size titleSize = await DrawTitleMeasure(rpt, g, ChartDefn.Title);
                Layout.TopMargin = titleSize.Height;

                double max = 0, min = 0;    // Get the max and min values
                                            // 20022008 AJM GJL - Now requires Y axis identifier
                (max, min) = await GetValueMaxMin(rpt, max, min, 0, 1);

                await DrawChartStyle(rpt, g);

                // Draw title; routine determines if necessary
                await DrawTitle(rpt, g, ChartDefn.Title, new Drawing.Rectangle(0, 0, _bm.Width, Layout.TopMargin));

                // Adjust the left margin to depend on the Category Axis
                Drawing.Size caSize = await CategoryAxisSize(rpt, g);
                Layout.LeftMargin = caSize.Width;

                // Adjust the bottom margin to depend on the Value Axis
                Drawing.Size vaSize = await ValueAxisSize(rpt, g, min, max);
                Layout.BottomMargin = vaSize.Height;

                // Draw legend
                Drawing.Rectangle lRect = await DrawLegend(rpt, g, false, true);
                // 20022008 AJM GJL - Requires Rpt and Graphics
                AdjustMargins(lRect, rpt, g);       // Adjust margins based on legend.

                // Draw Plot area
                await DrawPlotAreaStyle(rpt, g, lRect);

                // Draw Value Axis
                if (vaSize.Width > 0)   // If we made room for the axis - we need to draw it
                    await DrawValueAxis(rpt, g, min, max,
                        new Drawing.Rectangle(Layout.LeftMargin, _bm.Height - Layout.BottomMargin, _bm.Width - Layout.LeftMargin - Layout.RightMargin, vaSize.Height), Layout.TopMargin, _bm.Height - Layout.BottomMargin);

                // Draw Category Axis
                if (caSize.Height > 0)
                    await DrawCategoryAxis(rpt, g,
                        new Drawing.Rectangle(Layout.LeftMargin - caSize.Width, Layout.TopMargin, caSize.Width, _bm.Height - Layout.TopMargin - Layout.BottomMargin));

                if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), await _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked)
                    await DrawPlotAreaStacked(rpt, g, min, max);
                else if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), await _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
                    await DrawPlotAreaPercentStacked(rpt, g);
                else
                    await DrawPlotAreaPlain(rpt, g, min, max);

                await DrawLegend(rpt, g, false, false);     // after the plot is drawn
            }
        }

        async Task DrawPlotAreaPercentStacked(Report rpt, Drawing.Graphics g)
        {
            int barsNeeded = CategoryCount;
            int gapsNeeded = CategoryCount * 2;

            // Draw Plot area data
            double max = 1;

            int heightBar = (int)((Layout.PlotArea.Height - (gapsNeeded * _GapSize)) / barsNeeded);
            int maxBarWidth = (int)(Layout.PlotArea.Width);

            // Loop thru calculating all the data points
            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                int barLoc = (int)(Layout.PlotArea.Top + ((iRow - 1) * ((double)(Layout.PlotArea.Height) / CategoryCount)));
                barLoc += _GapSize; // space before series

                double sum = 0;
                for (int iCol = 1; iCol <= SeriesCount; iCol++)
                {
                    sum += await GetDataValue(rpt, iRow, iCol);
                }
                double v = 0;
                int saveX = 0;
                double t = 0;
                for (int iCol = 1; iCol <= SeriesCount; iCol++)
                {
                    t = await GetDataValue(rpt, iRow, iCol);
                    v += t;

                    int x = (int)((Math.Min(v / sum, max) / max) * maxBarWidth);

                    Drawing.Rectangle rect;
                    rect = new Drawing.Rectangle(Layout.PlotArea.Left + saveX, barLoc, x - saveX, heightBar);

                    await DrawColumnBar(rpt, g,
                        await GetSeriesBrush(rpt, iRow, iCol),
                         rect, iRow, iCol);

                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + t.ToString(_tooltipYFormat) + "|X:" + (int)rect.X + "|Y:" + (int)rect.Y + "|W:" + rect.Width + "|H:" + rect.Height;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }

                    saveX = x;
                }
            }

            return;
        }

        async Task DrawPlotAreaPlain(Report rpt, Drawing.Graphics g, double min, double max)
        {
            int barsNeeded = SeriesCount * CategoryCount;
            int gapsNeeded = CategoryCount * 2;

            // Draw Plot area data
            int heightBar = (int)((Layout.PlotArea.Height - (gapsNeeded * _GapSize)) / barsNeeded);
            int maxBarWidth = (int)(Layout.PlotArea.Width);

            //int barLoc=Layout.LeftMargin;
            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                int barLoc = (int)(Layout.PlotArea.Top + ((iRow - 1) * ((double)(Layout.PlotArea.Height) / CategoryCount)));
                barLoc += _GapSize; // space before series
                for (int iCol = 1; iCol <= SeriesCount; iCol++)
                {
                    double v = await this.GetDataValue(rpt, iRow, iCol);
                    int x = (int)(((Math.Min(v, max) - min) / (max - min)) * maxBarWidth);

                    await DrawColumnBar(rpt, g, await GetSeriesBrush(rpt, iRow, iCol),
                        new Drawing.Rectangle(Layout.PlotArea.Left, barLoc, x, heightBar), iRow, iCol);
                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + v.ToString(_tooltipYFormat) + "|X:" + (int)Layout.PlotArea.Left + "|Y:" + (int)(barLoc) + "|W:" + x + "|H:" + heightBar;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }
                    barLoc += heightBar;
                }
            }

            return;
        }

        async Task DrawPlotAreaStacked(Report rpt, Drawing.Graphics g, double min, double max)
        {
            int barsNeeded = CategoryCount;
            int gapsNeeded = CategoryCount * 2;

            int heightBar = (int)((Layout.PlotArea.Height - (gapsNeeded * _GapSize)) / barsNeeded);
            int maxBarWidth = (int)(Layout.PlotArea.Width);

            // Loop thru calculating all the data points
            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                int barLoc = (int)(Layout.PlotArea.Top + ((iRow - 1) * ((double)(Layout.PlotArea.Height) / CategoryCount)));
                barLoc += _GapSize; // space before series

                double v = 0;
                double t = 0;
                int saveX = 0;
                for (int iCol = 1; iCol <= SeriesCount; iCol++)
                {
                    t = await GetDataValue(rpt, iRow, iCol);
                    v += t;

                    int x = (int)(((Math.Min(v, max) - min) / (max - min)) * maxBarWidth);

                    Drawing.Rectangle rect;
                    rect = new Drawing.Rectangle(Layout.PlotArea.Left + saveX, barLoc, x - saveX, heightBar);

                    await DrawColumnBar(rpt, g, await GetSeriesBrush(rpt, iRow, iCol), rect, iRow, iCol);

                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + v.ToString(_tooltipYFormat) + "|X:" + (int)rect.X + "|Y:" + (int)rect.Y + "|W:" + rect.Width + "|H:" + rect.Height;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }
                    saveX = x;
                }
            }

            return;
        }

        // Calculate the size of the category axis
        async Task<Drawing.Size> CategoryAxisSize(Report rpt, Drawing.Graphics g)
        {
            _LastCategoryWidth = 0;

            Drawing.Size size = Drawing.Size.Empty;
            if (this.ChartDefn.CategoryAxis == null)
                return size;
            Axis a = this.ChartDefn.CategoryAxis.Axis;
            if (a == null)
                return size;
            Style s = a.Style;

            // Measure the title
            size = await DrawTitleMeasure(rpt, g, a.Title);

            if (!a.Visible)     // don't need to calculate the height
                return size;

            // Calculate the tallest category name
            TypeCode tc;
            int maxWidth = 0;
            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                (object v, tc) = await this.GetCategoryValue(rpt, iRow);
                Drawing.Size tSize;
                if (s == null)
                    tSize = await Style.MeasureStringDefaults(rpt, g, v, tc, null, int.MaxValue);

                else
                    tSize = await s.MeasureString(rpt, g, v, tc, null, int.MaxValue);

                if (tSize.Width > maxWidth)
                    maxWidth = tSize.Width;

                if (iRow == CategoryCount)
                    _LastCategoryWidth = tSize.Width;
            }

            // Add on the widest category name
            size.Width += maxWidth;
            return size;
        }

        // DrawCategoryAxis 
        async Task DrawCategoryAxis(Report rpt, Drawing.Graphics g, Drawing.Rectangle rect)
        {
            if (this.ChartDefn.CategoryAxis == null)
                return;
            Axis a = this.ChartDefn.CategoryAxis.Axis;
            if (a == null)
                return;
            Style s = a.Style;

            Drawing.Size tSize = await DrawTitleMeasure(rpt, g, a.Title);
            await DrawTitle(rpt, g, a.Title,
                new Drawing.Rectangle(rect.Left, rect.Top, tSize.Width, rect.Height));

            int drawHeight = rect.Height / CategoryCount;
            TypeCode tc;
            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                (object v, tc) = await this.GetCategoryValue(rpt, iRow);

                int drawLoc = (int)(rect.Top + ((iRow - 1) * ((double)rect.Height / CategoryCount)));

                // Draw the category text
                if (a.Visible)
                {
                    Drawing.Rectangle drawRect = new Drawing.Rectangle(rect.Left + tSize.Width, drawLoc, rect.Width - tSize.Width, drawHeight);
                    if (s == null)
                        Style.DrawStringDefaults(g, v, drawRect);
                    else
                        await s.DrawString(rpt, g, v, tc, null, drawRect);
                }
                // Draw the Major Tick Marks (if necessary)
                DrawCategoryAxisTick(g, true, a.MajorTickMarks, new Drawing.Point(rect.Right, drawLoc));
            }

            // Draw the end on (if necessary)
            DrawCategoryAxisTick(g, true, a.MajorTickMarks, new Drawing.Point(rect.Right, rect.Bottom));

            return;
        }

        protected void DrawCategoryAxisTick(Drawing.Graphics g, bool bMajor, AxisTickMarksEnum tickType, Drawing.Point p)
        {
            int len = bMajor ? AxisTickMarkMajorLen : AxisTickMarkMinorLen;
            switch (tickType)
            {
                case AxisTickMarksEnum.Outside:
                    g.DrawLine(Drawing.Pens.Black, new Drawing.Point(p.X, p.Y), new Drawing.Point(p.X - len, p.Y));
                    break;
                case AxisTickMarksEnum.Inside:
                    g.DrawLine(Drawing.Pens.Black, new Drawing.Point(p.X, p.Y), new Drawing.Point(p.X + len, p.Y));
                    break;
                case AxisTickMarksEnum.Cross:
                    g.DrawLine(Drawing.Pens.Black, new Drawing.Point(p.X - len, p.Y), new Drawing.Point(p.X + len, p.Y));
                    break;
                case AxisTickMarksEnum.None:
                default:
                    break;
            }
            return;
        }

        async Task DrawColumnBar(Report rpt, Drawing.Graphics g, Drawing.Brush brush, Drawing.Rectangle rect, int iRow, int iCol)
        {
            g.FillRectangle(brush, rect);
            g.DrawRectangle(Drawing.Pens.Black, rect);

            if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), await _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked ||
                (ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), await _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
            {
                await DrawDataPoint(rpt, g, rect, iRow, iCol);
            }
            else
            {
                Drawing.Point p;
                p = new Drawing.Point(rect.Right, rect.Top);
                await DrawDataPoint(rpt, g, p, iRow, iCol);
            }


            return;
        }

        protected async Task DrawValueAxis(Report rpt, Drawing.Graphics g, double min, double max, Drawing.Rectangle rect, int plotTop, int plotBottom)
        {
            if (this.ChartDefn.ValueAxis == null)
                return;
            Axis a = this.ChartDefn.ValueAxis.Axis;
            if (a == null)
                return;
            Style s = a.Style;

            // Account for tick marks
            int tickSize = 0;
            if (a.MajorTickMarks == AxisTickMarksEnum.Cross ||
                a.MajorTickMarks == AxisTickMarksEnum.Outside)
                tickSize = this.AxisTickMarkMajorLen;
            else if (a.MinorTickMarks == AxisTickMarksEnum.Cross ||
                a.MinorTickMarks == AxisTickMarksEnum.Outside)
                tickSize += this.AxisTickMarkMinorLen;

            int intervalCount;
            double incr;
            (incr, intervalCount) = await SetIncrementAndInterval(rpt, a, min, max);      // Calculate the interval count

            int maxValueHeight = 0;
            double v = min;
            Drawing.Size size = Drawing.Size.Empty;

            for (int i = 0; i < intervalCount + 1; i++)
            {
                int x = (int)(((Math.Min(v, max) - min) / (max - min)) * rect.Width);

                if (!a.Visible)
                {
                    // nothing to do
                }
                else if (s != null)
                {
                    size = await s.MeasureString(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    Drawing.Rectangle vRect =
                        new Drawing.Rectangle(rect.Left + x - (size.Width / 2), rect.Top + tickSize, size.Width, size.Height);
                    await s.DrawString(rpt, g, v, TypeCode.Double, null, vRect);
                }
                else
                {
                    size = await Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    Drawing.Rectangle vRect =
                        new Drawing.Rectangle(rect.Left + x - (size.Width / 2), rect.Top + tickSize, size.Width, size.Height);
                    Style.DrawStringDefaults(g, v, vRect);
                }
                if (size.Height > maxValueHeight)       // Need to keep track of the maximum height
                    maxValueHeight = size.Height;       //   this is probably overkill since it should always be the same??

                await DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Drawing.Point(rect.Left + x, plotTop), new Drawing.Point(rect.Left + x, plotBottom));
                await DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Drawing.Point(rect.Left + x, plotBottom));

                v += incr;
            }

            // Draw the end points of the major grid lines
            await DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Drawing.Point(rect.Left, plotTop), new Drawing.Point(rect.Left, plotBottom));
            await DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Drawing.Point(rect.Left, plotBottom));
            await DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Drawing.Point(rect.Right, plotTop), new Drawing.Point(rect.Right, plotBottom));
            await DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Drawing.Point(rect.Right, plotBottom));

            Drawing.Size tSize = await DrawTitleMeasure(rpt, g, a.Title);
            await DrawTitle(rpt, g, a.Title,
                new Drawing.Rectangle(rect.Left, rect.Top + maxValueHeight + tickSize, rect.Width, tSize.Height));

            return;
        }

        protected async Task DrawValueAxisGrid(Report rpt, Drawing.Graphics g, ChartGridLines gl, Drawing.Point s, Drawing.Point e)
        {
            if (gl == null || !gl.ShowGridLines)
                return;

            if (gl.Style != null)
                await gl.Style.DrawStyleLine(rpt, g, null, s, e);
            else
                g.DrawLine(Drawing.Pens.Black, s, e);

            return;
        }

        protected async Task DrawValueAxisTick(Report rpt, Drawing.Graphics g, bool bMajor, AxisTickMarksEnum tickType, ChartGridLines gl, Drawing.Point p)
        {
            if (tickType == AxisTickMarksEnum.None)
                return;

            int len = bMajor ? AxisTickMarkMajorLen : AxisTickMarkMinorLen;
            Drawing.Point s, e;
            switch (tickType)
            {
                case AxisTickMarksEnum.Inside:
                    s = new Drawing.Point(p.X, p.Y);
                    e = new Drawing.Point(p.X, p.Y - len);
                    break;
                case AxisTickMarksEnum.Cross:
                    s = new Drawing.Point(p.X, p.Y - len);
                    e = new Drawing.Point(p.X, p.Y + len);
                    break;
                case AxisTickMarksEnum.Outside:
                default:
                    s = new Drawing.Point(p.X, p.Y + len);
                    e = new Drawing.Point(p.X, p.Y);
                    break;
            }
            Style style = gl.Style;

            if (style != null)
                await style.DrawStyleLine(rpt, g, null, s, e);
            else
                g.DrawLine(Drawing.Pens.Black, s, e);

            return;
        }

        // Calculate the size of the value axis; width is max value width + title width
        //										 height is max value height
        protected async Task<Drawing.Size> ValueAxisSize(Report rpt, Drawing.Graphics g, double min, double max)
        {
            Drawing.Size size = Drawing.Size.Empty;
            if (ChartDefn.ValueAxis == null)
                return size;
            Axis a = ChartDefn.ValueAxis.Axis;
            if (a == null)
                return size;

            Drawing.Size minSize;
            Drawing.Size maxSize;
            if (!a.Visible)
            {
                minSize = maxSize = Drawing.Size.Empty;
            }
            else if (a.Style != null)
            {
                minSize = await a.Style.MeasureString(rpt, g, min, TypeCode.Double, null, int.MaxValue);
                maxSize = await a.Style.MeasureString(rpt, g, max, TypeCode.Double, null, int.MaxValue);
            }
            else
            {
                minSize = await Style.MeasureStringDefaults(rpt, g, min, TypeCode.Double, null, int.MaxValue);
                maxSize = await Style.MeasureStringDefaults(rpt, g, max, TypeCode.Double, null, int.MaxValue);
            }
            // Choose the largest
            size.Width = Math.Max(minSize.Width, maxSize.Width);
            size.Height = Math.Max(minSize.Height, maxSize.Height);

            // Now we need to add in the height of the title (if any)
            Drawing.Size titleSize = await DrawTitleMeasure(rpt, g, a.Title);
            size.Height += titleSize.Height;

            if (a.MajorTickMarks == AxisTickMarksEnum.Cross ||
                a.MajorTickMarks == AxisTickMarksEnum.Outside)
                size.Height += this.AxisTickMarkMajorLen;
            else if (a.MinorTickMarks == AxisTickMarksEnum.Cross ||
                a.MinorTickMarks == AxisTickMarksEnum.Outside)
                size.Height += this.AxisTickMarkMinorLen;

            return size;
        }
    }
}
