using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace fyiReporting.ReportServerMvc.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Setup()
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

            return View();
        }

        [HttpPost]
        public ActionResult SetupSubmit()
        {
            SqliteConnection cn = new SqliteConnection(Code.DAL.ConnectionString);

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

                string sql = "INSERT INTO users (Email, FirstName, LastName, RoleId, Password) VALUES(@email, @firstname, @lastname, 'Admin', @password)";
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", DbType.String).Value = TextBoxEmail.Text;
                cmd.Parameters.Add("@firstname", DbType.String).Value = TextBoxFirstName.Text;
                cmd.Parameters.Add("@lastname", DbType.String).Value = TextBoxLastName.Text;
                cmd.Parameters.Add("@password", DbType.String).Value = password;

                cn.Open();
                cmd.ExecuteNonQuery();

                this.Response.Redirect("Reports.aspx", true);
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

        public ActionResult Settings()
        {

            try
            {


                string sql = "SELECT role, tag FROM roleaccess WHERE role = @roleid and (tag = 'Admin/Role Management' or tag = 'Admin/Report Upload' or tag = 'Admin/User Management')";
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = new SqliteConnection(Code.DAL.ConnectionString);
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

            return View();
        }


        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
