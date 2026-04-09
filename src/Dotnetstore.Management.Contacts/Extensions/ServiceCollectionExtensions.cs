using Dotnetstore.Management.Contacts.CompanyCustomers;
using Dotnetstore.Management.Contacts.Data;
using Dotnetstore.Management.Contacts.PersonCustomers;
using Dotnetstore.Management.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnetstore.Management.Contacts.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContactsModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ContactsDataContext>(options =>
            options.UseSqlite(connectionString, b =>
                b.MigrationsHistoryTable("__EFMigrationsHistory_Contacts")));

        services
            .AddScoped<IPersonCustomerRepository, PersonCustomerRepository>()
            .AddScoped<ICompanyCustomerRepository, CompanyCustomerRepository>()
            .AddScoped<IPersonCustomerService, PersonCustomerService>()
            .AddScoped<ICompanyCustomerService, CompanyCustomerService>()
            .AddScoped<IModuleDatabaseInitializer, ContactsDatabaseInitializer>();

        return services;
    }
}
