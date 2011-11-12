/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

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
using System.Web.UI;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Caching;
using fyiReporting.RDL;

namespace fyiReporting.RdlAsp
{
	/// <summary>
	/// Summary description for RdlListReports.
	/// </summary>
	public class RdlListReports : Control
	{
		/// <summary>
		/// RdlListReports generates a list of reports available on the server
		/// </summary>
		/// 
		public string Frame=null;
		public string RunPage="ShowReport.aspx";
        public bool SilverlightViewer = false;

		protected override void Render(HtmlTextWriter tw)
		{
			string pfile = this.MapPathSecure("Reports\\");

			DirectoryInfo di;
			FileSystemInfo[] afsi;
			try 
			{
				di = new DirectoryInfo(pfile);
				afsi = di.GetFileSystemInfos();
			}
			catch(Exception ex)
			{
				tw.Write("<p> No reports!  Exception={0}", ex.Message);
				return;
			} 

			tw.Write("<table>");
			foreach (FileSystemInfo fsi in afsi)
			{
				if ((fsi.Attributes & FileAttributes.Directory) == 0 &&
					fsi.Extension.ToLower() != ".rdl")	// only show report files
					continue;

				string name = fsi.Name.Replace(" ", "%20");

				if ((fsi.Attributes & FileAttributes.Directory) == 0)	// handle files
				{
                    if (SilverlightViewer)
                    {
                        tw.Write("<tr><td><a href=\"\" onclick=\"return false;\"><span onclick=\"setReport('{0}');\" >{1}</span></a></td></tr>",
                            name, Path.GetFileNameWithoutExtension(fsi.Name));
                    }
                    else
                    {
                        tw.Write("<tr><td><a href=\"{3}?rs:url=Reports\\{0}\" target={2}>{1}</a></td></tr>",
                            name, Path.GetFileNameWithoutExtension(fsi.Name),
                            this.Frame == null ? "_self" : Frame, RunPage);
                    }
				}
				else		// handle directories
					tw.Write("<tr><td><a href=\"{1}\">{0}<a></td><td></td><td></td></tr>",
						fsi.Name, name);
			}
			tw.Write("</table>");

			return;
		}
	}
}
