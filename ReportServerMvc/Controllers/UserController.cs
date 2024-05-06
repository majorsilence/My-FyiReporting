using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;

namespace fyiReporting.ReportServerMvc.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignupSubmit()
        {

            using var cn = new SqliteConnection(Code.DAL.ConnectionString);

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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit()
        {
            try
            {


                string sql = "SELECT Email, FirstName, LastName, RoleId FROM users WHERE Email = @email AND Password = @password;";

                SqliteCommand cmd = new SqliteCommand();
                using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
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

        public ActionResult Logout()
        {
            SessionVariables.LoggedIn = false;
            SessionVariables.LoggedEmail = "Anonymous";
            SessionVariables.LoggedFirstName = "Anonymous";
            SessionVariables.LoggedLastName = "Anonymous";
            SessionVariables.LoggedRoleId = "Anonymous";
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePasswordSubmit()
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
                SqliteCommand cmd = new SqliteCommand();
                using var cn2 = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn2;
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


            using SqliteConnection cn = new SqliteConnection(Code.DAL.ConnectionString);
            cn.Open();
            using SqliteTransaction txn = cn.BeginTransaction();
            try
            {

                string sql = "UPDATE users SET Password = @password WHERE Email = @email;";
                SqliteCommand cmd = new SqliteCommand();
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

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
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

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
