using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;

namespace SampleCrossPlatformViewer
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Initialize(ToolkitType.Wpf);

            var app = new MainWindow();
            //var app = new MainWindowExample2();
            
            Application.Run();

            app.Dispose();
        }


       

    }
}
