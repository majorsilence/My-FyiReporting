using fyiReporting.RDL;
using System;
using System.IO;

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
        internal Stream ImageStream { get; set; }

        public ExcelImage(Image image, int imageIndex)
		{
			Image = image;
			ImageIndex = imageIndex;
        }
        public ExcelImage(Image image, Stream imageStream)
        {
            Image = image;
            ImageStream = imageStream;
        }
    }
}
