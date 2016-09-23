using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerIntegrationLib
{
    public class ConsistentCodeRequest
    {        
        public int CodeLength { get; set; }
        public int ColorCount { get; set; }
        public GuessOutcome[] GameInfo { get; set; }
    }
}
