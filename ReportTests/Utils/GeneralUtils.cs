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

        public static Uri OutputTestsFolder()
        {
            string tmpf = System.IO.Path.GetTempPath();
            return new Uri(System.IO.Path.Combine(tmpf, "rdlTestResults", Guid.NewGuid().ToString()));

        }
        public static Uri ReportsFolder(string subFoder)
        {
            string cwd = System.Environment.CurrentDirectory;
            return new Uri(System.IO.Path.Combine(cwd, "Reports", subFoder));

        }

    }
}
