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
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelError.Text = "";

            if (SessionVariables.LoggedIn == false)
            {
                this.Response.Redirect("Login.aspx", false);
            }
          
        }

        protected void ButtonChangePassword_Click(object sender, EventArgs e)
        {

            if (TextBoxNewPassword.Text.Length < 1)
            {
                LabelError.Text = "New password must be at least one character in length.";

                return; 
            }

            if (TextBoxNewPassword.Text != TextBoxConfirmNewPassword.Text)
            {

                LabelError.Text = "New passwords do not match.";
                
                return;
            }

            try
            {
                string sql = "SELECT Email FROM users WHERE Password = @password AND Email = @email;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@password", DbType.String).Value = Code.Hashes.GetSHA512StringHash(TextBoxOldPassword.Text);
                cmd.Parameters.Add("@email", DbType.String).Value = SessionVariables.LoggedEmail;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);
                if (dt.Rows.Count != 1)
                {
                    LabelError.Text = "Invalid old password";
                    return;
                }

            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
                return;
            }


            SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);
            cn.Open();
            SQLiteTransaction txn;
            txn = cn.BeginTransaction();
            try
            {

                string sql = "UPDATE users SET Password = @password WHERE Email = @email;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;
                cmd.Transaction = txn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@password", DbType.String).Value = Code.Hashes.GetSHA512StringHash(TextBoxNewPassword.Text);
                cmd.Parameters.Add("@email", DbType.String).Value = SessionVariables.LoggedEmail;
                cmd.ExecuteNonQuery();
              

                txn.Commit();

                LabelError.Text = "Password Changed";
            }
            catch (Exception ex)
            {
                txn.Rollback();
                LabelError.Text = ex.Message;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }

                TextBoxConfirmNewPassword.Text = "";
                TextBoxNewPassword.Text = "";
                TextBoxOldPassword.Text = "";

            }


        }
    }
}