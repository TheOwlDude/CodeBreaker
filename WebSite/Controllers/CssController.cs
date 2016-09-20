using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.Controllers
{
    public class CssController : ContentControllerBase
    {
        public override string ContentFolder
        {
            get 
            {
                return
                    Path.Combine(
                       Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                       "Content",
                       "Css"
                    );
            }
        }

        public override string MediaType
        {
            get { return "text/css"; }
        }
    }
}
