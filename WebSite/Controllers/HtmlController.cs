using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebSite.Controllers
{
    public class HtmlController : ContentControllerBase
    {
        public override string ContentFolder
        {
            get 
            {
                return 
                    Path.Combine(
                       Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                       "Content",
                       "Html"
                    ); 
            }
        }

        public override string MediaType { get { return "text/html"; } }
    }
}
