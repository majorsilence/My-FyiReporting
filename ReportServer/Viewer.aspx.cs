using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Data;
using System.Text;


namespace ReportServer
{
    public partial class Viewer : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string Name = Request.QueryString["rs:Name"];
            bool FirstRun = false;
            try
            {
                if (Request.QueryString["rs:FirstRun"] != null)
                {
                    FirstRun = bool.Parse(Request.QueryString["rs:FirstRun"]);
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception
            }
            string url = Request.QueryString["rs:url"];

            ReportSession ses = new ReportSession();
            ses.Name = Name;
            ses.FirstRun = FirstRun;
            ses.url = url;

            Session["CurrentPdfReport"] = ses;

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<a href=\"ShowReport.aspx?rs:url={0}&rs:Format=xml\" target=_self>XML</a> | ",  url));
             sb.Append(string.Format("<a href=\"ShowReport.aspx?rs:url={0}&rs:Format=csv\" target=_self>CSV</a> | ",  url));
            sb.Append(string.Format("<a href=\"ShowReport.aspx?rs:url={0}&rs:Format=html\" target=_self>HTML</a>",  url));

            LiteralOtherLinks.Text =sb.ToString();


        }

       
    }
}