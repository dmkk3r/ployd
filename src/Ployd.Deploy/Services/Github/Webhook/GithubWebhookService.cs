using Octokit;
using Ployd.Deploy.Models.Github;

namespace Ployd.Deploy.Services.Github.Webhook;

public class GithubWebhookService(IGithubClientFactory githubClientFactory) {
    private const string WebhookUrlBase = "/webhooks/github/";
    private const string NameBase = "ployd-webhook-";
    private const string Secret = "secret";

    public async Task CreateWebhookAsync(long repositoryId) {
        var client = githubClientFactory.Create("ployd");

        var githubRepository = await client.Repository.Get(repositoryId);

        var webhookUrl = $"{WebhookUrlBase}{repositoryId}";
        var webhookName = $"{NameBase}{repositoryId}";

        var hooks = await client.Repository.Hooks.GetAll(githubRepository.Id);

        if (hooks.Any(hook => hook.Name == webhookName))
        {
            return;
        }

        var config = new Dictionary<string, string>
        {
            { "url", webhookUrl },
            { "content_type", WebHookContentType.Json.ToString() },
            { "insecure_ssl", "0" },
            {
                "secret", Secret
            },
        };

        await client.Repository.Hooks.Create(githubRepository.Id, new NewRepositoryHook(webhookName, config));
    }

    public async Task ConsumeWebhookAsync(long repositoryId, long webhookId, GithubWebhookResponse response) {
        var client = githubClientFactory.Create("ployd");
        var githubRepository = await client.Repository.Get(repositoryId);
    }
}