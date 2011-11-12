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
	internal class ChartLine: ChartColumn
	{

        internal ChartLine(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX,Expression _ToolTipYFormat, Expression _ToolTipXFormat)
            : base(r, row, c, m, showTooltips, showTooltipsX, _ToolTipYFormat, _ToolTipXFormat)
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

				// Adjust the top margin to depend on the title height
				Size titleSize = DrawTitleMeasure(rpt, g, ChartDefn.Title);
				Layout.TopMargin = titleSize.Height;

				// 20022008 AJM GJL - Added new required info 
				double max=0,min=0;	// Get the max and min values
				GetValueMaxMin(rpt, ref max, ref min, 0,1);

				DrawChartStyle(rpt, g);
				
				// Draw title; routine determines if necessary
				DrawTitle(rpt, g, ChartDefn.Title, new System.Drawing.Rectangle(0, 0, Layout.Width, Layout.TopMargin));

				// Adjust the left margin to depend on the Value Axis
				Size vaSize = ValueAxisSize(rpt, g, min, max);
				Layout.LeftMargin = vaSize.Width;

				// Draw legend
				System.Drawing.Rectangle lRect = DrawLegend(rpt,g, ChartDefn.Type == ChartTypeEnum.Area? false: true, true);

				// Adjust the bottom margin to depend on the Category Axis
				Size caSize = CategoryAxisSize(rpt, g);
				Layout.BottomMargin = caSize.Height;

				AdjustMargins(lRect,rpt,g);		// Adjust margins based on legend.

				// Draw Plot area
				DrawPlotAreaStyle(rpt, g, lRect);
                int intervalCount = 0;
                double incr = 0;
				// Draw Value Axis
				if (vaSize.Width > 0)	// If we made room for the axis - we need to draw it
					DrawValueAxis(rpt, g, min, max, 
						new System.Drawing.Rectangle(Layout.LeftMargin - vaSize.Width, Layout.TopMargin, vaSize.Width, Layout.PlotArea.Height), Layout.LeftMargin, _bm.Width - Layout.RightMargin,out incr,out intervalCount);

				// Draw Category Axis
				if (caSize.Height > 0)
                    //09052008ajm passing chart bounds int
					DrawCategoryAxis(rpt, g,
                        new System.Drawing.Rectangle(Layout.LeftMargin, _bm.Height - Layout.BottomMargin, Layout.PlotArea.Width, caSize.Height), Layout.TopMargin,
                        caSize.Width);

				// Draw Plot area data 
				if (ChartDefn.Type == ChartTypeEnum.Area)
				{
                    if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked)
						DrawPlotAreaAreaStacked(rpt, g, min, max);
                    else if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
						DrawPlotAreaAreaPercentStacked(rpt, g);
					else
						DrawPlotAreaArea(rpt, g, min, max);
				}
				else
				{
					DrawPlotAreaLine(rpt, g, min, max);
				}
				DrawLegend(rpt, g, ChartDefn.Type == ChartTypeEnum.Area? false: true, false);
			}
		}

		void DrawPlotAreaArea(Report rpt, Graphics g, double min, double max)
		{
			// Draw Plot area data 
			int maxPointHeight = (int) Layout.PlotArea.Height;	
			double widthCat = ((double) (Layout.PlotArea.Width) / (CategoryCount-1));
			Point[] saveP = new Point[CategoryCount];	// used for drawing lines between points
			for (int iCol=1; iCol <= SeriesCount; iCol++)
			{
				for (int iRow=1; iRow <= CategoryCount; iRow++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);

					int x = (int) (Layout.PlotArea.Left + (iRow-1) * widthCat);
					int y = (int) (((Math.Min(v,max)-min) / (max-min)) * maxPointHeight);
					Point p = new Point(x, Layout.PlotArea.Top + (maxPointHeight -  y));
					saveP[iRow-1] = p;
                    DrawLinePoint(rpt, g, GetSeriesBrush(rpt, iRow, iCol), ChartMarkerEnum.None, p, iRow, iCol);

                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + v.ToString(_tooltipYFormat) + "|X:" + (int)(p.X - 5) + "|Y:" + (int)(p.Y - 5) + "|W:" + 10 + "|H:" + 10;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }
				}
                DrawAreaBetweenPoints(g, GetSeriesBrush(rpt, 1, iCol), saveP, null);
			}
			return;
		}

		void DrawPlotAreaAreaPercentStacked(Report rpt, Graphics g)
		{
			double max = 1;				// 100% is the max
			// Draw Plot area data 
			int maxPointHeight = (int) Layout.PlotArea.Height;	
			double widthCat = ((double) (Layout.PlotArea.Width) / (CategoryCount-1));
			Point[,] saveAllP = new Point[CategoryCount,SeriesCount];	// used to collect all data points

			// Loop thru calculating all the data points
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				int x = (int) (Layout.PlotArea.Left + (iRow-1) * widthCat);
				double sum=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					sum += GetDataValue(rpt, iRow, iCol);
				}
				double v=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					v += GetDataValue(rpt, iRow, iCol);

					int y = (int) ((Math.Min(v/sum,max) / max) * maxPointHeight);
					Point p = new Point(x, Layout.PlotArea.Top + (maxPointHeight -  y));
					saveAllP[iRow-1, iCol-1] = p;
				}
			}

			// Now loop thru and plot all the points
			Point[] saveP = new Point[CategoryCount];	// used for drawing lines between points
			Point[] priorSaveP= new Point[CategoryCount];
			for (int iCol=1; iCol <= SeriesCount; iCol++)
			{
				for (int iRow=1; iRow <= CategoryCount; iRow++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);

					int x = (int) (Layout.PlotArea.Left + (iRow-1) * widthCat);
					int y = (int) ((Math.Min(v,max) / max) * maxPointHeight);
					Point p = new Point(x, Layout.PlotArea.Top + (maxPointHeight -  y));
					saveP[iRow-1] = saveAllP[iRow-1, iCol-1];
                    DrawLinePoint(rpt, g, GetSeriesBrush(rpt, iRow, iCol), ChartMarkerEnum.None, p, iRow, iCol);

                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + v.ToString(_tooltipYFormat) + "|X:" + (int)(p.X - 5) + "|Y:" + (int)(p.Y - 5) + "|W:" + 10 + "|H:" + 10;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }
				}
                DrawAreaBetweenPoints(g, GetSeriesBrush(rpt, 1, iCol), saveP, iCol == 1 ? null : priorSaveP);
				// Save prior point values
				for (int i=0; i < CategoryCount; i++)
					priorSaveP[i] = saveP[i];
			}
			return;
		}

		void DrawPlotAreaAreaStacked(Report rpt, Graphics g, double min, double max)
		{
			// Draw Plot area data 
			int maxPointHeight = (int) Layout.PlotArea.Height;	
			double widthCat = ((double) (Layout.PlotArea.Width) / (CategoryCount-1));
			Point[,] saveAllP = new Point[CategoryCount,SeriesCount];	// used to collect all data points

			// Loop thru calculating all the data points
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				int x = (int) (Layout.PlotArea.Left + (iRow-1) * widthCat);
				double v=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					v += GetDataValue(rpt, iRow, iCol);
					int y = (int) (((Math.Min(v,max)-min) / (max-min)) * maxPointHeight);
					Point p = new Point(x, Layout.PlotArea.Top + (maxPointHeight -  y));
					saveAllP[iRow-1, iCol-1] = p;
				}
			}

			// Now loop thru and plot all the points
			Point[] saveP = new Point[CategoryCount];	// used for drawing lines between points
			Point[] priorSaveP= new Point[CategoryCount];
			for (int iCol=1; iCol <= SeriesCount; iCol++)
			{
				for (int iRow=1; iRow <= CategoryCount; iRow++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);

					int x = (int) (Layout.PlotArea.Left + (iRow-1) * widthCat);
					int y = (int) (((Math.Min(v,max)-min) / (max-min)) * maxPointHeight);
					Point p = new Point(x, Layout.PlotArea.Top + (maxPointHeight -  y));
					saveP[iRow-1] = saveAllP[iRow-1, iCol-1];
                    DrawLinePoint(rpt, g, GetSeriesBrush(rpt, iRow, iCol), ChartMarkerEnum.None, p, iRow, iCol);
                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + v.ToString(_tooltipYFormat) + "|X:" + (int)(saveP[iRow - 1].X - 5) + "|Y:" + (int)(saveP[iRow - 1].Y - 5) + "|W:" + 10 + "|H:" + 10;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }

                  
				}
                DrawAreaBetweenPoints(g, GetSeriesBrush(rpt, 1, iCol), saveP, iCol == 1 ? null : priorSaveP);
				// Save prior point values
				for (int i=0; i < CategoryCount; i++)
					priorSaveP[i] = saveP[i];
			}
			return;
		}

		void DrawPlotAreaLine(Report rpt, Graphics g, double min, double max)
		{
			// Draw Plot area data 
			int maxPointHeight = (int) Layout.PlotArea.Height;	
			double widthCat = ((double) (Layout.PlotArea.Width) / CategoryCount);
			Point[] saveP = new Point[CategoryCount];	// used for drawing lines between points
			for (int iCol=1; iCol <= SeriesCount; iCol++)
			{
				for (int iRow=1; iRow <= CategoryCount; iRow++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);

					int x = (int) (Layout.PlotArea.Left + (iRow-1) * widthCat + widthCat/2 );
					int y = (int) (((Math.Min(v,max)-min) / (max-min)) * maxPointHeight);
					Point p = new Point(x, Layout.PlotArea.Top + (maxPointHeight -  y));
					saveP[iRow-1] = p;
                    bool DrawPoint = getNoMarkerVal(rpt, iCol, 1) == false;
                    //dont draw the point if I say not to!
                    if (DrawPoint) { DrawLinePoint(rpt, g, GetSeriesBrush(rpt, iRow, iCol), SeriesMarker[iCol - 1], p, iRow, iCol); }

                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + v.ToString(_tooltipYFormat) + "|X:" + (int)(p.X - 5) + "|Y:" + (int)(p.Y - 5) + "|W:" + 10 + "|H:" + 10;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }                  
				}

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
                DrawLineBetweenPoints(g, rpt, GetSeriesBrush(rpt, 1, iCol), saveP, intLineSize);
			}
			return;
		}

		void DrawAreaBetweenPoints(Graphics g, Brush brush, Point[] points, Point[] previous)
		{
			if (points.Length <= 1)		// Need at least 2 points
				return;

			Pen p=null;
			try
			{
				p = new Pen(brush, 1);    // todo - use line from style ????
				g.DrawLines(p, points);
				PointF[] poly;
				if (previous == null)
				{	// The bottom is the bottom of the chart
					poly = new PointF[points.Length + 3];
					int i=0;
					foreach (Point pt in points)
					{
						poly[i++] = pt;
					}
					poly[i++] = new PointF(points[points.Length-1].X, Layout.PlotArea.Bottom);
					poly[i++] = new PointF(points[0].X, Layout.PlotArea.Bottom);
					poly[i] = new PointF(points[0].X, points[0].Y); 
				}
				else
				{	// The bottom is the previous line
					poly = new PointF[points.Length * 2 + 1];
					int i=0;
					foreach (Point pt in points)
					{
						poly[i] = pt;
						poly[points.Length+i] = previous[previous.Length - 1 - i];
						i++;
					}
					poly[poly.Length-1] = poly[0];
				}
				g.FillPolygon(brush, poly);
			}
			finally
			{
				if (p != null)
					p.Dispose();
			}
			return;
		}

        void DrawLineBetweenPoints(Graphics g, Report rpt, Brush brush, Point[] points)
		{
            DrawLineBetweenPoints(g, rpt, brush, points, 2);
		}

        void DrawLineBetweenPoints(Graphics g, Report rpt, Brush brush, Point[] points, int intLineSize)
        {
            if (points.Length <= 1)		// Need at least 2 points
                return;

            Pen p = null;
            try
            {
                // 20022008 AJM GJL - Added thicker lines


                p = new Pen(brush, intLineSize);    // todo - use line from style ????


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

		void DrawLinePoint(Report rpt, Graphics g, Brush brush, ChartMarkerEnum marker, Point p, int iRow, int iCol)
		{
			Pen pen=null;
			try
			{
				pen = new Pen(brush);
				// 20022008 AJM GJL - Added bigger points
				DrawLegendMarker(g, brush, pen, marker, p.X-5, p.Y-5, 10);
				DrawDataPoint(rpt, g, new Point(p.X-5, p.Y+5), iRow, iCol);
			}
			finally
			{
				if (pen != null)
					pen.Dispose();
			}

			return;
		}

	}
}
