using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleApp2_SetData
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form frm = new Form();
            fyiReporting.RdlDesign.RdlUserControl ctl = new fyiReporting.RdlDesign.RdlUserControl();
            ctl.OpenFile(@"C:\Users\Peter\Projects\My-FyiReporting\Examples\Examples\FileDirectoryTest.rdl");
            ctl.Dock = DockStyle.Fill;
            frm.Controls.Add(ctl);

            Application.Run(frm);

        }
    }
}
