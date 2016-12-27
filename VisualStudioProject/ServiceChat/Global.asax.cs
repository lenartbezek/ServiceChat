using System;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ServiceChat
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

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Login",
                "login",
                "~/Login.aspx");
            routes.MapPageRoute("Default",
                "",
                "~/Chat.aspx");
            routes.MapPageRoute("Admin",
                "",
                "~/Admin.aspx");
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Service1.svc/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}