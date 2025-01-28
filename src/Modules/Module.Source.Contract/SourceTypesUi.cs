﻿namespace Module.Source.Contract;

public static class SourceTypesUi
{
    public static SourceTypeUi Git => new()
    {
        Id = SourceTypes.Git,
        Name = "Git",
        Icon = "/logos/git.png",
        Description = "Clone the repository from the configured Git source."
    };

    public static SourceTypeUi GitHub => new()
    {
        Id = SourceTypes.GitHub,
        Name = "GitHub",
        Icon = "/logos/github.png",
        Description = "Clone the repository from the configured GitHub source."
    };

    public static SourceTypeUi GitLab => new()
    {
        Id = SourceTypes.GitLab,
        Name = "GitLab",
        Icon = "/logos/gitlab.png",
        Description = "Clone the repository from the configured GitLab source."
    };

    public static SourceTypeUi DockerHub =>
        new()
        {
            Id = SourceTypes.DockerHub,
            Name = "Docker Hub",
            Icon = "/logos/docker.png",
            Description = "Pull the image from Docker Hub."
        };

    public static SourceTypeUi Ghcr =>
        new()
        {
            Id = SourceTypes.Ghcr,
            Name = "GitHub Container Registry",
            Icon = "/logos/github.png",
            Description = "Pull the image from GitHub Container Registry."
        };

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
