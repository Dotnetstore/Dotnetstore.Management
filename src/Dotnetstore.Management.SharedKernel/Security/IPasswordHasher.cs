namespace Dotnetstore.Management.SharedKernel.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(string password);
    bool Verify(string password, byte[] hash, byte[] salt);
}

public readonly record struct PasswordHash(byte[] Hash, byte[] Salt);
