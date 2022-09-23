using System.Text;
using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace System.IO;

public static class StreamExtensions
{
    public static async Task<byte[]> GetBytesAsync(this Stream stream)
    {
        Guard.Against.Null(stream, nameof(stream));

        stream.Seek(0, SeekOrigin.Begin);
        var data = new byte[stream.Length];
        await stream.ReadAsync(data);
        return data;
    }

    public static async Task<string> GetStringAsync(
        this Stream stream,
        Encoding? encoding = null)
    {
        Guard.Against.Null(stream, nameof(stream));

        encoding ??= Encoding.UTF8;
        using var streamReader = new StreamReader(stream, encoding, true);
        return await streamReader.ReadToEndAsync();
    }
}