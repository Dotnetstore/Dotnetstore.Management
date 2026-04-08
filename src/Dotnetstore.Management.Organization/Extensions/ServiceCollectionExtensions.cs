using Dotnetstore.Management.Organization.Authentication;
using Dotnetstore.Management.Organization.Data;
using Dotnetstore.Management.Organization.Services;
using Dotnetstore.Management.Organization.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnetstore.Management.Organization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrganizationModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<OrganizationDataContext>(options =>
            options.UseSqlite(connectionString));

        services
            .AddScoped<IOrganizationUnitOfWork, OrganizationUnitOfWork>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IDatabaseInitializer, OrganizationDatabaseInitializer>();

        return services;
    }
}
