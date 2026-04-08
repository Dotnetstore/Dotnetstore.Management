using Dotnetstore.Management.Organization.Data;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Organization.Tests.Infrastructure;

/// <summary>
/// Creates a real file-based SQLite database for a test and deletes it on dispose.
/// We intentionally avoid EF InMemory — too much relational behavior (FKs, unique
/// indexes, migrations) is lost and would mask real bugs.
/// </summary>
internal sealed class SqliteTestDatabase : IDisposable
{
    private readonly string _path;

    public OrganizationDataContext Context { get; }

    public SqliteTestDatabase()
    {
        _path = Path.Combine(Path.GetTempPath(), $"dnsmgmt-test-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<OrganizationDataContext>()
            .UseSqlite($"Data Source={_path}")
            .Options;
        Context = new OrganizationDataContext(options);
        Context.Database.Migrate();
    }

    public OrganizationDataContext CreateSecondaryContext()
    {
        var options = new DbContextOptionsBuilder<OrganizationDataContext>()
            .UseSqlite($"Data Source={_path}")
            .Options;
        return new OrganizationDataContext(options);
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
