using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MACRO.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "WebApiName");
            })
            .EnableSwaggerUi();
        }
    }
}
