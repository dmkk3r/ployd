namespace Module.Source.Contract;

public static class SourceTypesUi
{
    public static SourceTypeUi Git => new() { Id = SourceTypes.Git, Name = "Git", Icon = "/logos/git.png" };
    public static SourceTypeUi GitHub => new() { Id = SourceTypes.GitHub, Name = "GitHub", Icon = "/logos/github.png" };
    public static SourceTypeUi GitLab => new() { Id = SourceTypes.GitLab, Name = "GitLab", Icon = "/logos/gitlab.png" };

    public static SourceTypeUi DockerHub =>
        new() { Id = SourceTypes.DockerHub, Name = "Docker Hub", Icon = "/logos/docker.png" };

    public static SourceTypeUi Ghcr =>
        new() { Id = SourceTypes.Ghcr, Name = "GitHub Container Registry", Icon = "/logos/github.png" };

    public static List<SourceTypeUi> All()
    {
        return
        [
            Git,
            GitHub,
            GitLab,
            DockerHub,
            Ghcr
        ];
    }
}
