using System;
using System.Threading.Tasks;
using Gtk;

namespace Majorsilence.Reporting.RdlGtk3
{
    public class MainWindow : Gtk.Window
    {
        private Gtk.Box vbox1;
        private Gtk.Box hbox1;
        private Gtk.Button OpenButton;
        private Gtk.Button SaveButton;
        private Gtk.Button PrintButton;
        private Gtk.Button FirstPageButton;
        private Gtk.Button PreviousButton;
        private Gtk.Button NextButton;
        private Gtk.Button LastPageButton;
        private Gtk.SpinButton CurrentPage;
        private Gtk.Label PageCountLabel;
        private Gtk.ScrolledWindow scrolledwindow1;
        private Majorsilence.Reporting.RdlGtk3.ReportViewer reportviewer1;

        public MainWindow()
            : base(Gtk.WindowType.Toplevel)
        {
            Build();
            this.Title = "Report Viewer";
        }

        protected virtual void Build()
        {
            reportviewer1 = new ReportViewer();
            this.SetDefaultSize(800, 600);
            this.vbox1 = new Gtk.Box(Gtk.Orientation.Vertical, 3);
            this.vbox1.Name = "vbox1";

            this.hbox1 = new Gtk.Box(Gtk.Orientation.Horizontal, 7);
            this.hbox1.Name = "hbox1";

            this.OpenButton = new Gtk.Button();
            this.OpenButton.CanFocus = true;
            this.OpenButton.Name = "OpenButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.OpenButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.OpenButton.UseUnderline = true;
            this.OpenButton.Label = "gtk-open";
            this.OpenButton.Clicked += new System.EventHandler(this.OnFileOpen_Activated);
            this.hbox1.PackStart(this.OpenButton, false, false, 0);

            this.SaveButton = new Gtk.Button();
            this.SaveButton.CanFocus = true;
            this.SaveButton.Name = "SaveButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.SaveButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.SaveButton.UseUnderline = true;
            this.SaveButton.Label = "gtk-save";
            this.SaveButton.Clicked += new System.EventHandler(this.OnFileSave_Activated);
            this.hbox1.PackStart(this.SaveButton, false, false, 0);

            this.PrintButton = new Gtk.Button();
            this.PrintButton.CanFocus = true;
            this.PrintButton.Name = "PrintButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.PrintButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.PrintButton.UseUnderline = true;
            this.PrintButton.Label = "gtk-print";
            this.PrintButton.Clicked += new System.EventHandler(this.OnFilePrint_Activated);
            this.hbox1.PackStart(this.PrintButton, false, false, 0);

            this.FirstPageButton = new Gtk.Button();
            this.FirstPageButton.CanFocus = true;
            this.FirstPageButton.Name = "FirstPageButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.FirstPageButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.FirstPageButton.UseUnderline = true;
            this.FirstPageButton.Label = "gtk-goto-first";
            this.FirstPageButton.Clicked += new System.EventHandler(this.OnFirstPageButton_Activated);
            //this.hbox1.PackStart(this.FirstPageButton, false, false, 0);

            this.PreviousButton = new Gtk.Button();
            this.PreviousButton.CanFocus = true;
            this.PreviousButton.Name = "PreviousButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.PreviousButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.PreviousButton.UseUnderline = true;
            this.PreviousButton.Label = "gtk-go-back";
            this.PreviousButton.Clicked += new System.EventHandler(this.OnPreviousButton_Activated);
            //this.hbox1.PackStart(this.PreviousButton, false, false, 0);

            this.NextButton = new Gtk.Button();
            this.NextButton.CanFocus = true;
            this.NextButton.Name = "NextButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.NextButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.NextButton.UseUnderline = true;
            this.NextButton.Label = "gtk-go-forward";
            this.NextButton.Clicked += new System.EventHandler(this.OnNextButton_Activated);
            //this.hbox1.PackStart(this.NextButton, false, false, 0);

            this.LastPageButton = new Gtk.Button();
            this.LastPageButton.CanFocus = true;
            this.LastPageButton.Name = "LastPageButton";
#pragma warning disable CS0612 // Type or member is obsolete
            this.LastPageButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            this.LastPageButton.UseUnderline = true;
            this.LastPageButton.Label = "gtk-goto-last";
            this.LastPageButton.Clicked += new System.EventHandler(this.OnLastPageButton_Activated);
            //this.hbox1.PackStart(this.LastPageButton, false, false, 0);

            this.CurrentPage = new Gtk.SpinButton(0, 999999, 1);
            this.CurrentPage.CanFocus = true;
            this.CurrentPage.Name = "CurrentPage";
            this.CurrentPage.Adjustment.PageIncrement = 10;
            this.CurrentPage.ClimbRate = 1;
            this.CurrentPage.Numeric = true;
            //this.hbox1.PackStart(this.CurrentPage, false, false, 0);

            this.PageCountLabel = new Gtk.Label();
            this.PageCountLabel.Name = "PageCountLabel";
            //this.hbox1.PackStart(this.PageCountLabel, false, false, 0);

            this.vbox1.PackStart(this.hbox1, false, false, 0);

            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.ShadowType = Gtk.ShadowType.In;

            //Gtk.Viewport w10 = new Gtk.Viewport();
            //w10.ShadowType = Gtk.ShadowType.None;

            //this.vboxImages = new Gtk.Box(Gtk.Orientation.Vertical, 6);
            //this.vboxImages.Name = "vboxImages";
            //w10.Add(this.vboxImages);

            //w10.Add(reportviewer1);
            this.scrolledwindow1.Add(reportviewer1);
            //this.scrolledwindow1.Add(w10);


            this.vbox1.PackStart(this.scrolledwindow1, true, true, 0);


            scrolledwindow1.ShowAll();

            this.Add(vbox1);
            this.ShowAll();

            this.Destroyed += OnDeleteEvent;
        }

        protected void OnDeleteEvent(object sender, EventArgs a)
        {
            Application.Quit();
        }

        protected async void OnFileOpen_Activated(object sender, System.EventArgs e)
        {
            object[] param = new object[4];
            param[0] = "Cancel";
            param[1] = Gtk.ResponseType.Cancel;
            param[2] = "Open";
            param[3] = Gtk.ResponseType.Accept;

            using (Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog("Open File",
                    null,
                    Gtk.FileChooserAction.Open,
                    param))
            {

                if (fc.Run() != (int)Gtk.ResponseType.Accept)
                {
                    fc.Destroy();
                    return;
                }

                try
                {
                    string filename = fc.Filename;
                    fc.Destroy();

                    if (System.IO.File.Exists(filename))
                    {
                        string parameters = await this.GetParameters(new Uri(filename));
                        await this.reportviewer1.LoadReport(new Uri(filename), parameters);
                    }
                }
                catch (Exception ex)
                {
                    using (Gtk.MessageDialog m = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info,
                                              Gtk.ButtonsType.Ok, false,
                                              "Error Opening File." + System.Environment.NewLine + ex.Message))
                    {
                        m.Run();
                        m.Destroy();
                    }
                }
            }

        }

        public async void OnFileSave_Activated(object sender, System.EventArgs e)
        {
            await this.reportviewer1.SaveReport();
        }

        public void OnFilePrint_Activated(object sender, System.EventArgs e)
        {
            this.reportviewer1.PrintReport();
        }

        public void OnFirstPageButton_Activated(object sender, System.EventArgs e)
        {
            //this.reportviewer1.FirstPage();
        }

        public void OnPreviousButton_Activated(object sender, System.EventArgs e)
        {
            // this.reportviewer1.PreviousPage();
        }

        public void OnNextButton_Activated(object sender, System.EventArgs e)
        {
            //this.reportviewer1.NextPage();
        }

        public void OnLastPageButton_Activated(object sender, System.EventArgs e)
        {
            //this.reportviewer1.LastPage();
        }

        private async Task<string> GetParameters(Uri sourcefile)
        {
            string parameters = "";
            string sourceRdl = System.IO.File.ReadAllText(sourcefile.LocalPath);
            Majorsilence.Reporting.Rdl.RDLParser parser = new Majorsilence.Reporting.Rdl.RDLParser(sourceRdl);
            await parser.Parse();

            if (parser.Report.UserReportParameters.Count > 0)
            {
                foreach (Rdl.UserReportParameter rp in parser.Report.UserReportParameters)
                {
                    parameters += "&" + rp.Name + "=";
                }

                Majorsilence.Reporting.RdlGtk3.ParameterPrompt prompt = new Majorsilence.Reporting.RdlGtk3.ParameterPrompt();
                prompt.Parameters = parameters;

                if (prompt.Run() == (int)Gtk.ResponseType.Ok)
                {
                    parameters = prompt.Parameters;
                }

                prompt.Destroy();
            }

            return parameters;
        }
    }
}
