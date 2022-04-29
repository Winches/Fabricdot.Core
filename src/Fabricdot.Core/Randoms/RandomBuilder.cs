using System;
using System.Text;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Randoms
{
    [Dependency(ServiceLifetime.Singleton)]
    public class RandomBuilder : IRandomBuilder
    {
        private const string NUMBER = "0123456789";
        private const string LETTER = "abcdefghijklmnopqrstuvwxyz";
        private readonly IRandomProvider _randomProvider;

        public RandomBuilder(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public string GetRandomString(string source, int length)
        {
            Guard.Against.NullOrEmpty(source, nameof(source));
            Guard.Against.NegativeOrZero(length, nameof(length));

            var builder = new StringBuilder();
            length.Times(_ => builder.Append(GetRandomChar(source)));
            return builder.ToString();
        }

        public string GetRandomNumbers(int length)
        {
            return GetRandomString(NUMBER, length);
        }

        public string GetRandomLetters(int length)
        {
            return GetRandomString(LETTER, length);
        }

        private char GetRandomChar(string source)
        {
            var index = _randomProvider.Next(0, source.Length);
            return source[index];
        }
    }
}