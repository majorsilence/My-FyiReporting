

using System;
#if DRAWINGCOMPAT
using Draw2 = Majorsilence.Drawing;
#else
using Draw2 = System.Drawing;
#endif
using Majorsilence.Reporting.RdlEngine.Resources;

namespace Majorsilence.Reporting.Rdl
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
		Draw2.Rectangle _PlotArea;
	
		internal ChartLayout(int width, int height)
		{
			_Width = width;
			_Height = height;
			_LeftMargin = _RightMargin = _TopMargin = _BottomMargin = 0;
			_PlotArea = Draw2.Rectangle.Empty;
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
            set { _LeftMargin = value; _PlotArea = Draw2.Rectangle.Empty; }
		}
		internal int RightMargin
		{
			get { return  _RightMargin; }
            set { _RightMargin = value; _PlotArea = Draw2.Rectangle.Empty; }
		}
		internal int TopMargin
		{
			get { return  _TopMargin; }
            set { _TopMargin = value; _PlotArea = Draw2.Rectangle.Empty; }
		}
		internal int BottomMargin
		{
			get { return  _BottomMargin; }
            set { _BottomMargin = value; _PlotArea = Draw2.Rectangle.Empty; }
		}
		internal Draw2.Rectangle PlotArea
		{
			get 
			{ 
				if (_PlotArea == Draw2.Rectangle.Empty)
				{
					int w = _Width - _LeftMargin - _RightMargin;
					if (w <= 0)
						throw new Exception(Strings.ChartLayout_Error_PlotAreaWidthIs0);
					int h =_Height - _TopMargin - _BottomMargin;
					if (h <= 0)
						throw new Exception(Strings.ChartLayout_Error_PlotAreaHeightIs0);
				
					_PlotArea = new Draw2.Rectangle(_LeftMargin, _TopMargin, w, h); 
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
