using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using Microsoft.Data.Sqlite;

namespace SampleApp2_SetData
{
    public class Form1 : Form
    {
        private fyiReporting.RdlViewer.RdlViewer rdlViewer1;
        private fyiReporting.RdlViewer.ViewerToolstrip reportStrip;

        public Form1()
        {
            InitializeComponent();

            // TODO: You must change this connection string to match where your database is
            string connectionString = @"Data Source=/home/peter/Projects/My-FyiReporting/Examples/northwindEF.db;Version=3;Pooling=True;Max Pool Size=100;";
            var cn = new SqliteConnection(connectionString);
            var cmd = new SqliteCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CategoryID, CategoryName, Description FROM Categories;";
            cmd.Connection = cn;
      

        }

       

        private void InitializeViewer()
        {
            this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
            this.rdlViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));

            this.rdlViewer1.Location = new System.Drawing.Point(40, 69);
            this.rdlViewer1.Name = "rdlViewer1";

            this.rdlViewer1.Size = new System.Drawing.Size(731, 381);


        }

       

        private void InitializeComponent()
        {
            InitializeViewer();

            reportStrip = new fyiReporting.RdlViewer.ViewerToolstrip(rdlViewer1);
            //reportStrip.Location = new Point(0, 0);
            this.Controls.Add(reportStrip);
       

         
            this.SuspendLayout();
 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 462);
            this.Controls.Add(this.rdlViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }



    }
}
