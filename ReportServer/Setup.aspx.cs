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
    public partial class Setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LabelError.Text = "";

            try
            {

                string sql = "SELECT email FROM users WHERE RoleId = 'Admin'";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    PanelAlreadySetup.Visible = true;
                    PanelNotSetup.Visible = false;
                }
                else
                {
                    PanelAlreadySetup.Visible = false;
                    PanelNotSetup.Visible = true;
                }


            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (TextBoxPassword.Text != TextBoxConfirmPassword.Text)
                {

                    LabelError.Text = "Password and confirm password do not match.";
                    return;
                }

                string password = Code.Hashes.GetSHA512StringHash(TextBoxPassword.Text);

                string sql = "INSERT INTO users (Email, FirstName, LastName, RoleId, Password) VALUES(@email, @firstname, @lastname, 'Admin', @password)";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", DbType.String).Value = TextBoxEmail.Text;
                cmd.Parameters.Add("@firstname", DbType.String).Value = TextBoxFirstName.Text;
                cmd.Parameters.Add("@lastname", DbType.String).Value = TextBoxLastName.Text;
                cmd.Parameters.Add("@password", DbType.String).Value = password;



            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
        }
    }
}