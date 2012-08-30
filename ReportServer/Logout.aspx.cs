using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportServer
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SessionVariables.LoggedIn = false;
            SessionVariables.LoggedEmail = "Anonymous";
            SessionVariables.LoggedFirstName = "Anonymous";
            SessionVariables.LoggedLastName = "Anonymous";
            SessionVariables.LoggedRoleId = "Anonymous";
          
        }
    }
}