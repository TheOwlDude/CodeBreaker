using CodeBreakerIntegrationLib;
using Microsoft.FSharp.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineTest
{
    [TestFixture]
    public class ConsistentCodeTests
    {
       
        [Test]
        public void DumpConsistentCodes()
        {
            GameState state = new GameState(4, 6, new int[] { 0, 0, 0, 1 });
            state.Guess(new int[] { 0, 0, 0, 0 });
            state.Guess(new int[] { 1, 1, 1, 1 });

            //FSharpList<FSharpList<int>> consistentCodes = Game.Engine.codesConsistentWithOutcomes(4, 6, SeqModule.ToList(state.Guesses));
            var consistentCodes = Game.Engine.codesConsistentWithOutcomes(4, 6, SeqModule.ToList(state.Guesses));
            Console.WriteLine(consistentCodes);


            //var convert = consistentCodes.Select(c => c.ToList()).ToList();
            //var convert = consistentCodes.ToList();

            //Console.WriteLine(convert);
        }

        [Test]
        public void OutcomeDistributionMapIsCorrect()
        {
            GameState state = new GameState(3, 3, new int[] { 0, 1, 2 });
            state.Guess(new int[] { 0, 0, 1 });

            Console.WriteLine("Guess Outcomes");
            foreach (Game.Engine.GuessOutcome guessOutcome in state.Guesses) Console.WriteLine("guess = {0}, outcome = [{1};{2}]", guessOutcome.guess, guessOutcome.result.blackCount, guessOutcome.result.whiteCount);
            Assert.AreEqual(1, state.Guesses[0].result.blackCount);
            Assert.AreEqual(1, state.Guesses[0].result.whiteCount);

            List<int> nextGuess = new List<int> { 2, 2, 1 };
            FSharpList<FSharpList<int>> possibleCodes = Game.Engine.codesConsistentWithOutcomes(state.CodeLength, state.ColorCount, SeqModule.ToList(state.Guesses));
            FSharpMap<Game.Engine.Outcome,int> map = Game.Engine.getOutcomeCountMapForGuess(3, 3, possibleCodes, SeqModule.ToList(state.Guesses), SeqModule.ToList(nextGuess));
            foreach(KeyValuePair<Game.Engine.Outcome,int> kvp in map)
            {
                Console.WriteLine("[blacks = {0} whites = {1}] X {2}", kvp.Key.blackCount, kvp.Key.whiteCount, kvp.Value);
            }
            Assert.AreEqual(2, map[new Game.Engine.Outcome(0, 2)]);
            Assert.AreEqual(2, map[new Game.Engine.Outcome(1, 0)]);
            

        }     
  

        [Test]
        public void WeightedExpectedIsCorrect()
        {
            GameState state = new GameState(3, 3, new int[] {1,2,1});
            state.Guess(new int[] {0,1,2});

            FSharpList<FSharpList<int>>  consistentCodes = Game.Engine.codesConsistentWithOutcomes(3, 3, SeqModule.ToList(state.Guesses));
            foreach(FSharpList<int> possibleCode in consistentCodes) Console.WriteLine(possibleCode);

            FSharpMap<Game.Engine.Outcome, int> map = Game.Engine.getOutcomeCountMapForGuess(3, 3, consistentCodes, SeqModule.ToList(state.Guesses), SeqModule.ToList(new List<int> { 1, 2, 1 }));
            foreach(KeyValuePair<Game.Engine.Outcome,int> kvp in map)
            {
                Console.WriteLine("({0},{1}) x {2}", kvp.Key.blackCount, kvp.Key.whiteCount, kvp.Value);
            }

            decimal ans = Game.Engine.calculateExpectedRemainingPossibleCodes(3, 3, consistentCodes, SeqModule.ToList(state.Guesses), SeqModule.ToList(new List<int> { 1, 2, 1 })).Item2;
            Console.WriteLine("Answer = {0}", ans);
        }


        [Test]
        public void WeightedExpectedIsCorrect2()
        {
            EvaluateState(3, 3, new int[] { 1, 2, 1 }, new List<int[]> { new int[] { 0, 1, 2} }, new int[] { 1, 2, 1 });
        }

        [Test]
        public void FourX3()
        {
            EvaluateState(4, 4, new int[] { 1, 2, 2, 1}, new List<int[]> { new int[] { 1, 1, 0, 2 }, new int[] {0, 2, 1, 0} }, new int[] { 0, 0, 1, 2 });
        }

        [Test]
        public void TestGameI()
        {
            EvaluateState(4, 4, new int[] { 1, 3, 0, 2 }, new List<int[]> { new int[] { 0, 1, 2, 3 }, new int[] { 1, 2, 3, 0 } }, new int[] { 3, 2, 0, 1 });
        }

        [Test]
        public void TestGameII()
        {
            EvaluateState(4, 4, new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 3, 2, 3, 1 }, new int[] { 0, 2, 1, 0 } }, new int[] { 2, 2, 3, 3 });
        }

        [Test]
        public void TestGameII_RightIsNotBest()
        {
            EvaluateState(4, 4, new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 3, 2, 3, 1 }, new int[] { 0, 2, 1, 0 } }, new int[] { 0, 1, 2, 3 });
        }

        [Test]
        public void CodesConsistentWithGameII()
        {
            GameState state = new GameState(4, 4, new int[] { 0, 1, 2, 3 });
            state.Guess(new int[] { 3, 2, 3, 1 });
            state.Guess(new int[] { 0, 2, 1, 0 });

            FSharpList<FSharpList<int>> codes = Game.Engine.codesConsistentWithOutcomes(4, 4, SeqModule.ToList(state.Guesses));
            foreach (FSharpList<int> code in codes) Console.WriteLine(ConvertFSharpListToString(code));
        }
        
        [Test]
        public void OnlyOneRemainingOutcomeGuessIsCorrect()
        {
            EvaluateState(2, 2, new int[] { 0, 0 }, new List<int[]> { new int[] { 1, 1 } }, new int[] { 0, 0 });
        }

        [Test]
        public void OnlyOneRemainingOutcomeGuessIsIncorrect()
        {
            EvaluateState(2, 2, new int[] { 0, 0 }, new List<int[]> { new int[] { 1, 1 } }, new int[] { 1, 1 });
        }



        public string ConvertFSharpListToString<T>(FSharpList<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool first = true;
            foreach (T item in list)
            {
                if (!first) sb.Append("; "); else first = false;
                sb.Append(item.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }


        public void EvaluateState(int codeLength, int colorCount, int[] code, List<int[]> guesses, int[] nextGuess)
        {
            GameState state = new GameState(codeLength, colorCount, code);
            foreach(int[] guess in guesses) state.Guess(guess);

            FSharpList<FSharpList<int>> consistentCodes = Game.Engine.codesConsistentWithOutcomes(codeLength, colorCount, SeqModule.ToList(state.Guesses));
            foreach (FSharpList<int> possibleCode in consistentCodes) Console.WriteLine(ConvertFSharpListToString<int>(possibleCode));

            FSharpMap<Game.Engine.Outcome, int> map = Game.Engine.getOutcomeCountMapForGuess(codeLength, colorCount, consistentCodes, SeqModule.ToList(state.Guesses), SeqModule.ToList(nextGuess));
            foreach (KeyValuePair<Game.Engine.Outcome, int> kvp in map)
            {
                Console.WriteLine("({0},{1}) x {2}", kvp.Key.blackCount, kvp.Key.whiteCount, kvp.Value);
            }

            decimal ans = Game.Engine.calculateExpectedRemainingPossibleCodes(codeLength, colorCount, consistentCodes, SeqModule.ToList(state.Guesses), SeqModule.ToList(nextGuess)).Item2;
            Console.WriteLine("Answer = {0}", ans);
        }


        [Test]
        public void DetermineGuesses()
        {
            PrintOrderedGuesses(4, 4, new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 3, 2, 3, 1 }, new int[] { 0, 2, 1, 0 } });

        }

        public void PrintOrderedGuesses(int codeLength, int colorCount, int[] code, List<int[]> guesses)
        {
            GameState state = new GameState(codeLength, colorCount, code);
            foreach (int[] guess in guesses) state.Guess(guess);

            FSharpList<Tuple<FSharpList<int>,decimal>> orderedGuesses = Game.Engine.getGuessesSortedByQuality(codeLength, colorCount, SeqModule.ToList(state.Guesses));
            foreach(Tuple<FSharpList<int>,decimal> guess in orderedGuesses)
            {
                Console.WriteLine("{0} {1}", ConvertFSharpListToString(guess.Item1), guess.Item2);
            }
        }

        int[] GetRandomCode(int codeLength, int colorCount, Random rand)
        {
            int[] code = new int[codeLength];
            for (int i = 0; i < codeLength; ++i) code[i] = rand.Next(colorCount);
            return code;
        }

        



        [Test]
        public void Play4x6()
        {
            AutoPlay(4, 6);
        }

        [Test]
        //[Explicit("Long running")]
        public void Play4x6x25()
        {
            AutoPlayMultiple(4, 6, 5);
        }


        [Test] 
        public void AutoPlayMultiple(int codeLength, int colorCount, int gamesToPlay)
        {
            int totalGuesses = 0;
            for (int i = 0; i < gamesToPlay; ++i) totalGuesses += AutoPlay(codeLength, colorCount);
            Console.WriteLine("Average Guesses per game: {0}", ((Decimal)totalGuesses) / ((Decimal)gamesToPlay));
        }

        public int AutoPlay(int codeLength, int colorCount)
        {
            Random rand = new Random();
            int[] code = GetRandomCode(codeLength, colorCount, rand);
            Console.WriteLine("Code = {0}", ConvertFSharpListToString(SeqModule.ToList(code)));

            GameState state = new GameState(codeLength, colorCount, code);
            int counter = 1;
            int[] guess = GetRandomCode(codeLength, colorCount, rand);
            Game.Engine.Outcome outcome = state.Guess(guess);
            while(true)
            {
                Console.WriteLine("Guess {0} = {1} Outcome = ({2}, {3})", counter++, ConvertFSharpListToString(SeqModule.ToList(guess)), outcome.blackCount, outcome.whiteCount);
                if (outcome.blackCount == codeLength) break;

                guess = Game.Engine.getGuessesSortedByQuality(codeLength, colorCount, SeqModule.ToList(state.Guesses)).First().Item1.ToArray();
                outcome = state.Guess(guess);
            }
            return state.Guesses.Count;
        }



        [Test]
        public void EverythingCodeIsConsistentWithNoInfo()
        {
            FSharpList<FSharpList<int>> consistentCodes = Game.Engine.codesConsistentWithOutcomes(4, 6, SeqModule.ToList(new List<Game.Engine.GuessOutcome>()));
            Assert.AreEqual(1296, consistentCodes.Count());
        }

    }
}
