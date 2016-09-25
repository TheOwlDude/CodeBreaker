using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using System.Web;
using System.Net.Http;
using log4net;
using System.Web.Http.Controllers;
using System.Web.Http;
using WebSite.Controllers.CodeBreakerControllers;

namespace WebSite.Controllers
{
    public class HintDrivenControllerSelector : IHttpControllerSelector
    {
        HttpConfiguration config;

        public HintDrivenControllerSelector(HttpConfiguration config)
        {
            this.config = config;
        }

        public IDictionary<string, System.Web.Http.Controllers.HttpControllerDescriptor> GetControllerMapping()
        {
            throw new NotImplementedException();
        }

        public HttpControllerDescriptor SelectController(System.Net.Http.HttpRequestMessage request)
        {
            System.Web.Http.Routing.IHttpRouteData routeData = request.GetRouteData();

            string controllerHint = routeData.Values[SiteManager.ControllerHintKey].ToString();
            if (string.IsNullOrWhiteSpace(controllerHint))
            {
                LogManager.GetLogger("Route Selection").ErrorFormat("No controller hint found for route {0}", routeData.Route.RouteTemplate);
                return null;
            }

            switch(controllerHint)
            {
                case SiteManager.homeControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(HomeController));
                case SiteManager.htmlControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(HtmlController));
                case SiteManager.javascriptControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(JavascriptController));
                case SiteManager.cssControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(CssController));
                case SiteManager.codebreakerGuessControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(GuessController));
                case SiteManager.codebreakerConsistentCodeControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(ConsistentCodeController));
                case SiteManager.codebreakerBestGuessControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(BestGuessController));
                case SiteManager.imageControllerHint:
                    return new HttpControllerDescriptor(config, controllerHint, typeof(ImageController));
                default:
                    LogManager.GetLogger("Route Selection").ErrorFormat("Unknown conntroller hint {0}", controllerHint);
                    return null;
            }
        }
    }
}
