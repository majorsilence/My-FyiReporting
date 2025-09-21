using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Caching;
using Majorsilence.Reporting.Rdl;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Majorsilence.Reporting.RdlAsp
{
    /// <summary>
    /// Summary description for RdlListReports.
    /// </summary>
    public class RdlListReports : Controller
    {
        /// <summary>
        /// RdlListReports generates a list of reports available on the server
        /// </summary>
        /// 
        public string Frame = null;
        
        private readonly Settings _settings;

        public RdlListReports(Settings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        [Route("msr/RdlList")]
        public ContentResult Index()
        {
            var html = new StringBuilder();

            DirectoryInfo di;
            FileSystemInfo[] afsi;
            try
            {
                di = new DirectoryInfo(_settings.ReportsFolder);
                afsi = di.GetFileSystemInfos();
            }
            catch (Exception ex)
            {
                html.AppendLine($"<p> No reports!  Exception={ex.Message}");
                return Content(html.ToString(), "text/html");
            }

            html.AppendLine("<table>");
            foreach (FileSystemInfo fsi in afsi)
            {
                if ((fsi.Attributes & FileAttributes.Directory) == 0 &&
                    fsi.Extension.ToLower() != ".rdl") // only show report files
                    continue;

                string name = fsi.Name.Replace(" ", "%20");

                if ((fsi.Attributes & FileAttributes.Directory) == 0) // handle files
                {
                    string target = this.Frame == null ? "_self" : Frame;
                    html.AppendLine(
                        $"<tr><td><a href=\"/msr/RdlReport/ShowFile/{name}/html\" target={target}>{Path.GetFileNameWithoutExtension(fsi.Name)}</a></td></tr>");
                }
                else // handle directories
                    html.AppendLine($"<tr><td><a href=\"{name}\">{fsi.Name}<a></td><td></td><td></td></tr>");
            }

            html.AppendLine("</table>");

            return Content(html.ToString(), "text/html");
        }
    }
}