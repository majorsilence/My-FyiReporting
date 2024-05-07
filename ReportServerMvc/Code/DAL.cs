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
            var origSate = cmd.Connection.State;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }

            DataTable dt = new DataTable();
            using var dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();

            if (origSate == ConnectionState.Closed)
            {
                cmd.Connection.Close();
            }

            return dt;
        }

        public static async Task<DataTable> ExecuteCmdTableAsync(Microsoft.Data.Sqlite.SqliteCommand cmd)
        {
            var origSate = cmd.Connection.State;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                await cmd.Connection.OpenAsync();
            }

            DataTable dt = new DataTable();
            using var dr = await cmd.ExecuteReaderAsync();
            dt.Load(dr);
            await dr.CloseAsync();

            if (origSate == ConnectionState.Closed)
            {
                await cmd.Connection.CloseAsync();
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