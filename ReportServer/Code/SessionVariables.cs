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


        public static string LoggedEmail
        {
            get
            {
                try
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context.Session["LoggedEmail"] == null)
                    {
                        return "Anonymous";
                    }
                    return context.Session["LoggedEmail"].ToString();
                }
                catch
                {
                    return "Anonymous";
                }
            }
            set
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Session["LoggedEmail"] = value;
            }
        }

        public static string LoggedFirstName
        {
            get
            {
                try
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context.Session["LoggedFirstName"] == null)
                    {
                        return "Anonymous";
                    }
                    return context.Session["LoggedFirstName"].ToString();
                }
                catch
                {
                    return "Anonymous";
                }
            }
            set
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Session["LoggedFirstName"] = value;
            }
        }

        public static string LoggedLastName
        {
            get
            {
                try
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context.Session["LoggedLastName"] == null)
                    {
                        return "Anonymous";
                    }
                    return context.Session["LoggedLastName"].ToString();
                }
                catch
                {
                    return "Anonymous";
                }
            }
            set
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Session["LoggedLastName"] = value;
            }
        }

        public static string LoggedRoleId
        {
            get
            {
                try
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context.Session["LoggedRoleId"] == null)
                    {
                        return "Anonymous";
                    }
                    return context.Session["LoggedRoleId"].ToString();
                }
                catch
                {
                    return "Anonymous";
                }
            }
            set
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Session["LoggedRoleId"] = value;
            }
        }

    }
}