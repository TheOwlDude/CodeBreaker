using Microsoft.FSharp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerIntegrationLib
{
    public class GameState
    {
        int codeLength;
        int colorCount;

        int[] code;

        List<Game.Engine.GuessOutcome> guesses = new List<Game.Engine.GuessOutcome>();

        Random rand = new Random();

        public GameState(int codeLength = 4, int colorCount = 6, int[] code = null)
        {
            if (code != null)
            {
                if (code.Length != codeLength) throw new ArgumentException(string.Format("The code must have length {0}", codeLength));
                this.code = code;
            }
            else
            {
                this.code = new int[codeLength];
                for (int i = 0; i < codeLength; ++i) code[i] = rand.Next(0, codeLength);
            }

            this.codeLength = codeLength;
            this.colorCount = colorCount;
        }


        public Game.Engine.Outcome Guess(int[] guess)
        {
            Game.Engine.Outcome outcome = Game.Engine.calculateGuessResult(SeqModule.ToList(code), SeqModule.ToList(guess));
            guesses.Add(new Game.Engine.GuessOutcome(SeqModule.ToList(guess), outcome));
            return outcome;
        }

        public List<Game.Engine.GuessOutcome> Guesses { get { return guesses.ToList(); } }
    }
}
