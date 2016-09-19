using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using WebSite.Controllers;

namespace WebSite
{
    public class SiteManager
    {
        HttpSelfHostServer server = null;
        public void StartSite()
        {            
            string baseAddress = "http://localhost:9000/";
            
            HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration(baseAddress);

            configuration.Routes.MapHttpRoute(
                "htmlRoute",
                "UI/{file}",
                new Dictionary<string, object>
                {
                    {SiteManager.ControllerHintKey, SiteManager.htmlControllerHint}
                }
            );

            configuration.Routes.MapHttpRoute(
                "javascriptRoute",
                "UI/Javascript/{file}",
                new Dictionary<string, object>
                {
                    {SiteManager.ControllerHintKey, SiteManager.javascriptControllerHint}
                }
            );

            configuration.Routes.MapHttpRoute(
                "home",
                "Home",
                new Dictionary<string, object>
                {
                    {SiteManager.ControllerHintKey, SiteManager.homeControllerHint}
                }
            );

            configuration.Services.Replace(typeof(IHttpControllerSelector), new HintDrivenControllerSelector(configuration));
            server = new HttpSelfHostServer(configuration);
            server.OpenAsync().Wait();
            Console.WriteLine("Server opened");
        }


        public const string ControllerHintKey = "controllerHint";
        public const string htmlControllerHint = "htmlController";
        public const string javascriptControllerHint = "javascriptController";
        public const string homeControllerHint = "homeController";
    }
}
