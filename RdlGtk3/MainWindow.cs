using Gtk;
using Majorsilence.Reporting.Rdl;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.RdlGtk3
{
    public class MainWindow : Window
    {
        private SpinButton CurrentPage;
        private Button FirstPageButton;
        private Box hbox1;
        private Button LastPageButton;
        private Button NextButton;
        private Button OpenButton;
        private Label PageCountLabel;
        private Button PreviousButton;
        private Button PrintButton;
        private ReportViewer reportviewer1;
        private Button SaveButton;
        private ScrolledWindow scrolledwindow1;
        private Box vbox1;

        public MainWindow()
            : base(WindowType.Toplevel)
        {
            Build();
            Title = "Report Viewer";
        }

        protected virtual void Build()
        {
            reportviewer1 = new ReportViewer();
            SetDefaultSize(800, 600);
            vbox1 = new Box(Orientation.Vertical, 3);
            vbox1.Name = "vbox1";

            hbox1 = new Box(Orientation.Horizontal, 7);
            hbox1.Name = "hbox1";

            OpenButton = new Button();
            OpenButton.CanFocus = true;
            OpenButton.Name = "OpenButton";
#pragma warning disable CS0612 // Type or member is obsolete
            OpenButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            OpenButton.UseUnderline = true;
            OpenButton.Label = "gtk-open";
            OpenButton.Clicked += OnFileOpen_Activated;
            hbox1.PackStart(OpenButton, false, false, 0);

            SaveButton = new Button();
            SaveButton.CanFocus = true;
            SaveButton.Name = "SaveButton";
#pragma warning disable CS0612 // Type or member is obsolete
            SaveButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            SaveButton.UseUnderline = true;
            SaveButton.Label = "gtk-save";
            SaveButton.Clicked += OnFileSave_Activated;
            hbox1.PackStart(SaveButton, false, false, 0);

            PrintButton = new Button();
            PrintButton.CanFocus = true;
            PrintButton.Name = "PrintButton";
#pragma warning disable CS0612 // Type or member is obsolete
            PrintButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            PrintButton.UseUnderline = true;
            PrintButton.Label = "gtk-print";
            PrintButton.Clicked += OnFilePrint_Activated;
            hbox1.PackStart(PrintButton, false, false, 0);

            FirstPageButton = new Button();
            FirstPageButton.CanFocus = true;
            FirstPageButton.Name = "FirstPageButton";
#pragma warning disable CS0612 // Type or member is obsolete
            FirstPageButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            FirstPageButton.UseUnderline = true;
            FirstPageButton.Label = "gtk-goto-first";
            FirstPageButton.Clicked += OnFirstPageButton_Activated;
            //this.hbox1.PackStart(this.FirstPageButton, false, false, 0);

            PreviousButton = new Button();
            PreviousButton.CanFocus = true;
            PreviousButton.Name = "PreviousButton";
#pragma warning disable CS0612 // Type or member is obsolete
            PreviousButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            PreviousButton.UseUnderline = true;
            PreviousButton.Label = "gtk-go-back";
            PreviousButton.Clicked += OnPreviousButton_Activated;
            //this.hbox1.PackStart(this.PreviousButton, false, false, 0);

            NextButton = new Button();
            NextButton.CanFocus = true;
            NextButton.Name = "NextButton";
#pragma warning disable CS0612 // Type or member is obsolete
            NextButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            NextButton.UseUnderline = true;
            NextButton.Label = "gtk-go-forward";
            NextButton.Clicked += OnNextButton_Activated;
            //this.hbox1.PackStart(this.NextButton, false, false, 0);

            LastPageButton = new Button();
            LastPageButton.CanFocus = true;
            LastPageButton.Name = "LastPageButton";
#pragma warning disable CS0612 // Type or member is obsolete
            LastPageButton.UseStock = true;
#pragma warning restore CS0612 // Type or member is obsolete
            LastPageButton.UseUnderline = true;
            LastPageButton.Label = "gtk-goto-last";
            LastPageButton.Clicked += OnLastPageButton_Activated;
            //this.hbox1.PackStart(this.LastPageButton, false, false, 0);

            CurrentPage = new SpinButton(0, 999999, 1);
            CurrentPage.CanFocus = true;
            CurrentPage.Name = "CurrentPage";
            CurrentPage.Adjustment.PageIncrement = 10;
            CurrentPage.ClimbRate = 1;
            CurrentPage.Numeric = true;
            //this.hbox1.PackStart(this.CurrentPage, false, false, 0);

            PageCountLabel = new Label();
            PageCountLabel.Name = "PageCountLabel";
            //this.hbox1.PackStart(this.PageCountLabel, false, false, 0);

            vbox1.PackStart(hbox1, false, false, 0);

            scrolledwindow1 = new ScrolledWindow();
            scrolledwindow1.CanFocus = true;
            scrolledwindow1.Name = "scrolledwindow1";
            scrolledwindow1.ShadowType = ShadowType.In;

            //Gtk.Viewport w10 = new Gtk.Viewport();
            //w10.ShadowType = Gtk.ShadowType.None;

            //this.vboxImages = new Gtk.Box(Gtk.Orientation.Vertical, 6);
            //this.vboxImages.Name = "vboxImages";
            //w10.Add(this.vboxImages);

            //w10.Add(reportviewer1);
            scrolledwindow1.Add(reportviewer1);
            //this.scrolledwindow1.Add(w10);


            vbox1.PackStart(scrolledwindow1, true, true, 0);


            scrolledwindow1.ShowAll();

            Add(vbox1);
            ShowAll();

            Destroyed += OnDeleteEvent;
        }

        protected void OnDeleteEvent(object sender, EventArgs a)
        {
            Application.Quit();
        }

        protected async void OnFileOpen_Activated(object sender, EventArgs e)
        {
            object[] param = new object[4];
            param[0] = "Cancel";
            param[1] = ResponseType.Cancel;
            param[2] = "Open";
            param[3] = ResponseType.Accept;

            FileChooserDialog fc =
                new("Open File",
                    null,
                    FileChooserAction.Open,
                    param);

            if (fc.Run() != (int)ResponseType.Accept)
            {
                fc.Destroy();
                return;
            }

            try
            {
                string filename = fc.Filename;
                fc.Destroy();

                if (File.Exists(filename))
                {
                    string parameters = await GetParameters(new Uri(filename));
                    await reportviewer1.LoadReport(new Uri(filename), parameters);
                }
            }
            catch (Exception ex)
            {
                MessageDialog m = new(null, DialogFlags.Modal, MessageType.Info,
                    ButtonsType.Ok, false,
                    "Error Opening File." + Environment.NewLine + ex.Message);
                m.Run();
                m.Destroy();
            }
        }

        public async void OnFileSave_Activated(object sender, EventArgs e)
        {
            await reportviewer1.SaveReport();
        }

        public void OnFilePrint_Activated(object sender, EventArgs e)
        {
            reportviewer1.PrintReport();
        }

        public void OnFirstPageButton_Activated(object sender, EventArgs e)
        {
            //this.reportviewer1.FirstPage();
        }

        public void OnPreviousButton_Activated(object sender, EventArgs e)
        {
            // this.reportviewer1.PreviousPage();
        }

        public void OnNextButton_Activated(object sender, EventArgs e)
        {
            //this.reportviewer1.NextPage();
        }

        public void OnLastPageButton_Activated(object sender, EventArgs e)
        {
            //this.reportviewer1.LastPage();
        }

        private async Task<string> GetParameters(Uri sourcefile)
        {
            string parameters = "";
            string sourceRdl = await File.ReadAllTextAsync(sourcefile.LocalPath);
            RDLParser parser = new(sourceRdl);
            await parser.Parse();

            if (parser.Report.UserReportParameters.Count > 0)
            {
                foreach (UserReportParameter rp in parser.Report.UserReportParameters)
                {
                    parameters += "&" + rp.Name + "=";
                }

                using ParameterPrompt prompt = new();
                prompt.Parameters = parameters;

                if (prompt.Run() == (int)ResponseType.Ok)
                {
                    parameters = prompt.Parameters;
                }

                prompt.Destroy();
            }

            return parameters;
        }
    }
}