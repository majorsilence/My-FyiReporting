using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Data;


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
               FirstRun = bool.Parse(Request.QueryString["rs:FirstRun"]);
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

        }

       
    }
}