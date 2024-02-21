using Octokit;

namespace Ployd.Deploy.Services.Github;

public interface IGithubClientFactory {
    GitHubClient Create(string ressource);
}