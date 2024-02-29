using System.IO.Compression;
using Octokit;

namespace Ployd.Peasant.Services.Github.Repository;

public class GithubRepositoryService(IGithubClientFactory githubClientFactory) {
    public async Task<long> GetRepositoryIdAsync(Uri repositoryUrl) {
        var client = githubClientFactory.Create("ployd");
        var (owner, name) = ParseRepositoryUrl(repositoryUrl);

        var repository = await client.Repository.Get(owner, name);
        return repository.Id;
    }

    public async Task<string> CloneAsync(long repositoryId) {
        var client = githubClientFactory.Create("ployd");
        var githubRepository = await client.Repository.Get(repositoryId);

        var contents = await client.Repository.Content.GetArchive(githubRepository.Id, ArchiveFormat.Zipball);

        var basePath = Path.Combine(Path.GetTempPath(), "ployd");
        var repositoryPath = Path.Combine(basePath, repositoryId.ToString());

        var zipFile = Path.Combine(repositoryPath, $"{githubRepository.Owner.Login}-{githubRepository.Name}.zip");

        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        if (Directory.Exists(repositoryPath))
        {
            Directory.Delete(repositoryPath, true);
        }

        Directory.CreateDirectory(repositoryPath);

        if (File.Exists(zipFile))
        {
            File.Delete(zipFile);
        }

        await File.WriteAllBytesAsync(zipFile, contents);

        ZipFile.ExtractToDirectory(zipFile, repositoryPath);

        File.Delete(zipFile);

        var directories = Directory.GetDirectories(repositoryPath);
        var extractedDirectory = directories.First();

        return extractedDirectory;
    }

    private static (string owner, string name) ParseRepositoryUrl(Uri repositoryUrl) {
        var segments = repositoryUrl.Segments;

        var owner = segments[1].Trim('/');
        var name = segments[2].Trim('/');

        return (owner, name);
    }
}