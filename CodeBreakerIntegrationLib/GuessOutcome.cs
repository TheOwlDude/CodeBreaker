using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerIntegrationLib
{
    public class GuessOutcome
    {
        public int CodeLength { get; set; }
        public int ColorCount { get; set; }
        public int[] Code { get; set; }
        public int[] Guess { get; set; }
        public int BlackCount { get; set; }
        public int WhiteCount { get; set; }
    }
}
