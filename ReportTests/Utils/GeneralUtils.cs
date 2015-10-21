using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ReportTests.Utils
{
    public static class GeneralUtils
    {
        public static void ChangeCurrentCultrue(string cultureName)
        {
            var thread = System.Threading.Thread.CurrentThread;

            thread.CurrentCulture = new CultureInfo(cultureName);
          
        }
    }
}
