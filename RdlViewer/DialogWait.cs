using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace fyiReporting.RdlViewer
{
    public partial class DialogWait : Form
    {
        private DateTime Started;
        private RdlViewer _viewer;

        public DialogWait(RdlViewer viewer)
        {
            InitializeComponent();
            _viewer = viewer;
            Started = DateTime.Now;
            timer1.Interval = 1000;
            timer1_Tick(null,null);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan time = DateTime.Now - Started;
            lblTimeTaken.Text = (((time.Days * 24 + time.Hours) * 60) + time.Minutes) + " Minutes " + time.Seconds + " Seconds";
            //lblStatus.Text = _viewer.ReportStatus();
            Application.DoEvents();
        }
    }
}