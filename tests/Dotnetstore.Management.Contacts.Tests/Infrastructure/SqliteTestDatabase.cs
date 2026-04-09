using Dotnetstore.Management.Contacts.Data;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.Tests.Infrastructure;

internal sealed class SqliteTestDatabase : IDisposable
{
    private readonly string _path;

    public ContactsDataContext Context { get; }

    public SqliteTestDatabase()
    {
        _path = Path.Combine(Path.GetTempPath(), $"dnsmgmt-contacts-test-{Guid.NewGuid():N}.db");
        Context = CreateContext();
        Context.Database.Migrate();
    }

    public ContactsDataContext CreateSecondaryContext() => CreateContext();

    private ContactsDataContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ContactsDataContext>()
            .UseSqlite($"Data Source={_path}")
            .Options;
        return new ContactsDataContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        if (File.Exists(_path))
        {
            try { File.Delete(_path); } catch { /* best effort */ }
        }
    }
}
