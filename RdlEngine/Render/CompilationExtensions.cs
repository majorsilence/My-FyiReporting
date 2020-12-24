using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace fyiReporting.RDL
{
    public static class CompilationExtensions
    {
#if NETSTANDARD2_0
        public static void SetRGBColorFill(this PdfContentByte contentByte, int red, int green, int blue)
        {
            contentByte.SetRgbColorFill(red, green, blue);
        }

        public static void SetRGBColorStroke(this PdfContentByte contentByte, int red, int green, int blue)
        {
            contentByte.SetRgbColorStroke(red, green, blue);
        }

        public static void SetRGBColorStrokeF(this PdfContentByte contentByte, int red, int green, int blue)
        {
            contentByte.SetRgbColorStrokeF(red, green, blue);
        }
#endif        

    }
}
