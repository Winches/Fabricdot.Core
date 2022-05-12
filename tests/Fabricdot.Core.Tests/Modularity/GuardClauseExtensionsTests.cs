using System;
using Ardalis.GuardClauses;
using Fabricdot.Core.Modularity;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity
{
    public class GuardClauseExtensionsTests
    {
        [Fact]
        public void InvalidModuleType_WhenGuardIsNull_ThrowException()
        {
            FluentActions.Invoking(() => ((IGuardClause)null).InvalidModuleType(GetType(), ""))
                         .Should()
                         .Throw<ArgumentException>();
        }

        [Fact]
        public void InvalidModuleType_WhenTypeIsNull_ThrowException()
        {
            FluentActions.Invoking(() => Guard.Against.InvalidModuleType(null, ""))
                         .Should()
                         .Throw<ArgumentNullException>();
        }

        [Fact]
        public void InvalidModuleType_WhenTypeIsInvalid_ThrowException()
        {
            FluentActions.Invoking(() => Guard.Against.InvalidModuleType(GetType(), ""))
                         .Should()
                         .Throw<ArgumentException>();
        }
    }
}