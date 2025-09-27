using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace SampleApp2_SetData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            rdlViewer1 = new Majorsilence.Reporting.RdlViewer.RdlViewer();
            rdlViewer1.Dock = DockStyle.Fill;
            this.Controls.Add(rdlViewer1);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // TODO: You must change this connection string to match where your database is
            string sqlFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\", @"..\", @"..\", @"..\", @"..\", "northwindEF.db");
            string connectionString = $"Data Source={sqlFile}";

            using SqliteConnection cn = new SqliteConnection(connectionString);
            using SqliteCommand cmd = new SqliteCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CategoryID, CategoryName, Description FROM Categories;";
            cmd.Connection = cn;
            DataTable dt = await GetTable(cmd);

            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleApp2-TestReport.rdl");
            await rdlViewer1.SetSourceFile(new Uri(filepath));
            await (await rdlViewer1.Report()).DataSets["Data"].SetData(dt);
            await rdlViewer1.Rebuild();
        }


        public async Task<DataTable> GetTable(SqliteCommand cmd)
        {
            System.Data.ConnectionState original = cmd.Connection.State;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }

            DataTable dt = new DataTable();
            SqliteDataReader dr;

            dr = await cmd.ExecuteReaderAsync();
            dt.Load(dr);
            dr.Close();
            dr.Dispose();

            if (original == ConnectionState.Closed)
            {
                cmd.Connection.Close();
            }

            return dt;
        }

    }
}
