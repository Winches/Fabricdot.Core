using System.Text.Json;
using Fabricdot.Domain.ValueObjects;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class EnumerationJsonConverterTests : TestFor<EnumerationJsonConverter<OrderStatus>>
{
    private readonly JsonSerializerOptions _options;

    public EnumerationJsonConverterTests()
    {
        _options = new JsonSerializerOptions(JsonSerializerDefaults.General);
        _options.Converters.Add(new EnumerationJsonConverterFactory());
    }

    [Fact]
    public void Read_Should_Correctly()
    {
        var valObj = OrderStatus.Completed;
        var json = $"[{valObj.Value}]";
        var data = JsonSerializer.Deserialize<OrderStatus[]>(json, _options);

        data.Should().ContainSingle(valObj);
    }

    [Fact]
    public void Write_Should_Correctly()
    {
        var valObj = OrderStatus.Completed;
        var expected = $"[{valObj.Value}]";
        var data = new[] { valObj };
        var json = JsonSerializer.Serialize(data, _options);

        json.Should().Be(expected);
    }

    [InlineAutoData(typeof(OrderStatus))]
    [InlineAutoData(typeof(Money))]
    [Theory]
    public void CanConvert_Should_Correctly(Type typeToConvert)
    {
        var expected = typeToConvert.IsAssignableTo(typeof(Enumeration));

        Sut.CanConvert(typeToConvert).Should().Be(expected);
    }
}
