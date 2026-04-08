using Dotnetstore.Management.SharedKernel.Domain;

namespace Dotnetstore.Management.Organization.Users;

internal sealed class User : BaseEntity
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public bool IsActive { get; set; } = true;
}
