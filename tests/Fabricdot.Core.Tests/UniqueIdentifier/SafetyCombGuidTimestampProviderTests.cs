using System;
using System.Collections.Generic;
using Fabricdot.Core.UniqueIdentifier.CombGuid;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.UniqueIdentifier;

public class SafetyCombGuidTimestampProviderTests
{
    [Fact]
    public void GetTimestamp_ReturnUniqueTimestamp()
    {
        var timestampList = new List<long>();
        500.Times(_ =>
        {
            var timestamp = SafetyCombGuidTimestampProvider.Instance.GetTimestamp();
            timestampList.Add(timestamp);
        });

        timestampList.Should().OnlyHaveUniqueItems();
    }
}