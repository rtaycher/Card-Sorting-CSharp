namespace CardSorting
{
    using System;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            var deck = Deck.GetDeck();
            if (!args.Any() || args[0] == "--help")
            {
                Console.WriteLine(
                    "Please enter --sorted or --random to print out a sorted or a randomized deck respectively.");
                Environment.Exit(1);
            }
            else if (args[0] == "--random")
            {
                deck.RandomSort();
            }
            else if (args[0] == "--sorted")
            {
                deck.SortAscending(SortStrategies.AcesHigh);
            }

            if (args.Contains("--plain"))
            {
                deck.Print(false);
            }
            else
            {
                Console.WriteLine(
                    "Printing deck with fancy unicode charecters, if this doesn't work on windows add the --plain flag");
                deck.Print(true);
            }
        }
    }
}