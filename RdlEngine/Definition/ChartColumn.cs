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


//GJL 110208 - Made some changes to allow second scale...


using System;
using System.Collections;
using System.Drawing;
//using System.Windows.Forms;JD 20080514 - Why? 
using System.Reflection.Emit;


namespace fyiReporting.RDL
{
	///<summary>
	/// Column chart definition and processing
	///</summary>
	internal class ChartColumn: ChartBase
	{
		int _GapSize=0;		// TODO: hard code for now - 06122007AJM Removed gap so that large category ranges display better

        internal ChartColumn(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX,Expression _ToolTipYFormat, Expression _ToolTipXFormat)
            : base(r, row, c, m, showTooltips,showTooltipsX,_ToolTipYFormat,_ToolTipXFormat)
		{
		}

		override internal void Draw(Report rpt)
		{
			CreateSizedBitmap();
            using (Graphics g1 = Graphics.FromImage(_bm))
            {              
                _aStream = new System.IO.MemoryStream();  
                IntPtr HDC = g1.GetHdc(); 
                //_mf = new System.Drawing.Imaging.Metafile(_aStream, HDC);
                _mf = new System.Drawing.Imaging.Metafile(_aStream, HDC, new RectangleF(0, 0, _bm.Width, _bm.Height),System.Drawing.Imaging.MetafileFrameUnit.Pixel);
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

				double max=0,min=0;	// Get the max and min values
				GetValueMaxMin(rpt, ref max, ref min,0, 1);

				DrawChartStyle(rpt, g);
				
				// Draw title; routine determines if necessary
				DrawTitle(rpt, g, ChartDefn.Title, new System.Drawing.Rectangle(0, 0, _bm.Width, Layout.TopMargin));

				// Adjust the left margin to depend on the Value Axis
				Size vaSize = ValueAxisSize(rpt, g, min, max);
				Layout.LeftMargin = vaSize.Width;

                // Adjust the right margin to depend on the Value Axis
                bool Show2ndY = ShowRightYAxis(rpt);
                Size vaSize2= vaSize;

                if (Show2ndY)
                {
                    double rmax = 0, rmin = 0;
                    GetMaxMinDataValue(rpt, out rmax, out rmin, 0, 2);
                    vaSize2 = ValueAxisSize(rpt, g, rmin, rmax);
                    Layout.RightMargin = vaSize2.Width;
                }

				// Draw legend
				System.Drawing.Rectangle lRect = DrawLegend(rpt, g, false, true);

				// Adjust the bottom margin to depend on the Category Axis
                Size caSize = CategoryAxisSize(rpt, g);
				Layout.BottomMargin = caSize.Height;

				AdjustMargins(lRect,rpt, g);		// Adjust margins based on legend.

				// Draw Plot area
				DrawPlotAreaStyle(rpt, g, lRect);

                int intervalCount = 0; //GJL - Used to get the interval count out of DrawValueAxis so that we don't recalculate it again.
                double incr = 0.0;	//GJL - As above
				// Draw Value Axis //GJL now as by ref params to return the values to the above variables
				if (vaSize.Width > 0)	// If we made room for the axis - we need to draw it
					DrawValueAxis(rpt, g, min, max, 
						new System.Drawing.Rectangle(Layout.LeftMargin - vaSize.Width, Layout.TopMargin, vaSize.Width, _bm.Height - Layout.TopMargin - Layout.BottomMargin), Layout.LeftMargin, Layout.Width - Layout.RightMargin,out incr,out intervalCount);
                
               
                //********************************************************************************************************************************************
                //Draw the 2nd value axis - obviously we will only want to do this if we choose a second axis        

                double ScaleFactor = 1.0;
                //Secong value axis            
                if (Show2ndY)
                    Draw2ndValueAxis(rpt, g, min, max, new System.Drawing.Rectangle(Layout.LeftMargin + Layout.PlotArea.Width, Layout.TopMargin, vaSize2.Width, _bm.Height - Layout.TopMargin - Layout.BottomMargin), Layout.LeftMargin, Layout.Width - Layout.RightMargin, incr, intervalCount, ref ScaleFactor);
                         
				// Draw Category Axis
				if (caSize.Height > 0)
                    // 090508ajm passing chart bounds in
					DrawCategoryAxis(rpt, g,
                        new System.Drawing.Rectangle(Layout.LeftMargin, _bm.Height - Layout.BottomMargin, Layout.PlotArea.Width, caSize.Height), Layout.TopMargin, caSize.Width);
                if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked)
					DrawPlotAreaStacked(rpt, g, max, min);
                else if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
					DrawPlotAreaPercentStacked(rpt, g);
				else
					DrawPlotAreaPlain(rpt, g, max, min,ScaleFactor);

				DrawLegend(rpt, g, false, false);

			}
		}

		void DrawPlotAreaPercentStacked(Report rpt, Graphics g)
		{
			int barsNeeded = CategoryCount; 
			int gapsNeeded = CategoryCount * 2;

			// Draw Plot area data
			double max = 1;

			int widthBar = (int) ((Layout.PlotArea.Width - gapsNeeded*_GapSize) / barsNeeded);
			int maxBarHeight = (int) (Layout.PlotArea.Height);	

			// Loop thru calculating all the data points
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				int barLoc=(int) (Layout.PlotArea.Left + (iRow-1) * ((double) (Layout.PlotArea.Width) / CategoryCount));
				barLoc += _GapSize;	// space before series

				double sum=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					double t = GetDataValue(rpt, iRow, iCol);
					if (t.CompareTo(double.NaN) == 0)
						t = 0;
					sum += t;
				}
				double v=0;
				Point saveP=Point.Empty;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					double t = GetDataValue(rpt, iRow, iCol);
					if (t.CompareTo(double.NaN)==0)
						t = 0;
					v += t;

					int h = (int) ((Math.Min(v/sum,max) / max) * maxBarHeight);
					Point p = new Point(barLoc, Layout.PlotArea.Top + (maxBarHeight -  h));

					System.Drawing.Rectangle rect;
					if (saveP == Point.Empty)
						rect = new System.Drawing.Rectangle(p, new Size(widthBar,h));
					else
						rect = new System.Drawing.Rectangle(p, new Size(widthBar, saveP.Y - p.Y));
                    DrawColumnBar(rpt, g, GetSeriesBrush(rpt, iRow, iCol), rect, iRow, iCol);

                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    //if (_showToolTips)
                    //{
                    //    String val = "ToolTip:" + t + "|X:" + (int)rect.X + "|Y:" + (int)(rect.Y) + "|W:" + rect.Width + "|H:" + rect.Height;
                    //    g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    //}

                    if (_showToolTips)
                    {
                        string display = ""; 
                        if (display.Length > 0) display += " , ";
                        display += t.ToString(_tooltipYFormat);
                        
                        String val = "ToolTip:" + display + "|X:" + (int)rect.X + "|Y:" + (int)rect.Y + "|W:" + rect.Width + "|H:" + rect.Height;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }
					saveP = p;
				}
			}

			return;
		}

        /* This method has been modified to allow for a Line plot type and to draw the chart correctly 
         * with selected plot type. 
         * 06122007AJM
         */
		void DrawPlotAreaPlain(Report rpt, Graphics g, double max, double min, double ScaleFactor)
		{
            /* Need to adjust bar count to allow for Line plot types
             * 06122007AJM */
            int ColumnCount = 0;
            for (int iCol = 1; iCol <= SeriesCount; iCol++)
            {
                if (GetPlotType(rpt, iCol, 1).ToUpper() != "LINE")
                {
                    ColumnCount++;
                }
            }
            if (ColumnCount == 0) { ColumnCount = 1; } //Handle no bars (All lines)
		    int barsNeeded = ColumnCount * CategoryCount;
            
			int gapsNeeded = CategoryCount * 2;

			// Draw Plot area data
			int widthBar = (int) ((Layout.PlotArea.Width - gapsNeeded*_GapSize) / barsNeeded);
			int maxBarHeight = (int) (Layout.PlotArea.Height);
            /* The following list has been added to keep track of the
             * previous point for drawing a line intead of a column
             * when the plottype is Line
             * 05122007AJM */
            bool DrawPoint;
            SortedList LastPoints = new SortedList();
            int lineLoc;
			for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				int barLoc=(int) (Layout.PlotArea.Left + (iRow-1) * ((double) (Layout.PlotArea.Width) / CategoryCount));

				barLoc += _GapSize;	// space before series
                lineLoc = barLoc + widthBar * ColumnCount / 2;
                for (int z = 0; z < 2; z++) 
                {
                    for (int iCol = 1; iCol <= SeriesCount; iCol++)
                    {
                        /* This for loop has been modified to select if the column should
                         * be drawn based on the Plot type of the column
                         * 05122007AJM */
                        if (GetPlotType(rpt, iCol, iRow).ToUpper() != "LINE")
                        {
                            if (z == 0)
                            {
                                double v = this.GetDataValue(rpt, iRow, iCol);
                                double tooltipVal = v;
                                if (GetYAxis(rpt, iCol, iRow).ToUpper() != "LEFT")
                                {
                                    //Scale the Y data...
                                    v /= ScaleFactor;
                                }

                                if (v.CompareTo(double.NaN) == 0)
                                    v = min;
                                int h = (int)(((Math.Min(v, max) - min) / (max - min)) * maxBarHeight);

                                DrawColumnBar(rpt, g, GetSeriesBrush(rpt, iRow, iCol),
                                    new System.Drawing.Rectangle(barLoc, Layout.PlotArea.Top + (maxBarHeight - h), widthBar, h), iRow, iCol);

                                //Add a metafilecomment to use as a tooltip GJL 26092008
                                if (_showToolTips)
                                {
                                    String val = "ToolTip:" + tooltipVal.ToString(_tooltipYFormat) + "|X:" + barLoc + "|Y:" + (int)(Layout.PlotArea.Top + (maxBarHeight - h)) + "|W:" + widthBar + "|H:" + h;
                                    g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                                }

                               

                                barLoc += widthBar;
                            }
                        }
                        else //This is a line type plot
                        {
                            if (z == 1)
                            {
                                double v = this.GetDataValue(rpt, iRow, iCol);
                                double tooltipVal = v;
                                if (GetYAxis(rpt, iCol, iRow).ToUpper() != "LEFT")
                                {
                                    //Scale the Y data...
                                    v /= ScaleFactor;
                                }

                                DrawPoint = true;

                                if (v.CompareTo(double.NaN) == 0)
                                {
                                    //don't draw null
                                        DrawPoint = false;
                                    //insert empty point
                                        LastPoints[iCol] = null;
                                   }
                                if (DrawPoint)
                                {
                                    int h = (int)(((Math.Min(v, max) - min) / (max - min)) * maxBarHeight);

                                    System.Drawing.Rectangle r = new System.Drawing.Rectangle(lineLoc, Layout.PlotArea.Top + (maxBarHeight - h), widthBar, h);

                                    Point p = new Point(r.Left, r.Top);
                                    if (LastPoints[iCol - 1] == null)
                                        LastPoints[iCol - 1] = p;
                                    bool DrawMarker = getNoMarkerVal(rpt, iCol, 1) == false;
                                    DrawDataPoint(rpt, g, new Point(p.X, p.Y - 14), iRow, iCol);  // todo: 14 is arbitrary
                                    if (DrawMarker) { DrawLegendLineMarker(g, GetSeriesBrush(rpt, iRow, iCol), new Pen(GetSeriesBrush(rpt, iRow, iCol)), SeriesMarker[iCol - 1], p.X - 5, p.Y - 5, 10);}
                                                                     

                                    if (LastPoints.ContainsKey(iCol) )
                                    {
                                        Point[] Points = new Point[2];
                                        Points[0] = p;
                                        Point pt = (Point)LastPoints[iCol];
                                        Points[1] = new Point(pt.X - 1, pt.Y);
                                        // 05052008AJM - Allowing for breaking lines in chart
                                        if (Points[1] != null) {
                                            String LineSize = getLineSize(rpt, iCol, 1);
                                            int intLineSize = 1;
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

                                            DrawLineBetweenPoints(g, rpt, GetSeriesBrush(rpt, iRow, iCol), Points,intLineSize);
                                        }
                                    }

                                    //Add a metafilecomment to use as a tooltip GJL 26092008
                                    if (_showToolTips)
                                    {
                                        String val = "ToolTip:" + tooltipVal.ToString(_tooltipYFormat) + "|X:" + (int)(p.X - 5) + "|Y:" + (int)(p.Y - 5) + "|W:10|H:10";
                                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                                    }

                                  

                                    LastPoints[iCol] = p;
                                }
                            }
                        }
                    }
				}
			}
		}

        internal void DrawLegendLineMarker(Graphics g, Brush b, Pen p, ChartMarkerEnum marker, int x, int y, int mSize)
        {
             if (b.GetType() == typeof(System.Drawing.Drawing2D.HatchBrush))
             {
                System.Drawing.Drawing2D.HatchBrush hb = ( System.Drawing.Drawing2D.HatchBrush) b;
                b = new SolidBrush(hb.ForegroundColor);
             }

            Pen p2;
            PointF[] points;
            switch (marker)
            {
                case ChartMarkerEnum.Bubble:
                case ChartMarkerEnum.Circle:
                    g.FillEllipse(b, x, y, mSize, mSize);
                    break;
                case ChartMarkerEnum.Square:
                    g.FillRectangle(b, x, y, mSize, mSize);
                    break;
                case ChartMarkerEnum.Plus:                
                   
                    p2 = new Pen(b, 2.0f);
                    g.DrawLine(p2, new Point(x + (mSize + 1) / 2, y), new Point(x + (mSize + 1) / 2, y + mSize));
                    //g.DrawLine(p2, new Point(x + (mSize + 1) / 2, y + (mSize + 1) / 2), new Point(x + mSize, y + (mSize + 1) / 2));
                    break;
                case ChartMarkerEnum.Diamond:
                    points = new PointF[5];
                    points[0] = points[4] = new PointF(x + (mSize + 1) / 2, y);	// starting and ending point
                    points[1] = new PointF(x, y + (mSize + 1) / 2);
                    points[2] = new PointF(x + (mSize + 1) / 2, y + mSize);
                    points[3] = new PointF(x + mSize, y + (mSize + 1) / 2);
                    g.FillPolygon(b, points);
                    break;
                case ChartMarkerEnum.Triangle:
                    points = new PointF[4];
                    points[0] = points[3] = new PointF(x + (mSize + 1) / 2, y);	// starting and ending point
                    points[1] = new PointF(x, y + mSize);
                    points[2] = new PointF(x + mSize, y + mSize);
                    g.FillPolygon(b, points);
                    break;
                case ChartMarkerEnum.X:
                    p2 = new Pen(b, 2.0f);
                    g.DrawLine(p2, new Point(x, y), new Point(x + mSize, y + mSize));
                    g.DrawLine(p2, new Point(x, y + mSize), new Point(x + mSize, y));
                    break;
            }
            return;
        }

        /* This code was copied from the Line drawing class. There may be a better
         * way to do this by reusing the code from the original class but this 
         * will work, the only issue being that if the line drawing is changed then 
         * this function will need to be updated as well
         * 05122007 AJM */
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

		void DrawPlotAreaStacked(Report rpt, Graphics g, double max, double min)
		{
			int barsNeeded = CategoryCount; 
			int gapsNeeded = CategoryCount * 2;

			int widthBar = (int) ((Layout.PlotArea.Width - gapsNeeded*_GapSize) / barsNeeded);
			int maxBarHeight = (int) (Layout.PlotArea.Height);	

			// Loop thru calculating all the data points
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				int barLoc=(int) (Layout.PlotArea.Left + (iRow-1) * ((double) (Layout.PlotArea.Width) / CategoryCount));
				barLoc += _GapSize;	// space before series

				double v=0;
				Point saveP=Point.Empty;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					double t = GetDataValue(rpt, iRow, iCol);
					if (t.CompareTo(double.NaN) == 0)
						t = 0;
					v += t;

					int h = (int) (((Math.Min(v,max)-min) / (max-min)) * maxBarHeight);
					Point p = new Point(barLoc, Layout.PlotArea.Top + (maxBarHeight -  h));

					System.Drawing.Rectangle rect;
					if (saveP == Point.Empty)
						rect = new System.Drawing.Rectangle(p, new Size(widthBar,h));
					else
						rect = new System.Drawing.Rectangle(p, new Size(widthBar, saveP.Y - p.Y));
                    DrawColumnBar(rpt, g, GetSeriesBrush(rpt, iRow, iCol), rect, iRow, iCol);

                    //Add a metafilecomment to use as a tooltip GJL 26092008
                    if (_showToolTips)
                    {
                        String val = "ToolTip:" + t.ToString(_tooltipYFormat) + "|X:" + (int)rect.X + "|Y:" + (int)(rect.Y) + "|W:" + rect.Width + "|H:" + rect.Height;
                        g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(val));
                    }

					saveP = p;
				}
			}

			return;
		}

		// Calculate the size of the category axis
		protected Size CategoryAxisSize(Report rpt, Graphics g)
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
			int maxHeight=0;
            int maxWidth = 0;
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

                if (tSize.Width > maxWidth)
                    maxWidth = tSize.Width;

				if (iRow == CategoryCount)
					_LastCategoryWidth = tSize.Width;
			}
            size.Width = maxWidth;          // set the widest entry

			// Add on the tallest category name
			size.Height += maxHeight;

			// Account for tick marks
			int tickSize=0;
			if (a.MajorTickMarks == AxisTickMarksEnum.Cross ||
				a.MajorTickMarks == AxisTickMarksEnum.Outside)
				tickSize = this.AxisTickMarkMajorLen;
			else if (a.MinorTickMarks == AxisTickMarksEnum.Cross ||
				a.MinorTickMarks == AxisTickMarksEnum.Outside)
				tickSize = this.AxisTickMarkMinorLen;

			size.Height += tickSize;
            /* This swap is done so that the size will work correctly with rotated
             * (vertical) category names
             * 06122007AJM */
            if (maxWidth > (CategoryCount * SeriesCount) && s == null)  // kas: swap only when no axis style info provided
            {
                int tmp = size.Height;
                size.Height = size.Width;
                size.Width = tmp;
            }
			return size;
		}

		// DrawCategoryAxis 
        //09052008ajm added plottop variable to collect the top of the chart
        protected void DrawCategoryAxis(Report rpt, Graphics g, System.Drawing.Rectangle rect, int plotTop, int maxWidth)
		{
			if (this.ChartDefn.CategoryAxis == null)
				return;
			Axis a = this.ChartDefn.CategoryAxis.Axis;
			if (a == null)
				return;
			Style s = a.Style;
			Size tSize = DrawTitleMeasure(rpt, g, a.Title);
			DrawTitle(rpt, g, a.Title, new System.Drawing.Rectangle(rect.Left, rect.Bottom-tSize.Height, rect.Width, tSize.Height));

			// Account for tick marks
			int tickSize=0;
			if (a.MajorTickMarks == AxisTickMarksEnum.Cross ||
				a.MajorTickMarks == AxisTickMarksEnum.Outside)
				tickSize = this.AxisTickMarkMajorLen;
			else if (a.MinorTickMarks == AxisTickMarksEnum.Cross ||
				a.MinorTickMarks == AxisTickMarksEnum.Outside)
				tickSize = this.AxisTickMarkMinorLen;

			int drawWidth;
			int catCount = ChartDefn.Type == ChartTypeEnum.Area? CategoryCount-1: CategoryCount;
			drawWidth = rect.Width / catCount;
            bool mustSize = a.CanOmit && (drawWidth < maxWidth || ChartDefn.Type == ChartTypeEnum.Area);
            int MajorGrid = 0;
            // 15052008AJM Fixed for charts without a set major interval
            if (a.MajorInterval != null) {
                // 09052008WRP  get major interval value  
                MajorGrid = (int)a.MajorInterval.EvaluateDouble(rpt,this._row); 
            }
            // 12052008WRP
            // setup month scale gridline plot - must check dealing with date data type
            DateTime CurrentDate = DateTime.Now; //used for checking change in month 
            DateTime OldDate = DateTime.Now; //used for checking change in month 
            DateTime TempDate = DateTime.Now;//working variable
            bool date = false; //used for confirming dealing with date data
            int PreviousLocation = rect.Left; //used to keep track of previous gridline location on x axis - set to x position of category axis at start
			TypeCode tc;
			object first = this.GetCategoryValue(rpt, 1, out tc);
            if (first != null)
            {
                switch (tc)
                {
                    case TypeCode.DateTime:
                       date = true;
                        break;
                    default:
                        break;
                }
            }
            if (date) //initialising date values for use with date scale
            {
                CurrentDate = (DateTime)first;
                OldDate = CurrentDate;
                TempDate = CurrentDate;
            }
            for (int iRow=1; iRow <= CategoryCount; iRow++)
			{
				object v = this.GetCategoryValue(rpt, iRow, out tc);
                //make sure we are dealing with datetime type
                if (date)
                {
                    CurrentDate = (DateTime)v;
                              
                }
                int drawLoc=(int) (rect.Left + (iRow-1) * ((double) rect.Width / catCount));
               
				// Draw the category text
                int skip = 0;
				if (a.Visible && !a.Month)  //18052008WRP only show category labels if not month scale
				{
					System.Drawing.Rectangle drawRect;
                    Size size = Size.Empty;

                    if (mustSize)
                    {	// Area chart - value is centered under the tick mark
                        if (s != null)
                        {
                            size = s.MeasureString(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                        }
                        else
                        {
                            size = Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                        }
                    }

					if (ChartDefn.Type == ChartTypeEnum.Area)
					{	// Area chart - value is centered under the tick mark
						drawRect = 
								new System.Drawing.Rectangle(drawLoc - size.Width/2, rect.Top+tickSize, size.Width, size.Height);
					}
					else	// Column/Line charts are just centered in the region.
						drawRect = new System.Drawing.Rectangle(drawLoc, rect.Top+tickSize, drawWidth, rect.Height-tSize.Height);

                    if (mustSize && drawRect.Width < size.Width)
                    {
                        skip = (int) (size.Width / drawWidth);
                        drawRect.Width = size.Width;
                    }

					if (s == null)
						Style.DrawStringDefaults(g, v, drawRect);
					else
						s.DrawString(rpt, g, v, tc, null, drawRect);
				}

                //09052008WRP Draw major gridlines and place category labels for months scale 
                if (a.Month && date && a.Visible)
                {

                    if (CurrentDate.Month != OldDate.Month)
                    {
                        TempDate = CurrentDate;
                        DrawCategoryAxisGrid(rpt, g, a.MajorGridLines, new Point(drawLoc, rect.Top), new Point(drawLoc, plotTop)); //Don't overdraw the Y axis on the first gridline
                        CurrentDate = CurrentDate.AddMonths(OldDate.Month - CurrentDate.Month); // get previous category month value
                        string MonthString = CurrentDate.ToString("MMMM");
                        Size lSize = DrawCategoryTitleMeasure(rpt, g, MonthString,s);
                        int catlabelLoc = (int)((drawLoc - PreviousLocation) / 2) + PreviousLocation - lSize.Width / 2;
                        DrawCategoryLabel(rpt, g, MonthString, a.Style, new System.Drawing.Rectangle(catlabelLoc, rect.Top - (lSize.Height - 25), lSize.Width, lSize.Height));
                        PreviousLocation = drawLoc;
                        OldDate = TempDate;
                       
                    }
                }
                if ((MajorGrid != 0) && ((iRow-1) % MajorGrid == 0) && !(a.Month))
                //if (((iRow - 1) % ((int)a.MajorInterval.EvaluateDouble(rpt, this.ChartRow.RowNumber)) == 0) && !(a.Month)) 
                {
                    DrawCategoryAxisGrid(rpt, g, a.MajorGridLines, new Point(drawLoc, rect.Top), new Point(drawLoc, plotTop));
                }
				// Draw the Major Tick Marks (if necessary)
				DrawCategoryAxisTick(g, true, a.MajorTickMarks, new Point(drawLoc, rect.Top));
                iRow += skip;
			}//exit from for loop - no more category data

            if (a.Month && date && a.Visible)// 16052008WRP draw last category label for months scale
            {
                string MonthString = OldDate.ToString("MMMM");
                Size lSize = DrawCategoryTitleMeasure(rpt, g, MonthString,s);
                int catlabelLoc = (int)((rect.Right - PreviousLocation) / 2) + PreviousLocation - lSize.Width / 2;
                DrawCategoryLabel(rpt, g, MonthString, a.Style, new System.Drawing.Rectangle(catlabelLoc, rect.Top - (lSize.Height - 25), lSize.Width, lSize.Height));
            }
                    
        
			// Draw the end on (if necessary)
			DrawCategoryAxisTick(g, true, a.MajorTickMarks, new Point(rect.Right, rect.Top));

			return;
		}

        

		protected void DrawCategoryAxisTick(Graphics g, bool bMajor, AxisTickMarksEnum tickType, Point p)
		{
			int len = bMajor? AxisTickMarkMajorLen: AxisTickMarkMinorLen;
			switch (tickType)
			{
				case AxisTickMarksEnum.Outside:
					g.DrawLine(Pens.Black, new Point(p.X, p.Y), new Point(p.X, p.Y+len));
					break;
				case AxisTickMarksEnum.Inside:
					g.DrawLine(Pens.Black, new Point(p.X, p.Y), new Point(p.X, p.Y-len));
					break;
				case AxisTickMarksEnum.Cross:
					g.DrawLine(Pens.Black, new Point(p.X, p.Y-len), new Point(p.X, p.Y+len));
					break;
				case AxisTickMarksEnum.None:
				default:
					break;
			}
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

		void DrawColumnBar(Report rpt, Graphics g, Brush brush, System.Drawing.Rectangle rect, int iRow, int iCol)
		{
			if (rect.Height <= 0)
				return;
            //we want to separate the bars with some whitespace.. GJL 080208
            rect = new System.Drawing.Rectangle(rect.Left + 2, rect.Top, rect.Width -3, rect.Height);
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
				p = new Point(rect.Left, rect.Top - 14); // todo: 14 is arbitrary
				DrawDataPoint(rpt, g, p, iRow, iCol);
			}

			return;
		}

		protected void DrawValueAxis(Report rpt, Graphics g, double min, double max,
                        System.Drawing.Rectangle rect, int plotLeft, int plotRight, out double incr,out int intervalCount)
		{
            incr = 0;
            intervalCount = 0;
			if (this.ChartDefn.ValueAxis == null)
				return;
			Axis a = this.ChartDefn.ValueAxis.Axis;
			if (a == null)
				return;
			Style s = a.Style;

           //int intervalCount;
            //double incr;
            SetIncrementAndInterval(rpt, a, min, max, out incr, out intervalCount);      // Calculate the interval count
	
			Size tSize = DrawTitleMeasure(rpt, g, a.Title);
            DrawTitle(rpt, g, a.Title, new System.Drawing.Rectangle(rect.Left, rect.Top, tSize.Width, rect.Height));

			double v = min;
			for (int i=0; i < intervalCount+1; i++)	 
			{
				int h = (int) (((Math.Min(v,max)-min) / (max-min)) * rect.Height);
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
						new System.Drawing.Rectangle(rect.Left+tSize.Width, rect.Top + rect.Height - h - size.Height/2, rect.Width-tSize.Width, size.Height);
					s.DrawString(rpt, g, v, TypeCode.Double, null, vRect);
				}
				else
				{
					Size size = Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
					System.Drawing.Rectangle vRect = 
						new System.Drawing.Rectangle(rect.Left+tSize.Width, rect.Top + rect.Height - h - size.Height/2, rect.Width-tSize.Width, size.Height);
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
			
            //12052008WRP this line not required adds tick at bottom end of right y axis.
            //DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(plotRight, rect.Bottom));

			return;
		}


        //*******************************************************************************************************************************
        //Draws the second value axis
        protected void Draw2ndValueAxis(Report rpt, Graphics g, double min, double max,
                        System.Drawing.Rectangle rect, int plotLeft, int plotRight,double incr,int intervalCount,ref double ScaleFactor)
        {           

            if (this.ChartDefn.ValueAxis == null)
                return;
            Axis a = this.ChartDefn.ValueAxis.Axis;
            if (a == null)
                return;
            Style s = a.Style;

            double thisMin = 0;
            double thisMax = 0;

            GetMaxMinDataValue(rpt, out thisMax, out thisMin, 0, 2);
            thisMin = 0; //Stop rescaling the min on autoscale


            //200208AJM GJL Yet another new scale
            _gridIncrs = 10; //PJR 20071113 - grid incrs set & adjusted in here now

            incr = thisMax / _gridIncrs;	// should be range between max and min?
            double log = Math.Floor(Math.Log10(Math.Abs(incr)));


            double logPow = Math.Pow(10, log) * Math.Sign(thisMax);
            double logDig = (int)(incr / logPow + .5);

            // promote the MSD to either 1, 2, or 5
            if (logDig > 5.0)
                logDig = 10.0;
            else if (logDig > 2.0)
                logDig = 5.0;
            else if (logDig > 1.0)
                logDig = 2.0;
            //PJR 20071113 - reduce scale for large overscale options by decreasing _gridIncrs
            while (thisMax < logDig * logPow * _gridIncrs)
            {
                _gridIncrs--;
            }
            //_gridIncrs++;

            //PJR 20071113 - expand scale so that it is able to fit the max value by increasing _gridIncrs
            while (thisMax > logDig * logPow * _gridIncrs)
            {
                _gridIncrs++;
            }

            double tmpMax = thisMax;
            thisMax = (int)(logDig * logPow * _gridIncrs + 0.5);
            if (tmpMax > thisMax - ((thisMax / _gridIncrs) * .5))
            {

                thisMax += (thisMax / _gridIncrs);
                _gridIncrs++;
            }
            ScaleFactor = thisMax / max;
            incr = thisMax / _gridIncrs;

            ////19022008AJM New Scaling
            //bool Happy = false;
            //double rIncr;
            //double rInt = intervalCount - 1;
            //double log;
            //double logPow;
            //double logDig;
            //double lMax = max;
            //for (int i = 0; i < 2; i++)
            //{
            //    rIncr = thisMax / rInt;
            //    log = Math.Floor(Math.Log10(Math.Abs(rIncr)));
                
            //    logPow = Math.Pow(10, log) * Math.Sign(thisMax);
            //    logDig = (int)(rIncr / logPow + .5);

            //    if (logDig > 5.0)
            //        logDig = 10.0;
            //    else if (logDig > 2.0)
            //        logDig = 5.0;
            //    else if (logDig > 1.0)
            //        logDig = 2.0;

            //    thisMax = (int)(logDig * logPow * rInt + 0.5);
            //    ScaleFactor = thisMax / lMax;
            //    if (thisMax < (ScaleFactor * (lMax - (incr / 2))))
            //    {
            //        i ++;
            //    }
            //    thisMax += logDig * logPow;
            //}



            //OK...  thisMax needs to be rounded up to the next interval...         
            //Scale based on thisMax and Max...        
                       
            //ScaleFactor = thisMax / (max - (max / intervalCount)) ;
            //if (thisMax < max)
            //{
            //    ScaleFactor = 1 / ScaleFactor;
            //}
            //ScaleFactor = Math.Round(ScaleFactor, 1);


            //double factor = System.Math.Pow(10, System.Math.Floor(System.Math.Log10(ScaleFactor)));
        
            //ScaleFactor = System.Math.Round(ScaleFactor / factor,1) * factor;                    
            //    if( ScaleFactor < 1)
            //    { 
            //        while ((ScaleFactor * 10) % 5 != 0 && (ScaleFactor * 10) % 2 != 0 && ScaleFactor != 1)
            //            {
            //                ScaleFactor += 0.1;
            //            }             
            //    }
            //    else if (ScaleFactor < 10)
            //    {
            //        while (ScaleFactor % 5 != 0 || ScaleFactor % 2 != 0)
            //        {
            //            ScaleFactor++;
            //        }
            //    }
            //    else
            //    {
            //        while (ScaleFactor % 5 != 0)
            //        {
            //            ScaleFactor++;
            //        }

            //    }

            //if (thisMax < max)
            //{
            //    ScaleFactor = 1 / ScaleFactor;
            //    ScaleFactor = Math.Round(ScaleFactor, (int)System.Math.Floor(System.Math.Log10(factor)));
            //}

       
            Size tSize = DrawTitleMeasure(rpt, g, a.Title2);         

           // rect.Width = (int)g.VisibleClipBounds.Width - rect.Left + 20;

            DrawTitle(rpt, g, a.Title2, new System.Drawing.Rectangle((int)rect.Right - tSize.Width, rect.Top, tSize.Width, rect.Height));

            double v = min;
            for (int i = 0; i < _gridIncrs + 1; i++)
            {
                int h = (int)(((Math.Min(v, thisMax) - thisMin) / (thisMax - thisMin)) * rect.Height);
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
                        new System.Drawing.Rectangle(rect.Left - (int)(tSize.Width * .5), rect.Top + rect.Height - h - size.Height / 2, rect.Width - tSize.Width, size.Height);
                    s.DrawString(rpt, g, v, TypeCode.Double, null, vRect);
                }
                else
                {
                    Size size = Style.MeasureStringDefaults(rpt, g, v, TypeCode.Double, null, int.MaxValue);
                    System.Drawing.Rectangle vRect =
                        new System.Drawing.Rectangle(rect.Left - (int)(tSize.Width * .5), rect.Top + rect.Height - h - size.Height / 2, rect.Width - tSize.Width * 2, size.Height);
                    Style.DrawStringDefaults(g, v, vRect);
                }          

                v += incr;

                DrawValueAxisTick(rpt, g, true, a.MajorTickMarks, a.MajorGridLines, new Point(plotRight - (AxisTickMarkMajorLen / 2), rect.Top + rect.Height - h));

            }

            return;
        }
        //*******************************************************************************************************************************

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
					e = new Point(p.X+len, p.Y);
					break;
				case AxisTickMarksEnum.Cross:
					s = new Point(p.X-len, p.Y);
					e = new Point(p.X+len, p.Y);
					break;
				case AxisTickMarksEnum.Outside:
				default:
					s = new Point(p.X-len, p.Y);
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

        //WhichAxis.... 1 = Left, 2 = Right GJL 140208
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

			// Now we need to add in the width of the title (if any)
			Size titleSize = DrawTitleMeasure(rpt, g, a.Title);
			size.Width += titleSize.Width;

			return size;
		}
	}
}
