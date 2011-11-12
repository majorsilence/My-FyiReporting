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
	/// Bar chart definition and processing.
	///</summary>
	internal class ChartBar: ChartBase
	{
		int _GapSize = 6;	//  TODO: hard code for now

        internal ChartBar(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX,Expression _ToolTipYFormat, Expression _ToolTipXFormat)
            : base(r, row, c, m,showTooltips,showTooltipsX,_ToolTipYFormat,_ToolTipXFormat)
		{
		}

		override internal void Draw(Report rpt)
		{
			CreateSizedBitmap();
			//AJM GJL 14082008 Using Vector Graphics
            using (Graphics g1 = Graphics.FromImage(_bm))
            {
                _aStream = new System.IO.MemoryStream();
                IntPtr HDC = g1.GetHdc();
                _mf = new System.Drawing.Imaging.Metafile(_aStream, HDC, new RectangleF(0, 0, _bm.Width, _bm.Height), System.Drawing.Imaging.MetafileFrameUnit.Pixel);
                g1.ReleaseHdc(HDC);
            }

			using (Graphics g = Graphics.FromImage(_mf))
			{
                // 06122007AJM Used to Force Higher Quality
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

				// Adjust the top margin to depend on the title height
				Size titleSize = DrawTitleMeasure(rpt, g, ChartDefn.Title);
				Layout.TopMargin = titleSize.Height;

				double max=0,min=0;	// Get the max and min values
				// 20022008 AJM GJL - Now requires Y axis identifier
				GetValueMaxMin(rpt, ref max, ref min, 0,1);

				DrawChartStyle(rpt, g);
				
				// Draw title; routine determines if necessary
				DrawTitle(rpt, g, ChartDefn.Title, new System.Drawing.Rectangle(0, 0, _bm.Width, Layout.TopMargin));

 				// Adjust the left margin to depend on the Category Axis
				Size caSize = CategoryAxisSize(rpt, g);
				Layout.LeftMargin = caSize.Width;

				// Adjust the bottom margin to depend on the Value Axis
				Size vaSize = ValueAxisSize(rpt, g, min, max);
				Layout.BottomMargin = vaSize.Height;

				// Draw legend
				System.Drawing.Rectangle lRect = DrawLegend(rpt, g, false, true);
				// 20022008 AJM GJL - Requires Rpt and Graphics
				AdjustMargins(lRect,rpt,g );		// Adjust margins based on legend.

				// Draw Plot area
				DrawPlotAreaStyle(rpt, g, lRect);
																															   
				// Draw Value Axis
				if (vaSize.Width > 0)	// If we made room for the axis - we need to draw it
					DrawValueAxis(rpt, g, min, max, 
						new System.Drawing.Rectangle(Layout.LeftMargin, _bm.Height-Layout.BottomMargin, _bm.Width - Layout.LeftMargin - Layout.RightMargin, vaSize.Height), Layout.TopMargin, _bm.Height - Layout.BottomMargin);

				// Draw Category Axis
				if (caSize.Height > 0)
					DrawCategoryAxis(rpt, g,  
						new System.Drawing.Rectangle(Layout.LeftMargin - caSize.Width, Layout.TopMargin, caSize.Width, _bm.Height - Layout.TopMargin - Layout.BottomMargin));

                if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked)
					DrawPlotAreaStacked(rpt, g, min, max);
                else if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
					DrawPlotAreaPercentStacked(rpt, g);
				else
					DrawPlotAreaPlain(rpt, g, min, max);

				DrawLegend(rpt, g, false, false);		// after the plot is drawn
			}
		}
 
		void DrawPlotAreaPercentStacked(Report rpt, Graphics g)
		{
			int barsNeeded = CategoryCount; 
			int gapsNeeded = CategoryCount * 2;

			// Draw Plot area data
			double max = 1;

			int heightBar = (int) ((Layout.PlotArea.Height - gapsNeeded*_GapSize) / barsNeeded);
			int maxBarWidth = (int) (Layout.PlotArea.Width);	

			// Loop thru calculating all the data points
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				int barLoc=(int) (Layout.PlotArea.Top  + (iRow-1) * ((double) (Layout.PlotArea.Height) / CategoryCount));
				barLoc += _GapSize;	// space before series

				double sum=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					sum += GetDataValue(rpt, iRow, iCol);
				}
				double v=0;
				int saveX=0;
                double t=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
                    t = GetDataValue(rpt, iRow, iCol);
                    v += t;

					int x = (int) ((Math.Min(v/sum,max) / max) * maxBarWidth);

					System.Drawing.Rectangle rect;
					rect = new System.Drawing.Rectangle(Layout.PlotArea.Left + saveX, barLoc, x - saveX, heightBar);

					DrawColumnBar(rpt, g, 
                        GetSeriesBrush(rpt, iRow, iCol),
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

		void DrawPlotAreaPlain(Report rpt, Graphics g, double min, double max)
		{
			int barsNeeded = SeriesCount * CategoryCount; 
			int gapsNeeded = CategoryCount * 2;

			// Draw Plot area data
			int heightBar = (int) ((Layout.PlotArea.Height - gapsNeeded*_GapSize) / barsNeeded);
			int maxBarWidth = (int) (Layout.PlotArea.Width);	

			//int barLoc=Layout.LeftMargin;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				int barLoc=(int) (Layout.PlotArea.Top  + (iRow-1) * ((double) (Layout.PlotArea.Height) / CategoryCount));
				barLoc += _GapSize;	// space before series
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);
					int x = (int) (((Math.Min(v,max)-min) / (max-min)) * maxBarWidth);
						
					DrawColumnBar(rpt, g, GetSeriesBrush(rpt, iRow, iCol), 
						new System.Drawing.Rectangle(Layout.PlotArea.Left, barLoc, x, heightBar), iRow, iCol);
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

		void DrawPlotAreaStacked(Report rpt, Graphics g, double min, double max)
		{
			int barsNeeded = CategoryCount; 
			int gapsNeeded = CategoryCount * 2;

			int heightBar = (int) ((Layout.PlotArea.Height - gapsNeeded*_GapSize) / barsNeeded);
			int maxBarWidth = (int) (Layout.PlotArea.Width);	

			// Loop thru calculating all the data points
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				int barLoc=(int) (Layout.PlotArea.Top  + (iRow-1) * ((double) (Layout.PlotArea.Height) / CategoryCount));
				barLoc += _GapSize;	// space before series

				double v=0;
                double t = 0;
				int saveX=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					t = GetDataValue(rpt, iRow, iCol);
                    v += t;

					int x = (int) (((Math.Min(v,max)-min) / (max-min)) * maxBarWidth);

					System.Drawing.Rectangle rect;
					rect = new System.Drawing.Rectangle(Layout.PlotArea.Left + saveX, barLoc, x - saveX, heightBar);

                    DrawColumnBar(rpt, g, GetSeriesBrush(rpt, iRow, iCol), rect, iRow, iCol);    

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
		Size CategoryAxisSize(Report rpt, Graphics g)
		{
			_LastCategoryWidth = 0;

			Size size=Size.Empty;
			if (this.ChartDefn.CategoryAxis == null)
				return size;
			Axis a = this.ChartDefn.CategoryAxis.Axis;
			if (a == null)
				return size;
			Style s = a.Style;

			// Measure the title
			size = DrawTitleMeasure(rpt, g, a.Title);

			if (!a.Visible)		// don't need to calculate the height
				return size;

			// Calculate the tallest category name
			TypeCode tc;
			int maxWidth=0;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				object v = this.GetCategoryValue(rpt, iRow, out tc);
				Size tSize;
				if (s == null)
					tSize = Style.MeasureStringDefaults(rpt, g, v, tc, null, int.MaxValue);

				else
					tSize =s.MeasureString(rpt, g, v, tc, null, int.MaxValue);

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
		void DrawCategoryAxis(Report rpt, Graphics g, System.Drawing.Rectangle rect)
		{
			if (this.ChartDefn.CategoryAxis == null)
				return;
			Axis a = this.ChartDefn.CategoryAxis.Axis;
			if (a == null)
				return;
			Style s = a.Style;

			Size tSize = DrawTitleMeasure(rpt, g, a.Title);
			DrawTitle(rpt, g, a.Title, 
				new System.Drawing.Rectangle(rect.Left, rect.Top, tSize.Width, rect.Height));

			int drawHeight = rect.Height / CategoryCount;
			TypeCode tc;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				object v = this.GetCategoryValue(rpt, iRow, out tc);

				int drawLoc=(int) (rect.Top + (iRow-1) * ((double) rect.Height / CategoryCount));

				// Draw the category text
				if (a.Visible)
				{
					System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle(rect.Left + tSize.Width, drawLoc, rect.Width-tSize.Width, drawHeight);
					if (s == null)
						Style.DrawStringDefaults(g, v, drawRect);
					else
						s.DrawString(rpt, g, v, tc, null, drawRect);
				}
				// Draw the Major Tick Marks (if necessary)
				DrawCategoryAxisTick(g, true, a.MajorTickMarks, new Point(rect.Right,  drawLoc));
			}

			// Draw the end on (if necessary)
			DrawCategoryAxisTick(g, true, a.MajorTickMarks, new Point(rect.Right, rect.Bottom));

			return;
		}

		protected void DrawCategoryAxisTick(Graphics g, bool bMajor, AxisTickMarksEnum tickType, Point p)
		{
			int len = bMajor? AxisTickMarkMajorLen: AxisTickMarkMinorLen;
			switch (tickType)
			{
				case AxisTickMarksEnum.Outside:
					g.DrawLine(Pens.Black, new Point(p.X, p.Y), new Point(p.X-len, p.Y));
					break;
				case AxisTickMarksEnum.Inside:
					g.DrawLine(Pens.Black, new Point(p.X, p.Y), new Point(p.X+len, p.Y));
					break;
				case AxisTickMarksEnum.Cross:
					g.DrawLine(Pens.Black, new Point(p.X-len, p.Y), new Point(p.X+len, p.Y));
					break;
				case AxisTickMarksEnum.None:
				default:
					break;
			}
			return;
		}

		void DrawColumnBar(Report rpt, Graphics g, Brush brush, System.Drawing.Rectangle rect, int iRow, int iCol)
		{
			g.FillRectangle(brush, rect);
			g.DrawRectangle(Pens.Black, rect);

            if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked ||
                (ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
			{
				DrawDataPoint(rpt, g, rect, iRow, iCol);
			}
			else
			{
				Point p;
				p = new Point(rect.Right, rect.Top);
				DrawDataPoint(rpt, g, p, iRow, iCol);
			}


			return;
		}

		protected void DrawValueAxis(Report rpt, Graphics g, double min, double max, System.Drawing.Rectangle rect, int plotTop, int plotBottom)
		{
			if (this.ChartDefn.ValueAxis == null)
				return;
			Axis a = this.ChartDefn.ValueAxis.Axis;
			if (a == null)
				return;
			Style s = a.Style;

			// Account for tick marks
			int tickSize=0;
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
			Size size= Size.Empty;

            for (int i = 0; i < intervalCount+1; i++)	 
			{
				int x = (int) (((Math.Min(v,max)-min) / (max-min)) * rect.Width);

				if (!a.Visible)
				{
					// nothing to do
				}
				else if (s != null)
				{
					size = s.MeasureString(rpt, g, v, TypeCode.Double, null, int.MaxValue);
					System.Drawing.Rectangle vRect = 
						new System.Drawing.Rectangle(rect.Left + x - size.Width/2, rect.Top+tickSize, size.Width, size.Height);
					s.DrawString(rpt, g, v, TypeCode.Double, null, vRect);
				}
				else
				{
					size = Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
					System.Drawing.Rectangle vRect = 
						new System.Drawing.Rectangle(rect.Left + x - size.Width/2, rect.Top+tickSize, size.Width, size.Height);
					Style.DrawStringDefaults(g, v, vRect);
				}
				if (size.Height > maxValueHeight)		// Need to keep track of the maximum height
					maxValueHeight = size.Height;		//   this is probably overkill since it should always be the same??

				DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Point(rect.Left + x, plotTop), new Point(rect.Left + x, plotBottom));
				DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(rect.Left + x, plotBottom ));

				v += incr;
			}

			// Draw the end points of the major grid lines
			DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Point(rect.Left, plotTop), new Point(rect.Left, plotBottom));
			DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(rect.Left, plotBottom));
			DrawValueAxisGrid(rpt, g, a.MajorGridLines, new Point(rect.Right, plotTop), new Point(rect.Right, plotBottom));
			DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(rect.Right, plotBottom));

			Size tSize = DrawTitleMeasure(rpt, g, a.Title);
			DrawTitle(rpt, g, a.Title, 
				new System.Drawing.Rectangle(rect.Left, rect.Top+maxValueHeight+tickSize, rect.Width, tSize.Height));

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

			int len = bMajor? AxisTickMarkMajorLen: AxisTickMarkMinorLen;
			Point s, e;
			switch (tickType)
			{
				case AxisTickMarksEnum.Inside:
					s = new Point(p.X, p.Y); 
					e = new Point(p.X, p.Y-len);
					break;
				case AxisTickMarksEnum.Cross:
					s = new Point(p.X, p.Y-len);
					e = new Point(p.X, p.Y+len);
					break;
				case AxisTickMarksEnum.Outside:
				default:
					s = new Point(p.X, p.Y+len);
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
		protected Size ValueAxisSize(Report rpt, Graphics g, double min, double max)
		{
			Size size=Size.Empty;
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
