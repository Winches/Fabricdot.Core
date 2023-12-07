using Fabricdot.Core.Randoms;

namespace Fabricdot.Core.Tests.Randoms;

public class RandomBuilderTests : TestFor<RandomBuilder>
{
    [InlineData(null, 1)]
    [InlineData("", 1)]
    [InlineData("abc", 0)]
    [InlineData("abc", -1)]
    [Theory]
    public void GetRandomString_GivenInvalidInput_ThrowException(
        string? source,
        int length)
    {
        Invoking(() => Sut.GetRandomString(source!, length))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public void GetRandomString_GivenInput_ReturnCorrectly(
        string source,
        uint leng)
    {
        var length = (int)leng;
        var text1 = Sut.GetRandomString(source, length);
        var text2 = Sut.GetRandomString(source, length);

        text1.Should().HaveLength(length);
        text1.Except(source).Should().BeEmpty();
        text1.Should().NotBe(text2);
    }

    [InlineData(0)]
    [InlineData(-1)]
    [Theory]
    public void GetRandomNumbers_GivenInvalidInput_ThrowException(int length)
    {
        Invoking(() => Sut.GetRandomNumbers(length))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void GetRandomNumbers_GivenInput_ReturnCorrectly()
    {
        const string source = "0123456789";
        var length = (int)Create<uint>();
        var text1 = Sut.GetRandomNumbers(length);
        var text2 = Sut.GetRandomNumbers(length);

        text1.Should().HaveLength(length);
        text1.Except(source).Should().BeEmpty();
        text1.Should().NotBe(text2);
    }

    [InlineData(0)]
    [InlineData(-1)]
    [Theory]
    public void GetRandomLetters_GivenInvalidInput_ThrowException(int length)
    {
        Invoking(() => Sut.GetRandomLetters(length))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void GetRandomLetters_GivenInput_ReturnCorrectly()
    {
        const string source = "abcdefghijklmnopqrstuvwxyz";
        var length = (int)Create<uint>();
        var text1 = Sut.GetRandomLetters(length);
        var text2 = Sut.GetRandomLetters(length);

        text1.Should().HaveLength(length);
        text1.Except(source).Should().BeEmpty();
        text1.Should().NotBe(text2);
    }

    protected override RandomBuilder CreateSut() => new(new DefaultRandomProvider());
}
