namespace Module.Resource.Contract;

public static class ResourceTypesUi
{
    public static ResourceTypeUi Container => new()
    {
        Id = ResourceTypes.Container,
        Name = "Container",
        Icon = "/logos/oci.png",
        Description = "Choose to build the resource as a container."
    };

    public static ResourceTypeUi DockerCompose => new()
    {
        Id = ResourceTypes.DockerCompose,
        Name = "Docker Compose",
        Icon = "/logos/docker.png",
        Description = "Choose to build the resource as a Docker Compose stack."
    };

    public static ResourceTypeUi PodmanCompose => new()
    {
        Id = ResourceTypes.PodmanCompose,
        Name = "Podman Compose",
        Icon = "/logos/podman.png",
        Description = "Choose to build the resource as a Podman Compose stack."
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
            Container,
            DockerCompose,
            PodmanCompose,
            Container,
            WebAssembly
        ];
    }

    public static List<ResourceTypeUi> DockerBased()
    {
        return
        [
            Container,
            DockerCompose,
            PodmanCompose
        ];
    }

    public static List<ResourceTypeUi> WebAssemblyBased()
    {
        return
        [
            WebAssembly
        ];
    }
}
