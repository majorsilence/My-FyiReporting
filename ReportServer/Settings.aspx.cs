using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Data;
using System.Text;

namespace ReportServer
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {


                string sql = "SELECT role, tag FROM roleaccess WHERE role = @roleid and (tag = 'Admin/Role Management' or tag = 'Admin/Report Upload' or tag = 'Admin/User Management')";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@roleid", DbType.String).Value = SessionVariables.LoggedRoleId;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<a href=\"ChangePassword.aspx\">Change Password</a><br />");   
                foreach (DataRow row in dt.Rows)
                {
                    if (row["tag"].ToString() == "Admin/Role Management")
                    {
                        sb.Append("<a href=\"AdminRoleManagement.aspx\">Role Management</a><br />");   
                    }
                    else if (row["tag"].ToString() == "Admin/Report Upload")
                    {
                        sb.Append("<a href=\"AdminReportUpload.aspx\">Upload Reports</a><br />");   
                    }
                    else if (row["tag"].ToString() == "Admin/User Management")
                    {
                        sb.Append("<a href=\"AdminRoleUsers.aspx\">User Management</a><br />");   
                    }
                }

                Literal1.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }


        }
    }
}