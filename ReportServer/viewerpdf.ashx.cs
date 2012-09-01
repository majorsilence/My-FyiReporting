using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fyiReporting.RdlAsp;

namespace ReportServer
{
    /// <summary>
    /// Summary description for viewerpdf
    /// </summary>
    public class viewerpdf : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {


        public RdlReport _Report { get; set; }

        //GJL 20080520 - Show report parameters without running it first (many line changes in this file)
        public bool error { get; set; }

       

        public void ProcessRequest(HttpContext context)
        {


            _Report = new RdlReport();

            ReportSession ses = (ReportSession)context.Session["CurrentPdfReport"];


            string Name = ses.Name;
            bool FirstRun = ses.FirstRun;

            if (Security.HasPermissions(ses.url) == false)
            {
                return;
            }

            if (FirstRun)
            {
                _Report.NoShow = true;
            }
            else
            {
                _Report.NoShow = false;
            }


            _Report.RenderType = "pdf";

            _Report.PassPhrase = "northwind";       // user should provide in some fashion (from web.config??)
            // ReportFile must be the last item set since it triggers the building of the report

            string arg = ses.url;
            if (arg != null)
            {
                _Report.ReportFile = arg;
            }

            if (_Report.Object == null)
            {
                error = true;
            }
            else
            {
                //context.Response.ContentType = "application/pdf";
     
                context.Response.AddHeader("content-disposition", "inline; filename=myFyiReportingReport.pdf");
                context.Response.BinaryWrite(_Report.Object);
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