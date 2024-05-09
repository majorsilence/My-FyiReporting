using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Data;

namespace fyiReporting.ReportServerMvc
{
    public class Security
    {
        public static async Task<bool> IsValidateRequestAsync(ISession Session, string tag)
        {
            if (SessionVariables.LoggedIn == false)
            {
                return false;
            }

            try
            {
                string sql = "SELECT role, tag FROM roleaccess WHERE role = @roleid and tag = @tag;";
                SqliteCommand cmd = new SqliteCommand();
                await using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@roleid", SqliteType.Text).Value = SessionVariables.LoggedRoleId;
                cmd.Parameters.Add("@tag", SqliteType.Text).Value = tag;

                DataTable dt = await Code.DAL.ExecuteCmdTableAsync(cmd);

                if (dt.Rows.Count == 0)
                {      
                    return false;
                }

                if (dt.Rows[0]["tag"].ToString() == tag)
                {
                    return true;
                }
            }
            catch (Exception)
            {     
                return false;
            }

            return false;
        }

        public static async Task<bool> HasPermissionsAsync(string reportName)
        {
            string nameOnly = System.IO.Path.GetFileName(reportName);
            string sql = "SELECT a.reportname, a.tag, c.description FROM reportfiles a ";
            sql += " JOIN roleaccess b ON a.tag=b.tag JOIN roletags c ON a.tag = c.tag ";
            sql += " WHERE b.role = @roleid AND a.reportname=@reportname;";
            SqliteCommand cmd = new SqliteCommand();
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);
            cmd.Connection = cn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.Add("@roleid", SqliteType.Text).Value = SessionVariables.LoggedRoleId;
            cmd.Parameters.Add("@reportname", SqliteType.Text).Value = nameOnly;

            DataTable dt = await Code.DAL.ExecuteCmdTableAsync(cmd);

            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}