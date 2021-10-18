using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.UniqueIdentifier.CombGuid
{
    //From:https://www.codeproject.com/Articles/388157/GUIDs-as-fast-primary-keys-under-multiple-database
    public class CombGuidGenerator
    {
        private const int RandomBytesLength = 10;

        private const int TimestampBytesLength = 6;

        private readonly ITimestampProvider _timestampProvider;

        public CombGuidGenerator(ITimestampProvider timestampProvider)
        {
            _timestampProvider = Guard.Against.Null(timestampProvider, nameof(timestampProvider));
        }

        public Guid Create(CombGuidType guidType)
        {
            var isReversed = guidType == CombGuidType.SequentialAtEnd;
            var partition = isReversed ? RandomBytesLength : TimestampBytesLength;

            var guid = GetGuidBytes(partition, out var low, out var high);
            var timestamp = GetTimestampBytes().Slice(2, TimestampBytesLength);

            switch (guidType)
            {
                case CombGuidType.SequentialAsString:
                    timestamp.CopyTo(low);
                    if (BitConverter.IsLittleEndian)
                    {
                        low.Slice(0, 4).Reverse();
                        low.Slice(4, 2).Reverse();
                    }

                    break;

                case CombGuidType.SequentialAsBinary:
                    timestamp.CopyTo(low);

                    break;

                case CombGuidType.SequentialAtEnd:
                    timestamp.CopyTo(high);

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(guidType));
            }

            return new Guid(guid);
        }

        protected virtual Span<byte> GetTimestampBytes()
        {
            var timestamp = _timestampProvider.GetTimestamp();
            var timestampBytes = BitConverter.GetBytes(timestamp).AsSpan();

            if (BitConverter.IsLittleEndian)
            {
                timestampBytes.Reverse();
            }

            return timestampBytes;
        }

        private static Span<byte> GetGuidBytes(
                    int partition,
            out Span<byte> low,
            out Span<byte> high)
        {
            var guidBytes = Guid.NewGuid().ToByteArray().AsSpan();
            low = guidBytes.Slice(0, partition);
            high = guidBytes.Slice(partition);

            return guidBytes;
        }
    }
}