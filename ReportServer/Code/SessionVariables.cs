using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportServer
{
    public class SessionVariables
    {
        
        public static bool LoggedIn
        {
            get 
            {
                try
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context.Session["UserLoggedIn"] == null)
                    {
                        return false;
                    }
                    return (bool)context.Session["UserLoggedIn"];
                }
                catch
                {
                    return false;
                }
            }
            set 
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Session["UserLoggedIn"] = value;
            }
        }

    }
}