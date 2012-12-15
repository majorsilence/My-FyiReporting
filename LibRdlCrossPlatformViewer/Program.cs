using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;


namespace LibRdlCrossPlatformViewer
{
    public class Program
    {

        [STAThread]
        public static void Main(string [] args)
        {
            Application.Initialize(ToolkitType.Gtk);

            Window w = new Window();
            w.Title = "Xwt Demo Application";
            w.Width = 500;
            w.Height = 400;

            ReportViewer rv = new ReportViewer();
            rv.DefaultBackend = Backend.PureXwt;
            rv.LoadReport(new Uri(@"C:\Users\Peter\Projects\My-FyiReporting\Examples\SqliteExamples\SimpleTest1.rdl"));

            w.Content = rv;

            w.Show();

            Application.Run();

            w.Dispose();
        }





    }

    

}
