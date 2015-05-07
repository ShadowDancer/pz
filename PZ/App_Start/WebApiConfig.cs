using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PZ
{
    class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{type}");
            configuration.Routes.MapHttpRoute("API Extended", "api/{controller}/{type}/{id}");

            var configurationGlobal = GlobalConfiguration.Configuration;
            configurationGlobal.Formatters.Remove(configurationGlobal.Formatters.XmlFormatter);
        }

        public static string UrlPrefix { get { return "api"; } }
        public static string UrlPrefixRelative { get { return "~/api"; } }
    }
}