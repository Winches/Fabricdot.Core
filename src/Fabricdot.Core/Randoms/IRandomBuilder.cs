namespace Fabricdot.Core.Randoms;

public interface IRandomBuilder
{
    /// <summary>
    ///     Get random string by given text
    /// </summary>
    /// <param name="source">source text</param>
    /// <param name="length">result length</param>
    /// <returns></returns>
    string GetRandomString(string source, int length);

    /// <summary>
    ///     Get random numbers(0-9)
    /// </summary>
    /// <param name="length">result length</param>
    /// <returns></returns>
    string GetRandomNumbers(int length);

    /// <summary>
    ///     Get random letters
    /// </summary>
    /// <param name="length">result length</param>
    /// <returns></returns>
    string GetRandomLetters(int length);
}