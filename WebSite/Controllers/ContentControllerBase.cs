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
    public abstract class ContentControllerBase : ApiController
    {
        public abstract string ContentFolder { get;}

        public abstract string MediaType { get; }


        [HttpGet]
        public HttpResponseMessage GetHtmlPage([FromUri] string file)
        {
            string filePath = Path.Combine(ContentFolder, file);

            if (!File.Exists(filePath))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            string fileContents;
            using(FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    fileContents = sr.ReadToEnd();
                }
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(fileContents);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
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
