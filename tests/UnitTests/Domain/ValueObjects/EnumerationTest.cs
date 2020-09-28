using System.Linq;
using Fabricdot.Domain.Core.ValueObjects;
using Xunit;

namespace UnitTests.Domain.ValueObjects
{
    public class EnumerationTest
    {
        public class CardType : Enumeration
        {
            public static readonly CardType Amex = new CardType(1, "Amex");
            public static readonly CardType Visa = new CardType(2, "Visa");
            public static readonly CardType MasterCard = new CardType(3, "MasterCard");

            public CardType(int id, string name) : base(id, name)
            {
            }
        }

        [Fact]
        public void TestCompareTo()
        {
        }

        [Fact]
        public void TestEquals()
        {
            Assert.Equal(CardType.Amex, new CardType(1, "Amex"));
            Assert.Equal(CardType.Amex, new CardType(1, "Amex1"));
            Assert.NotEqual(CardType.Amex, new CardType(2, "Amex"));
        }

        [Fact]
        public void TestGetAll()
        {
            var list = Enumeration.GetAll<CardType>().ToList();
            Assert.Contains(CardType.Amex, list);
            Assert.Contains(CardType.MasterCard, list);
            Assert.Contains(CardType.Visa, list);
            Assert.Contains(new CardType(1, "Amex1"), list);
            Assert.Equal(3, list.Count);
        }

        [Fact]
        public void TestFromValue()
        {
            var cardType = Enumeration.FromValue<CardType>(1);
            Assert.Equal(CardType.Amex, cardType);
        }

        [Fact]
        public void TestFromName()
        {
            var cardType = Enumeration.FromName<CardType>(CardType.Amex.Name);
            Assert.Equal(CardType.Amex, cardType);
        }
    }
}