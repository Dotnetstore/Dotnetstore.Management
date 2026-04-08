using Dotnetstore.Management.Organization.Users;
using Dotnetstore.Management.SharedKernel.Security;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Organization.Data;

internal sealed class OrganizationDatabaseInitializer(
    OrganizationDataContext context,
    IPasswordHasher passwordHasher) : IDatabaseInitializer
{
    private const string InitialEmail = "hasse29@hotmail.com";
    private const string InitialFirstname = "Hans";
    private const string InitialLastname = "Sjödin";
    private const string InitialPassword = "admin";

    public async Task InitializeAsync(
        IProgress<InitializationProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        Report(progress, 0.05, "Preparing database...");
        await Task.Delay(150, cancellationToken);

        Report(progress, 0.20, "Running migrations...");
        await context.Database.MigrateAsync(cancellationToken);

        Report(progress, 0.70, "Seeding data...");
        await SeedInitialUserAsync(cancellationToken);

        Report(progress, 1.00, "Ready.");
    }

    private async Task SeedInitialUserAsync(CancellationToken cancellationToken)
    {
        var exists = await context.Users
            .AnyAsync(u => u.Email == InitialEmail, cancellationToken);
        if (exists) return;

        var hash = passwordHasher.Hash(InitialPassword);
        var user = new User
        {
            Firstname = InitialFirstname,
            Lastname = InitialLastname,
            Email = InitialEmail,
            PasswordHash = hash.Hash,
            PasswordSalt = hash.Salt,
            IsActive = true,
        };

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static void Report(IProgress<InitializationProgress>? progress, double value, string message)
        => progress?.Report(new InitializationProgress(value, message));
}
