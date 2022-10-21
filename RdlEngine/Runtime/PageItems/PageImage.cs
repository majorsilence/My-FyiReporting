using System;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace fyiReporting.RDL
{
	public class PageImage : PageItem, ICloneable
	{
		string name;                // name of object if constant image
		ImageFormat imf;            // type of image; png, jpeg are supported
		byte[] imageData;
		int samplesW;
		int samplesH;
		ImageRepeat repeat;
		ImageSizingEnum sizing;

		public PageImage(ImageFormat im, byte[] image, int w, int h)
		{
			Debug.Assert(im == ImageFormat.Jpeg || im == ImageFormat.Png || im == ImageFormat.Gif || im == ImageFormat.Wmf,
				"PageImage only supports Jpeg, Gif and Png and WMF image formats (Thanks HYNE!).");
			imf = im;
			imageData = image;
			samplesW = w;
			samplesH = h;
			repeat = ImageRepeat.NoRepeat;
			sizing = ImageSizingEnum.AutoSize;
		}

		public byte[] ImageData
		{
			get { return imageData; }
		}

		public ImageFormat ImgFormat
		{
			get { return imf; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public ImageRepeat Repeat
		{
			get { return repeat; }
			set { repeat = value; }
		}

		public ImageSizingEnum Sizing
		{
			get { return sizing; }
			set { sizing = value; }
		}

		public int SamplesW
		{
			get { return samplesW; }
		}

		public int SamplesH
		{
			get { return samplesH; }
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
	
	public enum ImageRepeat
	{
		Repeat,         // repeat image in both x and y directions
		NoRepeat,       // don't repeat
		RepeatX,        // repeat image in x direction
		RepeatY         // repeat image in y direction
	}
}