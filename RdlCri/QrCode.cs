using System;
using System.Collections.Generic;
using System.Text;
using fyiReporting.RDL;

namespace fyiReporting.CRI
{
    public class QrCode : ICustomReportItem
    {
        #region ICustomReportItem Members

        bool ICustomReportItem.IsDataRegion()
        {
            throw new NotImplementedException();
        }

        void ICustomReportItem.DrawImage(System.Drawing.Bitmap bm)
        {
            throw new NotImplementedException();
        }

        void ICustomReportItem.DrawDesignerImage(System.Drawing.Bitmap bm)
        {
            throw new NotImplementedException();
        }

        void ICustomReportItem.SetProperties(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        object ICustomReportItem.GetPropertiesInstance(System.Xml.XmlNode node)
        {
            throw new NotImplementedException();
        }

        void ICustomReportItem.SetPropertiesInstance(System.Xml.XmlNode node, object inst)
        {
            throw new NotImplementedException();
        }

        string ICustomReportItem.GetCustomReportItemXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            return;
        }

        #endregion
    }
}
