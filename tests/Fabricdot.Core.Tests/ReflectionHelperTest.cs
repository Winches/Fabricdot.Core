using System;
using System.Linq;
using Fabricdot.Core.Reflection;
using Xunit;

namespace Fabricdot.Core.Tests
{
    public class ReflectionHelperTest
    {
        internal interface IAnimal
        {
        }

        internal interface IAnimal<in T>
        {
            void Feed(T food);
        }

        internal interface IWolf : IAnimal
        {
        }

        internal interface IRabbit : IAnimal
        {
        }

        internal abstract class WolfBase : IWolf, IAnimal<IRabbit>
        {
            /// <inheritdoc />
            public virtual void Feed(IRabbit food) => throw new NotImplementedException();
        }

        internal class GreyWolf : WolfBase
        {
        }

        internal class Rabbit : IRabbit
        {
        }


        [Fact]
        public void FindTypes_GivenDirectlyInheritedInterface_ReturnDerivedTypes()
        {
            var expected = typeof(GreyWolf);
            var types = ReflectionHelper.FindTypes(typeof(IWolf), typeof(ReflectionHelperTest).Assembly);
            var actual = types.SingleOrDefault();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FindTypes_GivenIndirectlyInheritedInterface_ReturnDerivedTypes()
        {
            var types = ReflectionHelper.FindTypes(typeof(IAnimal), typeof(ReflectionHelperTest).Assembly);
            Assert.Contains(typeof(GreyWolf), types);
            Assert.Contains(typeof(Rabbit), types);
        }

        [Fact]
        public void FindTypes_GivenGenericInterface_ReturnDerivedTypes()
        {
            var types = ReflectionHelper.FindTypes(typeof(IAnimal<>), typeof(ReflectionHelperTest).Assembly);
            var expected = typeof(GreyWolf);
            var actual = types.SingleOrDefault();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FindTypes_GivenDirectlyInheritedClass_ReturnDerivedTypes()
        {
            var expected = typeof(GreyWolf);
            var types = ReflectionHelper.FindTypes(typeof(WolfBase), typeof(ReflectionHelperTest).Assembly);
            var actual = types.SingleOrDefault();
            Assert.Equal(expected, actual);
        }
    }
}