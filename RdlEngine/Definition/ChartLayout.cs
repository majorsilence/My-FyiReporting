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
using System.Drawing;

namespace fyiReporting.RDL
{
	///<summary>
	/// Class for defining chart layout.  For example, the plot area of a chart.
	///</summary>
	internal class ChartLayout
	{
		int _Height;			// total width of layout
		int _Width;				// total height
		int _LeftMargin;		// Margins
		int _RightMargin;
		int _TopMargin;
		int _BottomMargin;
		System.Drawing.Rectangle _PlotArea;
	
		internal ChartLayout(int width, int height)
		{
			_Width = width;
			_Height = height;
			_LeftMargin = _RightMargin = _TopMargin = _BottomMargin = 0;
			_PlotArea = System.Drawing.Rectangle.Empty;
		}
		
		internal int Width
		{
            get { return _Width; }
		}
		internal int Height
		{
            get { return _Height; }
		}
		internal int LeftMargin
		{
			get { return  _LeftMargin; }
            set { _LeftMargin = value; _PlotArea = System.Drawing.Rectangle.Empty; }
		}
		internal int RightMargin
		{
			get { return  _RightMargin; }
            set { _RightMargin = value; _PlotArea = System.Drawing.Rectangle.Empty; }
		}
		internal int TopMargin
		{
			get { return  _TopMargin; }
            set { _TopMargin = value; _PlotArea = System.Drawing.Rectangle.Empty; }
		}
		internal int BottomMargin
		{
			get { return  _BottomMargin; }
            set { _BottomMargin = value; _PlotArea = System.Drawing.Rectangle.Empty; }
		}
		internal System.Drawing.Rectangle PlotArea
		{
			get 
			{ 
				if (_PlotArea == System.Drawing.Rectangle.Empty)
				{
					int w = _Width - _LeftMargin - _RightMargin;
					if (w <= 0)
						throw new Exception("Plot area width is less than or equal to 0");
					int h =_Height - _TopMargin - _BottomMargin;
					if (h <= 0)
						throw new Exception("Plot area height is less than or equal to 0");
				
					_PlotArea = new System.Drawing.Rectangle(_LeftMargin, _TopMargin, w, h); 
				}

				return _PlotArea;
			}
            set
            {
                _PlotArea = value;
            }
		}
	}
}
