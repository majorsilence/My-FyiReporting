using fyiReporting.ReportServerMvc.Models;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace fyiReporting.ReportServerMvc.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Setup()
        {
            try
            {
                string sql = "SELECT email FROM users WHERE RoleId = 'Admin'";
                SqliteCommand cmd = new SqliteCommand();
                await using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                DataTable dt = await Code.DAL.ExecuteCmdTableAsync(cmd);

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
                return BadRequest(ex.Message);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetupSubmit([FromBody] AdminSetupSubmission model)
        {
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);

            try
            {
                if (!string.Equals(model.Password, model.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Password and confirm password do not match.");
                }
                else if (model.Password.Length < 1)
                {
                    return BadRequest("Password must be at least one character in length.");
                }

                string password = Code.Hashes.GetSHA512StringHash(model.Password);

                string sql = "INSERT INTO users (Email, FirstName, LastName, RoleId, Password) VALUES(@email, @firstname, @lastname, 'Admin', @password)";
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", SqliteType.Text).Value = model.Email;
                cmd.Parameters.Add("@firstname", SqliteType.Text).Value = model.FirstName;
                cmd.Parameters.Add("@lastname", SqliteType.Text).Value = model.LastName;
                cmd.Parameters.Add("@password", SqliteType.Text).Value = password;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                this.Response.Redirect("~/Reports", true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        public async Task<ActionResult> Settings()
        {
            try
            {
                string sql = "SELECT role, tag FROM roleaccess WHERE role = @roleid and (tag = 'Admin/Role Management' or tag = 'Admin/Report Upload' or tag = 'Admin/User Management')";
                SqliteCommand cmd = new SqliteCommand();
                await using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@roleid", SqliteType.Text).Value = SessionVariables.LoggedRoleId;

                DataTable dt = await Code.DAL.ExecuteCmdTableAsync(cmd);

                if (dt.Rows.Count == 0)
                {
                    return View();
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
                return BadRequest(ex.Message);

            }

            return View();
        }

        [TypeFilter(typeof(IsValidRequestAttribute), Arguments = new object[] { @"Admin/Report Upload" })]
        public ActionResult ReportUpload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(IsValidRequestAttribute), Arguments = new object[] { @"Admin/Report Upload" })]
        public async Task<ActionResult> ReportUploadSubmit(IFormFile formFile)
        {
            try
            {
                string path = SessionVariables.ReportDirectory;

                String filename = formFile.Name;
                Stream inputStream = formFile.OpenReadStream();
              
                if (System.IO.File.Exists(System.IO.Path.Combine(path, filename)))
                {
                    return BadRequest(string.Format("File {0} already exists", filename));
                }
                FileStream fileStream = new FileStream(System.IO.Path.Combine(path, filename), FileMode.OpenOrCreate);
                inputStream.CopyTo(fileStream);
                fileStream.Close();

                // TODO: Add insert into report table.

                await using SqliteConnection cn = new SqliteConnection(Code.DAL.ConnectionString);
                await cn.OpenAsync();
                await using SqliteTransaction txn = cn.BeginTransaction();
                try
                {

                    string tag = @"Reports/" + filename;

                    string sql = "INSERT INTO roletags (tag, description) VALUES (@tag, @description);";
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.Connection = cn;
                    cmd.Transaction = txn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@tag", SqliteType.Text).Value = tag;
                    cmd.Parameters.Add("@description", SqliteType.Text).Value = filename + " report";
                    await cmd.ExecuteNonQueryAsync();

                    string sql2 = "INSERT INTO roleaccess (role, tag) VALUES (@role, @tag);";
                    SqliteCommand cmd2 = new SqliteCommand();
                    cmd2.Connection = cn;
                    cmd2.Transaction = txn;
                    cmd2.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandText = sql2;
                    cmd2.Parameters.Add("@role", SqliteType.Text).Value = "Admin";
                    cmd2.Parameters.Add("@tag", SqliteType.Text).Value = tag;
                    await cmd2.ExecuteNonQueryAsync();


                    string sql3 = "INSERT INTO reportfiles (reportname, tag) VALUES (@reportname, @tag);";
                    SqliteCommand cmd3 = new SqliteCommand();
                    cmd3.Connection = cn;
                    cmd3.Transaction = txn;
                    cmd3.CommandType = System.Data.CommandType.Text;
                    cmd3.CommandText = sql3;
                    cmd3.Parameters.Add("@reportname", SqliteType.Text).Value = filename;
                    cmd3.Parameters.Add("@tag", SqliteType.Text).Value = tag;
                    await cmd3.ExecuteNonQueryAsync();

                    await txn.CommitAsync();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    System.IO.File.Delete(System.IO.Path.Combine(path, filename));
                    return BadRequest(ex.Message);
                }

                return Json(new { success = true }); // when upload was successful
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [TypeFilter(typeof(IsValidRequestAttribute), Arguments = new object[] { @"Admin/Role Management" })]
        public ActionResult RoleManagement()
        {
            try
            {

                if (IsPostBack)
                {
                    return;
                }
                ListBoxRoleTags.SelectionMode = ListSelectionMode.Multiple;
                ListBoxRoleList.SelectionMode = ListSelectionMode.Multiple;

                string sql = "SELECT tag, description FROM roletags;";
                using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = cn;
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
            return View();
        }

        private void RefreshRoles()
        {
            ListBoxRoleList.Items.Clear();

            string sql = "SELECT name, description FROM roles;";
            SqliteCommand cmd = new SqliteCommand();
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);
            cmd.Connection = cn;
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

        [HttpPost]
        public ActionResult CreateRole()
        {
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);
            try
            {

                string sql = "INSERT INTO roles (name, description) VALUES(@name, @description);";
                SqliteCommand cmd = new SqliteCommand();
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

        [HttpPost]
        public JsonResult DeleteSelectedRole()
        {
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);
            cn.Open();
            using var txn = cn.BeginTransaction();
            try
            {

                foreach (ListItem item in ListBoxRoleList.Items)
                {
                    if (item.Selected)
                    {

                        string sql = "DELETE FROM roles WHERE name = @name;";
                        SqliteCommand cmd = new SqliteCommand();
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

        [HttpPost]
        public JsonResult RoleSelected()
        {

            foreach (ListItem item in ListBoxRoleTags.Items)
            {
                item.Selected = false;
            }

            string sql2 = "SELECT tag FROM roleaccess WHERE role = @role;";
            SqliteCommand cmd2 = new SqliteCommand();
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);
            cmd2.Connection = cn;
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

        [HttpPost]
        public JsonResult SaveRoleTags()
        {
            using var cn = new SqliteConnection(Code.DAL.ConnectionString);
            cn.Open();
            SqliteTransaction txn = cn.BeginTransaction();
            try
            {

                string sql = "DELETE FROM roleacces WHERE role = @role;";
                SqliteCommand cmd = new SqliteCommand();
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
                        SqliteCommand cmd2 = new SqliteCommand();
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

        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(IsValidRequestAttribute), Arguments = new object[] { @"Admin/User Management" })]
        public ActionResult RoleUsers()
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
                SqliteCommand cmd = new SqliteCommand();
                using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
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
            return View();
        }

        private void RefreshUsers()
        {


            try
            {


                string sql = "SELECT Email, FirstName, LastName, RoleID FROM users WHERE RoleID != 'Anonymous' AND RoleId != 'Admin'";
                SqliteCommand cmd = new SqliteCommand();
                using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
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

        [HttpPost]
        public JsonResult ChangeUserRole()
        {
            try
            {

                if (DropDownListRoles.Text.Trim() == "")
                {
                    LabelError.Text = "No role selected.  Must select a role before adding changing user role.";
                    return;
                }

                using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cn.Open();
                SqliteTransaction txn = cn.BeginTransaction();
                try
                {

                    foreach (ListItem item in ListBoxUserList.Items)
                    {
                        if (item.Selected)
                        {

                            string sql = "UPDATE users SET RoleID = @roleid WHERE email = @email;";
                            SqliteCommand cmd = new SqliteCommand();
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
