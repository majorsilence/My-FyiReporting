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
    public partial class AdminRoleManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelError.Text = "";

            try
            {

#if DEBUG == false

                Security.IsValidateRequest(this.Response, this.Session);
#endif

                if (IsPostBack)
                {
                    return;
                }

                RefreshRoles();

            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }
        }


        private void RefreshRoles()
        {
            ListBoxRoleList.Items.Clear();

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

                ListBoxRoleList.Items.Add(item);
            }
        }

        protected void ButtonCreateRole_Click(object sender, EventArgs e)
        {
            SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);
            try
            {

                string sql = "INSERT INTO roles (name, description) VALUES(@name, @description);";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@name", DbType.String).Value = TextBoxRoleName.Text.Trim();
                cmd.Parameters.Add("@description", DbType.String).Value = TextBoxRoleDescription.Text.Trim();

                cn.Open();
                cmd.ExecuteNonQuery();
         
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

            RefreshRoles();
        }

        protected void ButtonDeleteSelectedRole_Click(object sender, EventArgs e)
        {
            SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);
            cn.Open();
            SQLiteTransaction txn;
            txn = cn.BeginTransaction();
            try
            {

                foreach (ListItem item in ListBoxRoleList.Items)
                {
                    if (item.Selected)
                    {

                        string sql = "DELETE FROM roles WHERE name = @name;";
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.Connection = cn;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@name", DbType.String).Value = item.Value;

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

            RefreshRoles();
        }
    }
}