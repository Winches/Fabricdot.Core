using System;
using System.Linq;
using Fabricdot.Domain.ValueObjects;
using Xunit;

namespace Fabricdot.Domain.Tests
{
    public class EnumerationTest
    {
        public class CardType : Enumeration
        {
            public static readonly CardType Visa = new CardType(1, "Visa");
            public static readonly CardType MasterCard = new CardType(2, "MasterCard");

            public CardType(int id, string name) : base(id, name)
            {
            }
        }

        [Fact]
        public void Equal_GivenSameValueInstance_ReturnTrue()
        {
            var cardType = new CardType(1, CardType.Visa.Name);
            Assert.True(CardType.Visa.Equals(cardType));
        }

        [Fact]
        public void Equal_GivenSameValueDifferentNameInstance_ReturnTrue()
        {
            var cardType = new CardType(1, "Visa1");
            Assert.True(CardType.Visa.Equals(cardType));
        }

        [Fact]
        public void Equal_GivenNull_ReturnFalse()
        {
            Assert.False(CardType.Visa.Equals(null));
        }

        [Fact]
        public void EqualOperator_GivenSameValueInstance_ReturnTrue()
        {
            var cardType = new CardType(1, CardType.Visa.Name);
            Assert.True(CardType.Visa == cardType);
        }

        [Fact]
        public void EqualOperator_GivenNull_ReturnFalse()
        {
            Assert.False(CardType.Visa == null);
        }

        [Fact]
        public void EqualOperator_BothNull_ReturnTrue()
        {
            CardType cardType = null;
            Assert.True(cardType == null);
        }

        [Fact]
        public void NotEqualOperator_GivenSameValueInstance_ReturnFalse()
        {
            var cardType = new CardType(1, CardType.Visa.Name);
            Assert.False(CardType.Visa != cardType);
        }

        [Fact]
        public void NotEqualOperator_GivenNull_ReturnTrue()
        {
            Assert.True(CardType.Visa != null);
        }

        [Fact]
        public void AbsoluteDifference_GivenInstance_ReturnAbsoluteValue()
        {
            var cardType1 = CardType.Visa;
            var cardType2 = CardType.MasterCard;
            var expected = Math.Abs(cardType1.Value - cardType2.Value);
            var actual = Enumeration.AbsoluteDifference(cardType1, cardType2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAll_ReturnPublicStaticFields()
        {
            var cardTypes = Enumeration.GetAll<CardType>().ToList();
            Assert.Contains(CardType.Visa, cardTypes);
            Assert.Contains(CardType.MasterCard, cardTypes);
        }

        [Fact]
        public void FromValue_GivenExistedValue_ReturnInstance()
        {
            var cardType = Enumeration.FromValue<CardType>(1);
            Assert.Equal(CardType.Visa, cardType);
        }

        [Fact]
        public void FromValue_GivenNotExistedValue_ThrowException()
        {
            void Action() => Enumeration.FromValue<CardType>(0);
            Assert.Throws<InvalidOperationException>(Action);
        }

        [Fact]
        public void FromName_GivenExistedName_ReturnInstance()
        {
            var cardType = Enumeration.FromName<CardType>(CardType.Visa.Name);
            Assert.Equal(CardType.Visa, cardType);
        }

        [Fact]
        public void FromName_GivenNotExistedName_ThrowException()
        {
            void Action() => Enumeration.FromName<CardType>("InvalidName");
            Assert.Throws<InvalidOperationException>(Action);
        }
    }
}