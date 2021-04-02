using Fabricdot.Core.Reflection;
using Xunit;

namespace UnitTests.Common.Reflections
{
    public class ReflectionTest
    {
        internal interface IAnimal
        {
        }

        internal interface IWorkDog<T> : IAnimal
        {
            T Code { get; set; }
        }

        internal class Dog : IAnimal
        {
        }

        internal abstract class WorkDogBase<T> : Dog, IWorkDog<T>
        {
            public T Code { get; set; }
        }

        internal abstract class WorkDogBase : WorkDogBase<int>
        {
        }

        internal class GuidDogWithNumberCode : WorkDogBase
        {
        }

        internal class GuidDogWithCharacterCode : WorkDogBase<string>
        {
        }

        [Fact]
        public void TestFindTypesByInterface()
        {
            var types = ReflectionHelper.FindTypes<IAnimal>(typeof(ReflectionTest).Assembly);
            Assert.Equal(3, types.Count);
            Assert.Contains(types, v => v == typeof(Dog));
            Assert.Contains(types, v => v == typeof(GuidDogWithCharacterCode));
            Assert.Contains(types, v => v == typeof(GuidDogWithNumberCode));
        }

        [Fact]
        public void TestFindTypesByGenericInterface()
        {
            var types = ReflectionHelper.FindTypes(typeof(IWorkDog<>), typeof(ReflectionTest).Assembly);
            Assert.Equal(2, types.Count);
            Assert.Contains(types, v => v == typeof(GuidDogWithCharacterCode));
            Assert.Contains(types, v => v == typeof(GuidDogWithNumberCode));
        }

        [Fact]
        public void TestFindTypesByClass()
        {
            var types = ReflectionHelper.FindTypes(typeof(WorkDogBase), typeof(ReflectionTest).Assembly);
            Assert.Single(types);
            Assert.Contains(types, v => v == typeof(GuidDogWithNumberCode));
        }
    }
}