namespace Fabricdot.Core.Randoms;

public interface IRandomProvider
{
    int Next();

    int Next(int min, int max);
}
