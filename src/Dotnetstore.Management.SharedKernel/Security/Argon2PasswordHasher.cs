using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Dotnetstore.Management.SharedKernel.Security;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 64;
    private const int DegreeOfParallelism = 8;
    private const int Iterations = 4;
    private const int MemorySize = 1024 * 64; // 64 MB

    public PasswordHash Hash(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = ComputeHash(password, salt);
        return new PasswordHash(hash, salt);
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        if (string.IsNullOrEmpty(password) || hash is null || salt is null)
            return false;

        var computed = ComputeHash(password, salt);
        return CryptographicOperations.FixedTimeEquals(computed, hash);
    }

    private static byte[] ComputeHash(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize,
        };
        return argon2.GetBytes(HashSize);
    }
}
