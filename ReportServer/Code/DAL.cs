using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ReportServer.Code
{
    public class DAL
    {
        public static DataTable ExecuteCmdTable(System.Data.SQLite.SQLiteCommand cmd)
        {

            System.Data.ConnectionState origSate = cmd.Connection.State;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }


            DataTable dt;
            System.Data.SQLite.SQLiteDataReader dr;

            dt = new DataTable();
            dr = cmd.ExecuteReader();
            dt.Load(dr);

            dr.Close();
            dr = null;


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