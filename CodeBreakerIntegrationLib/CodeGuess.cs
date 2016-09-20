using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerIntegrationLib
{
    /// <summary>
    /// Type to Serialize a Guess from the browser into
    /// </summary>
    public class CodeGuess
    {
        public int CodeLength { get; set; }
        public int ColorCount { get; set; }
        public int[] Code { get; set; }
        public int[] Guess { get; set; }
    }
}
