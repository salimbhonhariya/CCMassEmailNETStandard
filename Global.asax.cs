using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CCMassEmailNETStandard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_Error()
        //{
        //    HttpContext con = HttpContext.Current;
        //    var v = Server.GetLastError();
        //    var httpEx = v as HttpException;
        //    if (httpEx != null && httpEx.GetHttpCode() == 404)
        //    {
        //        // code for error logs
        //        Server.Transfer("~/Error/NotFound");
        //    }
        //    else
        //    {
        //        Server.Transfer("~/Error/Error");
        //    }

        //}
        // error precedance
        // local page then global.asax for a single page error then custoem errors in web.config for different pages for different errors
    }
}
