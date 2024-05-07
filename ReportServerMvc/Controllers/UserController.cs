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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignupSubmit([FromBody] Models.Signup model)
        {
            await using var cn = new SqliteConnection(Code.DAL.ConnectionString);

            try
            {
                if (!string.Equals(model.Password, model.ConfirmPassword))
                {
                    return BadRequest("Password and confirm password do not match.");
                }
                else if (model.Password.Length < 1)
                {
                    return BadRequest("Password must be at least one character in length.");
                }

                // FIXME: salt and hash password
                string password = Code.Hashes.GetSHA512StringHash(model.Password);

                string sql = "INSERT INTO users (Email, FirstName, LastName, RoleId, Password) VALUES(@email, @firstname, @lastname, 'Users', @password)";
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

                this.Response.Redirect("~/User/Login", true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginSubmit([FromBody] Models.Login model)
        {
            try
            {

                string sql = "SELECT Email, FirstName, LastName, RoleId FROM users WHERE Email = @email AND Password = @password;";

                SqliteCommand cmd = new SqliteCommand();
                await using var cn = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@email", SqliteType.Text).Value = model.Email;
                cmd.Parameters.Add("@password", SqliteType.Text).Value = Code.Hashes.GetSHA512StringHash(model.Password);

                DataTable dt = await Code.DAL.ExecuteCmdTableAsync(cmd);

                if (dt.Rows.Count == 1)
                {
                    SessionVariables.LoggedIn = true;
                    SessionVariables.LoggedEmail = dt.Rows[0]["Email"].ToString();
                    SessionVariables.LoggedFirstName = dt.Rows[0]["FirstName"].ToString();
                    SessionVariables.LoggedLastName = dt.Rows[0]["LastName"].ToString();
                    SessionVariables.LoggedRoleId = dt.Rows[0]["RoleId"].ToString();
                    this.Response.Redirect("~/Reports", false);
                }
                else
                {
                    SessionVariables.LoggedIn = false;
                    SessionVariables.LoggedEmail = "Anonymous";
                    SessionVariables.LoggedFirstName = "Anonymous";
                    SessionVariables.LoggedLastName = "Anonymous";
                    SessionVariables.LoggedRoleId = "Anonymous";
                    return BadRequest("Unknown user name and/or password!");
                }

            }
            catch (System.Threading.ThreadAbortException tae)
            { }
            catch (Exception ex)
            {
                SessionVariables.LoggedIn = false;
                return BadRequest(ex.Message);
            }

            return Ok();
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordSubmit([FromBody] Models.ChangePassword model)
        {
            if (model.NewPassword.Length < 1)
            {
                return BadRequest("New password must be at least one character in length.");
            }

            if (!string.Equals(model.NewPassword, model.ConfirmNewPassword))
            {
                return BadRequest("New passwords do not match.");
            }

            try
            {
                string sql = "SELECT Email FROM users WHERE Password = @password AND Email = @email;";
                SqliteCommand cmd = new SqliteCommand();
                await using var cn2 = new SqliteConnection(Code.DAL.ConnectionString);
                cmd.Connection = cn2;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@password", SqliteType.Text).Value = Code.Hashes.GetSHA512StringHash(model.Password);
                cmd.Parameters.Add("@email", SqliteType.Text).Value = SessionVariables.LoggedEmail;

                DataTable dt = await Code.DAL.ExecuteCmdTableAsync(cmd);
                if (dt.Rows.Count != 1)
                {
                    return BadRequest("Invalid old password");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            await using SqliteConnection cn = new SqliteConnection(Code.DAL.ConnectionString);
            await cn.OpenAsync();
            await using SqliteTransaction txn = cn.BeginTransaction();
            try
            {

                string sql = "UPDATE users SET Password = @password WHERE Email = @email;";
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = cn;
                cmd.Transaction = txn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@password", SqliteType.Text).Value = Code.Hashes.GetSHA512StringHash(model.NewPassword);
                cmd.Parameters.Add("@email", SqliteType.Text).Value = SessionVariables.LoggedEmail;
                await cmd.ExecuteNonQueryAsync();

                await txn.CommitAsync();
            }
            catch (Exception ex)
            {
                txn.Rollback();
                return BadRequest(ex.Message);
            }

            return Ok("Password Changed");
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
