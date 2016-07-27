namespace CardSorting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Ordering = System.Int32;

    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    public enum Rank { Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14 }

    public enum SortStrategies { AcesHigh, AcesLow }

    public static class SuitExtension
    {
        public static char GetChar(this Suit suit)
        {
            switch (suit)
            {
                case Suit.Clubs:
                    return 'C';
                case Suit.Diamonds:
                    return 'D';
                case Suit.Hearts:
                    return 'H';
                case Suit.Spades:
                    return 'S';
                default:
                    throw new ArgumentException(
                        string.Format("{} is not a valid suit value", Enum.GetName(typeof(Suit), suit)));
            }
        }

        public static char GetSymbol(this Suit suit)
        {
            switch (suit)
            {
                case Suit.Clubs:
                    return '♣';
                case Suit.Diamonds:
                    return '♦';
                case Suit.Hearts:
                    return '♥';
                case Suit.Spades:
                    return '♠';
                default:
                    throw new ArgumentException(
                        string.Format("{} is not a valid suit value", Enum.GetName(typeof(Suit), suit)));
            }
        }
    }

    public class Deck
    {
        public List<Card> cards;

        private readonly Random random;

        public Deck(Random random = null)
        {
            this.cards = new List<Card>();
            this.random = random ?? new Random();
        }

        public static Deck GetDeck(Random rand = null)
        {
            var deck = new Deck(rand);
            foreach (var suit in new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades })
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    deck.cards.Add(new Card(suit, rank));
                }
            }

            deck.SortAscending(SortStrategies.AcesHigh);
            return deck;
        }

        public void Print(bool fancy)
        {
            if (fancy)
            {
                Console.Write(this.ToFancySting());
            }
            else
            {
                Console.Write(this.ToString());
            }
        }

        public void RandomSort()
        {
            // https://en.wikipedia.org/wiki/Fisher-Yates_shuffle#The_modern_algorithm
            for (int i = 0; i <= this.cards.Count() - 2; i++)
            {
                int j = this.random.Next(i, this.cards.Count());
                this.SwapElements(i, j);
            }
        }

        public void SortAscending(SortStrategies strategy, string suitOrder = "CDHS")
        {
            if (string.Concat(suitOrder.OrderBy(c => c)) != "CDHS")
            {
                throw new ArgumentException(string.Format("Invalid suit order string - " + suitOrder));
            }

            Func<Card, Card, int> func = (x, y) =>
                {
                    int adjustedXRankValue = 0;
                    int adjustedYRankValue = 0;
                    if (x.Rank == Rank.Ace && strategy == SortStrategies.AcesLow)
                    {
                        adjustedXRankValue = 1;
                    }
                    else
                    {
                        adjustedXRankValue = (int)x.Rank;
                    }

                    if (y.Rank == Rank.Ace && strategy == SortStrategies.AcesLow)
                    {
                        adjustedYRankValue = 1;
                    }
                    else
                    {
                        adjustedYRankValue = (int)y.Rank;
                    }

                    if (adjustedXRankValue == adjustedYRankValue)
                    {
                        return suitOrder.IndexOf(x.Suit.GetChar()).CompareTo(suitOrder.IndexOf(y.Suit.GetChar()));
                    }
                    else
                    {
                        return adjustedXRankValue.CompareTo(adjustedYRankValue);
                    }
                };
            this.SortAscending(func);
        }

        /// <summary>
        /// Sort Cards in Ascending Order
        /// </summary>
        /// <param name="compareFunc">
        /// Comperator function
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public void SortAscending(Func<Card, Card, int> compareFunc)
        {
            this.cards = this.cards.OrderBy(x => x, new FuncComparer<Card>(compareFunc)).ToList();
        }

        public string ToFancySting()
        {
            string buf = string.Empty;
            int i = 0;
            foreach (var card in this.cards)
            {
                buf += card.ToFancyString() + "|";

                if (i % 4 == 3)
                {
                    buf += "\n";
                }

                i++;
            }

            buf += "\n";
            return buf;
        }

        public override string ToString()
        {
            string buf = string.Empty;
            int i = 0;
            foreach (var card in this.cards)
            {
                buf += card.ToString() + "|";

                if (i % 4 == 3)
                {
                    buf += "\n";
                }

                i++;
            }

            buf += "\n";
            return buf;
        }

        private void SwapElements(int indexA, int indexB)
        {
            var temp = this.cards[indexA];
            this.cards[indexA] = this.cards[indexB];
            this.cards[indexB] = temp;
        }
    }

    public class FuncComparer<T> : IComparer<T>
    {
        private Func<T, T, int> compareFunc;

        public FuncComparer(Func<T, T, int> compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        public int Compare(T t1, T t2)
        {
            return this.compareFunc(t1, t2);
        }
    }

    public struct Card
    {
        public Suit Suit;

        public Rank Rank;

        public bool IsFaceCard
        {
            get
            {
                return this.Rank >= Rank.Jack;
            }
        }

        /// <summary>
        /// Create a Card from a Format string.
        /// </summary>
        /// <param name="cardString">
        /// The string to create the card from, in the format of "(S) R" where S is the card symbol and R is the Face/int value
        /// </param>
        /// <returns>
        /// The <see cref="Card"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static Card FromString(string cardString)
        {
            Regex re = new Regex(@"\(([CDHS♣♦♥♠])\)[ ]*(\d{0,2}[AKQJ]?)");
            Suit suit = Suit.Hearts;
            var match = re.Match(cardString.Trim());
            if (!match.Success || match.Groups.Count != 3)
            {
                throw new ArgumentException("invalid cardString for creating Card:" + cardString);
            }

            var suitString = match.Groups[1].Captures[0].Value;
            var rankString = match.Groups[2].Captures[0].Value;
            if (suitString.Contains("CDHS"))
            {
                suit = (Suit)Enum.Parse(typeof(Suit), suitString[0].ToString(), true);
            }
            else
            {
                foreach (Suit value in new List<Suit>() { Suit.Hearts, Suit.Clubs, Suit.Diamonds, Suit.Spades })
                {
                    if (value.GetSymbol() == suitString[0])
                    {
                        suit = value;
                    }
                }
            }

            int rank;
            if (!int.TryParse(rankString, out rank))
            {
                foreach (Rank value in new List<Rank>() { Rank.Ace, Rank.King, Rank.Queen, Rank.Jack })
                {
                    if (value.ToString()[0] == rankString[0])
                    {
                        rank = (int)value;
                    }
                }
            }

            return new Card(suit, (Rank)rank);
        }

        public Card(Suit suit, int rank)
        {
            this.Suit = suit;
            this.Rank = (Rank)rank;
        }

        public Card(Suit suit, Rank rank)
        {
            this.Suit = suit;
            this.Rank = rank;
        }

        public override string ToString()
        {
            if (this.IsFaceCard)
            {
                return string.Format("{0,2} ({1})", Enum.GetName(typeof(Rank), this.Rank)[0], this.Suit.GetChar());
            }
            else
            {
                return string.Format("{0,2} ({1})", (int)this.Rank, this.Suit.GetChar());
            }
        }

        public string ToFancyString()
        {
            if (this.IsFaceCard)
            {
                return string.Format("{0,2} ({1})", Enum.GetName(typeof(Rank), this.Rank)[0], this.Suit.GetSymbol());
            }
            else
            {
                return string.Format("{0,2} ({1})", (int)this.Rank, this.Suit.GetSymbol());
            }
        }
    }
}