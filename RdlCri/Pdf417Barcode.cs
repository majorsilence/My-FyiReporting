using System;
using System.Collections.Generic;
using System.Text;
using Majorsilence.Reporting.Rdl;
#if DRAWINGCOMPAT
using Draw2 = Majorsilence.Drawing;
#else
using Draw2 = System.Drawing;
#endif
using System.ComponentModel;
using System.Xml;
using ZXing;

namespace Majorsilence.Reporting.Cri
{
    public class Pdf417Barcode : ZxingBarcodes
    {
        public Pdf417Barcode()
        {
            format = ZXing.BarcodeFormat.PDF_417;
        }
    }
}