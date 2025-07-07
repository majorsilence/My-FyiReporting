/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

   This file is part of the fyiReporting RDL project.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/

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
                        $"<tr><td><a href=\"RdlReport/render?reportFile={name}&type=html\" target={target}>{Path.GetFileNameWithoutExtension(fsi.Name)}</a></td></tr>");
                }
                else // handle directories
                    html.AppendLine($"<tr><td><a href=\"{name}\">{fsi.Name}<a></td><td></td><td></td></tr>");
            }

            html.AppendLine("</table>");

            return Content(html.ToString(), "text/html");
        }
    }
}