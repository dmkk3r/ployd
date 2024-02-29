using Octokit;

namespace Ployd.Peasant.Services.Github;

public class GithubClientFactory(IConfiguration configuration) : IGithubClientFactory {
    public GitHubClient Create(string ressource) {
        var productInformation = new ProductHeaderValue(ressource);

        var client = new GitHubClient(productInformation)
        {
            Credentials = new Credentials(configuration["GH_ACCESS_TOKEN"])
        };

        return client;
    }
}