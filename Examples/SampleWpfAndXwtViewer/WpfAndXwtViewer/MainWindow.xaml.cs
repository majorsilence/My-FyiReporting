using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAndXwtViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private LibRdlCrossPlatformViewer.ReportViewer rv;

        // TODO: add a way that parameters can be entered by an end user.
        private string parameters = "";


        public MainWindow()
        {
            InitializeComponent();

            // Before using any xwt code you must initialize its engine as wpf
            // See https://groups.google.com/forum/?fromgroups=#!topic/xwt-list/9d2kb4cf5GU
            Xwt.Application.Initialize(Xwt.ToolkitType.Wpf);
            Xwt.Engine.Toolkit.ExitUserCode(null);

            rv = new LibRdlCrossPlatformViewer.ReportViewer();
            rv.DefaultBackend = LibRdlCrossPlatformViewer.Backend.XwtWinforms;

            // Since this is an example I just hard code the report path. 
            // In your own application you will want to provice a method to select reports.
#if DEBUG
            if (System.Environment.MachineName == "GILL-PC")
            {
                rv.LoadReport(new Uri(@"C:\Users\Peter\Projects\My-FyiReporting\Examples\SqliteExamples\SimpleTest1.rdl"));
            }
            else if (System.Environment.MachineName == "gill-desktop")
            {
                rv.LoadReport(new Uri(@"/home/peter/projects/My-FyiReporting/Examples/SqliteExamples/SimpleTest1.rdl"));
            }
#endif

            // Here we convert the xwt VBox to a WPF Panel
            Panel w = (Panel)Xwt.Engine.WidgetRegistry.GetNativeWidget(rv);

            this.Content = w;

        }
    }
}
