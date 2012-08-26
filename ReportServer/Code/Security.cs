using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportServer
{
    public class Security
    {


        public static bool IsValidateRequest(System.Web.HttpResponse Response, System.Web.SessionState.HttpSessionState Session)
        {

            if (SessionVariables.LoggedIn == false)
            {
                Response.Redirect("Login.aspx", true);
            }

            return false;
        }

    }
}