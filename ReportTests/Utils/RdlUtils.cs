using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Majorsilence.Reporting.Rdl;

namespace ReportTests.Utils
{
    public static class RdlUtils
    {
        public static async Task<Report> GetReport(Uri uri2Rdl, string overWriteConnectionString = null)
        {
            //string cwd = System.Environment.CurrentDirectory;

            //var rdlView = new Majorsilence.Reporting.RdlViewer.RdlViewer();
            //rdlView.SourceFile = new Uri(System.IO.Path.Combine(cwd,"Reports", "FunctionTest.rdl"));
            string filePath = uri2Rdl.LocalPath;
            // Now parse the file 
            string source = System.IO.File.ReadAllText(filePath);

            RDLParser rdlp;
            Report r;

            rdlp = new RDLParser(source);
            // RDLParser takes RDL XML and Parse compiles the report
            if (!string.IsNullOrWhiteSpace(overWriteConnectionString))
            {
                rdlp.OverwriteConnectionString = overWriteConnectionString;
            }

            r = await rdlp.Parse();
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