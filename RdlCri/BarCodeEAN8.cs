using System;
using System.Collections.Generic;
using System.Text;
using Majorsilence.Reporting.Rdl;
#if DRAWINGCOMPAT
using Majorsilence.Drawing;
#else
using System.Drawing;
#endif
using System.ComponentModel;
using System.Xml;

namespace Majorsilence.Reporting.Cri
{
    public class BarCodeEAN8 : ZxingBarcodes
    {
        public BarCodeEAN8() : base(35.91f, 65.91f) // Optimal width at mag 1
        {
            format = ZXing.BarcodeFormat.EAN_8;
        }
    }
}
