using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xwt;

namespace SampleCrossPlatformViewer
{
    public class MainWindowExample2 : Window
    {
        private LibRdlCrossPlatformViewer.ReportViewer rv;

        // TODO: add a way that parameters can be entered by an end user.
        private string parameters = "";


        public MainWindowExample2()
        {
            this.Title = "Xwt Demo Application";
            this.Width = 800;
            this.Height = 600;

            rv = new LibRdlCrossPlatformViewer.ReportViewer();
            rv.DefaultBackend = LibRdlCrossPlatformViewer.Backend.XwtWinforms;

            var path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "../", "../", "../", "../", "SqliteExamples", "SimpleTest1.rdl");
            rv.SourceRdl = File.ReadAllText(path);
            rv.Rebuild();

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
            dlg.Filters.Add(new FileDialogFilter("Report files", "*.rdl"));
            dlg.Filters.Add(new FileDialogFilter("All files", "*.*"));



            if (dlg.Run())
            {
                rv.LoadReport(new Uri(dlg.FileName), this.parameters);
            }

        }

        private void saveas_Clicked(object sender, EventArgs e)
        {

            rv.SaveAs();

            return;


        }



        private void exit_Clicked(object sender, EventArgs e)
        {
            Xwt.Application.Exit();
        }

    }
}
