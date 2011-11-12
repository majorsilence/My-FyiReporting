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
using System.Collections;
using System.Text;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesktop
{
	/// <summary>
	/// Summary description for ParameterHtml.
	/// </summary>
	internal class ReportRender
	{
		private Report _Report;
		private string _ParameterHtml;
		private string _ActionUrl;

		internal ReportRender(Report r)
		{
			_Report = r;
		}

		internal string ActionUrl
		{
			get {return _ActionUrl;}
			set 
			{
				_ActionUrl = value;
				_ParameterHtml = null;		// this is now invalid
			}
		}

		internal string MainHtml(Report report, string parmUrl, string renderUrl)
		{
			StringBuilder sb = new StringBuilder();
			if (report.Description == null)
				sb.Append("<html>");
			else
				sb.AppendFormat("<html><head><title>{0}</title></head>", XmlUtil.XmlAnsi(report.Description));
			sb.Append("<frameset rows=\"20%,*\" bordercolor=\"#CCCCCC\">");
			sb.AppendFormat("<frame src=\"{0}\" name=\"parms\" scrolling=\"Auto\" marginwidth=\"0\" marginheight=\"0\"/>", parmUrl);
			sb.AppendFormat("<frame src=\"{0}\" name=\"report\" scrolling=\"Auto\" marginwidth=\"0\" marginheight=\"0\"/>", renderUrl);
			sb.Append("</frameset></html>");
			return sb.ToString();
		}

		/// <summary>
		/// Returns the HTML needed to represent the parameters of a report.
		/// </summary>
		internal string ParameterHtml(IDictionary pd)
		{
			if (_ParameterHtml != null)
				return _ParameterHtml;

			StringBuilder pHtml = new StringBuilder();
			pHtml.AppendFormat("<html><form target=\"_top\" method=get action=\"{0}\"><table width=\"100%\">", _ActionUrl);
			int row=0;

			// Determine the selection of the render control
			string render = (string) pd["rs:Format"];
			if (render == null)
				render = "html";
			string rHTML="";
			string rPDF="";
			string rXML="";
			switch (render.ToLower())
			{
				case "xml":
					rXML=" selected";
					break;
				case "pdf":
					rPDF=" selected";
					break;
				case "html":
				default:
					rHTML=" selected";
					break;
			}

			foreach (UserReportParameter rp in _Report.UserReportParameters)
			{
				if (rp.Prompt == null)		// skip parameters that don't have a prompt
					continue;

				pHtml.Append("<tr>");		// Create a row for each parameter
				
				// Create the label
				pHtml.Append("<td>");		// Label definition
				pHtml.Append(rp.Prompt);

				pHtml.Append("</td><td>");	// end of label; start of control

				// Create the control
				string defaultValue;
				if (rp.DefaultValue != null)
				{
					StringBuilder sb = new StringBuilder();
					for (int i=0; i < rp.DefaultValue.Length; i++)
					{
						if (i > 0)
							sb.Append(", ");
						sb.Append(rp.DefaultValue[i].ToString());
					}
					defaultValue = sb.ToString();
				}
				else
					defaultValue="";

				if (rp.DisplayValues == null)
				{
					string pv = (string) pd[rp.Name];

					pHtml.AppendFormat("<input type=text name=\"{0}\" value=\"{1}\" size=15/></td>", 
						rp.Name,		// name
						pv == null? defaultValue: pv);		// provide actual value if passed as parm otherwise default
				}
				else
				{
					pHtml.AppendFormat("<select name=\"{0}\" size=1>", rp.Name);
					string pv = (string) pd[rp.Name];
					if (pv == null)
						pv = defaultValue;
					string selected;
					for (int i=0; i < rp.DisplayValues.Length; i++)
					{
						if (pv.CompareTo(rp.DataValues[i].ToString()) == 0)
							selected = " selected";
						else
							selected = "";

						if (rp.DataValues[i] is String &&
							rp.DisplayValues[i].CompareTo(rp.DataValues[i]) == 0)	// When display and data values are same don't put out a value tag
							pHtml.AppendFormat("<option{1}>{0}</option>", XmlUtil.XmlAnsi(rp.DisplayValues[i]), selected);	
						else
							pHtml.AppendFormat("<option value=\"{0}\"{2}>{1}</option>", XmlUtil.XmlAnsi(rp.DataValues[i].ToString()), XmlUtil.XmlAnsi(rp.DisplayValues[i]), selected);
					}
					pHtml.Append("</select></td>");
				}
				
				if (row==0)
				{	// On the first row add a column that will be the submit button
					pHtml.Append("<td rowspan=2><table><tr><td align=right><input type=\"submit\" value=\"Run Report\" style=\"font-family:Ariel; color: black; font-size=10pt; background: gainsboro;\"/></td></tr>");
					pHtml.AppendFormat("<tr><td align=right><select name=\"rs:Format\" size=1><option{0}>HTML</option><option{1}>PDF</option><option{2}>XML</option></select></td></tr></table></td>", rHTML, rPDF, rXML);
				}

				pHtml.Append("</tr>");		// End of row for each parameter
				row++;
			}
			if (row==0)
			{
				pHtml.Append("<tr><td><table><tr><td align=right><input type=\"submit\" value=\"Run Report\" style=\"font-family:Ariel; color: white; font-size=10pt; background: lightblue;\"/></td></tr>");
				pHtml.AppendFormat("<tr><td align=right><select name=\"rs:Format\" size=1><option{0}>HTML</option><option{1}>PDF</option><option{2}>XML</option></select></td></tr></table></td></tr>", rHTML, rPDF, rXML);
			}
			pHtml.Append("</table></form></html>");		// End of table, form, html
			_ParameterHtml = pHtml.ToString();
			return _ParameterHtml;
		}
	}
}
