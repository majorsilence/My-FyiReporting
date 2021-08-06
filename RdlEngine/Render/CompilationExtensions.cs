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
            contentByte.SetRGBColorFill(red, green, blue);
        }

        public static void SetRGBColorStroke(this PdfContentByte contentByte, int red, int green, int blue)
        {
            contentByte.SetRGBColorStroke(red, green, blue);
        }

        public static void SetRGBColorStrokeF(this PdfContentByte contentByte, int red, int green, int blue)
        {
            contentByte.SetRGBColorStrokeF(red, green, blue);
        }
#endif        

    }
}
