using Microsoft.Data.Sqlite;
using System.Data;

namespace fyiReporting.ReportServerMvc
{
    public class StartupWorker : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                string sql = "SELECT Email, RoleId FROM users WHERE roleid = 'Admin';";
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    HttpContext.Current.Response.Redirect("~/Admin/Setup", true);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("~/Admin/Setup", true);
            }

            return Task.CompletedTask;
        }
    }
}
