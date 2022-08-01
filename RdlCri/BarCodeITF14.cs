using fyiReporting.RDL;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.XPath;
using System.ComponentModel;

namespace fyiReporting.CRI
{
    public class BarCodeITF14 : ICustomReportItem
    {
        
        static public readonly float OptimalWidth = 78f;                    // Optimal width at mag 1
        static public readonly float OptimalHeight = 39f;      // use ratio of 2.5
        private string _Itf14Data;

        public void Dispose()
        {
            return;
        }

        public void DrawDesignerImage(ref Bitmap bm)
        {
            bm = new Bitmap(BarcodeLib.Barcode.DoEncode(BarcodeLib.TYPE.ITF14, "1234567891011"));
        }

        public void DrawImage(ref Bitmap bm)
        {
            bm = new Bitmap(BarcodeLib.Barcode.DoEncode(BarcodeLib.TYPE.ITF14, _Itf14Data));
        }

        public string GetCustomReportItemXml()
        {
            return "<CustomReportItem><Type>{0}</Type>" +
               string.Format("<Height>{0}mm</Height><Width>{1}mm</Width>", OptimalHeight, OptimalWidth) +
               "<CustomProperties>" +
               "<CustomProperty>" +
               "<Name>ITF14</Name>" +
               "<Value>Enter Your Value</Value>" +
               "</CustomProperty>" +
               "</CustomProperties>" +
               "</CustomReportItem>";
        }

        public object GetPropertiesInstance(XmlNode iNode)
        {
            BarCodePropertiesItf14 bcp = new BarCodePropertiesItf14(this, iNode);

            foreach (XmlNode n in iNode.ChildNodes)
            {
                if (n.Name != "CustomProperty")
                    continue;
                string pname = XmlHelpers.GetNamedElementValue(n, "Name", "");
                switch (pname)
                {
                    case "ITF14":
                        bcp.SetITF14(XmlHelpers.GetNamedElementValue(n, "Value", ""));
                        break;
                    default:
                        break;
                }
            }

            return bcp;
        }

        public bool IsDataRegion()
        {
            return false;
        }

        public void SetProperties(IDictionary<string, object> props)
        {
            try
            {
                _Itf14Data = props["ITF14"].ToString();
                if (_Itf14Data.Length < 13 || _Itf14Data.Length > 14)
                    throw new Exception("ITF 14 data must be of length 13 or 14");

            }
            catch (KeyNotFoundException)
            {
                throw new Exception("ITF14 property must be specified");
            }

        }

        public void SetPropertiesInstance(XmlNode node, object inst)
        {
            node.RemoveAll();       // Get rid of all properties

            var itfCode = inst as BarCodePropertiesItf14;
            if (itfCode == null)
                return;

            XmlHelpers.CreateChild(node, "ITF14", itfCode.Itf24);
        }


        /// <summary>
        /// BarCodeProperties- All properties are type string to allow for definition of
        /// a runtime expression.
        /// </summary>
        public class BarCodePropertiesItf14
        {
            string _itf14Data;
            BarCodeITF14 _itf14;
            XmlNode _node;

            internal BarCodePropertiesItf14(BarCodeITF14 bc, XmlNode node)
            {
                _itf14 = bc;
                _node = node;
            }

            internal void SetITF14(string ns)
            {
                _itf14Data = ns;
            }

            [Category("ITF14"), Description("The text string to be encoded as a ITF14 Code.")]
            public string Itf24
            {
                get { return _itf14Data; }
                set { _itf14Data = value; _itf14.SetPropertiesInstance(_node, this); }
            }


        }
    }
}
