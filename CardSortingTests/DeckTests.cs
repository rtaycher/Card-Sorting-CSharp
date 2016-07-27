namespace CardSortingTests
{
    using System;
    using System.Linq;

    using CardSorting;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class DeckTests
    {
        [TestMethod()]
        public void AcesHighSortTest()
        {
            var deck = Deck.GetDeck();
            deck.SortAscending(SortStrategies.AcesHigh);
            Assert.AreEqual(Card.FromString("(♣) 2"), deck.cards[0]);
            Assert.AreEqual(Card.FromString("(♠) A"), deck.cards.Last());
        }

        [TestMethod()]
        public void AcesLowSortTest()
        {
            var deck = Deck.GetDeck();
            deck.SortAscending(SortStrategies.AcesLow);
            Assert.AreEqual(Card.FromString("(♣) A"), deck.cards[0]);
            Assert.AreEqual(Card.FromString("(♠) K"), deck.cards.Last());
        }

        [TestMethod()]
        public void CustomSortTest()
        {
            // sort Hearts first
            var deck = Deck.GetDeck();
            deck.SortAscending(
                (c1, c2) =>
                    {
                        if (c1.Suit == c2.Suit)
                        {
                            return c1.Rank.CompareTo(c2.Rank);
                        }
                        else
                        {
                            return c1.Suit.CompareTo(c2.Suit);
                        }
                    });
            foreach (int i in Enumerable.Range(0, 13))
            {
                Assert.AreEqual(Suit.Clubs, deck.cards[i].Suit);
                Assert.AreEqual(Suit.Spades, deck.cards[deck.cards.Count - (i + 1)].Suit);
            }
        }

        [TestMethod()]
        public void DeckBasicsTest()
        {
            // Deck has 52 cards, 13 suits, regardless of sort order
            var deck = Deck.GetDeck();
            Assert.AreEqual(52, deck.cards.Count);
            Assert.AreEqual(52 / 4, deck.cards.Select(c => c.Rank).Distinct().Count());

            deck.SortAscending(SortStrategies.AcesLow);

            Assert.AreEqual(52, deck.cards.Count);
            Assert.AreEqual(52 / 4, deck.cards.Select(c => c.Rank).Distinct().Count());

            deck.RandomSort();

            Assert.AreEqual(52, deck.cards.Count);
            Assert.AreEqual(52 / 4, deck.cards.Select(c => c.Rank).Distinct().Count());
        }

        [TestMethod()]
        public void RandomSortTest()
        {
            var rand = new Random(5);
            var deck = Deck.GetDeck(rand);
            deck.RandomSort();
            Assert.AreEqual(Card.FromString("(♦) 6"), deck.cards[0]);
            Assert.AreEqual(Card.FromString("(♣) J"), deck.cards.Last());

            // After Random sort cards should probably(high probability) not be in the same order, due to the seed we know they aren't
            deck.RandomSort();
            Assert.AreNotEqual(Card.FromString("(♦) 6"), deck.cards[0]);
            Assert.AreNotEqual(Card.FromString("(♣) J"), deck.cards.Last());
        }
    }

    [TestClass()]
    public class CardTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Assert.AreEqual(new Card(Suit.Hearts, Rank.King), Card.FromString("(♥) K"));
            Assert.AreEqual(new Card(Suit.Diamonds, Rank.Ten), Card.FromString("(♦) 10"));
            Assert.AreEqual(new Card(Suit.Spades, Rank.Five), Card.FromString("(♠) 5"));
        }
    }
}