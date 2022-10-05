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
using System.Drawing;


namespace fyiReporting.RDL
{
	///<summary>
	/// Pie chart definition and processing.
	///</summary>
	internal class ChartPie: ChartBase
	{

        internal ChartPie(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX,Expression _ToolTipYFormat, Expression _ToolTipXFormat)
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

				DrawChartStyle(rpt, g);
				
				// Draw title; routine determines if necessary
				DrawTitle(rpt, g, ChartDefn.Title, new System.Drawing.Rectangle(0, 0, _bm.Width, Layout.TopMargin));

				// Draw legend
				System.Drawing.Rectangle lRect = DrawLegend(rpt, g, false, true);

				// Adjust the bottom margin to depend on the Category Axis
				Size caSize = CategoryAxisSize(rpt, g);
				Layout.BottomMargin = caSize.Height;

				// 20022008 AJM GJL - Added required info
				AdjustMargins(lRect,rpt,g);		// Adjust margins based on legend.

				// Draw Plot area
				DrawPlotAreaStyle(rpt, g, lRect);

				// Draw Category Axis
				if (caSize.Height > 0)
					DrawCategoryAxis(rpt, g,  
						new System.Drawing.Rectangle(Layout.LeftMargin, _bm.Height-Layout.BottomMargin, _bm.Width - Layout.LeftMargin - Layout.RightMargin, caSize.Height));

				if (ChartDefn.Type == ChartTypeEnum.Doughnut)
					DrawPlotAreaDoughnut(rpt, g);
				else 
					DrawPlotAreaPie(rpt, g);

				DrawLegend(rpt, g, false, false);
			}
		}

		void DrawPlotAreaDoughnut(Report rpt, Graphics g)
		{
			// Draw Plot area data
			int widthPie = Layout.PlotArea.Width;
			int maxHeight = Layout.PlotArea.Height;	
			int maxPieSize = Math.Min(widthPie, maxHeight);
			int doughWidth = maxPieSize / 4 / CategoryCount;
			if (doughWidth < 2)
				doughWidth = 2;			// enforce minimum width

			float startAngle;
			float endAngle;
			int pieLocX;
			int pieLocY;
			int pieSize;

			// Go and draw the pies
			int left = Layout.PlotArea.Left + (maxPieSize == widthPie? 0: (widthPie - maxPieSize) / 2);
			int top =   Layout.PlotArea.Top + (maxPieSize == maxHeight? 0: (maxHeight - maxPieSize) / 2);
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				pieLocX= left + ((iRow-1) * doughWidth);
				pieLocY= top + ((iRow-1) * doughWidth);

				double total=0;		// sum up for this category
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					total += this.GetDataValue(rpt, iRow, iCol);
				}

				// Pie size decreases as we go in
				startAngle=0.0f;
				pieSize = maxPieSize - ((iRow - 1) * doughWidth * 2);
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);
					endAngle = (float) (startAngle + (v / total * 360));

                    DrawPie(g, rpt, GetSeriesBrush(rpt, iRow, iCol), 
						new System.Drawing.Rectangle(pieLocX, pieLocY, pieSize, pieSize), iRow, iCol, startAngle, endAngle);

					startAngle = endAngle;
				}
			}
			// Now draw the center hole with the plot area style
			if (ChartDefn.PlotArea == null || ChartDefn.PlotArea.Style == null)
				return;
			pieLocX= left + (CategoryCount * doughWidth);
			pieLocY= top + (CategoryCount * doughWidth);
			pieSize = maxPieSize - (CategoryCount * doughWidth * 2);
			System.Drawing.Rectangle rect = new System.Drawing.Rectangle(pieLocX, pieLocY, pieSize, pieSize);
			Style s = ChartDefn.PlotArea.Style;

			Rows cData = ChartDefn.ChartMatrix.GetMyData(rpt);
			Row r = cData.Data[0];
			s.DrawBackgroundCircle(rpt, g, r, rect);
		}

		void DrawPlotAreaPie(Report rpt, Graphics g)
		{
			int piesNeeded = CategoryCount; 
			int gapsNeeded = CategoryCount + 1;
			int gapSize=13;

			// Draw Plot area data
			int widthPie = (int) ((Layout.PlotArea.Width - (gapsNeeded*gapSize)) / piesNeeded);
			int maxHeight = (int) (Layout.PlotArea.Height);	
			int maxPieSize = Math.Min(widthPie, maxHeight);
			int pieLocY = Layout.PlotArea.Top + ((maxHeight - maxPieSize) / 2);

			float startAngle;
			float endAngle;

			// calculate the size of the largest category
			//   all pie sizes will be relative to that maximum
			double maxCategory=0;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				double total=0;
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					total += this.GetDataValue(rpt, iRow, iCol);
				}
				if (total > maxCategory)
					maxCategory = total;
			}

			// Go and draw the pies
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				int pieLocX=(int) (Layout.PlotArea.Left + ((iRow-1) * ((double) (Layout.PlotArea.Width) / CategoryCount)));

				pieLocX += gapSize;	// space before series
				double total=0;
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					total += this.GetDataValue(rpt, iRow, iCol);
				}

				// Pie size is a ratio of the area of the pies (not the diameter)
				startAngle=0.0f;
				int pieSize = (int) (2 * Math.Sqrt(Math.PI * ((maxPieSize/2) * (maxPieSize/2) * total/maxCategory) / Math.PI));
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					double v = this.GetDataValue(rpt, iRow, iCol);
					endAngle = (float) (startAngle + (v / total * 360));

                    DrawPie(g, rpt, GetSeriesBrush(rpt, iRow, iCol), 
						new System.Drawing.Rectangle(pieLocX, pieLocY, pieSize, pieSize), iRow, iCol, startAngle, endAngle);

					startAngle = endAngle;
				}
			}
		}

		// Calculate the size of the category axis
		protected Size CategoryAxisSize(Report rpt, Graphics g)
		{
			Size size=Size.Empty;
			if (this.ChartDefn.CategoryAxis == null || 
				this.ChartDefn.Type == ChartTypeEnum.Doughnut)	// doughnut doesn't get this
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
			int maxHeight=0;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				object v = this.GetCategoryValue(rpt, iRow, out tc);
				Size tSize;
				if (s == null)
					tSize = Style.MeasureStringDefaults(rpt, g, v, tc, null, int.MaxValue);

				else
					tSize =s.MeasureString(rpt, g, v, tc, null, int.MaxValue);

				if (tSize.Height > maxHeight)
					maxHeight = tSize.Height;
			}

			// Add on the tallest category name
			size.Height += maxHeight;
			return size;
		}

		// DrawCategoryAxis 
		protected void DrawCategoryAxis(Report rpt, Graphics g, System.Drawing.Rectangle rect)
		{
			if (this.ChartDefn.CategoryAxis == null)
				return;
			Axis a = this.ChartDefn.CategoryAxis.Axis;
			if (a == null)
				return;
			Style s = a.Style;

			Size tSize = DrawTitleMeasure(rpt, g, a.Title);
			DrawTitle(rpt, g, a.Title, new System.Drawing.Rectangle(rect.Left, rect.Bottom-tSize.Height, rect.Width, tSize.Height));

			int drawWidth = rect.Width / CategoryCount;
			TypeCode tc;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				object v = this.GetCategoryValue(rpt, iRow, out tc);

				int drawLoc=(int) (rect.Left + ((iRow-1) * ((double) rect.Width / CategoryCount)));

				// Draw the category text
				if (a.Visible)
				{
					System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle(drawLoc, rect.Top, drawWidth, rect.Height-tSize.Height);
					if (s == null)
						Style.DrawStringDefaults(g, v, drawRect);
					else
						s.DrawString(rpt, g, v, tc, null, drawRect);
				}
			}

			return;
		}

		void DrawPie(Graphics g, Report rpt, Brush brush, System.Drawing.Rectangle rect, int iRow, int iCol, float startAngle, float endAngle)
		{
            if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Exploded)
			{
				// Need to adjust the rectangle 
				int side = (int) (rect.Width * .75);	// side needs to be smaller to account for exploded pies
				int offset = (int) (side * .1);			//  we add a little to the left and top
				int adjX, adjY;
				adjX = adjY = (int) (side * .1);

				float midAngle = startAngle + ((endAngle - startAngle)/2);

				if (midAngle < 90)
				{
				}
				else if (midAngle < 180)
				{
					adjX = -adjX;
				}
				else if (midAngle < 270)
				{
					adjX = adjY = -adjX;
				}
				else
				{
					adjY = - adjY;
				}
				rect = new System.Drawing.Rectangle(rect.Left + adjX + offset, rect.Top + adjY + offset, side, side);
			}

			g.FillPie(brush, rect, startAngle, endAngle - startAngle);
			g.DrawPie(Pens.Black, rect, startAngle, endAngle - startAngle);

			return;
		}
	}
}
