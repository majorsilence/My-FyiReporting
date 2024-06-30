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
using System.Collections.Generic;
#if DRAWINGCOMPAT
using Drawing = System.DrawingCore;
using Imaging = System.DrawingCore.Imaging;
#else
using Drawing = System.Drawing;
using Imaging = System.Drawing.Imaging;
#endif
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;


namespace fyiReporting.RDL
{
	///<summary>
	/// Column chart definition and processing
	///</summary>
	internal class ChartMap: ChartBase
	{

        internal ChartMap(Report r, Row row, Chart c, MatrixCellEntry[,] m, Expression showTooltips, Expression showTooltipsX, Expression _ToolTipYFormat, Expression _ToolTipXFormat)
            : base(r, row, c, m, showTooltips, showTooltipsX, _ToolTipYFormat, _ToolTipXFormat)
		{
		}

		override internal void Draw(Report rpt)
		{
			CreateSizedBitmap();
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				using (Drawing.Graphics g1 = Drawing.Graphics.FromImage(_bm))
				{
					_aStream = new System.IO.MemoryStream();
					IntPtr HDC = g1.GetHdc();
					//_mf = new System.Drawing.Imaging.Metafile(_aStream, HDC);
					_mf = new Imaging.Metafile(_aStream, HDC, new Drawing.RectangleF(0, 0, _bm.Width, _bm.Height),
						Imaging.MetafileFrameUnit.Pixel);
					g1.ReleaseHdc(HDC);
				}
			}

			using(Drawing.Graphics g = Drawing.Graphics.FromImage(_mf != null ? _mf : _bm))
			{
                // 06122007AJM Used to Force Higher Quality
                g.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.None;
                g.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality;

				// Adjust the top margin to depend on the title height
				Drawing.Size titleSize = DrawTitleMeasure(rpt, g, ChartDefn.Title);
				Layout.TopMargin = titleSize.Height;

				double max=0,min=0;	// Get the max and min values
		//		GetValueMaxMin(rpt, ref max, ref min,0, 1);

				DrawChartStyle(rpt, g);
				
				// Draw title; routine determines if necessary
				DrawTitle(rpt, g, ChartDefn.Title, new Drawing.Rectangle(0, 0, _bm.Width, Layout.TopMargin));

				Layout.LeftMargin = 0;
                Layout.RightMargin = 0;

				// Draw legend
				Drawing.Rectangle lRect = DrawLegend(rpt, g, false, true);

				Layout.BottomMargin = 0;

				AdjustMargins(lRect,rpt, g);		// Adjust margins based on legend.

				// Draw Plot area
				DrawPlotAreaStyle(rpt, g, lRect);

                string subtype = _ChartDefn.Subtype.EvaluateString(rpt, _row);
                
                DrawMap(rpt, g, subtype, max, min);

				DrawLegend(rpt, g, false, false);

			}
		}

        private void DrawMap(Report rpt, Drawing.Graphics g, string mapfile, double max, double min)
        {
            string file = XmlUtil.XmlFileExists(mapfile);

            MapData mp;
            if (file != null)
                mp = MapData.Create(file);
            else
            {
                rpt.rl.LogError(4, string.Format("Map Subtype file {0} not found.", mapfile));                
                mp = new MapData();         // we'll at least put up something; but it won't be right
            }
            float scale = mp.GetScale(Layout.PlotArea.Width, Layout.PlotArea.Height);

            for (int iRow = 1; iRow <= CategoryCount; iRow++)
            {
                for (int iCol = 1; iCol <= SeriesCount; iCol++)
                {
                    string sv = GetSeriesValue(rpt, iCol);

                    string c = this.GetDataValueString(rpt, iRow, iCol);
                    List<MapPolygon> pl = mp.GetPolygon(sv);
                    if (pl == null)
                        continue;
                    Drawing.Brush br = new Drawing.SolidBrush(XmlUtil.ColorFromHtml(c, Drawing.Color.Transparent));
                    foreach (MapPolygon mpoly in pl)
                    {
	                    Drawing.PointF[] polygon = mpoly.Polygon;
	                    Drawing.PointF[] drawpoly = new Drawing.PointF[polygon.Length];
                        // make points relative to plotarea --- need to scale this as well
                        for (int ip = 0; ip < drawpoly.Length; ip++)
                        {
                            drawpoly[ip] = new Drawing.PointF(Layout.PlotArea.X + (polygon[ip].X * scale), Layout.PlotArea.Y + (polygon[ip].Y * scale));
                        }
                        g.FillPolygon(br, drawpoly);
                        if (_showToolTips)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("PolyToolTip:");
                            sb.Append(sv.Replace('|', '/'));        // we treat '|' as a separator character; don't allow in string
                            sb.Append(' ');
                            sb.Append(c.Replace('|', '/'));
                            foreach (Drawing.PointF pf in drawpoly)
                                sb.AppendFormat(NumberFormatInfo.InvariantInfo, "|{0}|{1}", pf.X, pf.Y);
                            g.AddMetafileComment(new System.Text.ASCIIEncoding().GetBytes(sb.ToString()));
                        }
                    }
                    br.Dispose();
                }
            }
            // draw the outline of the map
            foreach (MapObject mo in mp.MapObjects)
            {
                mo.Draw(g, scale, Layout.PlotArea.X, Layout.PlotArea.Y);
            }

        }
    }
}
