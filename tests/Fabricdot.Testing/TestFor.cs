namespace Fabricdot.Testing;

public abstract class TestFor<TSut> : TestBase
{
    private readonly Lazy<TSut> _lazySut;
    protected TSut Sut => _lazySut.Value;

    protected TestFor()
    {
        _lazySut = new Lazy<TSut>(CreateSut);
    }

    protected virtual TSut CreateSut()
    {
        return Create<TSut>();
    }
}

public abstract class TestFor<TSut, TCustmization> : TestBase where TCustmization : ICustomization, new()
{
    private readonly Lazy<TSut> _lazySut;
    protected TSut Sut => _lazySut.Value;

    protected TestFor()
    {
        Fixture.Customize(new TCustmization());
        _lazySut = new Lazy<TSut>(CreateSut);
    }

    protected virtual TSut CreateSut()
    {
        return Create<TSut>();
    }
}
