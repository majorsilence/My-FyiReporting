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
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {

            SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);

            try
            {

                if (TextBoxPassword.Text != TextBoxConfirmPassword.Text)
                {

                    LabelError.Text = "Password and confirm password do not match.";
                    return;
                }
                else if (TextBoxPassword.Text.Length < 1)
                {
                    LabelError.Text = "Password must be at least one character in length.";
                    return;
                }

                string password = Code.Hashes.GetSHA512StringHash(TextBoxPassword.Text);

                string sql = "INSERT INTO users (Email, FirstName, LastName, RoleId, Password) VALUES(@email, @firstname, @lastname, 'Users', @password)";
                SQLiteCommand cmd = new SQLiteCommand();

                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", DbType.String).Value = TextBoxEmail.Text;
                cmd.Parameters.Add("@firstname", DbType.String).Value = TextBoxFirstName.Text;
                cmd.Parameters.Add("@lastname", DbType.String).Value = TextBoxLastName.Text;
                cmd.Parameters.Add("@password", DbType.String).Value = password;

                cn.Open();
                cmd.ExecuteNonQuery();

                this.Response.Redirect("Login.aspx", true);


            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
        }
    }
}