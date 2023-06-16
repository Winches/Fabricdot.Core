using System.Text.Json;
using Fabricdot.Core.Validation;

namespace Fabricdot.Core.Tests.Validation;

public class NotificationErrorConverterTests : TestFor<Notification.Error>
{
    [Fact]
    public void Write_SerializeJson_Correctly()
    {
        var json = JsonSerializer.Serialize(new[] { Sut });
        var expected = $"[\"{Sut}\"]";

        json.Should().Be(expected);
    }

    [Fact]
    public void Read_DeserializeJson_Correctly()
    {
        var json = $"[\"{Sut}\"]";
        var data = JsonSerializer.Deserialize<Notification.Error[]>(json);

        data.Should().BeEquivalentTo(new[] { Sut });
    }
}
