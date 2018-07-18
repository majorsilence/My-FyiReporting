using System;
using fyiReporting.RDL;
using NPOI.Util;
using NPOI.XSSF.UserModel;

namespace RdlEngine.Render.ExcelConverter
{
	internal class ExcelImage
	{
		public Image Image { get; set; }
		public int ImageIndex { get; set; }

		public float AbsoluteTop { get; set; }
		public float AbsoluteLeft { get; set; }

		public float ImageWidth {
			get {
				return Image.Width.Points;
			}
		}

		public float ImageHeight {
			get {
				return Image.Height.Points;
			}
		}

		public ExcelImage(Image image, int imageIndex)
		{
			Image = image;
			ImageIndex = imageIndex;
		}
	}
}
