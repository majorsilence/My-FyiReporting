using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ReportServer
{
    /// <summary>
    /// Summary description for AdminReportUploadHandler
    /// </summary>
    public class AdminReportUploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
           // Security.IsValidateRequest(context.Response, context.Session);


            context.Response.ContentType = "application/json";
            
            try
            {
                string path = SessionVariables.ReportDirectory;

                Stream inputStream;
                String filename;

                if (context.Request.Browser.Type.StartsWith("IE"))
                {
                    inputStream = context.Request.Files[0].InputStream;
                    filename = context.Request.Files[0].FileName;
                }
                else
                {
                    filename  = HttpContext.Current.Request.Headers["X-File-Name"];
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