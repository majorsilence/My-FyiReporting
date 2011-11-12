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
using System.Text;
using System.Web.Caching;
using fyiReporting.RDL;

namespace fyiReporting.RdlAsp
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class RdlReport : Control
	{
		/// <summary>
		/// RdlReport generates an HTML report from a RDL file.
		/// </summary>
		/// 
		private const string STATISTICS="statistics";
		private string _ReportFile=null;
		private ArrayList _Errors=null;
		private int _MaxSeverity=0;
		private string _CSS=null;
		private string _JavaScript=null;
		private string _Html=null;
		private string _Xml=null;
        private string _Csv = null;
        private byte[] _Object = null;
		private string _ParameterHtml=null;
		private OutputPresentationType _RenderType=OutputPresentationType.ASPHTML;
        private string _PassPhrase = null;
        private bool _NoShow;

		protected override void Render(HtmlTextWriter tw)
		{
			if (_ReportFile == null)
			{
				this.AddError(8, "ReportFile not specified.");
				return;
			}
			else if (_ReportFile == STATISTICS)
			{
				DoStatistics(tw);
				return; 
			}
			else if (_Html != null)
				tw.Write(_Html);
			else if (_Object != null)
			{
				// TODO -   shouldn't use control to write out object???
				throw new Exception("_Object needed in render");
			}
			else	// we never generated anything!
			{
				if (_Errors != null)
				{
					tw.RenderBeginTag(HtmlTextWriterTag.Table);
					tw.RenderBeginTag(HtmlTextWriterTag.Tr);
					tw.RenderBeginTag(HtmlTextWriterTag.Td);
					tw.Write("Errors");
					tw.RenderEndTag();
					tw.RenderEndTag();
					foreach(string e in _Errors)
					{
						tw.RenderBeginTag(HtmlTextWriterTag.Tr);
						tw.RenderBeginTag(HtmlTextWriterTag.Td);
						tw.Write(e);
						tw.RenderEndTag();
						tw.RenderEndTag();
					}
					tw.RenderEndTag();
				}
			}
 
			return;
		}
        /// <summary>
        /// When true report won't be shown but parameters (if any) will be
        /// </summary>
        public bool NoShow
        {
            get { return _NoShow; }
            set { _NoShow = value; }
        }
		public string RenderType
		{
			get 
			{
				switch (_RenderType)
				{
					case OutputPresentationType.ASPHTML:
					case OutputPresentationType.HTML:
						return "html";
					case OutputPresentationType.PDF:
						return "pdf";
					case OutputPresentationType.XML:
						return "xml";
                    case OutputPresentationType.CSV:
                        return "csv";
                    case OutputPresentationType.Excel:
                        return "xlsx";
                    case OutputPresentationType.RTF:
                        return "rtf";
                    default:
						return "html";
				}
			}
			set 
			{
				_RenderType = this.GetRenderType(value);
			}
		}

		public string ReportFile
		{
			get {return _ReportFile;}
			set 
			{
				_ReportFile = value;
				// Clear out old report information (if any)
				this._Errors = null;
				this._MaxSeverity = 0;
				_CSS=null;
				_JavaScript=null;
				_Html=null;
				_ParameterHtml=null;

                if (_ReportFile == STATISTICS)
                {
                    StringWriter sw=null;
                    HtmlTextWriter htw=null;
                    try
                    {
                        sw = new StringWriter();
                        htw = new HtmlTextWriter(sw);
                        DoStatistics(htw);
                        htw.Flush();
                        _Html = sw.ToString(); 
                    }
                    finally
                    {
                        if (htw != null)
                        {
                            htw.Close();
                            htw.Dispose();
                        }
                        if (sw != null)
                        {
                            sw.Close();
                            sw.Dispose();
                        }
                    }
                    return;
                }

				// Build the new report
				string pfile = this.MapPathSecure(_ReportFile);
				DoRender(pfile);
			}
		}

        public string PassPhrase
        {
            set { _PassPhrase = value; }
        }

        private string GetPassword()
        {
            return _PassPhrase;
        }


		public string Html
		{
			get {return _Html;}
		}

		public string Xml
		{
			get {return _Xml;}
		}

        public string CSV
        {
            get { return _Csv; }
        }

		public byte[] Object
		{
			get {return _Object;}
		}

		public ArrayList Errors
		{
			get {return _Errors;}
		}

		public int MaxErrorSeverity
		{
			get {return _MaxSeverity;}
		}

		public string CSS
		{
			get {return _CSS;}
		}

		public string JavaScript
		{
			get {return _JavaScript;}
		}

		public string ParameterHtml
		{
			get 
			{
				return _ParameterHtml;
			}
		}


		// Render the report files with the requested types
		private void DoRender(string file)
		{
           
			string source;
			Report report=null;
			NameValueCollection nvc;

			nvc = this.Context.Request.QueryString;		// parameters
			ListDictionary ld = new ListDictionary();
            try
            {
			    for (int i=0; i < nvc.Count; i++)
			    {
				    ld.Add(nvc.GetKey(i), nvc[i]);
			    }

 //               if (!_NoShow) { report = GetCachedReport(file); }
                report = ReportHelper.GetCachedReport(file, this.Context.Cache, this.Context.Application);

			    if (report == null) // couldn't obtain report definition from cache
			    {
				    // Obtain the source
				    source = ReportHelper.GetSource(file);
				    if (source == null)			
					    return;					// GetSource reported the error

				    // Compile the report
				    report = this.GetReport(source, file);
				    if (report == null)
					    return;

                    ReportHelper.SaveCachedReport(report, file, this.Context.Cache);
			    }
			    // Set the user context information: ID, language
                ReportHelper.SetUserContext(report, this.Context, new RDL.NeedPassword(GetPassword));

			    // Obtain the data if report is being generated
                if (!_NoShow)
                {
			        report.RunGetData(ld);
			        Generate(report);
                }
            }
            catch (Exception exe)
            {
                AddError(8, "Error: {0}", exe.Message);
            }

            if (_ParameterHtml == null)
                _ParameterHtml = ReportHelper.GetParameterHtml(report, ld, this.Context, _ReportFile, _NoShow);	// build the parameter html
            
		}

		private void AddError(int severity, string err, params object[] args)
		{
			if (_MaxSeverity < severity)
				_MaxSeverity = severity;

			string error = string.Format(err, args);
			if (_Errors == null)
				_Errors = new ArrayList();
			_Errors.Add(error);
		}

		private void AddError(int severity, IList errors)
		{
			if (_MaxSeverity < severity)
				_MaxSeverity = severity;
			if (_Errors == null)
			{	// if we don't have any we can just start with this list
				_Errors = new ArrayList(errors);
				return;
			}
			
			// Need to copy all items in the errors array
			foreach(string err in errors)
				_Errors.Add(err);

			return;
		}

		private void DoStatistics(HtmlTextWriter tw)
		{
			RdlSession rs = Context.Application[RdlSession.SessionStat] as RdlSession;
            ReportHelper s = ReportHelper.Get(this.Context.Application);
			Cache c = this.Context.Cache; 

			int sessions=0;
			if (rs != null)
				sessions = rs.Count;

			tw.Write("<p>{0} sessions", sessions);
			tw.Write("<p>{0} items are in the cache", c.Count);
			tw.Write("<p>{0} cache hits", s.CacheHits);
			tw.Write("<p>{0} cache misses", s.CacheMisses);

			foreach(DictionaryEntry de in c)
			{
				if (de.Value is ReportDefn)
					tw.Write("<p>file=" + de.Key.ToString());
				else
					tw.Write("<p>key=" + de.Key.ToString());
			}
		}

		private void Generate(Report report)
		{
			MemoryStreamGen sg=null;
			try
			{
				sg = new MemoryStreamGen("ShowFile.aspx?type=", null, this.RenderType);

				report.RunRender(sg, _RenderType, this.UniqueID);
				_CSS = "";
				_JavaScript = "";
				switch (_RenderType)
				{
					case OutputPresentationType.ASPHTML:
					case OutputPresentationType.HTML:
                        _CSS = report.CSS;//.Replace("position: relative;", "position: absolute;");
						_JavaScript = report.JavaScript;
						_Html = sg.GetText();
						break;
					case OutputPresentationType.XML:
						_Xml = sg.GetText();
						break;
                    case OutputPresentationType.CSV:
                        _Csv = sg.GetText();
                        break;
                    case OutputPresentationType.PDF:
					{
						MemoryStream ms = sg.MemoryList[0] as MemoryStream;
						_Object = ms.ToArray();
						break;
					}
				}

				// Now save off the other streams in the session context for later use
				IList strms = sg.MemoryList;
				IList names = sg.MemoryNames;
				for (int i=1; i < sg.MemoryList.Count; i++)	// we skip the first one
				{
					string n = names[i] as string;
					MemoryStream ms = strms[i] as MemoryStream;
					Context.Session[n] = ms.ToArray();
				}

			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (sg != null)
				{
					sg.CloseMainStream();
				}
			}

			if (report.ErrorMaxSeverity > 0) 
			{
				AddError(report.ErrorMaxSeverity, report.ErrorItems);
				report.ErrorReset();
			}

			return;
		}

		private OutputPresentationType GetRenderType(string type)
		{
			switch (type.ToLower())
			{
                case "htm":
				case "html":
					return OutputPresentationType.ASPHTML;
				case "pdf":
					return OutputPresentationType.PDF;
				case "xml":
					return OutputPresentationType.XML;
                case "csv":
                    return OutputPresentationType.CSV;
                case "xlsx":
                    return OutputPresentationType.Excel;
                case "rtf":
                    return OutputPresentationType.RTF;
                default:
					return OutputPresentationType.ASPHTML;
			}
		}


		private Report GetReport(string prog, string file)
		{
			// Now parse the file
			RDLParser rdlp;
			Report r;
			try
			{
                // Make sure RdlEngine is configed before we ever parse a program
                //   The config file must exist in the Bin directory.
                string searchDir = this.MapPathSecure(this.ReportFile.StartsWith("~") ? "~/Bin" : "/Bin") + Path.DirectorySeparatorChar;
                RdlEngineConfig.RdlEngineConfigInit(searchDir);

				rdlp =  new RDLParser(prog);
				string folder = Path.GetDirectoryName(file);
				if (folder == "")
					folder = Environment.CurrentDirectory;
				rdlp.Folder = folder;
				rdlp.DataSourceReferencePassword = new NeedPassword(this.GetPassword);

				r = rdlp.Parse();
				if (r.ErrorMaxSeverity > 0)
				{
					AddError(r.ErrorMaxSeverity, r.ErrorItems);
					if (r.ErrorMaxSeverity >= 8)
						r = null;
					r.ErrorReset();
				}

				// If we've loaded the report; we should tell it where it got loaded from
				if (r != null)
				{
					r.Folder = folder;
					r.Name = Path.GetFileNameWithoutExtension(file);
					r.GetDataSourceReferencePassword = new RDL.NeedPassword(GetPassword);
				}
			}
			catch(Exception e)
			{
				r = null;
				AddError(8, "Exception parsing report {0}.  {1}", file, e.Message);
			}
			return r;
		}
	}

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

        static internal ReportHelper Get(HttpApplicationState app)
        {
            ReportHelper s = app["fyistats"] as ReportHelper;
            if (s == null)
            {
                s = new ReportHelper();
                app["fyistats"] = s;
            }
            return s;
        }
        static internal void IncrHits(HttpApplicationState app)
        {
            ReportHelper s = Get(app);
            lock (s)
            {
                s.CacheHits++;
            }
        }
        static internal void IncrMisses(HttpApplicationState app)
        {
            ReportHelper s = Get(app);
            lock (s)
            {
                s.CacheMisses++;
            }
        }

        static internal Report GetCachedReport(string file, Cache c, HttpApplicationState app)
        {
            if (!ReportHelper.DoCaching)			// caching is disabled
            {
                ReportHelper.IncrMisses(app);
                return null;
            }

        //    Cache c = this.Context.Cache;
            ReportDefn rd = c[file] as ReportDefn;
            if (rd == null)
            {
                ReportHelper.IncrMisses(app);
                return null;
            }
            ReportHelper.IncrHits(app);
            Report r = new Report(rd);

            return r;
        }

        static internal void SaveCachedReport(Report r, string file, Cache c)
        {
            if (!ReportHelper.DoCaching)			// caching is disabled
                return;

            c.Insert(file, r.ReportDefinition, new CacheDependency(file));
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
            for (int i = 0; i < nvc.Count; i++)
            {
                string key = nvc.GetKey(i);
                if (!key.StartsWith("rs:"))
                    args.AppendFormat("&{0}={1}",
                        System.Web.HttpUtility.UrlEncode(key), System.Web.HttpUtility.UrlEncode(nvc[i]));
            }
            string sargs = args.ToString();

            string lpdf =
                string.Format("<a href=\"ShowReport.aspx?rs:url={0}&rs:Format=pdf{1}\" target=_blank>PDF</a>",
                reportFile, sargs);
            string lxml =
                string.Format("<a href=\"ShowReport.aspx?rs:url={0}&rs:Format=xml{1}\" target=_blank>XML</a>",
                reportFile, sargs);
            string lcsv =
                string.Format("<a href=\"ShowReport.aspx?rs:url={0}&rs:Format=csv{1}\" target=_blank>CSV</a>",
                reportFile, sargs);

            pHtml.AppendFormat("<tr><td align=right>{0}</td><td align=right>{1}&nbsp;{2}</td></tr></table></td>",
                lpdf, lxml, lcsv);
        }

    }
}
