using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerIntegrationLib
{
    /// <summary>
    /// Associates with a guess a numeric score of its quality. Smaller numbers are better. The score is the expected number of remaining unguessed consistent 
    /// codes if the guess is made. The unguessed is important. If the guess is in fact the correct code, the number of consistent codes is 1 and the game is won. 
    /// A guess could also narrow the count of consistent codes to 1 without being correct, so the game isn't yet won. Clearly these are distinct cases with the 
    /// former clearly superior. In order to distinguish these the quality of the correct code is 0.
    /// </summary>
    public class GuessQuality
    {
        public int[] Guess { get; set; }
        public decimal Quality { get; set; }
    }

    public class GuessQualities
    {
        public GuessQuality[] BestGuesses;
    }
}
