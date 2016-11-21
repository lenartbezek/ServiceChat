using System;
using System.Web;
using System.Web.Routing;

namespace ChatDB
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.MapPageRoute("Login",
                "login",
                "~/Login.aspx");
            routes.MapPageRoute("Default",
                "",
                "~/Chat.aspx");
        }
    }
}