using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using fyiReporting.RdlAsp;

namespace ReportServer
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _Report.RenderType = "html";
            _Report.PassPhrase = "northwind";
            // ReportFile must be the last item set since it triggers the building of the report
            string report_url = Request.QueryString["rs:url"];
            if (report_url != null)
                _Report.ReportFile = report_url;

        }


        private RdlReport _Report = new RdlReport();

        string Meta
        {
            get
            {
                if (_Report.ReportFile == "statistics")
                    return "<meta http-equiv=\"Refresh\" contents=\"10\"/>";
                else
                    return "";
            }
        }

    }
}
