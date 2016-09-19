using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSite;

namespace WebSiteHost
{
    class Program
    {
        static void Main(string[] args)
        {
            SiteManager sm = new SiteManager();
            sm.StartSite();
            Console.ReadKey(); 
        }
    }
}
