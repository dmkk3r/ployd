namespace Module.Resource.Contract;

public static class ResourceTypesUi
{
    public static ResourceTypeUi Dockerfile => new()
    {
        Id = ResourceTypes.Dockerfile, Name = "Dockerfile", Icon = "/logos/docker.png"
    };

    public static ResourceTypeUi DockerCompose => new()
    {
        Id = ResourceTypes.DockerCompose, Name = "Docker Compose", Icon = "/logos/docker.png"
    };

    public static ResourceTypeUi PodmanCompose => new()
    {
        Id = ResourceTypes.PodmanCompose, Name = "Podman Compose", Icon = "/logos/podman.png"
    };

    public static ResourceTypeUi OciImage => new()
    {
        Id = ResourceTypes.OciImage, Name = "OCI Image", Icon = "/logos/oci.png"
    };

    public static ResourceTypeUi WebAssembly => new()
    {
        Id = ResourceTypes.WebAssembly, Name = "WebAssembly", Icon = "/logos/webassembly.png"
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
