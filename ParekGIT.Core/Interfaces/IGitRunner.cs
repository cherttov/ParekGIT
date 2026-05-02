namespace ParekGIT.Core.Interfaces
{
    internal interface IGitRunner
    {
        Task<string> ExecuteCommandAsync(string repositoryPath, string arguments);
    }
}
