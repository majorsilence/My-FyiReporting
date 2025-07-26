using Majorsilence.Reporting.Rdl;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.RdlGtk3
{
    public static class ReportExporter
    {
        private static Report report;
        private static string connectionString;
        private static bool overwriteSubreportConnection;
        private static string workingDirectory;
        private static Uri sourceFileUri;

        // GetParameters creates a list dictionary
        // consisting of a report parameter name and a value.
        private static ListDictionary GetParmeters(string parms)
        {
            if (string.IsNullOrWhiteSpace(parms.Trim()))
            {
                return null;
            }

            ListDictionary ld = new();
            if (parms == null)
            {
                return ld; // dictionary will be empty in this case
            }

            // parms are separated by & 

            char[] breakChars = new[] { '&' };
            string[] ps = parms.Split(breakChars);

            foreach (string p in ps)
            {
                int iEq = p.IndexOf("=");
                if (iEq > 0)
                {
                    string name = p.Substring(0, iEq);
                    string val = p.Substring(iEq + 1);
                    ld.Add(name, val);
                }
            }

            return ld;
        }

        private static async Task<Report> GetReport(string reportSource)
        {
            // Now parse the file 

            RDLParser rdlp;
            Report r;

            rdlp = new RDLParser(reportSource);
            rdlp.Folder = workingDirectory;
            rdlp.OverwriteConnectionString = connectionString;
            rdlp.OverwriteInSubreport = overwriteSubreportConnection;
            // RDLParser takes RDL XML and Parse compiles the report

            r = await rdlp.Parse();
            if (r.ErrorMaxSeverity > 0)
            {
                foreach (string emsg in r.ErrorItems)
                {
                    Console.WriteLine(emsg);
                }
            }

            return r;
        }

        #region Export to file

        /// <summary>
        ///     Export the report to file
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="parameters">Example: parameter1=someValue&parameter2=anotherValue</param>
        /// <param name="connectionString">Relace all Connection string in report.</param>
        /// <param name="overwriteSubreportConnection">If true connection string in subreport also will be overwrite</param>
        public static async Task Export(Uri filename, string parameters, string connectionStr, string fileName,
            OutputPresentationType exportType, bool overwriteSubreportCon = false)
        {
            //SourceFile = filename;

            connectionString = connectionStr;
            overwriteSubreportConnection = overwriteSubreportCon;

            await Export(filename, parameters, fileName, exportType);
        }

        /// <summary>
        ///     Export the report to file
        /// </summary>
        /// <param name="source">Xml source of report</param>
        /// <param name="parameters">Example: parameter1=someValue&parameter2=anotherValue</param>
        /// <param name="connectionString">Relace all Connection string in report.</param>
        /// <param name="overwriteSubreportConnection">If true connection string in subreport also will be overwrite</param>
        public static async Task Export(string source, string parameters, string connectionStr, string fileName,
            OutputPresentationType exportType, bool overwriteSubreportCon = false)
        {
            connectionString = connectionStr;
            overwriteSubreportConnection = overwriteSubreportCon;

            await Export(source, parameters, fileName, exportType);
        }

        /// <summary>
        ///     Export the report to file
        /// </summary>
        /// <param name='filename'>
        ///     Filename.
        /// </param>
        public static async Task Export(Uri filename, string fileName, OutputPresentationType exportType)
        {
            await Export(filename, "", fileName, exportType);
        }

        /// <summary>
        ///     Export the report to file
        /// </summary>
        /// <param name='sourcefile'>
        ///     Filename.
        /// </param>
        /// <param name='parameters'>
        ///     Example: parameter1=someValue&parameter2=anotherValue
        /// </param>
        public static async Task Export(Uri sourcefile, string parameters, string fileName,
            OutputPresentationType exportType)
        {
            sourceFileUri = sourcefile;
            workingDirectory = Path.GetDirectoryName(sourcefile.LocalPath);

            string source = File.ReadAllText(sourcefile.LocalPath);
            await Export(source, parameters, fileName, exportType);
        }

        /// <summary>
        ///     Export the report to file
        /// </summary>
        /// <param name="source">Xml source of report</param>
        /// <param name="parameters">Example: parameter1=someValue&parameter2=anotherValue</param>
        public static async Task Export(string source, string parameters, string fileName,
            OutputPresentationType exportType)
        {
            // Compile the report 
            report = await GetReport(source);
            if (report == null)
            {
                throw new ArgumentException("Can not compile report");
            }

            await report.RunGetData(GetParmeters(parameters));

            OneFileStreamGen sg = null;

            try
            {
                sg = new OneFileStreamGen(fileName, true);
                await report.RunRender(sg, exportType);
            }
            finally
            {
                if (sg != null)
                {
                    sg.CloseMainStream();
                }
            }
        }

        #endregion

        #region Export to memory stream

        /// <summary>
        ///     Export the report to memory stream
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="parameters">Example: parameter1=someValue&parameter2=anotherValue</param>
        /// <param name="connectionString">Relace all Connection string in report.</param>
        /// <param name="overwriteSubreportConnection">If true connection string in subreport also will be overwrite</param>
        public static async Task<MemoryStream> ExportToMemoryStream(Uri filename, string parameters,
            string connectionStr, OutputPresentationType exportType, bool overwriteSubreportCon = false)
        {
            //SourceFile = filename;

            connectionString = connectionStr;
            overwriteSubreportConnection = overwriteSubreportCon;

            return await ExportToMemoryStream(filename, parameters, exportType);
        }

        /// <summary>
        ///     Export the report to memory stream
        /// </summary>
        /// <param name="source">Xml source of report</param>
        /// <param name="parameters">Example: parameter1=someValue&parameter2=anotherValue</param>
        /// <param name="connectionString">Relace all Connection string in report.</param>
        /// <param name="overwriteSubreportConnection">If true connection string in subreport also will be overwrite</param>
        public static async Task<MemoryStream> ExportToMemoryStream(string source, string parameters,
            string connectionStr, OutputPresentationType exportType, bool overwriteSubreportCon = false)
        {
            connectionString = connectionStr;
            overwriteSubreportConnection = overwriteSubreportCon;

            return await ExportToMemoryStream(source, parameters, exportType);
        }

        /// <summary>
        ///     Export the report to memory stream
        /// </summary>
        /// <param name='filename'>
        ///     Filename.
        /// </param>
        public static async Task<MemoryStream> ExportToMemoryStream(Uri filename, OutputPresentationType exportType)
        {
            return await ExportToMemoryStream(filename, "", exportType);
        }

        /// <summary>
        ///     Export the report to memory stream
        /// </summary>
        /// <param name='sourcefile'>
        ///     Filename.
        /// </param>
        /// <param name='parameters'>
        ///     Example: parameter1=someValue&parameter2=anotherValue
        /// </param>
        public static async Task<MemoryStream> ExportToMemoryStream(Uri sourcefile, string parameters,
            OutputPresentationType exportType)
        {
            sourceFileUri = sourcefile;
            workingDirectory = Path.GetDirectoryName(sourcefile.LocalPath);

            string source = File.ReadAllText(sourcefile.LocalPath);
            return await ExportToMemoryStream(source, parameters, exportType);
        }

        /// <summary>
        ///     Export the report to memory stream
        /// </summary>
        /// <param name="source">Xml source of report</param>
        /// <param name="parameters">Example: parameter1=someValue&parameter2=anotherValue</param>
        public static async Task<MemoryStream> ExportToMemoryStream(string source, string parameters,
            OutputPresentationType exportType)
        {
            // Compile the report 
            report = await GetReport(source);
            if (report == null)
            {
                throw new ArgumentException("Can not compile report");
            }

            await report.RunGetData(GetParmeters(parameters));

            using (MemoryStreamGen ms = new())
            {
                try
                {
                    await report.RunRender(ms, exportType);
                    return ms.GetStream() as MemoryStream;
                }
                finally
                {
                    ms.CloseMainStream();
                    report?.Dispose();
                    report = null;
                }
            }
        }

        #endregion
    }
}