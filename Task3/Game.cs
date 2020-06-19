using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;

namespace Task3
{
    public enum GameStatus
    {
        NonStarted,
        InProgress,
        End
    }

    public enum Player
    {
        Human, Machine
    }
    public class Game
    {
        public int elementByHuman { get; set; }
        public int elementByMachine { get; set; }
        public int CountElements { get; private set; }
        public string[] ClassicGame = {"Exit","Rock", "Paper", "Scissiors"};
        public string[] AdvancedGame = {"Exit","Rock", "Paper", "Scissiors","Lizard","Spock"};
        public GameStatus ThisGameStatus { get; private set; }
        public Player Turn { get; private set; }
        public int RemainingElement { get; private set; }
        public event EventHandler<int> HumanStep; 
        public event Action<int> MachineStep;
        public event Action<Player> EndGame;
        public Game(int countElements, Player whofirst)
        {

            switch (countElements)
                {
                    case 3:
                    {
                        for (int i = 0; i < ClassicGame.Length; i++)
                        {
                            Console.WriteLine(i + " " + ClassicGame[i]);
                        }

                        break;
                    }
                    case 5:
                    {
                        for (int i = 0; i < AdvancedGame.Length; i++)
                        {
                            Console.WriteLine(i + " " + AdvancedGame[i]);
                        }

                        break;
                    }
                    default:
                    {
                        throw new ArgumentException("Count elements must be have value 3 or 5");
                    }
                }
            

        ThisGameStatus = GameStatus.NonStarted;
            CountElements = countElements;
            RemainingElement = CountElements;
            Turn = whofirst;
        }

        public void Start()
        {
            if (ThisGameStatus == GameStatus.End)
                RemainingElement = CountElements;
            if (ThisGameStatus == GameStatus.InProgress)
            {
                throw new ArgumentException("This game is started!");
            }

            ThisGameStatus = GameStatus.InProgress;
            while (ThisGameStatus == GameStatus.InProgress)
            {
                if (Turn == Player.Machine)
                {
                    MachineMakesStep();
                }
                else
                {
                    HumanMakeStep();
                }

                EndOfGame();
                Turn = Turn == Player.Machine ? Player.Human : Player.Machine;
            }
        }

        public void HumanTakeElement(int element)
        {
            if (element < 0 || element > CountElements)
            {
                throw new ArgumentException($"You can take from 1 to {CountElements} number element");
            }

            else if (element == 0)
            {
                Environment.Exit(0);
            }
            else
            {
                elementByHuman = element;
                TakeElement();
            }
        }
        public void EndOfGame()
        {
            if (RemainingElement == CountElements - 2)
            {
                ThisGameStatus = GameStatus.End;
                EndGame(elementByMachine > elementByHuman ? Player.Machine : Player.Human );
            }
        }

        public void HumanMakeStep()
        {
            HumanStep(this, RemainingElement);
        }

        public void MachineMakesStep()
        {
         HMACResult();   
        }
        public void HMACResult()
        {
            int rnd;
            if (CountElements == 3)
            {
                rnd = NextInt(1, 4);
                elementByMachine = rnd;
            }

            else
            {
                rnd = NextInt(1, 6);
                elementByMachine = rnd;

            }
            SHA256 sha256 = SHA256.Create();
            byte[] buffer = sha256.ComputeHash(Encoding.UTF8.GetBytes(rnd.ToString()));
            string result = BitConverter.ToString(buffer).Replace("-", "");
            Console.WriteLine($"HMAC: {result}");
            MachineStep(rnd);
            TakeElement();
        }
        public int NextInt(int min, int max)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[4];
            rng.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);
            return new Random(result).Next(min, max);
        }

        public void TakeElement(int elements = 1)
        {
            RemainingElement -= elements;
        }
    }
}
