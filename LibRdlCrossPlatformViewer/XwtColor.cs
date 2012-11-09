using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibRdlCrossPlatformViewer
{
    public class XwtColor
    {

        public static Xwt.Drawing.Color SystemColorToXwtColor(System.Drawing.Color theColor)
        {

            double a = Convert.ToDouble(theColor.A);
            double b = Convert.ToDouble(theColor.B);
            double g = Convert.ToDouble(theColor.G);
            double r = Convert.ToDouble(theColor.R);
            Xwt.Drawing.Color ec = new Xwt.Drawing.Color(r, g, b, a);

            return ec;
        }

    }
}
