using System.Text.Json;

namespace Fabricdot.Core.Tests.System.Text.Json;

public class JsonExtensionsTests : TestBase
{
    [Fact]
    public void FromJson_GivenInvalidJson_ThrowException()
    {
        Invoking(() => (null as string)!.FromJson<object>())
                     .Should()
                     .Throw<ArgumentNullException>();

        Invoking(() => "".FromJson<object>())
                     .Should()
                     .Throw<JsonException>();
    }

    [Fact]
    public void FromJson_GivenJson_DeserializeJson()
    {
        var obj = new
        {
            Value = 1
        };
        var json = JsonSerializer.Serialize(obj);
        var jsonObj = json.FromJson(obj);

        jsonObj.Should().BeEquivalentTo(obj);
    }

    [Fact]
    public void ToJson_GivenNull_ThrowException()
    {
        Invoking(() => (null as object).ToJson())
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToJson_GivenInput_SerializeObject()
    {
        var obj = new
        {
            Value = 1
        };
        var expected = JsonSerializer.Serialize(obj);

        obj.ToJson().Should().Be(expected);
    }
}
