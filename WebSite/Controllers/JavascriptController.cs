using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Controllers
{
    public class JavascriptController : ContentControllerBase
    {
        public override string ContentFolder
        {
            get
            {
                return
                    Path.Combine(
                       Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                       "Content",
                       "Javascript"
                    );
            }
        }

        public override string MediaType { get { return "application/javascript"; } }

    }
}
