using Gtk;

namespace RdlGtk3Viewer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            var win = new Majorsilence.Reporting.RdlGtk3.MainWindow();
            win.Show();
            Application.Run();
        }
    }
}