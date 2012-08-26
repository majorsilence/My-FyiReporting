using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Data;

namespace ReportServer
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
 

            }
            catch (Exception ex)
            {
                lResult.Text = ex.Message;
            }
        }

        protected void bSignOn_Click(object sender, EventArgs e)
        {
            try
            {


                string sql = "SELECT Email, RoleId FROM users WHERE Email = @email AND Password = @password;";

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", DbType.String).Value = TextBoxUser.Text;
                cmd.Parameters.Add("@password", DbType.String).Value = TextBoxPassword.Text;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 1)
                {
                    SessionVariables.LoggedIn = true;
                    this.Response.Redirect("Reports.aspx", true);
                }
                else
                {
                    SessionVariables.LoggedIn = false;
                    lResult.Text = "Unknown user name and/or password!";
                }




            }
            catch (Exception ex)
            {
                SessionVariables.LoggedIn = false;
                lResult.Text = ex.Message;
            }
        }
    }
}