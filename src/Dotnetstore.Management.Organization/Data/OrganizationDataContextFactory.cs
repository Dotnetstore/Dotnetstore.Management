using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dotnetstore.Management.Organization.Data;

/// <summary>
/// Used by the EF Core CLI tooling (dotnet ef migrations add / database update).
/// Not consumed at runtime — the application configures the context via DI.
/// </summary>
internal sealed class OrganizationDataContextFactory : IDesignTimeDbContextFactory<OrganizationDataContext>
{
    public OrganizationDataContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<OrganizationDataContext>()
            .UseSqlite("Data Source=design-time.db", b =>
                b.MigrationsHistoryTable("__EFMigrationsHistory_Organization"))
            .Options;
        return new OrganizationDataContext(options);
    }
}
