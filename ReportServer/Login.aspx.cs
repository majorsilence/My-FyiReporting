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


                string sql = "SELECT Email, FirstName, LastName, RoleId FROM users WHERE Email = @email AND Password = @password;";

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", DbType.String).Value = TextBoxUser.Text;
                cmd.Parameters.Add("@password", DbType.String).Value = Code.Hashes.GetSHA512StringHash(TextBoxPassword.Text);

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 1)
                {
                    SessionVariables.LoggedIn = true;
                    SessionVariables.LoggedEmail = dt.Rows[0]["Email"].ToString();
                    SessionVariables.LoggedFirstName = dt.Rows[0]["FirstName"].ToString();
                    SessionVariables.LoggedLastName = dt.Rows[0]["LastName"].ToString();
                    SessionVariables.LoggedRoleId = dt.Rows[0]["RoleId"].ToString();
                    this.Response.Redirect("Reports.aspx", false);
                }
                else
                {
                    SessionVariables.LoggedIn = false;
                    SessionVariables.LoggedEmail = "Anonymous";
                    SessionVariables.LoggedFirstName = "Anonymous";
                    SessionVariables.LoggedLastName = "Anonymous";
                    SessionVariables.LoggedRoleId = "Anonymous";
                    lResult.Text = "Unknown user name and/or password!";
                }




            }
            catch (System.Threading.ThreadAbortException tae)
            { }
            catch (Exception ex)
            {
                SessionVariables.LoggedIn = false;
                lResult.Text = ex.Message;
            }
        }
    }
}