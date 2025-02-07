namespace Module.Source.Contract;

public static class SourceTypes
{
    public static Guid Git => new(GitId);
    public static Guid GitHub => new(GitHubId);
    public static Guid GitLab => new(GitLabId);
    public static Guid DockerHub => new(DockerHubId);
    public static Guid Ghcr => new(GhcrId);

    public const string GitId = "c2d684bd-2044-417a-9e46-aa7287d5aba8";
    public const string GitHubId = "ca7043c2-e23b-4eba-b7f2-c4009af6c7be";
    public const string GitLabId = "fede047f-8ef3-4048-be1c-2284b8b8930d";
    public const string DockerHubId = "1692a46a-cb75-470f-b042-78ec9b938e1f";
    public const string GhcrId = "fc189c6f-b705-41d4-a798-e7f5e00d774d";
}
