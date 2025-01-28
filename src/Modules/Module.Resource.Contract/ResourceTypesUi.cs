namespace Module.Resource.Contract;

public static class ResourceTypesUi
{
    public static ResourceTypeUi Dockerfile => new()
    {
        Id = ResourceTypes.Dockerfile,
        Name = "Dockerfile",
        Icon = "/logos/docker.png",
        Description = "Choose a Dockerfile to build the resource."
    };

    public static ResourceTypeUi DockerCompose => new()
    {
        Id = ResourceTypes.DockerCompose,
        Name = "Docker Compose",
        Icon = "/logos/docker.png",
        Description = "Choose a Docker Compose file to build the resource."
    };

    public static ResourceTypeUi PodmanCompose => new()
    {
        Id = ResourceTypes.PodmanCompose,
        Name = "Podman Compose",
        Icon = "/logos/podman.png",
        Description = "Choose a Podman Compose file to build the resource."
    };

    public static ResourceTypeUi OciImage => new()
    {
        Id = ResourceTypes.OciImage,
        Name = "OCI Image",
        Icon = "/logos/oci.png",
        Description = "Choose an OCI compliant image to build the resource."
    };

    public static ResourceTypeUi WebAssembly => new()
    {
        Id = ResourceTypes.WebAssembly,
        Name = "WebAssembly",
        Icon = "/logos/webassembly.png",
        Description = "Choose a WebAssembly module to build the resource."
    };

    public static List<ResourceTypeUi> All()
    {
        return
        [
            Dockerfile,
            DockerCompose,
            PodmanCompose,
            OciImage,
            WebAssembly
        ];
    }
}
