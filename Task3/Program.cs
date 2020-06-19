using System;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace Task3
{
    class Program
    {
        public int chooseelemHuman { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Choose count element (3 or 5 elements)");
            int choose = Int32.Parse(Console.ReadLine());
            var game = new Game(choose, Player.Human);
            game.MachineStep += Game_MachineStep;
            game.HumanStep += Game_HumanStep;
            game.EndGame += Game_EndGame;
            game.Start();
        }

        private static void Game_EndGame(Player player)
        {
            Console.WriteLine($"Winner: {player}");
        }

        private static void Game_HumanStep(object sender, int number)
        {
            Console.WriteLine($"Remaining elements: {number}");
            Console.WriteLine("Choose one element");
            bool isCorrectly = false;
            while (!isCorrectly)
            {
                if (int.TryParse(Console.ReadLine(), out int takeelement))
                {
                    var game = (Game) sender;
                    try
                    {
                        game.HumanTakeElement(takeelement);
                        isCorrectly = true;
                        Console.WriteLine($"You can choose: {game.AdvancedGame[takeelement]}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private static void Game_MachineStep(int number)
        {
            Console.WriteLine($"Machine choose: {number}");
        }
    }
}
