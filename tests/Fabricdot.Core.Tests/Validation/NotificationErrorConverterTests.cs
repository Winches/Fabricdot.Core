using System.Text.Json;
using Fabricdot.Core.Validation;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Validation;

public class NotificationErrorConverterTests
{
    [Fact]
    public void Write_SerializeJson_Correctly()
    {
        var error = new Notification.Error("error");
        var json = JsonSerializer.Serialize(new[] { error });
        var expected = $"[\"{error}\"]";

        json.Should().Be(expected);
    }

    [Fact]
    public void Read_DeserializeJson_Correctly()
    {
        var error = new Notification.Error("error");
        var json = $"[\"{error}\"]";
        var data = JsonSerializer.Deserialize<Notification.Error[]>(json);

        data.Should().BeEquivalentTo(new[] { error });
    }
}