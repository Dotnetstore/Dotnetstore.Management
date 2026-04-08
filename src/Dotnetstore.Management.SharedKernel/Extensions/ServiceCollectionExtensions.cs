using Dotnetstore.Management.SharedKernel.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnetstore.Management.SharedKernel.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        return services;
    }
}
