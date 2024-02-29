using Octokit;

namespace Ployd.Peasant.Services.Github;

public interface IGithubClientFactory {
    GitHubClient Create(string ressource);
}