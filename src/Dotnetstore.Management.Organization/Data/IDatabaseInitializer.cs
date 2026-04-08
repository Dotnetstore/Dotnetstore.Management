namespace Dotnetstore.Management.Organization.Data;

public interface IDatabaseInitializer
{
    /// <summary>
    /// Runs migrations and seeds required data. Reports progress as a value between 0.0 and 1.0.
    /// </summary>
    Task InitializeAsync(IProgress<InitializationProgress>? progress = null, CancellationToken cancellationToken = default);
}

public readonly record struct InitializationProgress(double Value, string Message);
