namespace Dotnetstore.Management.SharedKernel.Persistence;

public interface IModuleDatabaseInitializer
{
    /// <summary>
    /// Runs migrations and seeds required data for a single module.
    /// Reports progress as a value between 0.0 and 1.0 scoped to this module.
    /// </summary>
    Task InitializeAsync(IProgress<InitializationProgress>? progress = null, CancellationToken cancellationToken = default);
}

public readonly record struct InitializationProgress(double Value, string Message);
