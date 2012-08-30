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

                Security.IsValidateRequest(this.Response, this.Session, @"Admin/Role Management");


                if (IsPostBack)
                {
                    return;
                }


                string sql = "SELECT tag, description FROM roletags;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                DataTable dtAllTags = Code.DAL.ExecuteCmdTable(cmd);

                foreach (DataRow row in dtAllTags.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = row["tag"].ToString();
                    item.Text = row["tag"].ToString() + " - " + row["description"].ToString();

                    ListBoxRoleTags.Items.Add(item);
                }

                RefreshRoles();


                // On page load do an initial tag selection.
                DropDownList1_SelectedIndexChanged(sender, e);

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
                DropDownList1.Items.Add(item);
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
                        cmd.Transaction = txn;
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

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (ListItem item in ListBoxRoleTags.Items)
            {
                item.Selected = false;
            }

            string sql2 = "SELECT tag FROM roleaccess WHERE role = @role;";
            SQLiteCommand cmd2 = new SQLiteCommand();
            cmd2.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.CommandText = sql2;
            cmd2.Parameters.Add("@role", DbType.String).Value = DropDownList1.Text;

            DataTable dtCurrentRoleTags = Code.DAL.ExecuteCmdTable(cmd2);

            foreach (ListItem item in ListBoxRoleTags.Items)
            {
                foreach (DataRow row in dtCurrentRoleTags.Rows)
                {
                    if (row["tag"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void ButtonSaveRoleTags_Click(object sender, EventArgs e)
        {
            SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);
            cn.Open();
            SQLiteTransaction txn;
            txn = cn.BeginTransaction();
            try
            {

                string sql = "DELETE FROM roleacces WHERE role = @role;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;
                cmd.Transaction = txn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@role", DbType.String).Value = DropDownList1.Text;

                cmd.ExecuteNonQuery();
                    

                foreach (ListItem item in ListBoxRoleList.Items)
                {
                    if (item.Selected)
                    {

                        string sql2 = "INSERT INTO roleaccess(role, tag) VALUES (@role, @tag);";
                        SQLiteCommand cmd2 = new SQLiteCommand();
                        cmd2.Connection = cn;
                        cmd2.CommandType = System.Data.CommandType.Text;
                        cmd2.CommandText = sql2;
                        cmd2.Parameters.Add("@role", DbType.String).Value = DropDownList1.Text;
                        cmd2.Parameters.Add("@tag", DbType.String).Value = item.Value;

                        cmd2.ExecuteNonQuery();
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
        }
    }
}