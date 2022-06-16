namespace Fabricdot.Testing;

public abstract class TestBase
{
    protected IFixture Fixture { get; }

    protected TestBase()
    {
        Fixture = FixtureFactory.Create();
    }

    protected virtual T Create<T>() => Fixture.Create<T>();

    protected virtual Mock<T> Mock<T>() where T : class => Create<Mock<T>>();

    protected virtual T Inject<T>(T instance) where T : class
    {
        Fixture.Inject(instance);
        return instance;
    }

    protected Mock<T> InjectMock<T>(params object[] args) where T : class
    {
        var mock = new Mock<T>(args);
        Fixture.Inject(mock.Object);
        return mock;
    }
}
