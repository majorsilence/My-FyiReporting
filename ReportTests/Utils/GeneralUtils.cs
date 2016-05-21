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

        public static Uri ReportsFolder(string subFoder = null)
        {
            string defaultReportsFolder = "Reports/";
            string cwd = CurrentDirectory();
            if (subFoder != null)
                return new Uri(System.IO.Path.Combine(cwd, defaultReportsFolder, subFoder));
            else
                return new Uri(System.IO.Path.Combine(cwd, defaultReportsFolder));


        }

        static string CurrentDirectory()
        {
            // Works from within nunit and regular execution
            var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            var u = new UriBuilder(codeBase);

            var path = Uri.UnescapeDataString(u.Path);
            return System.IO.Path.GetDirectoryName(path);
        }

    }
}
