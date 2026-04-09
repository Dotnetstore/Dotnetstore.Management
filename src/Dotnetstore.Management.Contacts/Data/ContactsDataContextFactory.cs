using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dotnetstore.Management.Contacts.Data;

/// <summary>
/// Used by the EF Core CLI tooling. Not consumed at runtime — the application
/// configures the context via DI in ServiceCollectionExtensions.
/// </summary>
internal sealed class ContactsDataContextFactory : IDesignTimeDbContextFactory<ContactsDataContext>
{
    public ContactsDataContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ContactsDataContext>()
            .UseSqlite("Data Source=design-time.db", b =>
                b.MigrationsHistoryTable("__EFMigrationsHistory_Contacts"))
            .Options;
        return new ContactsDataContext(options);
    }
}
