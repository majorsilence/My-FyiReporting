using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fyiReporting.RDL;

namespace ReportTests.Utils
{
    public static class RdlUtils
    {
        public static Report GetReport(Uri uri2Rdl)
        {
            //string cwd = System.Environment.CurrentDirectory;

            //var rdlView = new fyiReporting.RdlViewer.RdlViewer();
            //rdlView.SourceFile = new Uri(System.IO.Path.Combine(cwd,"Reports", "FunctionTest.rdl"));
            string filePath = uri2Rdl.LocalPath;
            // Now parse the file 
            string source = System.IO.File.ReadAllText(filePath);

            RDLParser rdlp;
            Report r;

            rdlp = new RDLParser(source);
            // RDLParser takes RDL XML and Parse compiles the report

            r = rdlp.Parse();
            if (r.ErrorMaxSeverity > 0)
            {

                foreach (string emsg in r.ErrorItems)
                {
                    //  Console.WriteLine(emsg);
                }

                int severity = r.ErrorMaxSeverity;
                r.ErrorReset();
                if (severity > 4)
                {
                    r = null; // don't return when severe errors
                }
            }

            return r;
        }
    }
}
