﻿using System;

namespace Fabricdot.Infrastructure.Core.Tracing
{
    public readonly struct CorrelationId
    {
        public string Value { get; }

        private CorrelationId(string value)
        {
            Value = value;
        }

        private CorrelationId(Guid guid)
        {
            Value = guid.ToString("N");
        }

        /// <inheritdoc />
        public override string ToString() => Value;

        public bool Equals(CorrelationId other)
        {
            return Value == other.Value;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CorrelationId other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public static CorrelationId New() => new CorrelationId(Guid.NewGuid().ToString("N"));

        public static implicit operator CorrelationId(string value)
        {
            return Guid.TryParse(value, out var guid) ? new CorrelationId(guid) : throw new ArgumentException("Invalid Correlation Id.", nameof(value));
        }

        public static implicit operator CorrelationId(Guid guid) => new CorrelationId(guid);
    }
}