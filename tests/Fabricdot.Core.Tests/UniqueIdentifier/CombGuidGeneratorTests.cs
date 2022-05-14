using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Core.UniqueIdentifier.CombGuid;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.UniqueIdentifier
{
    public class CombGuidGeneratorTests
    {
        [Fact]
        public void Constructor_GivenNull_ThrowException()
        {
            FluentActions.Invoking(() => new CombGuidGenerator(null))
                         .Should()
                         .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_GivenInvalidGuidType_ThrowException()
        {
            FluentActions.Invoking(() => GuidFactories.Comb.Create(99.To<CombGuidType>()))
                         .Should()
                         .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Create_WhenSequentialAsString_ReturnCorrectly()
        {
            var list = new List<Guid>();
            100.Times(_ =>
            {
                var guid = GuidFactories.SafetyComb.Create(CombGuidType.SequentialAsString);
                list.Add(guid);
            });

            list.OrderBy(v => v.ToString()).Should().BeEquivalentTo(list);
        }

        [Fact]
        public void Create_WhenSequentialAsBinary_ReturnCorrectly()
        {
            var list = new List<Guid>();
            100.Times(_ =>
            {
                var guid = GuidFactories.SafetyComb.Create(CombGuidType.SequentialAsBinary);
                list.Add(guid);
            });

            list.OrderBy(v => Encoding.UTF8.GetString(v.ToByteArray())).Should().BeEquivalentTo(list);
        }

        [Fact]
        public void Create_WhenSequentialAtEnd_ReturnCorrectly()
        {
            var list = new List<Guid>();
            100.Times(_ =>
            {
                var guid = GuidFactories.SafetyComb.Create(CombGuidType.SequentialAtEnd);
                list.Add(guid);
            });

            list.OrderBy(v => v.ToString("N").Substring(16, 12)).Should().BeEquivalentTo(list);
        }
    }
}