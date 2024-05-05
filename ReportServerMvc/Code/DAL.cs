using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace fyiReporting.ReportServerMvc.Code
{
    public class DAL
    {
        public static DataTable ExecuteCmdTable(Microsoft.Data.Sqlite.SqliteCommand cmd)
        {

            System.Data.ConnectionState origSate = cmd.Connection.State;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }


            DataTable dt;


            dt = new DataTable();
            using var dr = cmd.ExecuteReader();
            dt.Load(dr);

            dr.Close();


            if (origSate == ConnectionState.Closed)
            {
                cmd.Connection.Close();
            }


            return dt;
        }

        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ServerData"].ToString();
            }
        }

    }
}