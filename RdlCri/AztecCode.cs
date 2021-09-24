using System;
using System.Collections.Generic;
using System.Text;
using fyiReporting.RDL;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

namespace fyiReporting.CRI
{
    public class AztecCode : ICustomReportItem
    {

        static public readonly float OptimalHeight = 35.91f;          // Optimal height at magnification 1    
        static public readonly float OptimalWidth = 35.91f;            // Optimal width at mag 1

        #region ICustomReportItem Members

        bool ICustomReportItem.IsDataRegion()
        {
            return false;
        }

        void ICustomReportItem.DrawImage(ref System.Drawing.Bitmap bm)
        {
            DrawImage(ref bm, _aztecCode);
        }

        /// <summary>
        /// Does the actual drawing of the image.
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="AztecCode"></param>
        internal void DrawImage(ref System.Drawing.Bitmap bm, string aztecCode)
        {
#if NETSTANDARD2_0
            var writer = new ZXing.BarcodeWriter<Bitmap>();
#else
            var writer = new ZXing.BarcodeWriter();
#endif
            writer.Format = ZXing.BarcodeFormat.AZTEC;

            Graphics g = null;
            g = Graphics.FromImage(bm);
            float mag = PixelConversions.GetMagnification(g, bm.Width, bm.Height, OptimalHeight, OptimalWidth);

            int barHeight = PixelConversions.PixelXFromMm(g, OptimalHeight * mag);
            int barWidth = PixelConversions.PixelYFromMm(g, OptimalWidth * mag);

            writer.Options.Height = barHeight;
            writer.Options.Width = barWidth;


            bm = writer.Write(aztecCode);

        }

        /// <summary>
        /// Design time: Draw a hard coded BarCode for design time;  Parameters can't be
        /// relied on since they aren't available.
        /// </summary>
        /// <param name="bm"></param>
        void ICustomReportItem.DrawDesignerImage(ref System.Drawing.Bitmap bm)
        {
            DrawImage(ref bm, "https://github.com/majorsilence/My-FyiReporting");
        }

        private string _aztecCode = "";
        void ICustomReportItem.SetProperties(IDictionary<string, object> props)
        {
            try
            {
                _aztecCode = props["AztecCode"].ToString();
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("AztecCode property must be specified");
            }
        }

        object ICustomReportItem.GetPropertiesInstance(System.Xml.XmlNode iNode)
        {
            BarCodePropertiesAztecCode bcp = new BarCodePropertiesAztecCode(this, iNode);
            foreach (XmlNode n in iNode.ChildNodes)
            {
                if (n.Name != "CustomProperty")
                    continue;
                string pname = XmlHelpers.GetNamedElementValue(n, "Name", "");
                switch (pname)
                {
                    case "AztecCode":
                        bcp.SetAztecCode(XmlHelpers.GetNamedElementValue(n, "Value", ""));
                        break;
                    default:
                        break;
                }
            }

            return bcp;
        }

        public void SetPropertiesInstance(System.Xml.XmlNode node, object inst)
        {
            node.RemoveAll();       // Get rid of all properties

            BarCodePropertiesAztecCode bcp = inst as BarCodePropertiesAztecCode;
            if (bcp == null)
                return;


            XmlHelpers.CreateChild(node, "AztecCode", bcp.AztecCode);

        }




        /// <summary>
        /// Design time call: return string with <CustomReportItem> ... </CustomReportItem> syntax for 
        /// the insert.  The string contains a variable {0} which will be substituted with the
        /// configuration name.  This allows the name to be completely controlled by
        /// the configuration file.
        /// </summary>
        /// <returns></returns>
        string ICustomReportItem.GetCustomReportItemXml()
        {
            return "<CustomReportItem><Type>{0}</Type>" +
                string.Format("<Height>{0}mm</Height><Width>{1}mm</Width>", OptimalHeight, OptimalWidth) +
                "<CustomProperties>" +
                "<CustomProperty>" +
                "<Name>AztecCode</Name>" +
                "<Value>Enter Your Value</Value>" +
                "</CustomProperty>" +
                "</CustomProperties>" +
                "</CustomReportItem>";
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            return;
        }

        #endregion

        /// <summary>
        /// BarCodeProperties- All properties are type string to allow for definition of
        /// a runtime expression.
        /// </summary>
        public class BarCodePropertiesAztecCode
        {
            string _AztecCode;
            AztecCode _az;
            XmlNode _node;

            internal BarCodePropertiesAztecCode(AztecCode bc, XmlNode node)
            {
                _az = bc;
                _node = node;
            }

            internal void SetAztecCode(string ns)
            {
                _AztecCode = ns;
            }

            [Category("AztecCode"), Description("The text string to be encoded as a AztecCode Code.")]
            public string AztecCode
            {
                get { return _AztecCode; }
                set { _AztecCode = value; _az.SetPropertiesInstance(_node, this); }
            }


        }
    }


}
