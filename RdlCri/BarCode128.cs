using System;
using System.Collections.Generic;
using System.Drawing;
using fyiReporting.RDL;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace fyiReporting.CRI
{
	public class BarCode128 : ICustomReportItem
	{
		static public readonly float OptimalHeight = 35.91f;          // Optimal height at magnification 1    
		static public readonly float OptimalWidth = 65.91f;            // Optimal width at mag 1
		private string _code128 = "";

		#region ICustomReportItem Members

		public bool IsDataRegion()
		{
			return false;
		}

		public void DrawImage(ref Bitmap bm)
		{
			DrawImage(ref bm, _code128.ToUpper());
		}

		/// <summary>
		/// Design time: Draw a hard coded BarCode for design time;  Parameters can't be
		/// relied on since they aren't available.
		/// </summary>
		/// <param name="bm"></param>
		public void DrawDesignerImage(ref System.Drawing.Bitmap bm)
		{
			DrawImage(ref bm, "MYFYI");
		}

		public void DrawImage(ref Bitmap bm, string code128)
		{
#if NETSTANDARD2_0
            var writer = new ZXing.BarcodeWriter<Bitmap>();
#else
			var writer = new ZXing.BarcodeWriter();
#endif
			writer.Format = ZXing.BarcodeFormat.CODE_128;

			Graphics g = null;
			g = Graphics.FromImage(bm);
			float mag = PixelConversions.GetMagnification(g, bm.Width, bm.Height,
				OptimalHeight, OptimalWidth);

			int barHeight = PixelConversions.PixelXFromMm(g, OptimalHeight * mag);
			int barWidth = PixelConversions.PixelYFromMm(g, OptimalWidth * mag);


			writer.Options.Height = barHeight;
			writer.Options.Width = barWidth;


			bm = writer.Write(code128);
		}

		public void SetProperties(IDictionary<string, object> props)
		{
			try
			{
				_code128 = props["Code"].ToString();
			}
			catch (KeyNotFoundException)
			{
				throw new Exception("Code property must be specified");
			}
		}

		public object GetPropertiesInstance(XmlNode iNode)
		{
			BarCodeProperties bcp = new BarCodeProperties(this, iNode);
			foreach (XmlNode n in iNode.ChildNodes)
			{
				if (n.Name != "CustomProperty")
					continue;
				string pname = XmlHelpers.GetNamedElementValue(n, "Name", "");
				switch (pname)
				{
					case "Code":
						bcp.SetBarCode(XmlHelpers.GetNamedElementValue(n, "Value", ""));
						break;
					default:
						break;
				}
			}

			return bcp;
		}

		public void SetPropertiesInstance(XmlNode node, object inst)
		{
			node.RemoveAll();       // Get rid of all properties

			BarCodeProperties bcp = inst as BarCodeProperties;
			if (bcp == null)
				return;


			XmlHelpers.CreateChild(node, "Code", bcp.Code);
		}


		/// <summary>
		/// Design time call: return string with <CustomReportItem> ... </CustomReportItem> syntax for 
		/// the insert.  The string contains a variable {0} which will be substituted with the
		/// configuration name.  This allows the name to be completely controlled by
		/// the configuration file.
		/// </summary>
		/// <returns></returns>
		public string GetCustomReportItemXml()
		{
			return "<CustomReportItem><Type>{0}</Type>" +
				string.Format("<Height>{0}mm</Height><Width>{1}mm</Width>", OptimalHeight, OptimalWidth) +
				"<CustomProperties>" +
				"<CustomProperty>" +
				"<Name>Code</Name>" +
				"<Value>Enter Your Value</Value>" +
				"</CustomProperty>" +
				"</CustomProperties>" +
				"</CustomReportItem>";
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			return;
		}

		#endregion


		/// <summary>
		/// BarCodeProperties- All properties are type string to allow for definition of
		/// a runtime expression.
		/// </summary>
		public class BarCodeProperties
		{
			string _code128;
			BarCode128 _bc;
			XmlNode _node;

			internal BarCodeProperties(BarCode128 bc, XmlNode node)
			{
				_bc = bc;
				_node = node;
			}

			internal void SetBarCode(string ns)
			{
				_code128 = ns;
			}
			[Category("Code"),
			   Description("The text string to be encoded as a BarCode128 Code.")]
			public string Code
			{
				get { return _code128; }
				set { _code128 = value; _bc.SetPropertiesInstance(_node, this); }
			}


		}

	}
}
