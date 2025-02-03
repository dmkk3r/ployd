namespace Module.Resource.Contract;

public static class ResourceTypesUi
{
    public static ResourceTypeUi Container => new()
    {
        Id = ResourceTypes.Container,
        Name = "Container",
        Icon = "/logos/oci.png",
        Description = "Choose to build the resource as a container.",
        Group = "Container"
    };

    public static ResourceTypeUi DockerCompose => new()
    {
        Id = ResourceTypes.DockerCompose,
        Name = "Docker Compose",
        Icon = "/logos/docker.png",
        Description = "Choose to build the resource as a Docker Compose stack.",
        Group = "Stack"
    };

    public static ResourceTypeUi PodmanCompose => new()
    {
        Id = ResourceTypes.PodmanCompose,
        Name = "Podman Compose",
        Icon = "/logos/podman.png",
        Description = "Choose to build the resource as a Podman Compose stack.",
        Group = "Stack"
    };

    public static ResourceTypeUi WebAssembly => new()
    {
        Id = ResourceTypes.WebAssembly,
        Name = "WebAssembly",
        Icon = "/logos/webassembly.png",
        Description = "Choose a WebAssembly module to build the resource.",
        Group = "WebAssembly"
    };

    public static List<ResourceTypeUi> All()
    {
        return
        [
            Container,
            DockerCompose,
            PodmanCompose,
            WebAssembly
        ];
    }

    public static Dictionary<string, List<ResourceTypeUi>> AllGrouped()
    {
        return new Dictionary<string, List<ResourceTypeUi>>
        {
            {
                "Container", [
                    Container
                ]
            },
            {
                "Stack", [
                    DockerCompose,
                    PodmanCompose
                ]
            },
            {
                "WebAssembly", [
                    WebAssembly
                ]
            }
        };
    }
}
