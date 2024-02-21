using Octokit;

namespace Ployd.Deploy.Services.Github;

public class GithubClientFactory : IGithubClientFactory {
    private const string AccessToken = "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";

    public GitHubClient Create(string ressource) {
        var productInformation = new ProductHeaderValue(ressource);

        var client = new GitHubClient(productInformation)
        {
            Credentials = new Credentials(AccessToken)
        };

        return client;
    }
}