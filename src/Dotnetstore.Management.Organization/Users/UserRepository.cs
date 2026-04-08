using Dotnetstore.Management.Organization.Data;
using Dotnetstore.Management.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Organization.Users;

internal sealed class UserRepository(OrganizationDataContext context)
    : GenericRepository<User>(context), IUserRepository
{
    public ValueTask<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return new ValueTask<User?>(
            context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken));
    }
}
