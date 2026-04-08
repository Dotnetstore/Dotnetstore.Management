using Dotnetstore.Management.SharedKernel.Services;

namespace Dotnetstore.Management.Organization.Users;

internal interface IUserRepository : IGenericRepository<User>
{
    ValueTask<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}