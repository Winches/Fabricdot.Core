using System.Text;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Core.Tests.UniqueIdentifier;

public class SafetyCombGuidGeneratorTests : TestFor<CombGuidGenerator>
{
    private const int Count = 1000;

    [Fact]
    public void Constructor_GivenNull_ThrowException()
    {
        var sut = typeof(CombGuidGenerator).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Create_GivenInvalidGuidType_ThrowException()
    {
        Invoking(() => Sut.Create(99.To<CombGuidType>()))
                     .Should()
                     .Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_WhenSequentialAsString_ReturnCorrectly()
    {
        var guids = Enumerable.Range(1, Count)
                              .Select(_ => Sut.Create(CombGuidType.SequentialAsString))
                              .ToArray();

        guids.OrderBy(v => v.ToString()).Should().BeEquivalentTo(guids);
    }

    [Fact]
    public void Create_WhenSequentialAsBinary_ReturnCorrectly()
    {
        var guids = Enumerable.Range(1, Count)
                      .Select(_ => Sut.Create(CombGuidType.SequentialAsBinary))
                      .ToArray();

        guids.OrderBy(v => Encoding.UTF8.GetString(v.ToByteArray())).Should().BeEquivalentTo(guids);
    }

    [Fact]
    public void Create_WhenSequentialAtEnd_ReturnCorrectly()
    {
        var guids = Enumerable.Range(1, Count)
                      .Select(_ => Sut.Create(CombGuidType.SequentialAtEnd))
                      .ToArray();

        guids.OrderBy(v => v.ToString("N").Substring(16, 12)).Should().BeEquivalentTo(guids);
    }

    protected override CombGuidGenerator CreateSut() => GuidFactories.SafetyComb;
}
