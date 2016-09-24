using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerIntegrationLib
{
    public class ExpectedPossibilitiesForGuess
    {
        public int[] Guess { get; set; }
        public decimal ExpectedPossibilities { get; set; }
    }

    public class BestGuessResponse
    {
        public ExpectedPossibilitiesForGuess[] BestGuesses;
    }
}
