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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace fyiReporting.RDL
{
	///<summary>
	/// Base class of all charts.
	///</summary>
	internal abstract class ChartBase : IDisposable
	{
		protected Chart _ChartDefn; // GJL 14082008 Using Vector Graphics
		MatrixCellEntry[,] _DataDefn;
		protected Bitmap _bm;
        protected Metafile _mf; // GJL 14082008 Using Vector Graphics
        public System.IO.MemoryStream _aStream; // GJL 14082008 Using Vector Graphics
		protected ChartLayout Layout;
		Brush[] _SeriesBrush;
		ChartMarkerEnum[] _SeriesMarker;
		protected int _LastCategoryWidth=0;
		protected Row _row;					// row chart created on
        protected int _gridIncrs = 10; //PJR 20071113 - made global to class so it can be "adjusted" to fit MAX value
        protected bool _showToolTips;
        protected bool _showToolTipsX;
        protected string _tooltipXFormat;
        protected string _tooltipYFormat;

        internal ChartBase(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX,Expression _ToolTipYFormat, Expression _ToolTipXFormat)
		{
			_ChartDefn = c;
			_row = row;
			_DataDefn = m;
			_bm = null;
			int width = _ChartDefn.WidthCalc(r, null);
			int height = RSize.PixelsFromPoints(_ChartDefn.HeightOrOwnerHeight);
			Layout = new ChartLayout(width, height);
			_SeriesBrush = null;
			_SeriesMarker = null;
            _showToolTips = showTooltips.EvaluateBoolean(r, row);
            _showToolTipsX = showTooltipsX.EvaluateBoolean(r, row);
            _tooltipYFormat = _ToolTipYFormat.EvaluateString(r, row);
            _tooltipXFormat = _ToolTipXFormat.EvaluateString(r, row);

		}

		internal virtual void Draw(Report rpt)
		{
		}

        protected Row ChartRow
        {
            get { return _row; }
        }

		internal void Save(Report rpt, System.IO.Stream stream, ImageFormat im)
		{
			if (_bm == null)
				Draw(rpt);
            _mf.Save(stream, im);
	//		_bm.Save(stream, im);
		}

		internal System.Drawing.Imaging.Metafile Image(Report rpt) // GJL 14082008 Using Vector Graphics
		{
			if (_bm == null)
				Draw(rpt);

			//return _bm;
            return _mf;

		}

		protected Bitmap CreateSizedBitmap()
		{
			if (_bm != null)
			{
				_bm.Dispose();
				_bm = null;
			}
			_bm = new Bitmap(Layout.Width, Layout.Height);
			return _bm;
		}

        protected Bitmap CreateSizedBitmap(int W, int H)
        {
            if (_bm != null)
            {
                _bm.Dispose();
                _bm = null;
            }
            _bm = new Bitmap(W, H);
            return _bm;
        }

		protected int AxisTickMarkMajorLen
		{
			get{return 6;}
		}

		protected int AxisTickMarkMinorLen
		{
			get{return 3;}
		}

		protected int CategoryCount
		{
			get{return (_DataDefn.GetLength(0) - 1);}
		}

		protected Chart ChartDefn
		{
			get{return _ChartDefn;}
		}

		protected MatrixCellEntry[,] DataDefn
		{
			get{return _DataDefn;}
		}

		protected Brush[] SeriesBrush(Report rpt, Row row, ReportDefn defn)
		{
				if (_SeriesBrush == null)
					_SeriesBrush = GetSeriesBrushes(rpt,row,defn);	// These are all from Brushes class; so no Dispose should be used
				return _SeriesBrush;
		}
	
		protected ChartMarkerEnum[] SeriesMarker
		{
			get
			{
				if (_SeriesMarker == null)
					_SeriesMarker = GetSeriesMarkers();
				return _SeriesMarker;
			}
		}

		protected int SeriesCount
		{
			get{return (_DataDefn.GetLength(1) - 1);}
		}

		protected void DrawChartStyle(Report rpt, Graphics g)
		{
			System.Drawing.Rectangle rect = new  System.Drawing.Rectangle(0, 0, Layout.Width, Layout.Height);
			if (_ChartDefn.Style == null)
			{
				g.FillRectangle(Brushes.White, rect);
			}
			else
			{
				Row r = FirstChartRow(rpt);
				_ChartDefn.Style.DrawBorder(rpt, g, r, rect);
				_ChartDefn.Style.DrawBackground(rpt, g, r, rect);
			}

			return;
		}

		// Draws the Legend and then returns the rectangle it drew in
		protected System.Drawing.Rectangle DrawLegend(Report rpt, Graphics g, bool bMarker, bool bBeforePlotDrawn)
		{
			Legend l = _ChartDefn.Legend;
			if (l == null)
				return System.Drawing.Rectangle.Empty;
			if (!l.Visible)
				return System.Drawing.Rectangle.Empty;
			if (_ChartDefn.SeriesGroupings == null)
				return System.Drawing.Rectangle.Empty;
			if (bBeforePlotDrawn)
			{
				if (this.IsLegendInsidePlotArea())
					return System.Drawing.Rectangle.Empty;
			}
			else if (!IsLegendInsidePlotArea())			// Only draw legend after if inside the plot
				return System.Drawing.Rectangle.Empty;

			Font drawFont = null;
			Brush drawBrush = null;
			StringFormat drawFormat = null;
	
			// calculated bounding rectangle of the legend
			System.Drawing.Rectangle rRect;
			Style s = l.Style;
			try		// no matter what we want to dispose of the graphic resources
			{
				if (s == null)
				{
					drawFont = new Font("Arial", 10);
					drawBrush = new SolidBrush(Color.Black);
					drawFormat = new StringFormat();
					drawFormat.Alignment = StringAlignment.Near;
				}
				else
				{
					drawFont = 	s.GetFont(rpt, null);
					drawBrush = s.GetBrush(rpt, null);
					drawFormat = s.GetStringFormat(rpt, null, StringAlignment.Near);
				}

				int x, y, h;
				int maxTextWidth, maxTextHeight;
				drawFormat.FormatFlags |= StringFormatFlags.NoWrap;
				Size[] sizes = DrawLegendMeasure(rpt, g, drawFont, drawFormat, 
					new SizeF(Layout.Width, Layout.Height), out maxTextWidth, out maxTextHeight);
				int boxSize = (int) (maxTextHeight * .8);
				int totalItemWidth = 0;			// width of a legend item
				int totalWidth, totalHeight;	// final height and width of legend

				// calculate the height and width of the rectangle
				switch (l.Layout)
				{
					case LegendLayoutEnum.Row:
						// we need to loop thru all the width
						totalWidth=0;
						for (int i = 0; i < SeriesCount; i++)
						{
                            if (sizes[i].Width !=0)  //14052008WRP when legend valeus are 0 don't add extra boxsize
							totalWidth += (sizes[i].Width + (boxSize * 2));
						}
						totalHeight = (int) (maxTextHeight + (maxTextHeight * .1));
						h = totalHeight;
						totalItemWidth = maxTextWidth + (boxSize * 2); 
						drawFormat.Alignment = StringAlignment.Near;	// Force alignment to near
						break;
					case LegendLayoutEnum.Table:
                        // for table we simplify to have TWO columns (i.e. don't do anything too tricky
                        totalWidth = totalItemWidth = (maxTextWidth + (boxSize * 2)) * 2;     // make width twice as big as longest entry
                        h = (int)(maxTextHeight + (maxTextHeight * .1));
                        totalHeight = h * (SeriesCount + (SeriesCount % 2)) / 2;
                        break;
					case LegendLayoutEnum.Column:
					default:
						totalWidth = totalItemWidth = maxTextWidth + (boxSize * 2); 
						h = (int) (maxTextHeight + (maxTextHeight * .1));
						totalHeight = h * SeriesCount;
						break;
				}

				// calculate the location of the legend rectangle
				if (this.IsLegendInsidePlotArea())
				switch (l.Position)
				{
					case LegendPositionEnum.BottomCenter:
						x = Layout.PlotArea.X + (Layout.PlotArea.Width / 2) - (totalWidth / 2);
						y = Layout.PlotArea.Y + Layout.PlotArea.Height - totalHeight - 2;
						break;
					case LegendPositionEnum.BottomLeft:
					case LegendPositionEnum.LeftBottom:
						x = Layout.PlotArea.X+2;
						y = Layout.PlotArea.Y + Layout.PlotArea.Height - totalHeight - 2;
						break;
					case LegendPositionEnum.BottomRight:
					case LegendPositionEnum.RightBottom:
						x = Layout.PlotArea.X + Layout.PlotArea.Width - totalWidth;
						y = Layout.PlotArea.Y + Layout.PlotArea.Height - totalHeight - 2;
						break;
					case LegendPositionEnum.LeftCenter:
						x = Layout.PlotArea.X + 2;
						y = Layout.PlotArea.Y + (Layout.PlotArea.Height / 2) - (totalHeight/2);
						break;
					case LegendPositionEnum.LeftTop:
					case LegendPositionEnum.TopLeft:
						x = Layout.PlotArea.X + 2;
						y = Layout.PlotArea.Y+2;
						break;
					case LegendPositionEnum.RightCenter:
						x = Layout.PlotArea.X + Layout.PlotArea.Width - totalWidth - 2;
						y = Layout.PlotArea.Y + (Layout.PlotArea.Height / 2) - (totalHeight/2);
						break;
					case LegendPositionEnum.TopCenter:
						x = Layout.PlotArea.X + (Layout.PlotArea.Width / 2) - (totalWidth / 2);
						y = Layout.PlotArea.Y + +2;
						break;
					case LegendPositionEnum.TopRight:
					case LegendPositionEnum.RightTop:
					default:
						x = Layout.PlotArea.X + Layout.PlotArea.Width-totalWidth - 2;
						y = Layout.PlotArea.Y + +2;
						break;
				}
				else switch (l.Position)
				{
					case LegendPositionEnum.BottomCenter:
						x = (Layout.Width / 2) - (totalWidth / 2);
						y = Layout.Height - totalHeight - 2;
						break;
					case LegendPositionEnum.BottomLeft:
					case LegendPositionEnum.LeftBottom:
						if (IsLegendInsidePlotArea())
							x = Layout.LeftMargin;
						else
							x = 0;
						y = Layout.Height - totalHeight - 2;
						break;
					case LegendPositionEnum.BottomRight:
					case LegendPositionEnum.RightBottom:
						x = Layout.Width - totalWidth;
						y = Layout.Height - totalHeight - 2;
						break;
					case LegendPositionEnum.LeftCenter:
						x = 2;
						y = (Layout.Height / 2) - (totalHeight/2);
						break;
					case LegendPositionEnum.LeftTop:
					case LegendPositionEnum.TopLeft:
						x = 2;
						y = Layout.TopMargin+2;
						break;
					case LegendPositionEnum.RightCenter:
						x = Layout.Width - totalWidth - 2;
						y = (Layout.Height / 2) - (totalHeight/2);
						break;
					case LegendPositionEnum.TopCenter:
						x = (Layout.Width / 2) - (totalWidth / 2);
						y = Layout.TopMargin+2;
						break;
					case LegendPositionEnum.TopRight:
					case LegendPositionEnum.RightTop:
					default:
						x = Layout.Width-totalWidth - 2;
						y = Layout.TopMargin+2;
						break;
				}

				// We now know enough to calc the bounding rectangle of the legend
				rRect = new System.Drawing.Rectangle(x-1, y-1, totalWidth+2, totalHeight+2);
				if (s != null)
				{
					s.DrawBackground(rpt, g, null, rRect);	// draw (or not draw) background 
					s.DrawBorder(rpt, g, null, rRect);		// draw (or not draw) border depending on style
				}

                int saveX = x;
                ChartMarkerEnum cm = this.ChartDefn.Type == ChartTypeEnum.Bubble ? ChartMarkerEnum.Bubble : ChartMarkerEnum.None;
				for (int iCol=1; iCol <= SeriesCount; iCol++)
				{
					string c = GetSeriesValue(rpt, iCol);
                    if (c != "") //14052008WRP Cater for empty strings in the legend
                    {
					System.Drawing.Rectangle rect;
                        // 20022008 AJM GJL - Draw correct legend icon (Column\Line)
                        Type t = null;
                        t = GetSeriesBrush(rpt, 1, iCol).GetType();
                    /* The following 2 lines have been added to draw the correct legend for column chart with line plot types
                     * 06122007AJM */
                    cm = ChartMarkerEnum.None;
                        bool isLine = GetPlotType(rpt, iCol, 1).ToUpper() == "LINE";                      

                        if ((bMarker || isLine) || (this.ChartDefn.Type == ChartTypeEnum.Scatter && t == typeof(System.Drawing.Drawing2D.HatchBrush)))
                        cm = SeriesMarker[iCol - 1];
                        if (isLine && this.ChartDefn.Type == ChartTypeEnum.Scatter)
                        {
                            cm = ChartMarkerEnum.Line;
                        }  
                        
                        bool NoMarker = getNoMarkerVal(rpt, iCol, 1);
                        if (NoMarker) { cm = ChartMarkerEnum.Line; }

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


                        SolidBrush b;
					switch (l.Layout)
					{
						case LegendLayoutEnum.Row:
							rect = new System.Drawing.Rectangle(x + boxSize + (boxSize/2), y, totalItemWidth - boxSize - (boxSize/2), h);
                                if (c != "") //14052008WRP to cater for empty strings in the legend
                                {
							g.DrawString(c, drawFont, drawBrush, rect, drawFormat);

                                    if ((cm != ChartMarkerEnum.None || this.ChartDefn.Type == ChartTypeEnum.Scatter) && (t == typeof(System.Drawing.Drawing2D.HatchBrush))) //GJL 110208 - Don't draw pattern for lines or Bubbles
                                    {
                                        System.Drawing.Drawing2D.HatchBrush hb = (System.Drawing.Drawing2D.HatchBrush)GetSeriesBrush(rpt, 1, iCol);
                                        b = new SolidBrush(hb.ForegroundColor);

                                        DrawLegendBox(g, b,
                                        cm, x, y + 1, boxSize,intLineSize);
                                    }
                                    else
                                    {
                            DrawLegendBox(g, GetSeriesBrush(rpt, 1, iCol),
								cm, x, y+1, boxSize,intLineSize);
                                    }


							x += (sizes[iCol-1].Width + (boxSize*2)); 
                                }
							break;
						case LegendLayoutEnum.Table:
                            rect = new System.Drawing.Rectangle(x + boxSize + (boxSize / 2), y, maxTextWidth, h);
                            g.DrawString(c, drawFont, drawBrush, rect, drawFormat);

                                if (cm != ChartMarkerEnum.None && (t == typeof(System.Drawing.Drawing2D.HatchBrush))) //GJL 110208 - Don't draw pattern for lines
                                {
                                    System.Drawing.Drawing2D.HatchBrush hb = (System.Drawing.Drawing2D.HatchBrush)GetSeriesBrush(rpt, 1, iCol);
                                    b = new SolidBrush(hb.ForegroundColor);

                                    DrawLegendBox(g, /*GetSeriesBrushesExcel(iCol - 1)*/ b,
                                    cm, x, y + 1, boxSize,intLineSize);
                                }
                                else
                                { DrawLegendBox(g, GetSeriesBrush(rpt, 1, iCol), cm, x + 1, y, boxSize,intLineSize); }


                            if (iCol % 2 == 0)
                            {
                                y += h;
                                x = saveX;
                            }
                            else
                            {
                                x = saveX + (rRect.Width / 2);
                            }
                            break;
                        case LegendLayoutEnum.Column:
						default:
							rect = new System.Drawing.Rectangle(x + boxSize + (boxSize/2), y, maxTextWidth, h);
							g.DrawString(c, drawFont, drawBrush, rect, drawFormat);


                                if (cm != ChartMarkerEnum.None && (t == typeof(System.Drawing.Drawing2D.HatchBrush)))       //GJL 110208 - Don't draw pattern for lines                          
                                {
                                    System.Drawing.Drawing2D.HatchBrush hb = (System.Drawing.Drawing2D.HatchBrush)GetSeriesBrush(rpt, 1, iCol);
                                    b = new SolidBrush(hb.ForegroundColor);

                                    DrawLegendBox(g, /*GetSeriesBrushesExcel(iCol - 1)*/ b,
                                    cm, x, y + 1, boxSize,intLineSize);
                                }
                                else
                                {
                            DrawLegendBox(g, GetSeriesBrush(rpt, 1, iCol),
								cm, x+1, y, boxSize,intLineSize);
                                }

							y += h;
							break;
					}
				}
			}
            }
			finally
			{
				if (drawFont != null)
					drawFont.Dispose();
				if (drawBrush != null)
					drawBrush.Dispose();
				if (drawFormat != null)
					drawFormat.Dispose();
			}
			if (s != null)
				rRect = s.PaddingAdjust(rpt, null, rRect, true);
			return rRect;
	        
		}

        void DrawLegendBox(Graphics g, Brush b, ChartMarkerEnum marker, int x, int y, int boxSize)
        {
            DrawLegendBox(g, b, marker, x, y, boxSize, 2);
        }

		void DrawLegendBox(Graphics g, Brush b, ChartMarkerEnum marker, int x, int y, int boxSize, int intLineSize)
		{
			Pen p=null;
			int mSize= boxSize / 2;		// Marker size is 1/2 of box size	
			try
			{
				if (marker < ChartMarkerEnum.Count)
				{
					p = new Pen(b,intLineSize);
                    if (this.ChartDefn.Type != ChartTypeEnum.Scatter)
                    {
					g.DrawLine(p,  new Point(x, y + ((boxSize + 1)/2)), new Point(x + boxSize, y + ((boxSize + 1)/2)));
                    }
					x = x + ((boxSize - mSize)/2);
					y = y + ((boxSize - mSize)/2);
					if (mSize % 2 == 0)		
						mSize++;
				}

				if (marker == ChartMarkerEnum.None)
				{
					g.FillRectangle(b, x, y, boxSize, boxSize);
				}
                else if (marker == ChartMarkerEnum.Bubble)
				{
					g.FillEllipse(b, x, y, boxSize, boxSize);
				}
                else if (marker == ChartMarkerEnum.Line)
                {
                    // this is only to draw lines for line plot types on scatter charts
                    p = new Pen(b, intLineSize);
                    g.DrawLine(p, new Point(x, y + ((boxSize + 1) / 2)), new Point(x + boxSize, y + ((boxSize + 1) / 2)));
                }
				else
				{
					DrawLegendMarker(g, b, p, marker, x, y, mSize);
				}
			}
			finally
			{
				if (p != null)
					p.Dispose();
			}
		}

		internal void DrawLegendMarker(Graphics g, Brush b, Pen p, ChartMarkerEnum marker, int x, int y, int mSize)
		{
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
					// 20022008 AJM GJL - Changed to line - plus is hard to see
                    p = new Pen(p.Brush, 2);
					g.DrawLine(p, new Point(x + ((mSize + 1)/2), y), new Point(x + ((mSize + 1)/2), y + mSize));
					//g.DrawLine(p, new Point(x + (mSize + 1)/2, y + (mSize+1)/2), new Point(x + mSize, y + (mSize+1)/2));
					break;
				case ChartMarkerEnum.Diamond:
					points = new PointF[5];
					points[0] = points[4] = new Point(x + ((mSize + 1)/2), y);	// starting and ending point
					points[1] = new PointF(x, y + ((mSize+1)/2));
					points[2] = new PointF(x + ((mSize+1)/2), y+mSize);
					points[3] = new PointF(x + mSize, y + ((mSize+1)/2));
					g.FillPolygon(b, points);
					break;
				case ChartMarkerEnum.Triangle:
					points = new PointF[4];
					points[0] = points[3] = new PointF(x + ((mSize + 1)/2), y);	// starting and ending point
					points[1] = new PointF(x, y + mSize);
					points[2] = new PointF(x + mSize, y + mSize);
					g.FillPolygon(b, points);
                    break;
				case ChartMarkerEnum.X:
                    p = new Pen(p.Brush, 2);// 20022008 AJM GJL
					g.DrawLine(p, new Point(x, y), new Point(x + mSize, y + mSize));
					g.DrawLine(p, new Point(x, y + mSize), new Point(x + mSize, y));// 20022008 AJM GJL
					break;
			}
			return;
		}

		// Measures the Legend and then returns the rectangle it drew in
		protected Size[] DrawLegendMeasure(Report rpt, Graphics g, Font f, StringFormat sf, SizeF maxSize, out int maxWidth, out int maxHeight)
		{
			Size[] sizes = new Size[SeriesCount];
			maxWidth = maxHeight = 0;

			for (int iCol=1; iCol <= SeriesCount; iCol++)
			{
				string c = GetSeriesValue(rpt, iCol);
                if (c != "")  //14052008WRP cater for empty strings in legend names
                {
				SizeF ms = g.MeasureString(c, f, maxSize, sf);
				sizes[iCol-1] = new Size((int) Math.Ceiling(ms.Width), 
										 (int) Math.Ceiling(ms.Height));
				if (sizes[iCol-1].Width > maxWidth)
					maxWidth = sizes[iCol-1].Width;
				if (sizes[iCol-1].Height > maxHeight)
					maxHeight = sizes[iCol-1].Height;
			}
                
			}
			return sizes;
		}

		protected void DrawPlotAreaStyle(Report rpt, Graphics g, System.Drawing.Rectangle crect)
		{
			if (_ChartDefn.PlotArea == null || _ChartDefn.PlotArea.Style == null)
				return;
			System.Drawing.Rectangle rect = Layout.PlotArea;
			Style s = _ChartDefn.PlotArea.Style;

            Row r = FirstChartRow(rpt);

			if (rect.IntersectsWith(crect))
			{
				// This occurs when the legend is drawn inside the plot area
				//    we don't want to draw in the legend
				Region rg=null;
				try
				{
	//				rg = new Region(rect);	// TODO: this doesn't work; nothing draws
	//				rg.Complement(crect);
	//				Region saver = g.Clip;
	//				g.Clip = rg;
					s.DrawBackground(rpt, g, r, rect);
	//				g.Clip = saver;
				}
				finally
				{
					if (rg != null)
						rg.Dispose();
				}
			}
			else
				s.DrawBackground(rpt, g, r, rect);
			
			return;
		}

		protected void DrawTitle(Report rpt, Graphics g, Title t, System.Drawing.Rectangle rect)
		{
			if (t == null)
				return;

			if (t.Caption == null)
				return;

			Row r = FirstChartRow(rpt);
			object title = t.Caption.Evaluate(rpt, r);
			if (t.Style != null)
			{
				t.Style.DrawString(rpt, g, title, t.Caption.GetTypeCode(), r, rect);
				t.Style.DrawBorder(rpt, g, r, rect);
			}
			else
				Style.DrawStringDefaults(g, title, rect);

			return;
		}

		protected Size DrawTitleMeasure(Report rpt, Graphics g, Title t)
		{
			Size size=Size.Empty;

			if (t == null || t.Caption == null)
				return size;

			Row r = FirstChartRow(rpt);
			object title = t.Caption.Evaluate(rpt, r);
			if (t.Style != null)
				size = t.Style.MeasureString(rpt, g, title, t.Caption.GetTypeCode(), r, int.MaxValue);
			else
				size = Style.MeasureStringDefaults(rpt, g, title, t.Caption.GetTypeCode(), r, int.MaxValue);
			
			return size;
		}

        //15052008WRP - Draw category month labels
        protected void DrawCategoryLabel(Report rpt, Graphics g, string t, Style a, System.Drawing.Rectangle rect)
        {
           
            if (t == null)
                return;

            Row r = FirstChartRow(rpt);
           
            if (a != null)
            {
                a.DrawString(rpt, g, t, t.GetTypeCode(), r, rect);
                a.DrawBorder(rpt, g, r, rect);
            }
            else
                Style.DrawStringDefaults(g, t, rect);
            return;
        }

        //15052008WRP - Measure category title size
        protected Size DrawCategoryTitleMeasure(Report rpt, Graphics g, string t, Style a)
        {
            Size size = Size.Empty;
            Row r = FirstChartRow(rpt);

            if (t == null || t == "")
                return size;

            if (a != null)
                size = a.MeasureString(rpt, g, t, t.GetTypeCode(), r, int.MaxValue);
            else
                size = Style.MeasureStringDefaults(rpt, g, t, t.GetTypeCode(), r, int.MaxValue);

            return size;

        }      

		protected object GetCategoryValue(Report rpt, int row, out TypeCode tc)
		{
			MatrixCellEntry mce = _DataDefn[row, 0];
			if (mce == null)
			{
				tc = TypeCode.String;
				return "";					// Not sure what this really means TODO:
			}

			Row lrow;
			this._ChartDefn.ChartMatrix.SetMyData(rpt, mce.Data);		// Must set this for evaluation
			if (mce.Data.Data.Count > 0)
				lrow = mce.Data.Data[0];
			else
				lrow = null;
			ChartExpression ce = (ChartExpression) (mce.DisplayItem);

			object v = ce.Value.Evaluate(rpt, lrow);
			tc = ce.Value.GetTypeCode();
			return v;
		}

		protected double GetDataValue(Report rpt, int row, int col)
		{
            return GetDataValue(rpt, row, col, 0);
		}

		/* Added this function to return the plot type
		 * 05122007AJM */
        protected string GetPlotType(Report rpt, int row, int col)
        {
            try
            {
                if (this is ChartColumn || this is ChartBubble) 
                {
                    return ((ChartExpression)_DataDefn[col, row].DisplayItem).PlotType.Source;
                }
            }
            catch //(Exception e)
            {
               //ignore 
            }
            return "Auto";
        }
// 20022008 AJM GJL
        protected string GetYAxis(Report rpt, int row, int col)
        {
            try
            {
                if (this is ChartColumn)
                {
                    return ((ChartExpression)_DataDefn[col, row].DisplayItem).YAxis.Source;
                }
            }
            catch //(Exception e)
            {
                //ignore 
            }
            return "Left";
        }

        protected bool getNoMarkerVal(Report rpt, int row, int col)
        {
            try
            {
               return Boolean.Parse(((ChartExpression)_DataDefn[col, row].DisplayItem).NoMarker.Source);
                
            }
            catch
            { 
            }
            return false;
        }


        protected String getLineSize(Report rpt, int row, int col)
        {
            try
            {
                return ((ChartExpression)_DataDefn[col, row].DisplayItem).LineSize.Source;

            }
            catch
            {
            }
            return "Regular";
        }

        protected String getColour(Report rpt, int row, int col)
        {
            try
            {
                return ((ChartExpression)_DataDefn[row, col].DisplayItem).Colour.Source;
            }
            catch
            { 
            }
            return "";
        }
        protected double GetDataValue(Report rpt, int row, int col, int xyb)
        {
            MatrixCellEntry mce = _DataDefn[row, col];
            if (mce == null)
                return 0;					// Not sure what this really means TODO:
            if (mce.Value != double.MinValue && xyb == 0)
                return mce.Value;

            // Calculate this value; usually a fairly expensive operation
            //   due to the common use of aggregate values.  We need to
            //   go thru the data more than once if we have to auto scale.
            Row lrow;
            this._ChartDefn.ChartMatrix.SetMyData(rpt, mce.Data);		// Must set this for evaluation
            if (mce.Data.Data.Count > 0)
                lrow = mce.Data.Data[0];
            else
                lrow = null;
            ChartExpression ce = (ChartExpression)(mce.DisplayItem);

            double v=double.MinValue;
            if (xyb == 0)
            {
                v = ce.Value.EvaluateDouble(rpt, lrow);
                mce.Value = v;					// cache so we don't need to calculate again
            }
            else if (xyb == 1)
                v = ce.Value2.EvaluateDouble(rpt, lrow);
            else if (xyb == 2)
                v = ce.Value3.EvaluateDouble(rpt, lrow);

            return v;
        }
        protected string GetDataValueString(Report rpt, int row, int col)
        {
            MatrixCellEntry mce = _DataDefn[row, col];
            if (mce == null)
                return null;					// Not sure what this really means TODO:

            // Calculate this value; usually a fairly expensive operation
            //   due to the common use of aggregate values.  We need to
            //   go thru the data more than once if we have to auto scale.
            Row lrow;
            this._ChartDefn.ChartMatrix.SetMyData(rpt, mce.Data);		// Must set this for evaluation
            if (mce.Data.Data.Count > 0)
                lrow = mce.Data.Data[0];
            else
                lrow = null;
            ChartExpression ce = (ChartExpression)(mce.DisplayItem);

            string v = ce.Value.EvaluateString(rpt, lrow);
            return v;
        }

        protected Brush GetSeriesBrush(Report rpt, int row, int col)
        {
            Brush br = SeriesBrush(rpt, _row, ChartDefn.OwnerReport)[col - 1];            // this will be the default brush

            //  obtain the rows we're acting upon
            MatrixCellEntry mce = _DataDefn[row, col];
            if (mce == null)
                return br;

            ChartExpression ce = (ChartExpression)(mce.DisplayItem);
            if (ce.DP == null || ce.DP.Style == null || ce.DP.Style.BackgroundColor == null)
                return br;

            this._ChartDefn.ChartMatrix.SetMyData(rpt, mce.Data);		// Must set this for evaluation
            Row lrow = (mce.Data.Data.Count > 0) ? mce.Data.Data[0] : null;

            Style s = ce.DP.Style;
            string sc = s.BackgroundColor.EvaluateString(rpt, lrow);

            Color rc = XmlUtil.ColorFromHtml(sc, Color.Empty, rpt);
            if (rc != Color.Empty)
                br = new SolidBrush(rc);
            
            return br; 
        }

		protected void DrawDataPoint(Report rpt, Graphics g, Point p, int row, int col)
		{
			DrawDataPoint(rpt, g, p, System.Drawing.Rectangle.Empty, row, col);
		}

		protected void DrawDataPoint(Report rpt, Graphics g, System.Drawing.Rectangle rect, int row, int col)
		{
			DrawDataPoint(rpt, g, Point.Empty, rect, row, col);
		}

		void DrawDataPoint(Report rpt, Graphics g, Point p, System.Drawing.Rectangle rect, int row, int col)
		{
			MatrixCellEntry mce = _DataDefn[row, col];
			if (mce == null)
				return;					// Not sure what this really means TODO:

			ChartExpression ce = (ChartExpression) (mce.DisplayItem);
			DataPoint dp = ce.DP;

			if (dp.DataLabel == null || !dp.DataLabel.Visible)
				return;

			// Calculate the DataPoint value; usually a fairly expensive operation
			//   due to the common use of aggregate values.  
			Row lrow;
			this._ChartDefn.ChartMatrix.SetMyData(rpt, mce.Data);		// Must set this for evaluation
			if (mce.Data.Data.Count > 0)
				lrow = mce.Data.Data[0];
			else
				lrow = null;

			object v=null;
			TypeCode tc;
			if (dp.DataLabel.Value == null)
			{		// No DataLabel value specified so we use the actual value
				v = ce.Value.EvaluateDouble(rpt, lrow);
				tc = TypeCode.Double;
			}
			else
			{		// Evaluate the DataLable value for the display
				v = dp.DataLabel.Value.Evaluate(rpt, lrow);
				tc = dp.DataLabel.Value.GetTypeCode();
			}

			if (dp.DataLabel.Style == null)
			{
				if (rect == System.Drawing.Rectangle.Empty)
				{
					Size size = Style.MeasureStringDefaults(rpt, g, v, tc, lrow, int.MaxValue);
					rect = new System.Drawing.Rectangle(p, size);
				}
				Style.DrawStringDefaults(g, v, rect);
			}
			else
			{
				if (rect == System.Drawing.Rectangle.Empty)
				{
					Size size = dp.DataLabel.Style.MeasureString(rpt, g, v, tc, lrow, int.MaxValue);
					rect = new System.Drawing.Rectangle(p, size);
				}
				dp.DataLabel.Style.DrawString(rpt, g, v, tc, lrow, rect);
			}

			return;
		}

		protected string GetSeriesValue(Report rpt, int iCol)
		{
			MatrixCellEntry mce = _DataDefn[0, iCol];
			Row lrow;
			if (mce.Data.Data.Count > 0)
				lrow = mce.Data.Data[0];
			else
				lrow = null;
			ChartExpression ce = (ChartExpression) (mce.DisplayItem);

			string v = ce.ChartLabel == null ?
                ce.Value.EvaluateString(rpt, lrow) :
                ce.ChartLabel.EvaluateString(rpt, lrow);
            
            return v;
		}

		// 20022008 AJM GJL - Should the Second Y axis be shown?
        protected bool ShowRightYAxis(Report rpt)
        {
            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                for (int iCol = 1; iCol <= SeriesCount; iCol++)
                {
                    if (GetYAxis(rpt, iCol, 1).ToUpper() == "RIGHT")
                    {
                        return true;  
                    }
                }
            }
            return false;
        }
		protected void GetMaxMinDataValue(Report rpt, out double max, out double min, int xyb,int WhichYAxis)
		{
            if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.Stacked)
			{
				GetMaxMinDataValueStacked(rpt, out max, out min);
				return;
			}
			min = double.MaxValue;
			max = double.MinValue;

			double v = 0;// 20022008 AJM GJL
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					// 20022008 AJM GJL
                    if (WhichYAxis == 2)
                    {
                        if (GetYAxis(rpt, iCol, 1).ToUpper() == "RIGHT")
                        {
                            v = GetDataValue(rpt, iRow, iCol, xyb);
                            if (v < min)
                                min = v;
                            if (v > max)
                                max = v;
                            
                        }
                    }
                    else
                    {
                        if (GetYAxis(rpt, iCol, 1).ToUpper() != "RIGHT")
                        {
					v = GetDataValue(rpt, iRow, iCol, xyb);
					if (v < min)
						min = v;
					if (v > max)
						max = v;

                        }
                    }                
				}
			}
		}

		void GetMaxMinDataValueStacked(Report rpt, out double max, out double min)
		{
			min = double.MaxValue;
			max = double.MinValue;

			double v;
			for (int iRow = 1; iRow <= CategoryCount; iRow++)
			{
				v=0;
				for (int iCol = 1; iCol <= SeriesCount; iCol++)
				{
					v += GetDataValue(rpt, iRow, iCol);
				}
				if (v < min)
					min = v;
				if (v > max)
					max = v;
			}
		}
		
		protected Brush[] GetSeriesBrushes(Report rpt, Row row, ReportDefn defn)
		{
			Brush[] b = new Brush[SeriesCount];

			for (int i=0; i < SeriesCount; i++)
			{
				// TODO: In general all the palettes could use a good going over
				//   both in terms of the colors in the lists and their order
                switch (ChartPalette.GetStyle(ChartDefn.Palette.EvaluateString(rpt, row), defn.rl))
				{
					case ChartPaletteEnum.Default:
						b[i] = GetSeriesBrushesExcel(i); break;
					case ChartPaletteEnum.EarthTones:
						b[i] = GetSeriesBrushesEarthTones(i); break;
					case ChartPaletteEnum.Excel:
						b[i] = GetSeriesBrushesExcel(i); break;
					case ChartPaletteEnum.GrayScale:
						b[i] = GetSeriesBrushesGrayScale(i); break;
					case ChartPaletteEnum.Light:
						b[i] = GetSeriesBrushesLight(i); break;
					case ChartPaletteEnum.Pastel:
						b[i] = GetSeriesBrushesPastel(i); break;
					case ChartPaletteEnum.SemiTransparent:
						b[i] = GetSeriesBrushesExcel(i); break;	// TODO
					// 20022008 AJM GJL - New black & white printer friendly palette (NOT TO RDL SPEC BUT REQUIRED!)
                    case ChartPaletteEnum.Patterned:
                        b[i] = GetSeriesBrushesPatterned(i);break;
                    case ChartPaletteEnum.PatternedBlack:
                        b[i] = GetSeriesBrushesPatternedBlack(i); break;
                    case ChartPaletteEnum.Custom:

                        b[i] = new SolidBrush(Color.FromName(getColour(rpt, 1, i + 1))); break;
					default:
						b[i] = GetSeriesBrushesExcel(i); break;
				}
			}

			return b;
		}

		Brush GetSeriesBrushesEarthTones(int i)
		{
			switch (i % 22)
			{
				case 0: return Brushes.Maroon;
				case 1: return Brushes.Brown;
				case 2: return Brushes.Chocolate;
				case 3: return Brushes.IndianRed;
				case 4: return Brushes.Peru;
				case 5: return Brushes.BurlyWood;
				case 6: return Brushes.AntiqueWhite;
				case 7: return Brushes.FloralWhite;
				case 8: return Brushes.Ivory;
				case 9: return Brushes.LightCoral;
				case 10:return Brushes.DarkSalmon;
				case 11: return Brushes.LightSalmon;
				case 12: return Brushes.PeachPuff;
				case 13: return Brushes.NavajoWhite;
				case 14: return Brushes.Moccasin;
				case 15: return Brushes.PapayaWhip;
				case 16: return Brushes.Goldenrod;
				case 17: return Brushes.DarkGoldenrod;
				case 18: return Brushes.DarkKhaki;
				case 19: return Brushes.Khaki;
				case 20: return Brushes.Beige;
				case 21: return Brushes.Cornsilk;
				default: return Brushes.Brown;
			}
		}

		Brush GetSeriesBrushesExcel(int i)
		{
			switch (i % 11)				// Just a guess at what these might actually be
			{
                //Gil's Excel 080208 - from excel 2007
                case 0: return Brushes.Blue;
                case 1: return Brushes.Red;
                case 2: return Brushes.Green;
                case 3: return Brushes.Purple;
                case 4: return Brushes.DeepSkyBlue;
                case 5: return Brushes.Orange;
                case 6: return Brushes.Magenta;
                case 7: return Brushes.Gold;
                case 8: return Brushes.Lime;
                case 9: return Brushes.Teal;
                case 10: return Brushes.Pink;
                default: return Brushes.Blue;               
			}
		}
// 20022008 AJM GJL
        Brush GetSeriesBrushesPatterned(int i)
        {
            HatchBrush PatternBrush;
            switch (i % 10)
            {
                case 0:
                    PatternBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Blue,Color.White);
                    break;
                case 1:
                    PatternBrush = new HatchBrush(HatchStyle.Cross, Color.Red, Color.White); // was weave... but I Especially didn't want to draw that in PDF - GJL
                    break;
                case 2:
                    PatternBrush = new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Green, Color.White);
                    break;
                case 3:
                    PatternBrush = new HatchBrush(HatchStyle.OutlinedDiamond, Color.Purple, Color.White);
                    break;
                case 4:
                    PatternBrush = new HatchBrush(HatchStyle.DarkHorizontal, Color.DeepSkyBlue, Color.White);
                    break;
                case 5:
                    PatternBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.Orange, Color.White);
                    break;
                case 6:
                    PatternBrush = new HatchBrush(HatchStyle.HorizontalBrick, Color.Magenta, Color.White);
                    break;
                case 7:
                    PatternBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.Gold, Color.White); // was wave... but I didn't want to draw that in PDF - GJL
                    break;
                case 8:
                    PatternBrush = new HatchBrush(HatchStyle.Vertical, Color.Lime, Color.White);
                    break;
                case 9:
                    PatternBrush = new HatchBrush(HatchStyle.SolidDiamond, Color.Teal, Color.White);
                    break;
                case 10:
                    PatternBrush = new HatchBrush(HatchStyle.DiagonalBrick, Color.Pink, Color.White);
                    break;
                default:
                    PatternBrush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Blue, Color.White);
                    break;
                   
            } 
            return PatternBrush;
        }


        Brush GetSeriesBrushesPatternedBlack(int i)
        {
            HatchBrush PatternBrush;
            switch (i % 10)
            {
                case 0:
                    PatternBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.White);
                    break;
                case 1:
                    PatternBrush = new HatchBrush(HatchStyle.Weave, Color.Black, Color.White);
                    break;
                case 2:
                    PatternBrush = new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Black, Color.White);
                    break;
                case 3:
                    PatternBrush = new HatchBrush(HatchStyle.OutlinedDiamond, Color.Black, Color.White);
                    break;
                case 4:
                    PatternBrush = new HatchBrush(HatchStyle.DarkHorizontal, Color.Black, Color.White);
                    break;
                case 5:
                    PatternBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.Black, Color.White);
                    break;
                case 6:
                    PatternBrush = new HatchBrush(HatchStyle.HorizontalBrick, Color.Black, Color.White);
                    break;
                case 7:
                    PatternBrush = new HatchBrush(HatchStyle.Wave, Color.Black, Color.White);
                    break;
                case 8:
                    PatternBrush = new HatchBrush(HatchStyle.Vertical, Color.Black, Color.White);
                    break;
                case 9:
                    PatternBrush = new HatchBrush(HatchStyle.SolidDiamond, Color.Black, Color.White);
                    break;
                case 10:
                    PatternBrush = new HatchBrush(HatchStyle.DiagonalBrick, Color.Black, Color.White);
                    break;
                default:
                    PatternBrush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Black, Color.White);
                    break;

			}
            return PatternBrush;
		}

		Brush GetSeriesBrushesGrayScale(int i)
		{
			switch (i % 10)			
			{
				case 0: return Brushes.Gray;
				case 1: return Brushes.SlateGray;
				case 2: return Brushes.DarkGray;
				case 3: return Brushes.LightGray;
				case 4: return Brushes.DarkSlateGray;
				case 5: return Brushes.DimGray;
				case 6: return Brushes.LightSlateGray;
				case 7: return Brushes.Black;
				case 8: return Brushes.White;
				case 9: return Brushes.Gainsboro;
				default: return Brushes.Gray;
			}
		}

		Brush GetSeriesBrushesLight(int i)
		{
			switch (i % 13)
			{
				case 0: return Brushes.LightBlue;
				case 1: return Brushes.LightCoral;
				case 2: return Brushes.LightCyan;
				case 3: return Brushes.LightGoldenrodYellow;
				case 4: return Brushes.LightGray;
				case 5: return Brushes.LightGreen;
				case 6: return Brushes.LightPink;
				case 7: return Brushes.LightSalmon;
				case 8: return Brushes.LightSeaGreen;
				case 9: return Brushes.LightSkyBlue;
				case 10: return Brushes.LightSlateGray;
				case 11: return Brushes.LightSteelBlue;
				case 12: return Brushes.LightYellow;
				default: return Brushes.LightBlue;
			}
		}

		Brush GetSeriesBrushesPastel(int i)
		{
			switch (i % 26)	
			{
				case 0: return Brushes.CadetBlue;
				case 1: return Brushes.MediumTurquoise;
				case 2: return Brushes.Aquamarine;
				case 3: return Brushes.LightCyan;
				case 4: return Brushes.Azure;
				case 5: return Brushes.AliceBlue;
				case 6: return Brushes.MintCream;
				case 7: return Brushes.DarkSeaGreen;
				case 8: return Brushes.PaleGreen;
				case 9: return Brushes.LightGreen;
				case 10: return Brushes.MediumPurple;
				case 11: return Brushes.CornflowerBlue;
				case 12: return Brushes.Lavender;
				case 13: return Brushes.GhostWhite;
				case 14: return Brushes.PaleGoldenrod;
				case 15: return Brushes.LightGoldenrodYellow;
				case 16: return Brushes.LemonChiffon;
				case 17: return Brushes.LightYellow;
				case 18: return Brushes.Orchid;
				case 19: return Brushes.Plum;
				case 20: return Brushes.LightPink;
				case 21: return Brushes.Pink;
				case 22: return Brushes.LavenderBlush;
				case 23: return Brushes.Linen;
				case 24: return Brushes.PaleTurquoise;
				case 25: return Brushes.OldLace;
				default: return Brushes.CadetBlue;
			}
		}

		protected ChartMarkerEnum[] GetSeriesMarkers()
		{
			ChartMarkerEnum[] m = new ChartMarkerEnum[SeriesCount];

			for (int i=0; i < SeriesCount; i++)
			{
				m[i] = (ChartMarkerEnum) ( i % (int) ChartMarkerEnum.Count);
			}

			return m;
		}

        protected void GetValueMaxMin(Report rpt, ref double max, ref double min, int xyb, int WhichYAxis)// 20022008 AJM GJL
		{

            if ((ChartSubTypeEnum)Enum.Parse(typeof(ChartSubTypeEnum), _ChartDefn.Subtype.EvaluateString(rpt, _row)) == ChartSubTypeEnum.PercentStacked)
			{	// Percent stacked is easy; and overrides user provided values
				max = 1;
				min = 0;
				return;
			}
            Axis ax;
            if (_ChartDefn.Type == ChartTypeEnum.Bubble ||
                _ChartDefn.Type == ChartTypeEnum.Scatter)
            {
                // Axis actually depends on the xyb parameter : 0 is X axis: 1 is Y Axis; 
                if (xyb == 0)
                    ax = _ChartDefn.CategoryAxis != null ? _ChartDefn.CategoryAxis.Axis : null;
                else if (xyb == 1)
                    ax = _ChartDefn.ValueAxis != null ? _ChartDefn.ValueAxis.Axis : null;
                else
                    ax = null;
            }
            else
            {               
                ax = _ChartDefn.ValueAxis != null ? _ChartDefn.ValueAxis.Axis : null;
            }
                

			double vAxisMax;
            double vAxisMin;
			if (ax != null)
			{
				vAxisMax = ax.MaxEval(rpt, _row);
				vAxisMin = ax.MinEval(rpt, _row);
			}
			else
			{
				vAxisMax = vAxisMin = int.MinValue;
			}

			// Check for case where both min and max are provided
            if (vAxisMax != int.MinValue && ! vAxisMax.Equals(double.NaN) &&
                vAxisMin != int.MinValue && ! vAxisMin.Equals(double.NaN))
			{
				max = vAxisMax;
				min = vAxisMin;
				return;
			}

			// OK We have to work for it;  Calculate min/max of data
			GetMaxMinDataValue(rpt, out max, out min, xyb,1);	// 20022008 AJM GJL
			
            if (vAxisMax != int.MinValue && ! vAxisMax.Equals(double.NaN))
				max = vAxisMax;
			else
			{
				//
				//int gridIncrs=10;		// assume 10 grid increments for now
                _gridIncrs = 10; //PJR 20071113 - grid incrs set & adjusted in here now

                double incr = max / _gridIncrs;	// should be range between max and min?
				double log = Math.Floor(Math.Log10(Math.Abs(incr)));


				double logPow = Math.Pow(10, log) * Math.Sign(max);
                double logDig = (int) ((incr / logPow) + .5);

				// promote the MSD to either 1, 2, or 5
                if (logDig > 5.0)
                    logDig = 10.0;
                else if (logDig > 2.0)
                    logDig = 5.0;
                else if (logDig > 1.0)
                    logDig = 2.0;
                //PJR 20071113 - reduce scale for large overscale options by decreasing _gridIncrs
                while (max < logDig * logPow * _gridIncrs)
                {
                    _gridIncrs--;
                }
                //_gridIncrs++;

                //PJR 20071113 - expand scale so that it is able to fit the max value by increasing _gridIncrs
                while (max > logDig * logPow * _gridIncrs)
                {
                    _gridIncrs++;
                }
// 20022008 AJM GJL
                double tmpMax = max;
                // 04032008 AJM - Fixing Strange Number Choice with small numbers
                max = /*(int)*/ (logDig * logPow * _gridIncrs /*+ 0.5*/);
               
                if (tmpMax > max - ((max / _gridIncrs) * .5))
                {

                    max += (max / _gridIncrs);
                    _gridIncrs++;
                }
			}

            if (vAxisMin != int.MinValue && ! vAxisMin.Equals(double.NaN))
				min = vAxisMin;
			else if (min > 0)
				min = 0;
			else
			{
				min = Math.Floor(min);
			}

			return;
		}

		protected void AdjustMargins(System.Drawing.Rectangle legendRect, Report rpt, Graphics g)
		{
           // //110208AJM GJL Making room for second y axis        

           // if (ShowRightYAxis(rpt) && !(IsLegendRight()))
           //{              
           //     Layout.RightMargin = (int)(Layout.LeftMargin * 1.5);
           //}


			// Adjust the margins based on the legend
			if (!IsLegendInsidePlotArea())	// When inside plot area we don't adjust plot margins
			{
				if (IsLegendLeft())
					Layout.LeftMargin += legendRect.Width;
				else if (IsLegendRight())
					Layout.RightMargin += legendRect.Width;
				if (IsLegendTop())
					Layout.TopMargin += legendRect.Height;
				else if (IsLegendBottom())
					Layout.BottomMargin += legendRect.Height;
			}
			// Force some margins; if any are too small
			int min = new RSize(ChartDefn.OwnerReport, ".2 in").PixelsX;

			if (Layout.RightMargin < min + (this._LastCategoryWidth/2))
				Layout.RightMargin = min + (this._LastCategoryWidth/2);
			if (Layout.LeftMargin < min)
				Layout.LeftMargin = min;
			if (Layout.TopMargin < min)
				Layout.TopMargin = min;
			if (Layout.BottomMargin < min)
				Layout.BottomMargin = min;
		}

		protected bool IsLegendLeft()
		{
			Legend l = _ChartDefn.Legend;
			if (l == null || !l.Visible)
				return false;

			bool rc;
			switch (l.Position)
			{
				case LegendPositionEnum.BottomLeft:
				case LegendPositionEnum.LeftBottom:
				case LegendPositionEnum.LeftCenter:
				case LegendPositionEnum.LeftTop:
				case LegendPositionEnum.TopLeft:
					rc=true;
					break;
				default:
					rc=false;
					break;
			}

			return rc;
		}

        protected void SetIncrementAndInterval(Report rpt, Axis a, double min, double max, out double incr, out int interval)
        {
            interval = _gridIncrs; //PJR 20071113 - gridincrements set by Max value now                 // assume an interval count of 10 to start

            if (a.MajorInterval != null)
            {
                incr = a.MajorInterval.EvaluateDouble(rpt, this.ChartRow);
                if (incr.CompareTo(double.MinValue) == 0)
                    incr = (max - min) / interval;
                else
                {
                    interval = (int)((int)(Math.Abs(max - min) / incr));
                }
            }
            else
                incr = (max - min) / interval;

            return;
        }

		protected bool IsLegendInsidePlotArea()
		{
			Legend l = _ChartDefn.Legend;
			if (l == null || !l.Visible)
				return false;				// doesn't really matter
			else
				return l.InsidePlotArea;
		}

		protected bool IsLegendRight()
		{
			Legend l = _ChartDefn.Legend;
			if (l == null || !l.Visible)
				return false;

			bool rc;
			switch (l.Position)
			{
				case LegendPositionEnum.BottomRight:
				case LegendPositionEnum.RightBottom:
				case LegendPositionEnum.RightCenter:
				case LegendPositionEnum.TopRight:
				case LegendPositionEnum.RightTop:
					rc=true;
					break;
				default:
					rc=false;
					break;
			}
			return rc;
		}

		protected bool IsLegendTop()
		{
			Legend l = _ChartDefn.Legend;
			if (l == null || !l.Visible)
				return false;

			bool rc;
			switch (l.Position)
			{
				case LegendPositionEnum.LeftTop:
				case LegendPositionEnum.TopLeft:
				case LegendPositionEnum.TopCenter:
				case LegendPositionEnum.TopRight:
				case LegendPositionEnum.RightTop:
					rc=true;
					break;
				default:
					rc=false;
					break;
			}
			return rc;
		}

		protected bool IsLegendBottom()
		{
			Legend l = _ChartDefn.Legend;
			if (l == null || !l.Visible)
				return false;

			bool rc;
			switch (l.Position)
			{
				case LegendPositionEnum.BottomCenter:
				case LegendPositionEnum.BottomLeft:
				case LegendPositionEnum.LeftBottom:
				case LegendPositionEnum.BottomRight:
				case LegendPositionEnum.RightBottom:
					rc=true;
					break;
				default:
					rc=false;
					break;
			}
			return rc;
		}

		private Row FirstChartRow(Report rpt)
		{
			Rows _Data = _ChartDefn.ChartMatrix.GetMyData(rpt);
			if (_Data != null &&
				_Data.Data.Count > 0)
				return _Data.Data[0];
			else
				return null;

		}
		#region IDisposable Members

		public void Dispose()
		{
			if (_bm != null)
				_bm.Dispose();
		}

		#endregion
	}
}
