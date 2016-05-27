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
        private Report _report;
        private Pages pages;
        public NeedPassword DataSourceReferencePassword = null;
        private Xwt.ScrollView scrollView;
        private Xwt.VBox vboxPages;
        private Xwt.HBox vboxToolMenu;
        private Xwt.VBox vboxContents;

        private Backend _defaultBackend = Backend.XwtWinforms;
        public Backend DefaultBackend
        {
            get { return _defaultBackend; }
            set { _defaultBackend = value; }
        }

        private string connstr_param_name = "connection_string";
        public string ConnectionStringParameterName
        {
            get { return connstr_param_name; }
            set { connstr_param_name = value; }
        }

        private string conntype_param_name = "connection_type";
        public string ConnectionTypeParameterName
        {
            get { return conntype_param_name; }
            set { conntype_param_name = value; }
        }

        private bool displayButtonMenuToolbar = false;
        public bool DisplayMenuToolBar
        {
            get { return displayButtonMenuToolbar; }
            set
            {
                vboxToolMenu.Visible = value;
                displayButtonMenuToolbar = value;
            }
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

        private string _SourceRdl;
        public string SourceRdl
        {
            get
            {
                return _SourceRdl;
            }
            set
            {
                _SourceRdl = value;
                Rebuild();
            }
        }

        public fyiReporting.RDL.Report Report
        {
            get
            {
                return this._report;
            }
        }

        public ReportViewer()
        {
            // Setup layout boxes
            vboxContents = new Xwt.VBox();
            vboxToolMenu = new Xwt.HBox();

            // Setup tool button menu
            Xwt.Button buttonExport = new Xwt.Button("Export");
            buttonExport.Clicked += delegate (object sender, EventArgs e)
            {
                SaveAs();
            };
            vboxToolMenu.PackStart(buttonExport);

            Xwt.Button buttonPrint = new Xwt.Button("Print");
            vboxToolMenu.PackStart(buttonPrint);

            // Add vboxContent widgets
            vboxPages = new Xwt.VBox();
            vboxContents.PackStart(vboxToolMenu);
            vboxContents.PackStart(vboxPages);


            // Setup Controls Contents
            scrollView = new Xwt.ScrollView();
            scrollView.Content = vboxContents;
            scrollView.VerticalScrollPolicy = ScrollPolicy.Automatic;
            scrollView.BorderVisible = true;
            this.PackStart(scrollView, true, true);

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
            SourceRdl = System.IO.File.ReadAllText(sourcefile.AbsolutePath);
            // GetSource is omitted: all it does is read the file.
            // Compile the report 

            Rebuild();
        }

        public void Rebuild()
        {
            _report = this.GetReport(SourceRdl);
            _report.RunGetData(Parameters);
            pages = _report.BuildPages();


            List<ReportArea> tempList = new List<ReportArea>();
            foreach (ReportArea w in this.vboxPages.Children)
            {
                tempList.Add(w);
            }
            foreach (ReportArea w in tempList)
            {
                vboxPages.Remove(w);
            }

            for (int pageCount = 0; pageCount < pages.Count; pageCount++)
            {
                ReportArea area = new ReportArea(this.DefaultBackend);
                area.SetReport(_report, pages[pageCount]);

                vboxPages.PackStart(area, true, true);
            }

            this.Show();


            if (_report.ErrorMaxSeverity > 0)
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


        public void SaveAs(string FileName, fyiReporting.RDL.OutputPresentationType type)
        {
            fyiReporting.RDL.OneFileStreamGen sg = null;

            try
            {
                // Must use the RunGetData before each export or there is no data.
                _report.RunGetData(this.Parameters);

                sg = new fyiReporting.RDL.OneFileStreamGen(FileName, true);
                _report.RunRender(sg, type);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex.Message);
            }
            finally
            {
                if (sg != null)
                {
                    sg.CloseMainStream();
                }
            }
            return;
        }

        public void SaveAs()
        {

            SaveFileDialog dlg = new SaveFileDialog("Select a file");
            dlg.Multiselect = false;
            dlg.Filters.Add(new FileDialogFilter("PDF files", "*.pdf"));
            dlg.Filters.Add(new FileDialogFilter("XML files", "*.xml"));
            dlg.Filters.Add(new FileDialogFilter("HTML files", "*.html"));
            dlg.Filters.Add(new FileDialogFilter("CSV files", "*.csv"));
            dlg.Filters.Add(new FileDialogFilter("RTF files", "*.rtf"));
            dlg.Filters.Add(new FileDialogFilter("TIF files", "*.tif"));
            dlg.Filters.Add(new FileDialogFilter("Excel files", "*.xlsx"));
            dlg.Filters.Add(new FileDialogFilter("MHT files", "*.mht"));


            Uri file = this.SourceFile;

            if (file != null)
            {
                dlg.InitialFileName = "*.pdf";
            }
            else
            {
                dlg.InitialFileName = "*.pdf";
            }



            if (dlg.Run() == false)
            {
                return;
            }

            // save the report in a rendered format 
            string ext = null;
            int i = dlg.FileName.LastIndexOf('.');
            if (i < 1)
            {
                ext = "";
            }
            else
            {
                ext = dlg.FileName.Substring(i + 1).ToLower();
            }

            fyiReporting.RDL.OutputPresentationType type = fyiReporting.RDL.OutputPresentationType.Internal;
            switch (ext)
            {
                case "pdf":
                    type = fyiReporting.RDL.OutputPresentationType.PDF;
                    break;
                case "xml":
                    type = fyiReporting.RDL.OutputPresentationType.XML;
                    break;
                case "html":
                    type = fyiReporting.RDL.OutputPresentationType.HTML;
                    break;
                case "htm":
                    type = fyiReporting.RDL.OutputPresentationType.HTML;
                    break;
                case "csv":
                    type = fyiReporting.RDL.OutputPresentationType.CSV;
                    break;
                case "rtf":
                    type = fyiReporting.RDL.OutputPresentationType.RTF;
                    break;
                case "mht":
                    type = fyiReporting.RDL.OutputPresentationType.MHTML;
                    break;
                case "mhtml":
                    type = fyiReporting.RDL.OutputPresentationType.MHTML;
                    break;
                case "xlsx":
                    type = fyiReporting.RDL.OutputPresentationType.Excel;
                    break;
                case "tif":
                    type = fyiReporting.RDL.OutputPresentationType.TIF;
                    break;
                case "tiff":
                    type = fyiReporting.RDL.OutputPresentationType.TIF;
                    break;
                default:
                    MessageDialog.ShowMessage(String.Format("{0} is not a valid file type.  File extension must be PDF, XML, HTML, CSV, MHT, RTF, TIF, XLSX.", dlg.FileName));
                    break;
            }


            SaveAs(dlg.FileName, type);

        }
    }
}
