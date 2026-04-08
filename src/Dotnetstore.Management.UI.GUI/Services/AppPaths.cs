namespace Dotnetstore.Management.UI.GUI.Services;

internal static class AppPaths
{
    private const string AppFolderName = "Dotnetstore.Management";
    private const string DatabaseFileName = "management.db";

    public static string DatabaseFilePath
    {
        get
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppFolderName);
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, DatabaseFileName);
        }
    }

    public static string SqliteConnectionString => $"Data Source={DatabaseFilePath}";
}
