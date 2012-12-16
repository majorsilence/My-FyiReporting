using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;

namespace SampleCrossPlatformViewer
{
    public class MainWindow : Window
    {
        private LibRdlCrossPlatformViewer.ReportViewer rv;

        // TODO: add a way that parameters can be entered by an end user.
        private string parameters = "";


        public MainWindow()
        {
            this.Title = "Xwt Demo Application";
            this.Width = 800;
            this.Height = 600;


            rv = new LibRdlCrossPlatformViewer.ReportViewer();
            rv.DefaultBackend = LibRdlCrossPlatformViewer.Backend.PureXwt;

#if DEBUG
            rv.LoadReport(new Uri(@"C:\Users\Peter\Projects\My-FyiReporting\Examples\SqliteExamples\SimpleTest1.rdl"));
#endif

            this.Content = rv;


            this.MainMenu = CreateMenu();
            this.Show();

        }

        private Menu CreateMenu()
        {
            Menu m = new Menu();
            MenuItem file = new MenuItem("File");
            Menu fileSubMenu = new Menu();

            MenuItem open = new MenuItem("Open");
            open.Clicked += open_Clicked;
            fileSubMenu.Items.Add(open);

            MenuItem saveas = new MenuItem("Save As");
            saveas.Clicked += saveas_Clicked;
            fileSubMenu.Items.Add(saveas);


            MenuItem exit = new MenuItem("Exit");
            exit.Clicked += exit_Clicked;
            fileSubMenu.Items.Add(exit);


            file.SubMenu = fileSubMenu;
            m.Items.Add(file);

            return m;
        }

        private void open_Clicked(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog("Select a file");
            dlg.InitialFileName = "Some file";
            dlg.Multiselect = false;
            dlg.Filters.Add(new FileDialogFilter("Xwt files", "*.rdl"));
            dlg.Filters.Add(new FileDialogFilter("All files", "*.*"));



            if (dlg.Run())
            {
                rv.LoadReport(new Uri(dlg.FileName), this.parameters);
            }

        }

        private void saveas_Clicked(object sender, EventArgs e)
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


            Uri file = rv.SourceFile;

            if (file != null)
            {
                int index = file.LocalPath.LastIndexOf('.');
                if (index > 1)
                    dlg.InitialFileName = file.LocalPath.Substring(0, index) + ".pdf";
                else
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
                ext = "";
            else
                ext = dlg.FileName.Substring(i + 1).ToLower();

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


            rv.SaveAs(dlg.FileName, type);


            return;


        }

       

        private void exit_Clicked(object sender, EventArgs e)
        {
            Xwt.Application.Exit();
        }

    }
}
