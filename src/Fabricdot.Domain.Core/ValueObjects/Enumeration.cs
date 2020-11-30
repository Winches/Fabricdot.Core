using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fabricdot.Domain.Core.ValueObjects
{
    /// <summary>
    ///     Enum object
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        public string Name { get; }

        public int Value { get; }

        protected Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration otherValue))
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            return left?.Equals(right) ?? false;
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !left?.Equals(right) ?? false;
        }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        private static T Parse<T, TK>(
            TK value,
            string description,
            Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }
    }
}