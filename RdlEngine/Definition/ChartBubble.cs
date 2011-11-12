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
using System.Drawing;


namespace fyiReporting.RDL
{
	///<summary>
	/// Line chart definition and processing.
	///</summary>
	[Serializable]
	internal class ChartBubble: ChartBase
	{

        internal ChartBubble(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX,Expression _ToolTipYFormat, Expression _ToolTipXFormat)
            : base(r, row, c, m,showTooltips,showTooltipsX,_ToolTipYFormat, _ToolTipXFormat)
		{
		}

		override internal void Draw(Report rpt)
		{
			CreateSizedBitmap();


            using (Graphics g1 = Graphics.FromImage(_bm))
            {              
                _aStream = new System.IO.MemoryStream();  
                IntPtr HDC = g1.GetHdc();
                _mf = new System.Drawing.Imaging.Metafile(_aStream, HDC, new RectangleF(0, 0, _bm.Width, _bm.Height), System.Drawing.Imaging.MetafileFrameUnit.Pixel);
                g1.ReleaseHdc(HDC);
            }
                      
            using(Graphics g = Graphics.FromImage(_mf))
			{
                // 06122007AJM Used to Force Higher Quality
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.PageUnit = GraphicsUnit.Pixel;

				// Adjust the top margin to depend on the title height
				Size titleSize = DrawTitleMeasure(rpt, g, ChartDefn.Title);
				Layout.TopMargin = titleSize.Height;

				// 20022008 AJM GJL - Added new required info 
				double ymax=0,ymin=0;	// Get the max and min values for the y axis
				GetValueMaxMin(rpt, ref ymax, ref ymin, 1,1);

                double xmax = 0, xmin = 0;  // Get the max and min values for the x axis
                GetValueMaxMin(rpt, ref xmax, ref xmin, 0,1);

                double bmax = 0, bmin = 0;  // Get the max and min values for the bubble size
                if (ChartDefn.Type == ChartTypeEnum.Bubble)     // only applies to bubble (not scatter)
                    GetValueMaxMin(rpt, ref bmax, ref bmin, 2,1);

                DrawChartStyle(rpt, g);
				
				// Draw title; routine determines if necessary
				DrawTitle(rpt, g, ChartDefn.Title, new System.Drawing.Rectangle(0, 0, Layout.Width, Layout.TopMargin));

				// Adjust the left margin to depend on the Value Axis
				Size vaSize = ValueAxisSize(rpt, g, ymin, ymax);
				Layout.LeftMargin = vaSize.Width;

				// Draw legend
				System.Drawing.Rectangle lRect = DrawLegend(rpt,g, false, true);

				// Adjust the bottom margin to depend on the Category Axis
				Size caSize = CategoryAxisSize(rpt, g, xmin, xmax);
				Layout.BottomMargin = caSize.Height;

				AdjustMargins(lRect,rpt,g);		// Adjust margins based on legend.

				// Draw Plot area
				DrawPlotAreaStyle(rpt, g, lRect);

				// Draw Value Axis
				if (vaSize.Width > 0)	// If we made room for the axis - we need to draw it
					DrawValueAxis(rpt, g, ymin, ymax, 
						new System.Drawing.Rectangle(Layout.LeftMargin - vaSize.Width, Layout.TopMargin, vaSize.Width, Layout.PlotArea.Height), Layout.LeftMargin, _bm.Width - Layout.RightMargin);

                // Draw Category Axis
                if (caSize.Height > 0)
                    DrawCategoryAxis(rpt, g, xmin, xmax,
                        new System.Drawing.Rectangle(Layout.LeftMargin, _bm.Height - Layout.BottomMargin, _bm.Width - Layout.LeftMargin - Layout.RightMargin, vaSize.Height), 
                        Layout.TopMargin, _bm.Height - Layout.BottomMargin);

				// Draw Plot area data 
				DrawPlot(rpt, g, xmin, xmax, ymin, ymax, bmin, bmax);
                DrawLegend(rpt, g, false, false);
			}
			
		}

        void DrawPlot(Report rpt, Graphics g, double xmin, double xmax, double ymin, double ymax, double bmin, double bmax)
        {
            // Draw Plot area data 
            int maxPointHeight = (int)Layout.PlotArea.Height;
            int maxPointWidth = (int)Layout.PlotArea.Width;

          

            for (int iCol = 1; iCol <= SeriesCount; iCol++)
            {
                //handle either line scatter or line plot type GJL 020308
                Point lastPoint = new Point();
                Point[] Points = new Point[2];
                bool isLine = GetPlotType(rpt, iCol, 1).ToUpper() == "LINE";
                for (int iRow = 1; iRow <= CategoryCount; iRow++)
                {
                    double xv = this.GetDataValue(rpt, iRow, iCol, 0);
                    double yv = this.GetDataValue(rpt, iRow, iCol, 1);
                    double bv = this.ChartDefn.Type == ChartTypeEnum.Bubble ?
                        this.GetDataValue(rpt, iRow, iCol, 2) : 0;
                    if (xv < xmin || yv < ymin || xv > xmax || yv > ymax) 
                        continue;
                    int x = (int)(((Math.Min(xv, xmax) - xmin) / (xmax - xmin)) * maxPointWidth);
                    int y = (int)(((Math.Min(yv, ymax) - ymin) / (ymax - ymin)) * maxPointHeight);
                    if (y != int.MinValue && x != int.MinValue)
                    {                    
                    Point p = new Point(Layout.PlotArea.Left + x, Layout.PlotArea.Top + (maxPointHeight - y));
                        //GJL 010308 Line subtype scatter plot
                    if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Line || (ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.SmoothLine || isLine)
                        {
                            if (!(lastPoint.IsEmpty))
                            {
                            Points[0] = lastPoint;
                            Points[1] = p;
                            String LineSize = getLineSize(rpt, iCol, 1);
                            int intLineSize = 2;
                            switch (LineSize)
                            {
                                case "Small": intLineSize = 1;
                                    break;
                                case "Regular": intLineSize = 2;
                                    break;
                                case "Large": intLineSize = 3;
                                    break;
                                case "Extra Large": intLineSize = 4;
                                    break;
                                case "Super Size": intLineSize = 5;
                                    break;
                            }
                            DrawLineBetweenPoints(g, rpt, GetSeriesBrush(rpt, iRow, iCol), Points, intLineSize);
                            //Add a metafilecomment to use as a tooltip GJL 26092008                          

                            if (_showToolTips || _showToolTipsX)
                            {
                                string display = "";
                                if (_showToolTipsX) display = xv.ToString(_tooltipXFormat);
                                if (_showToolTips)
                                {
                                    if (display.Length > 0) display += " , ";
                                    display += yv.ToString(_tooltipYFormat);
                                }
                                String val = "ToolTip:" + display + "|X:" + (int)(p.X - 3) + "|Y:" + (int)(p.Y - 3) + "|W:" + 6 + "|H:" + 6;                             
                                g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                            }


                            }
                        }
                        else
                        {  
                            DrawBubble(rpt, g, GetSeriesBrush(rpt, iRow, iCol), p, iRow, iCol, bmin, bmax, bv,xv,yv);

                        }
                        lastPoint = p; 
                    }                 
                }
            }
            return;
        }

        /* This code was copied from the Line drawing class. 
        * 010308 GJL */
        void DrawLineBetweenPoints(Graphics g, Report rpt, Brush brush, Point[] points)
        {
            DrawLineBetweenPoints(g, rpt, brush, points, 2);
        }

        void DrawLineBetweenPoints(Graphics g, Report rpt, Brush brush, Point[] points,int intLineSize)
        {
            if (points.Length <= 1)		// Need at least 2 points
                return;

            Pen p = null;
            try
            {
                if (brush.GetType() == typeof(System.Drawing.Drawing2D.HatchBrush))
                {
                    System.Drawing.Drawing2D.HatchBrush tmpBrush = (System.Drawing.Drawing2D.HatchBrush)brush;
                    p = new Pen(new SolidBrush(tmpBrush.ForegroundColor), intLineSize); //1.5F);    // todo - use line from style ????
                }
                else
                {
                    p = new Pen(brush, intLineSize);
                }

                if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Smooth && points.Length > 2)
                    g.DrawCurve(p, points, 0.5F);
                else
                    g.DrawLines(p, points);
            }
            finally
            {
                if (p != null)
                    p.Dispose();
            }
            return;
        }

        void DrawBubble(Report rpt, Graphics g, Brush brush, Point p, int iRow, int iCol, double bmin, double bmax, double bv,double xv,double yv)
		{
			Pen pen=null;
            int diameter = BubbleSize(rpt, iRow, iCol, bmin, bmax, bv);          // set diameter of bubble
            
            int radius=  diameter /2;
			try
			{
                if (this.ChartDefn.Type == ChartTypeEnum.Scatter &&
                    brush.GetType() == typeof(System.Drawing.Drawing2D.HatchBrush))
                {
                    System.Drawing.Drawing2D.HatchBrush tmpBrush = (System.Drawing.Drawing2D.HatchBrush)brush;
                    SolidBrush br = new SolidBrush(tmpBrush.ForegroundColor);
                    pen = new Pen(new SolidBrush(tmpBrush.ForegroundColor));
                    DrawLegendMarker(g, br, pen, SeriesMarker[iCol - 1], p.X - radius, p.Y - radius, diameter);                    
                    DrawDataPoint(rpt, g, new Point(p.X - 3, p.Y + 3), iRow, iCol);                 

                }
                else
                {
				    pen = new Pen(brush);
                    DrawLegendMarker(g, brush, pen, ChartMarkerEnum.Bubble, p.X - radius, p.Y - radius, diameter);
                    DrawDataPoint(rpt, g, new Point(p.X - 3, p.Y + 3), iRow, iCol);
			    }
                //Add a metafilecomment to use as a tooltip GJL 26092008               

                if (_showToolTips || _showToolTipsX)
                {
                    string display = "";
                    if (_showToolTipsX) display = xv.ToString(_tooltipXFormat);
                    if (_showToolTips)
                    {
                        if (display.Length > 0) display += " , ";
                        display += yv.ToString(_tooltipYFormat);
                    }
                    String val = "ToolTip:" + display + "|X:" + (int)(p.X - 3) + "|Y:" + (int)(p.Y - 3) + "|W:" + 6 + "|H:" + 6;    
                    g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                }
			}
			finally
			{
				if (pen != null)
					pen.Dispose();
			}

			return;
		}

        private int BubbleSize(Report rpt, int iRow, int iCol, double minB, double maxB, double bv)
        {
            int diameter = 5;
            if (ChartDefn.Type != ChartTypeEnum.Bubble)
                return diameter;

            int bubbleMax = 30;           // maximum bubble size
            int bubbleMin = 4;          // minimum bubble size

            double diff = maxB - minB;
            double vdiff = bv - minB;

            if (Math.Abs(diff) < 1e-9d)     // very small difference between max and min?
                return diameter;            // just use the smallest 

            diameter = (int) ((vdiff / diff) * (bubbleMax - bubbleMin) + bubbleMin); 

            return diameter;
        }

        // Calculate the size of the value axis; width is max value width + title width
        //										 height is max value height
        protected Size ValueAxisSize(Report rpt, Graphics g, double min, double max)
        {
            Size size = Size.Empty;
            if (ChartDefn.ValueAxis == null)
                return size;
            Axis a = ChartDefn.ValueAxis.Axis;
            if (a == null)
                return size;

            Size minSize;
            Size maxSize;
            if (!a.Visible)
            {
                minSize = maxSize = Size.Empty;
            }
            else if (a.Style != null)
            {
                minSize = a.Style.MeasureString(rpt, g, min, TypeCode.Double, null, int.MaxValue);
                maxSize = a.Style.MeasureString(rpt, g, max, TypeCode.Double, null, int.MaxValue);
            }
            else
            {
                minSize = Style.MeasureStringDefaults(rpt, g, min, TypeCode.Double, null, int.MaxValue);
                maxSize = Style.MeasureStringDefaults(rpt, g, max, TypeCode.Double, null, int.MaxValue);
            }
            // Choose the largest
            size.Width = Math.Max(minSize.Width, maxSize.Width);
            size.Height = Math.Max(minSize.Height, maxSize.Height);

            // Now we need to add in the width of the title (if any)
            Size titleSize = DrawTitleMeasure(rpt, g, a.Title);
            size.Width += titleSize.Width;

            return size;
        }

        protected void DrawValueAxis(Report rpt, Graphics g, double min, double max,
                        System.Drawing.Rectangle rect, int plotLeft, int plotRight)
        {
            if (this.ChartDefn.ValueAxis == null)
                return;
            Axis a = this.ChartDefn.ValueAxis.Axis;
            if (a == null)
                return;
            Style s = a.Style;

            int intervalCount;
            double incr;
            SetIncrementAndInterval(rpt, a, min, max, out incr, out intervalCount);      // Calculate the interval count

            Size tSize = DrawTitleMeasure(rpt, g, a.Title);
            DrawTitle(rpt, g, a.Title, new System.Drawing.Rectangle(rect.Left, rect.Top, tSize.Width, rect.Height));

            double v = min;
            for (int i = 0; i < intervalCount + 1; i++)
            {
                int h = (int)(((Math.Min(v, max) - min) / (max - min)) * rect.Height);
                if (h < 0)		// this is really some form of error
                {
                    v += incr;
                    continue;
                }

                if (!a.Visible)
                {
                    // nothing to do
                }
                else if (s != null)
                {
                    Size size = s.MeasureString(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    System.Drawing.Rectangle vRect =
                        new System.Drawing.Rectangle(rect.Left + tSize.Width, rect.Top + rect.Height - h - size.Height / 2, rect.Width - tSize.Width, size.Height);
                    s.DrawString(rpt, g, v, TypeCode.Double, null, vRect);
                }
                else
                {
                    Size size = Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    System.Drawing.Rectangle vRect =
                        new System.Drawing.Rectangle(rect.Left + tSize.Width, rect.Top + rect.Height - h - size.Height / 2, rect.Width - tSize.Width, size.Height);
                    Style.DrawStringDefaults(g, v, vRect);
                }

                DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Point(plotLeft, rect.Top + rect.Height - h), new Point(plotRight, rect.Top + rect.Height - h));
                DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(plotLeft, rect.Top + rect.Height - h));

                v += incr;
            }

            // Draw the end points of the major grid lines
            DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Point(plotLeft, rect.Top), new Point(plotLeft, rect.Bottom));
            DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(plotLeft, rect.Top));
            DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Point(plotRight, rect.Top), new Point(plotRight, rect.Bottom));
            DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(plotRight, rect.Bottom));

            return;
        }

        protected void DrawValueAxisGrid(Report rpt, Graphics g, ChartGridLines gl, Point s, Point e)
        {
            if (gl == null || !gl.ShowGridLines)
                return;

            if (gl.Style != null)
                gl.Style.DrawStyleLine(rpt, g, null, s, e);
            else
                g.DrawLine(Pens.Black, s, e);

            return;
        }

        protected void DrawValueAxisTick(Report rpt, Graphics g, bool bMajor, AxisTickMarksEnum tickType, ChartGridLines gl, Point p)
        {
            if (tickType == AxisTickMarksEnum.None)
                return;

            int len = bMajor ? AxisTickMarkMajorLen : AxisTickMarkMinorLen;
            Point s, e;
            switch (tickType)
            {
                case AxisTickMarksEnum.Inside:
                    s = new Point(p.X, p.Y);
                    e = new Point(p.X + len, p.Y);
                    break;
                case AxisTickMarksEnum.Cross:
                    s = new Point(p.X - len, p.Y);
                    e = new Point(p.X + len, p.Y);
                    break;
                case AxisTickMarksEnum.Outside:
                default:
                    s = new Point(p.X - len, p.Y);
                    e = new Point(p.X, p.Y);
                    break;
            }
            Style style = gl.Style;

            if (style != null)
                style.DrawStyleLine(rpt, g, null, s, e);
            else
                g.DrawLine(Pens.Black, s, e);

            return;
        }
/////////////////////////
        protected void DrawCategoryAxis(Report rpt, Graphics g, double min, double max, System.Drawing.Rectangle rect, int plotTop, int plotBottom)
        {
            if (this.ChartDefn.CategoryAxis == null)
                return;
            Axis a = this.ChartDefn.CategoryAxis.Axis;
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
            SetIncrementAndInterval(rpt, a, min, max, out incr, out intervalCount);      // Calculate the interval count

            int maxValueHeight = 0;
            double v = min;
            Size size = Size.Empty;

            for (int i = 0; i < intervalCount + 1; i++)
            {
                int x = (int)(((Math.Min(v, max) - min) / (max - min)) * rect.Width);

                if (!a.Visible)
                {
                    // nothing to do
                }
                else if (s != null)
                {
                    size = s.MeasureString(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    System.Drawing.Rectangle vRect =
                        new System.Drawing.Rectangle(rect.Left + x - size.Width / 2, rect.Top + tickSize, size.Width, size.Height);
                    s.DrawString(rpt, g, v, TypeCode.Double, null, vRect);
                }
                else
                {
                    size = Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    System.Drawing.Rectangle vRect =
                        new System.Drawing.Rectangle(rect.Left + x - size.Width / 2, rect.Top + tickSize, size.Width, size.Height);
                    Style.DrawStringDefaults(g, v, vRect);
                }
                if (size.Height > maxValueHeight)		// Need to keep track of the maximum height
                    maxValueHeight = size.Height;		//   this is probably overkill since it should always be the same??

                DrawCategoryAxisGrid(rpt, g, a.MajorGridLines, new Point(rect.Left + x, plotTop), new Point(rect.Left + x, plotBottom));
                DrawCategoryAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(rect.Left + x, plotBottom));

                v += incr;
            }

            // Draw the end points of the major grid lines
            DrawCategoryAxisGrid(rpt, g, a.MajorGridLines, new Point(rect.Left, plotTop), new Point(rect.Left, plotBottom));
            DrawCategoryAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(rect.Left, plotBottom));
            DrawCategoryAxisGrid(rpt, g, a.MajorGridLines, new Point(rect.Right, plotTop), new Point(rect.Right, plotBottom));
            DrawCategoryAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(rect.Right, plotBottom));

            Size tSize = DrawTitleMeasure(rpt, g, a.Title);
            DrawTitle(rpt, g, a.Title,
                new System.Drawing.Rectangle(rect.Left, rect.Top + maxValueHeight + tickSize, rect.Width, tSize.Height));

            return;
        }

        protected void DrawCategoryAxisGrid(Report rpt, Graphics g, ChartGridLines gl, Point s, Point e)
        {
            if (gl == null || !gl.ShowGridLines)
                return;

            if (gl.Style != null)
                gl.Style.DrawStyleLine(rpt, g, null, s, e);
            else
                g.DrawLine(Pens.Black, s, e);

            return;
        }

        protected void DrawCategoryAxisTick(Report rpt, Graphics g, bool bMajor, AxisTickMarksEnum tickType, ChartGridLines gl, Point p)
        {
            if (tickType == AxisTickMarksEnum.None)
                return;

            int len = bMajor ? AxisTickMarkMajorLen : AxisTickMarkMinorLen;
            Point s, e;
            switch (tickType)
            {
                case AxisTickMarksEnum.Inside:
                    s = new Point(p.X, p.Y);
                    e = new Point(p.X, p.Y - len);
                    break;
                case AxisTickMarksEnum.Cross:
                    s = new Point(p.X, p.Y - len);
                    e = new Point(p.X, p.Y + len);
                    break;
                case AxisTickMarksEnum.Outside:
                default:
                    s = new Point(p.X, p.Y + len);
                    e = new Point(p.X, p.Y);
                    break;
            }
            Style style = gl.Style;

            if (style != null)
                style.DrawStyleLine(rpt, g, null, s, e);
            else
                g.DrawLine(Pens.Black, s, e);

            return;
        }

        // Calculate the size of the value axis; width is max value width + title width
        //										 height is max value height
        protected Size CategoryAxisSize(Report rpt, Graphics g, double min, double max)
        {
            Size size = Size.Empty;
            if (ChartDefn.CategoryAxis == null)
                return size;
            Axis a = ChartDefn.CategoryAxis.Axis;//Not ValueAxis...
            if (a == null)
                return size;

            Size minSize;
            Size maxSize;
            if (!a.Visible)
            {
                minSize = maxSize = Size.Empty;
            }
            else if (a.Style != null)
            {
                minSize = a.Style.MeasureString(rpt, g, min, TypeCode.Double, null, int.MaxValue);
                maxSize = a.Style.MeasureString(rpt, g, max, TypeCode.Double, null, int.MaxValue);
            }
            else
            {
                minSize = Style.MeasureStringDefaults(rpt, g, min, TypeCode.Double, null, int.MaxValue);
                maxSize = Style.MeasureStringDefaults(rpt, g, max, TypeCode.Double, null, int.MaxValue);
            }
            // Choose the largest
            size.Width = Math.Max(minSize.Width, maxSize.Width);
            size.Height = Math.Max(minSize.Height, maxSize.Height);

            // Now we need to add in the height of the title (if any)
            Size titleSize = DrawTitleMeasure(rpt, g, a.Title);
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
