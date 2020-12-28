using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fabricdot.Common.Core.IO
{
    public static class StreamExtensions
    {
        public static async Task<byte[]> GetBytesAsync(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            stream.Seek(0, SeekOrigin.Begin);
            var data = new byte[stream.Length];
            await stream.ReadAsync(data, 0, data.Length);
            return data;
        }

        public static async Task<string> GetStringAsync(this Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var streamReader = new StreamReader(stream, encoding, true);
            return await streamReader.ReadToEndAsync();
        }
    }
}