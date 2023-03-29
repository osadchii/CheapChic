namespace CheapChic.Infrastructure.Extensions;

public static class ByteArrayExtensions
{
    public static string GetHashSha512(this byte[] data)
    {
        return string.Concat(System.Security.Cryptography.SHA512.HashData(data).Select(x => x.ToString("X2")));
    }
}