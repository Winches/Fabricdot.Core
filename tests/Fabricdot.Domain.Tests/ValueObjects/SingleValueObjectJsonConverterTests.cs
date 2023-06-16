using System.Text.Json;
using Fabricdot.Domain.ValueObjects;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Domain.Tests.ValueObjects;

public class SingleValueObjectJsonConverterTests : TestFor<SingleValueObjectJsonConverter<Money>>
{
    private readonly JsonSerializerOptions _options;

    public SingleValueObjectJsonConverterTests()
    {
        _options = new JsonSerializerOptions(JsonSerializerDefaults.General);
        _options.Converters.Add(new SingleValueObjectJsonConverterFactory());
    }

    [AutoData]
    [Theory]
    public void Read_Should_Correctly(Money money)
    {
        var json = $"[{money.Value}]";
        var data = JsonSerializer.Deserialize<Money[]>(json, _options);

        data.Should().ContainSingle(money);
    }

    [AutoData]
    [Theory]
    public void Write_Should_Correctly(Money money)
    {
        var expected = $"[{money.Value}]";
        var data = new[] { money };
        var json = JsonSerializer.Serialize(data, _options);

        json.Should().Be(expected);
    }

    [InlineAutoData(typeof(OrderStatus))]
    [InlineAutoData(typeof(Money))]
    [Theory]
    public void CanConvert_Should_Correctly(Type typeToConvert)
    {
        var expected = typeToConvert.IsAssignableToGenericType(typeof(SingleValueObject<>));

        Sut.CanConvert(typeToConvert).Should().Be(expected);
    }
}
