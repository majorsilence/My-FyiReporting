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
using System.Text;
using System.Web.Caching;
using Majorsilence.Reporting.Rdl;
using Microsoft.Extensions.Caching.Memory;

namespace Majorsilence.Reporting.RdlAsp
{
    internal class ReportHelper
    {
        static internal bool DoCaching = true;
        internal int CacheHits;
        internal int CacheMisses;

        private ReportHelper()
        {
            CacheHits = 0;
            CacheMisses = 0;
        }

        static internal ReportHelper Get(IMemoryCache app)
        {
            ReportHelper s = app.Get("fyistats") as ReportHelper;
            if (s == null)
            {
                s = new ReportHelper();
                app.Set("fyistats", s);
            }
            return s;
        }
        static internal void IncrHits(IMemoryCache app)
        {
            ReportHelper s = Get(app);
            lock (s)
            {
                s.CacheHits++;
            }
        }
        static internal void IncrMisses(IMemoryCache app)
        {
            ReportHelper s = Get(app);
            lock (s)
            {
                s.CacheMisses++;
            }
        }

        static internal Report GetCachedReport(string file, IMemoryCache c)
        {
            if (!ReportHelper.DoCaching)			// caching is disabled
            {
                ReportHelper.IncrMisses(c);
                return null;
            }

        //    Cache c = this.Context.Cache;
            ReportDefn rd = c.Get(file) as ReportDefn;
            if (rd == null)
            {
                ReportHelper.IncrMisses(c);
                return null;
            }
            ReportHelper.IncrHits(c);
            Report r = new Report(rd);

            return r;
        }

        static internal void SaveCachedReport(Report r, string file, IMemoryCache c)
        {
            if (!ReportHelper.DoCaching)			// caching is disabled
                return;

            c.Set(file, r.ReportDefinition);
            return;
        }
        
        static internal ListDictionary GetParameters(string parms)
        {
            ListDictionary ld = new ListDictionary();
            if (parms == null)
                return ld;				// dictionary will be empty in this case

            // parms are separated by &
            char[] breakChars = new char[] { '&' };
            string[] ps = parms.Split(breakChars);
            foreach (string p in ps)
            {
                int iEq = p.IndexOf("=");
                if (iEq > 0)
                {
                    string name = p.Substring(0, iEq);
                    string val = p.Substring(iEq + 1);
                    ld.Add(name, val);
                }
            }
            return ld;
        }

        static internal string GetSource(string file)
        {
            StreamReader fs = null;
            string prog = null;
            try
            {
                fs = new StreamReader(file);
                prog = fs.ReadToEnd();
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return prog;
        }

        static internal void SetUserContext(Report r, HttpContext context, NeedPassword np)
        {
            r.GetDataSourceReferencePassword = np;
            if (context == null)                // may not always have a context
                return;

            HttpRequest req = context.Request;
            if (req != null && req.UserLanguages!= null && req.UserLanguages.Length > 0)
            {
                string l = req.UserLanguages[0];
                r.ClientLanguage = l;
            }

            if (context.User != null && context.User.Identity != null)
            {
                System.Security.Principal.IIdentity id = context.User.Identity;
                if (id.IsAuthenticated)
                {
                    r.UserID = id.Name;
                }
            }
            return;
        }
        /// <summary>
        /// Returns the HTML needed to represent the parameters of a report.
        /// </summary>
        static internal string GetParameterHtml(Report rpt, IDictionary pd, HttpContext context, string reportFile, bool bShow)
        {
            if (rpt == null)
                return "";

            StringBuilder pHtml = new StringBuilder();

            pHtml.AppendFormat("<form target=\"_self\" method=get action=\"{0}\">", context.Request.Url);
            // hidden field
            pHtml.AppendFormat("<input type=hidden name=\"rs:url\" value=\"{0}\" />",
                reportFile);
            pHtml.AppendFormat("<table width=\"100%\">");

            int row = 0;
            foreach (UserReportParameter rp in rpt.UserReportParameters)
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
                    for (int i = 0; i < rp.DefaultValue.Length; i++)
                    {
                        if (i > 0)
                            sb.Append(", ");
                        sb.Append(rp.DefaultValue[i].ToString());
                    }
                    defaultValue = sb.ToString();
                }
                else
                    defaultValue = "";

                if (rp.DisplayValues == null)
                {
                    string pv = (string)pd[rp.Name];

                    switch (rp.dt)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int16:
                        case TypeCode.Int64:
                        case TypeCode.UInt32:
                        case TypeCode.UInt16:
                        case TypeCode.UInt64:
                            pHtml.AppendFormat("<input type=text name=\"{0}\" value=\"{1}\" tabindex=\"{2}\" size=32 onKeyPress=\"javascript:return limitinput(event, '0123456789', true);\"/></td>",
                            rp.Name,		// name
                            pv == null ? defaultValue : pv,		// provide actual value if passed as parm otherwise default
                            row + 1);    //use the row to set the tab order
                            break;
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.Single:
                            pHtml.AppendFormat("<input type=text name=\"{0}\" value=\"{1}\" tabindex=\"{2}\" size=32 onKeyPress=\"javascript:return limitinput(event, '0123456789.', true);\"/></td>",
                        rp.Name,		// name
                        pv == null ? defaultValue : pv,		// provide actual value if passed as parm otherwise default
                        row + 1);    //use the row to set the tab order
                            break;
                        default:
                            pHtml.AppendFormat("<input type=text name=\"{0}\" value=\"{1}\" tabindex=\"{2}\" size=32/></td>",
                            rp.Name,		// name
                            pv == null ? defaultValue : pv,		// provide actual value if passed as parm otherwise default
                            row + 1);    //use the row to set the tab order
                            break;
                    }

                    /* pHtml.AppendFormat("<input type=text name=\"{0}\" value=\"{1}\" tabindex=\"{2}\" size=32/></td>", 
                         rp.Name,		// name
                         pv == null? defaultValue: pv,		// provide actual value if passed as parm otherwise default
                         row + 1);    //use the row to set the tab order*/

                    if (rp.dt == TypeCode.DateTime)
                    {
                        //pHtml.AppendFormat("<td><input id=\"{0}\" type=\"button\" value=\"Date\" />", rp.Name + "Dtbtn /><td>");
                        pHtml.AppendFormat("<td><A id=\"lnk" + rp.Name + "\" href=\"javascript:NewCal('{0}','ddmmmyyyy',true,24)\" runat=\"server\"><IMG height=\"16\" alt=\"Pick a date\" src=\"SmallCalendar.gif\" width=\"16\" border=\"0\"/></td>", rp.Name);
                    }

                }
                else
                {
                    pHtml.AppendFormat("<select name=\"{0}\" tabindex=\"{1}\"size=1>", rp.Name, row + 1);
                    string pv = (string)pd[rp.Name];
                    if (pv == null)
                        pv = defaultValue;
                    string selected;
                    for (int i = 0; i < rp.DisplayValues.Length; i++)
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



                if (row == 0)
                {	// On the first row add a column that will be the submit button
                    pHtml.AppendFormat("<td rowspan=2><table><tr><td align=right><input type=\"submit\" value=\"Run Report\" style=\"font-family:Ariel; color: black; font-size=10pt; background: gainsboro;\" tabindex=\"{0}\"/></td></tr>", rpt.UserReportParameters.Count + 1);
                    if (!bShow) { ReportHelper.AppendExports(pHtml, context, reportFile); } else { pHtml.AppendFormat("<tr><td align=right></td><td align=right>&nbsp;</td></tr></table></td>"); }
                }

                pHtml.Append("</tr>");		// End of row for each parameter
                row++;
            }
            if (row == 0)
            {
                pHtml.AppendFormat("<tr><td><table><tr><td align=right><input type=\"submit\" value=\"Run Report\" style=\"font-family:Ariel; color: white; font-size=10pt; background: lightblue;\" tabindex=\"{0}\"/></td></tr>", rpt.UserReportParameters.Count + 1);
                if (!bShow) { ReportHelper.AppendExports(pHtml, context, reportFile); } else { pHtml.AppendFormat("<tr><td align=right></td><td align=right>&nbsp;</td></tr></table></td>"); }
            }
            pHtml.Append("</table></form>");		// End of table, form, html
            return pHtml.ToString();
        }

        static private void AppendExports(StringBuilder pHtml, HttpContext context, string reportFile)
        {
            StringBuilder args = new StringBuilder();
            NameValueCollection nvc;

            nvc = context.Request.QueryString;		// parameters
            foreach (var key in nvc.AllKeys)
            {
                if (!key.StartsWith("rs:"))
                    args.AppendFormat("&{0}={1}",
                        System.Web.HttpUtility.UrlEncode(key), System.Web.HttpUtility.UrlEncode(nvc[key]));
            }
            string sargs = args.ToString();

            string lpdf =
                $"<a href=\"/msr/RdlReport/ShowFile/{reportFile}/pdf{sargs}\" target=_blank>PDF</a>";
            string lxml =
                $"<a href=\"/msr/RdlReport/ShowFile/{reportFile}/xml{sargs}\" target=_blank>XML</a>";
            string lcsv =
                $"<a href=\"/msr/RdlReport/ShowFile/{reportFile}/csv{sargs}\" target=_blank>CSV</a>";

            pHtml.AppendFormat("<tr><td align=right>{0}</td><td align=right>{1}&nbsp;{2}</td></tr></table></td>",
                lpdf, lxml, lcsv);
        }

    }
}
