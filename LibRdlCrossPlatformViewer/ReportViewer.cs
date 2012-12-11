using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using fyiReporting.RDL;
using Xwt;

namespace LibRdlCrossPlatformViewer
{
    public class ReportViewer : VBox
    {
        private Report report;
        private Pages pages;
        public NeedPassword DataSourceReferencePassword = null;
        private Xwt.ScrollView scrollView;
        private Xwt.VBox vboxPages;


        public string connstr_param_name = "connection_string";
        public string ConnectionStringParameterName
        {
            get { return connstr_param_name; }
            set { connstr_param_name = value; }
        }

        public string conntype_param_name = "connection_type";
        public string ConnectionTypeParameterName
        {
            get { return conntype_param_name; }
            set { conntype_param_name = value; }
        }

        public ListDictionary Parameters { get; private set; }

        bool show_errors;
        public bool ShowErrors
        {
            get
            {
                return show_errors;
            }
            set
            {
                show_errors = value;
            }
        }

        bool show_params;
        public bool ShowParameters
        {
            get
            {
                return show_params;
            }
            set
            {
                show_params = value;

            }
        }

        public Uri SourceFile { get; private set; }

        public ReportViewer()
        {
            
            vboxPages = new Xwt.VBox();
            scrollView = new Xwt.ScrollView();
            scrollView.Content = vboxPages;
            scrollView.VerticalScrollPolicy = ScrollPolicy.Automatic;
            scrollView.BorderVisible = true;
            this.PackStart(scrollView, BoxMode.FillAndExpand);

            Parameters = new ListDictionary();

            ShowErrors = false;
        }

        public void LoadReport(Uri filename, string parameters, string connectionString)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename.AbsolutePath);

            foreach (XmlNode node in xmlDoc.GetElementsByTagName("ConnectString"))
            {
                node.InnerText = connectionString;
            }

            xmlDoc.Save(filename.AbsolutePath);

            LoadReport(filename, parameters);
        }


        /// <summary>
        /// Loads the report.
        /// </summary>
        /// <param name='filename'>
        /// Filename.
        /// </param>
        public void LoadReport(Uri filename)
        {
            LoadReport(filename, "");
        }

        /// <summary>
        /// Loads the report.
        /// </summary>
        /// <param name='filename'>
        /// Filename.
        /// </param>
        /// <param name='parameters'>
        /// Example: parameter1=someValue&parameter2=anotherValue
        /// </param>
        public void LoadReport(Uri sourcefile, string parameters)
        {
            SourceFile = sourcefile;

            string source;
            // Any parameters?  e.g.  file1.rdl?orderid=5 
            if (parameters.Trim() != "")
            {
                this.Parameters = this.GetParmeters(parameters);
            }
            else
            {
                this.Parameters = null;
            }

            // Obtain the source 
            source = System.IO.File.ReadAllText(sourcefile.AbsolutePath);
            // GetSource is omitted: all it does is read the file.
            // Compile the report 
            report = this.GetReport(source);

            RefreshReport();

            
        
        }

        void RefreshReport()
        {

            report.RunGetData(Parameters);
            pages = report.BuildPages();


            foreach (Xwt.VBox w in this.vboxPages.Children)
            {
                this.Remove(w);
            }

            for (int pageCount = 0; pageCount < pages.Count; pageCount++)
            {
                ReportArea area = new ReportArea();
                area.SetReport(report, pages[pageCount]);

                // TODO: set the correct height
                area.MinHeight = 600;
                vboxPages.PackStart(area, BoxMode.FillAndExpand);
            }

            this.Show();


            if (report.ErrorMaxSeverity > 0)
            {
                // TODO: add error messages back
                //SetErrorMessages(report.ErrorItems);
            }

        }

        private System.Collections.Specialized.ListDictionary GetParmeters(string parms)
        {
            System.Collections.Specialized.ListDictionary ld = new System.Collections.Specialized.ListDictionary();
            if (parms == null)
            {
                return ld; // dictionary will be empty in this case
            }

            // parms are separated by & 

            char[] breakChars = new char[] { '&' };
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

        private Report GetReport(string reportSource)
        {
            // Now parse the file 

            RDLParser rdlp;
            Report r;

            rdlp = new RDLParser(reportSource);
            // RDLParser takes RDL XML and Parse compiles the report

            r = rdlp.Parse();
            if (r.ErrorMaxSeverity > 0)
            {

                foreach (string emsg in r.ErrorItems)
                {
                    Console.WriteLine(emsg);
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
