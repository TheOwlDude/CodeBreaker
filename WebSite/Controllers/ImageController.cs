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
    public class ImageController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetHtmlPage([FromUri] string file)
        {
            string filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Content",
                "Images",
                file
            );

            if (!File.Exists(filePath))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            byte[] fileContents;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, Convert.ToInt32(fs.Length));
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream stream = new MemoryStream(fileContents);
            response.Content = new StreamContent(stream, fileContents.Length);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
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
