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
using System.Xml;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Net;

namespace fyiReporting.RDL
{
	///<summary>
	/// Represents an image.  Source of image can from database, external or embedded. 
	///</summary>
	[Serializable]
	internal class Image : ReportItem
	{
		ImageSourceEnum _ImageSource;	// Identifies the source of the image:
		Expression _Value;		// See Source. Expected datatype is string or
								// binary, depending on Source. If the Value is
								// null, no image is displayed.
		Expression _MIMEType;	// (string) An expression, the value of which is the
								//	MIMEType for the image.
								//	Valid values are: image/bmp, image/jpeg,
								//	image/gif, image/png, image/x-png
								// Required if Source = Database. Ignored otherwise.
		ImageSizingEnum _Sizing;	// Defines the behavior if the image does not fit within the specified size.
	
		bool _ConstantImage;	// true if Image is a constant at runtime

		internal Image(ReportDefn r, ReportLink p, XmlNode xNode):base(r,p,xNode)
		{
			_ImageSource=ImageSourceEnum.Unknown;
			_Value=null;
			_MIMEType=null;
			_Sizing=ImageSizingEnum.AutoSize;
			_ConstantImage = false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Source":
						_ImageSource = fyiReporting.RDL.ImageSource.GetStyle(xNodeLoop.InnerText);
						break;
					case "Value":
						_Value = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					case "MIMEType":
						_MIMEType = new Expression(r, this, xNodeLoop, ExpressionType.String);
						break;
					case "Sizing":
						_Sizing = ImageSizing.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						if (ReportItemElement(xNodeLoop))	// try at ReportItem level
							break;
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Image element " + xNodeLoop.Name + " ignored.");
						break;
				}
			}
			if (_ImageSource==ImageSourceEnum.Unknown)
				OwnerReport.rl.LogError(8, "Image requires a Source element.");
			if (_Value == null)
				OwnerReport.rl.LogError(8, "Image requires the Value element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			base.FinalPass();

			_Value.FinalPass();
			if (_MIMEType != null)
				_MIMEType.FinalPass();

			_ConstantImage = this.IsConstant();
			
			return;
		}

		// Returns true if the image and style remain constant at runtime
		bool IsConstant()
		{
			
			if (_Value.IsConstant())
			{
				if (_MIMEType == null || _MIMEType.IsConstant())
				{
//					if (this.Style == null || this.Style.ConstantStyle)
//						return true;
					return true;	// ok if style changes
				}
			}
			return false;
		}

		override internal void Run(IPresent ip, Row row)
		{
			base.Run(ip, row);

			string mtype=null; 
			Stream strm=null;
			try 
			{
				strm = GetImageStream(ip.Report(), row, out mtype);

				ip.Image(this, row, mtype, strm);
			}
			catch
			{
				// image failed to load;  continue processing
			}
			finally
			{
				if (strm != null)
					strm.Close();
			}
			return;
		}

		override internal void RunPage(Pages pgs, Row row)
		{
			Report r = pgs.Report;
            bool bHidden = IsHidden(r, row);

			WorkClass wc = GetWC(r);
			string mtype=null; 
			Stream strm=null;
			System.Drawing.Image im=null;

			SetPagePositionBegin(pgs);
            if (bHidden)
            {
                PageImage pi = new PageImage(ImageFormat.Jpeg, null, 0, 0);
                this.SetPagePositionAndStyle(r, pi, row);
                SetPagePositionEnd(pgs, pi.Y + pi.H);
                return;
            }

			if (wc.PgImage != null)
			{	// have we already generated this one
				// reuse most of the work; only position will likely change
				PageImage pi = new PageImage(wc.PgImage.ImgFormat, wc.PgImage.ImageData, wc.PgImage.SamplesW, wc.PgImage.SamplesH);
				pi.Name = wc.PgImage.Name;				// this is name it will be shared under
				pi.Sizing = this._Sizing;
				this.SetPagePositionAndStyle(r, pi, row);
				pgs.CurrentPage.AddObject(pi);
                SetPagePositionEnd(pgs, pi.Y + pi.H);
				return;
			}

			try 
			{
				strm = GetImageStream(r, row, out mtype);
                if (strm == null)
                {
                    r.rl.LogError(4, string.Format("Unable to load image {0}.", this.Name.Nm));
                    return;
                }
				im = System.Drawing.Image.FromStream(strm);
				int height = im.Height;
				int width = im.Width;
				MemoryStream ostrm = new MemoryStream();
                // 140208AJM Better JPEG Encoding

				ImageFormat imf;
				//				if (mtype.ToLower() == "image/jpeg")    //TODO: how do we get png to work
				//					imf = ImageFormat.Jpeg;
				//				else
				imf = ImageFormat.Jpeg;
                System.Drawing.Imaging.ImageCodecInfo[] info;
                info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters encoderParameters;
                encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, ImageQualityManager.EmbeddedImageQuality);
                System.Drawing.Imaging.ImageCodecInfo codec = null;
                for (int i = 0; i < info.Length; i++)
                {
                    if (info[i].FormatDescription == "JPEG")
                    {
                        codec = info[i];
                        break;
                    }
                }
                im.Save(ostrm, codec, encoderParameters);

				// im.Save(ostrm, imf, encoderParameters);

                //END 140208AJM
				byte[] ba = ostrm.ToArray();
				ostrm.Close();
				PageImage pi = new PageImage(imf, ba, width, height);
				pi.Sizing = this._Sizing;
				this.SetPagePositionAndStyle(r, pi, row);

				pgs.CurrentPage.AddObject(pi);
				if (_ConstantImage)
				{
					wc.PgImage = pi;
					// create unique name; PDF generation uses this to optimize the saving of the image only once
					pi.Name = "pi" + Interlocked.Increment(ref Parser.Counter).ToString();	// create unique name
				}

                SetPagePositionEnd(pgs, pi.Y + pi.H);
            }
			catch (Exception e)
			{	
				// image failed to load, continue processing
				r.rl.LogError(4, "Image load failed.  " + e.Message);
			}
			finally
			{
				if (strm != null)
					strm.Close();
				if (im != null)
					im.Dispose();
			}
			return;
		}

		Stream GetImageStream(Report rpt, Row row, out string mtype)
		{
			mtype=null; 
			Stream strm=null;
			try 
			{
				switch (this.ImageSource)
				{
					case ImageSourceEnum.Database:
						if (_MIMEType == null)
							return null;
						mtype = _MIMEType.EvaluateString(rpt, row);
						object o = _Value.Evaluate(rpt, row);
						strm = new MemoryStream((byte[]) o);
						break;
					case ImageSourceEnum.Embedded:
						string name = _Value.EvaluateString(rpt, row);
						EmbeddedImage ei = (EmbeddedImage) OwnerReport.LUEmbeddedImages[name];
						mtype = ei.MIMEType;
						byte[] ba = Convert.FromBase64String(ei.ImageData);
						strm = new MemoryStream(ba);
						break;
					case ImageSourceEnum.External:
						string fname = _Value.EvaluateString(rpt, row);
						mtype = GetMimeType(fname);
						if (fname.StartsWith("http:") ||
							fname.StartsWith("file:") ||
							fname.StartsWith("https:"))
						{
							WebRequest wreq = WebRequest.Create(fname);
							WebResponse wres = wreq.GetResponse();
							strm = wres.GetResponseStream();
						}
						else
							strm = new FileStream(fname, System.IO.FileMode.Open, FileAccess.Read);		
						break;
					default:
						return null;
				}
			}
			catch (Exception e)
			{
				if (strm != null)
				{
					strm.Close();
					strm = null;
				}
				rpt.rl.LogError(4, string.Format("Unable to load image. {0}", e.Message));
			}

			return strm;
		}

		internal ImageSourceEnum ImageSource
		{
			get { return  _ImageSource; }
			set {  _ImageSource = value; }
		}

		internal Expression Value
		{
			get { return  _Value; }
			set {  _Value = value; }
		}

		internal Expression MIMEType
		{
			get { return  _MIMEType; }
			set {  _MIMEType = value; }
		}

		internal ImageSizingEnum Sizing
		{
			get { return  _Sizing; }
			set {  _Sizing = value; }
		}

		internal bool ConstantImage
		{
			get { return _ConstantImage; }
		}

		static internal string GetMimeType(string file)
		{
			String fileExt;
			
			int startPos = file.LastIndexOf(".") + 1;

			fileExt = file.Substring(startPos).ToLower();

			switch (fileExt)
			{
				case "bmp":
					return "image/bmp";
				case "jpeg":
				case "jpe":
				case "jpg":
				case "jfif":
					return "image/jpeg";
				case "gif":
					return "image/gif";
				case "png":
					return "image/png";
				case "tif":
				case "tiff":
					return "image/tiff";
				default:
					return null;
			}
		}

		private WorkClass GetWC(Report rpt)
		{
			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void RemoveImageWC(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		class WorkClass
		{
			internal PageImage PgImage;	// When ConstantImage is true this will save the PageImage for reuse
			internal WorkClass()
			{
				PgImage=null;
			}
		}

	}
}
