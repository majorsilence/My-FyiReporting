using System;
using System.Collections.Generic;
using System.Text;
using fyiReporting.RDL;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

namespace fyiReporting.CRI
{
    public class Code39 : ICustomReportItem
    {
        #region ICustomReportItem Members

        public bool IsDataRegion()
        {
            throw new NotImplementedException();
        }

        public void DrawImage(ref Bitmap bm)
        {
            throw new NotImplementedException();
        }

        public void DrawDesignerImage(ref Bitmap bm)
        {
            throw new NotImplementedException();
        }

        public void SetProperties(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public object GetPropertiesInstance(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public void SetPropertiesInstance(XmlNode node, object inst)
        {
            throw new NotImplementedException();
        }

        public string GetCustomReportItemXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
