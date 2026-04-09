using Dotnetstore.Management.Contacts.Data;
using Dotnetstore.Management.Contacts.Tests.Infrastructure;
using Dotnetstore.Management.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Dotnetstore.Management.Contacts.Tests.Data;

public class ContactsDatabaseInitializerTests
{
    [Fact]
    public async Task InitializeAsync_applies_migrations_and_reports_progress()
    {
        using var db = new SqliteTestDatabase();
        // Drop the migrated schema so the initializer re-creates it.
        await db.Context.Database.EnsureDeletedAsync();

        var reports = new List<InitializationProgress>();
        var initializer = new ContactsDatabaseInitializer(db.Context);

        await initializer.InitializeAsync(new Progress<InitializationProgress>(reports.Add));

        // give Progress callbacks time to drain
        await Task.Delay(50);

        reports.ShouldNotBeEmpty();
        reports[^1].Value.ShouldBe(1.0);
        (await db.Context.Database.GetAppliedMigrationsAsync()).ShouldNotBeEmpty();
    }

    [Fact]
    public async Task InitializeAsync_is_idempotent()
    {
        using var db = new SqliteTestDatabase();
        var initializer = new ContactsDatabaseInitializer(db.Context);

        await initializer.InitializeAsync();
        await initializer.InitializeAsync();

        (await db.Context.Database.GetAppliedMigrationsAsync()).ShouldNotBeEmpty();
    }
}
