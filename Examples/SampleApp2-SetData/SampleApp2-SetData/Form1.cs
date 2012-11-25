using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;

namespace SampleApp2_SetData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // TODO: You must change this connection string to match where your database is
            string connectionString = @"Data Source=C:\Users\Peter\Projects\My-FyiReporting\Examples\northwindEF.db;Version=3;Pooling=True;Max Pool Size=100;";
            SQLiteConnection cn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CategoryID, CategoryName, Description FROM Categories;";
            cmd.Connection = cn;
            DataTable dt = GetTable(cmd);


            string filepath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory() , "SampleApp2-TestReport.rdl");
            rdlViewer1.SourceFile = new Uri(filepath);
            rdlViewer1.Report.DataSets["Data"].SetData(dt);
            rdlViewer1.Rebuild();
        }


        public DataTable GetTable(SQLiteCommand cmd)
        {
            System.Data.ConnectionState original = cmd.Connection.State;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }

            DataTable dt = new DataTable();
            SQLiteDataReader dr;

            dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            dr.Dispose();

            if (original == ConnectionState.Closed)
            {
                cmd.Connection.Clone();
            }

            return dt;
        }

    }
}
