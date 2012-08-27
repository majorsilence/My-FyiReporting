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
    public partial class AdminRoleUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelError.Text = "";

            try
            {

                if (IsPostBack)
                {
                    return;
                }

                RefreshUsers();

                string sql = "SELECT name, description FROM roles;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                foreach (DataRow row in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = row["name"].ToString();
                    item.Text = row["name"].ToString() + " - " + row["description"].ToString();

                    DropDownListRoles.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
        }

        private void RefreshUsers()
        {

          
            try
            {

 
                string sql = "SELECT Email, FirstName, LastName, RoleID FROM users WHERE RoleID != 'Anonymous' AND RoleId != 'Admin'";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                foreach (DataRow row in dt.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = row["Email"].ToString();
                    item.Text = row["Email"].ToString() + " - " + row["RoleId"].ToString() + " - " + row["FirstName"].ToString() + " " + row["LastName"].ToString();

                    ListBoxUserList.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
 

        }


        protected void ButtonChangeUserRole_Click(object sender, EventArgs e)
        {
            try
            {

                if (DropDownListRoles.Text.Trim() == "")
                {
                    LabelError.Text = "No role selected.  Must select a role before adding changing user role.";
                    return;
                }

                SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);
                cn.Open();
                SQLiteTransaction txn;
                txn = cn.BeginTransaction();
                try
                {

                    foreach (ListItem item in ListBoxUserList.Items)
                    {
                        if (item.Selected)
                        {

                            string sql = "UPDATE users SET RoleID = @roleid WHERE email = @email;";
                            SQLiteCommand cmd = new SQLiteCommand();
                            cmd.Connection = cn;
                            cmd.Transaction = txn;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = sql;
                            cmd.Parameters.Add("@roleid", DbType.String).Value = DropDownListRoles.Text;
                            cmd.Parameters.Add("@email", DbType.String).Value = item.Value;

                            cmd.ExecuteNonQuery();
                        }
                    }

                    txn.Commit();

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
                }

                RefreshUsers();
            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
        }
    }
}