using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.SQLite;
using System.Data;

namespace ReportServer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {

                string sql = "SELECT Email, RoleId FROM users WHERE roleid = 'Admin';";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    HttpContext.Current.Response.Redirect("Setup.aspx", true);
                }

            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Setup.aspx", true);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            fyiReporting.RdlAsp.RdlSession rs =
                Application[fyiReporting.RdlAsp.RdlSession.SessionStat] as fyiReporting.RdlAsp.RdlSession;

            if (rs == null)
            {
                Application["SessionStat"] = rs = new fyiReporting.RdlAsp.RdlSession();
            }
            lock (rs)
            {
                rs.Count++;
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            fyiReporting.RdlAsp.RdlSession rs =
                Application[fyiReporting.RdlAsp.RdlSession.SessionStat] as fyiReporting.RdlAsp.RdlSession;

            if (rs != null)
            {
                lock (rs)
                {
                    rs.Count--;
                }
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}