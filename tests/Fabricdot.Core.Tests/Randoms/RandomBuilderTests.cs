using System;
using Fabricdot.Core.Randoms;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Randoms
{
    public class RandomBuilderTests
    {
        protected RandomBuilder RandomBuilder { get; } = new(new DefaultRandomProvider());

        [InlineData(null, 1)]
        [InlineData("", 1)]
        [InlineData("abc", 0)]
        [InlineData("abc", -1)]
        [Theory]
        public void GetRandomString_GivenInvalidInput_ThrowException(
            string source,
            int length)
        {
            FluentActions.Invoking(() => RandomBuilder.GetRandomString(source, length))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void GetRandomString_GivenInput_ReturnCorrectly()
        {
            const string source = "abcde";
            const int length = 10;
            var text1 = RandomBuilder.GetRandomString(source, length);
            var text2 = RandomBuilder.GetRandomString(source, length);

            text1.Should().HaveLength(length);
            text1.ToCharArray().Should().OnlyContain(v => source.Contains(v));
            text1.Should().NotBe(text2);
        }

        [InlineData(0)]
        [InlineData(-1)]
        [Theory]
        public void GetRandomNumbers_GivenInvalidInput_ThrowException(int length)
        {
            FluentActions.Invoking(() => RandomBuilder.GetRandomNumbers(length))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void GetRandomNumbers_GivenInput_ReturnCorrectly()
        {
            const string source = "0123456789";
            const int length = 10;
            var text1 = RandomBuilder.GetRandomNumbers(length);
            var text2 = RandomBuilder.GetRandomNumbers(length);

            text1.Should().HaveLength(length);
            text1.ToCharArray().Should().OnlyContain(v => source.Contains(v));
            text1.Should().NotBe(text2);
        }

        [InlineData(0)]
        [InlineData(-1)]
        [Theory]
        public void GetRandomLetters_GivenInvalidInput_ThrowException(int length)
        {
            FluentActions.Invoking(() => RandomBuilder.GetRandomLetters(length))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void GetRandomLetters_GivenInput_ReturnCorrectly()
        {
            const string source = "abcdefghijklmnopqrstuvwxyz";
            const int length = 10;
            var text1 = RandomBuilder.GetRandomLetters(length);
            var text2 = RandomBuilder.GetRandomLetters(length);

            text1.Should().HaveLength(length);
            text1.ToCharArray().Should().OnlyContain(v => source.Contains(v));
            text1.Should().NotBe(text2);
        }
    }
}