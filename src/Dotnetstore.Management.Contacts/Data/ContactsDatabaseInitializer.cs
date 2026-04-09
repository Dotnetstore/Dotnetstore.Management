using Dotnetstore.Management.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.Data;

internal sealed class ContactsDatabaseInitializer(ContactsDataContext context) : IModuleDatabaseInitializer
{
    public async Task InitializeAsync(
        IProgress<InitializationProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        Report(progress, 0.10, "Preparing contacts database...");
        Report(progress, 0.40, "Running contacts migrations...");
        await context.Database.MigrateAsync(cancellationToken);
        Report(progress, 1.00, "Contacts ready.");
    }

    private static void Report(IProgress<InitializationProgress>? progress, double value, string message)
        => progress?.Report(new InitializationProgress(value, message));
}
