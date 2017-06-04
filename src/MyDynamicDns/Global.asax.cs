using System;
using System.Web.Http;
using Serilog;

namespace MyDynamicDns
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static string _logPath;

        protected void Application_Start()
        {
            _logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/log-{Date}.txt");

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(_logPath)
                .CreateLogger();
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
