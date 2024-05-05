using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data;

namespace fyiReporting.ReportServerMvc
{
    public class Security
    {


        public static bool IsValidateRequest(System.Web.HttpResponse Response, System.Web.SessionState.HttpSessionState Session, string tag)
        {

            if (SessionVariables.LoggedIn == false)
            {
                Response.Redirect("Login.aspx", true);
                return false;
            }


            try
            {
               

                string sql = "SELECT role, tag FROM roleaccess WHERE role = @roleid and tag = @tag;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@roleid", DbType.String).Value = SessionVariables.LoggedRoleId;
                cmd.Parameters.Add("@tag", DbType.String).Value = tag;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("Login.aspx", true);
                    return false;
                }

                if (dt.Rows[0]["tag"].ToString() == tag)
                {
                     return true;
                }

            }
            catch (Exception ex)
            {
                Response.Redirect("Login.aspx", true);
                return false;
            }

            Response.Redirect("Login.aspx", true);
            return false;
        }

        public static bool HasPermissions(string reportName)
        {
            string nameOnly = System.IO.Path.GetFileName(reportName);
            string sql = "SELECT a.reportname, a.tag, c.description FROM reportfiles a ";
            sql += " JOIN roleaccess b ON a.tag=b.tag JOIN roletags c ON a.tag = c.tag ";
            sql += " WHERE b.role = @roleid AND a.reportname=@reportname;";
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.Add("@roleid", DbType.String).Value = SessionVariables.LoggedRoleId;
            cmd.Parameters.Add("@reportname", DbType.String).Value = nameOnly;

            DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }


    }
}