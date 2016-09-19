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
    public class HomeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetHtmlPage()
        {
            string htmlPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Content",
                "Html",
                "homepage.html"
            );


            if (!File.Exists(htmlPath))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            string fileContents;
            using (FileStream fs = new FileStream(htmlPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    fileContents = sr.ReadToEnd();
                }
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(fileContents);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                NoStore = true,
                NoCache = true,
                MustRevalidate = true
            };

            return response;
        }
    }
}
