using System;
using fyiReporting.RDL;
using NPOI.Util;
namespace RdlEngine.Render.ExcelConverter
{
	internal class ExcelLine
	{
		public ExcelLine(Line line)
		{
			Line = line;
		}

		public Line Line { get; set; }
		public float BorderWidth { get; set; }

		public float AbsoluteTop { get; set; }
		public float AbsoluteLeft { get; set; }
		public float Right { get; set; }
		public float Bottom { get; set; }

		public bool FlipH {
			get {
				return Right < AbsoluteLeft;
			}
		}

		public bool FlipV {
			get {
				return Bottom < AbsoluteTop;
			}
		}
	}
}
