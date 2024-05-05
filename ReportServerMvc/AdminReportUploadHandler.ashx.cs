using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace ReportServer
{
    /// <summary>
    /// Summary description for AdminReportUploadHandler
    /// </summary>
    public class AdminReportUploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Security.IsValidateRequest(context.Response, context.Session, @"Admin/Report Upload");


            context.Response.ContentType = "application/json";
            
            try
            {
                string path = SessionVariables.ReportDirectory;

                Stream inputStream;
                String filename;

                if (context.Request.Browser.Type.StartsWith("IE"))
                {
                    inputStream =  context.Request.Files[0].InputStream;
                    filename = HttpUtility.UrlDecode( context.Request.Files[0].FileName);
                }
                else
                {
                    filename = HttpUtility.UrlDecode(HttpContext.Current.Request.Headers["X-File-Name"]);
                     inputStream = HttpContext.Current.Request.InputStream;
                }



                if (System.IO.File.Exists(System.IO.Path.Combine(path, filename)))
                {
                    context.Response.Write(string.Format("{\"error\":\"File {0} already exists\"}", filename)); // in case of error
                    return;
                }
                FileStream fileStream = new FileStream(System.IO.Path.Combine(path, filename), FileMode.OpenOrCreate);
                inputStream.CopyTo(fileStream);
                fileStream.Close();

                // TODO: Add insert into report table.

                SQLiteConnection cn = new SQLiteConnection(Code.DAL.ConnectionString);
                cn.Open();
                SQLiteTransaction txn;
                txn = cn.BeginTransaction();
                try
                {

                    string tag = @"Reports/" + filename;

                    string sql = "INSERT INTO roletags (tag, description) VALUES (@tag, @description);";
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = cn;
                    cmd.Transaction = txn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@tag", DbType.String).Value = tag;
                    cmd.Parameters.Add("@description", DbType.String).Value = filename + " report";
                    cmd.ExecuteNonQuery();

                    string sql2 = "INSERT INTO roleaccess (role, tag) VALUES (@role, @tag);";
                    SQLiteCommand cmd2 = new SQLiteCommand();
                    cmd2.Connection = cn;
                    cmd2.Transaction = txn;
                    cmd2.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandText = sql2;
                    cmd2.Parameters.Add("@role", DbType.String).Value = "Admin";
                    cmd2.Parameters.Add("@tag", DbType.String).Value = tag;
                    cmd2.ExecuteNonQuery();


                    string sql3 = "INSERT INTO reportfiles (reportname, tag) VALUES (@reportname, @tag);";
                    SQLiteCommand cmd3 = new SQLiteCommand();
                    cmd3.Connection = cn;
                    cmd3.Transaction = txn;
                    cmd3.CommandType = System.Data.CommandType.Text;
                    cmd3.CommandText = sql3;
                    cmd3.Parameters.Add("@reportname", DbType.String).Value = filename;
                    cmd3.Parameters.Add("@tag", DbType.String).Value = tag;
                    cmd3.ExecuteNonQuery();


                    txn.Commit();

                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    System.IO.File.Delete(System.IO.Path.Combine(path, filename));
                    context.Response.Write(string.Format("{\"error\":\"{0}\"}", ex.Message)); // in case of error
                    return;
                }
                finally
                {
                    if (cn.State != ConnectionState.Closed)
                    {
                        cn.Close();
                    }
                }


                context.Response.Write("{\"success\":true}"); // when upload was successful







            }
             catch (Exception ex)
            {
                context.Response.Write(string.Format("{\"error\":\"{0}\"}", ex.Message)); // in case of error
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}