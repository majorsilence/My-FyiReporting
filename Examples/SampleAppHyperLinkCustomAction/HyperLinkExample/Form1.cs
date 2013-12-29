using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyperLinkExample
{
    public partial class Form1 : Form
    {

        private string file = @"C:\Users\Peter\Projects\My-FyiReporting\Examples\SqliteExamples\SimpleTest1.rdl";

        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonReloadReport_Click(object sender, EventArgs e)
        {
            rdlViewer1.SourceFile = new Uri(file);
            rdlViewer1.Rebuild();
        }

        private void rdlViewer1_Hyperlink(object source, fyiReporting.RdlViewer.HyperlinkEventArgs e)
        {
            // report LastName field as HyperLink action
            var url = new Uri(e.Hyperlink);
            if (url.Scheme == "lastname" )
            {
                e.Cancel = true;
                MessageBox.Show(url.ToString());
            }

        }
    }
}
