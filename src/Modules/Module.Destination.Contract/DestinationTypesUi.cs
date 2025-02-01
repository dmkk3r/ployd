namespace Module.Destination.Contract;

public static class DestinationTypesUi
{
    public static DestinationTypeUi DockerContainer =>
        new()
        {
            Id = DestinationTypes.DockerContainer,
            Name = "Docker Container",
            Icon = "/logos/docker.png",
            Description = "Deploy the resource to the configured Docker destination."
        };

    public static DestinationTypeUi PodmanContainer =>
        new()
        {
            Id = DestinationTypes.PodmanContainer,
            Name = "Podman Container",
            Icon = "/logos/podman.png",
            Description = "Deploy the resource to the configured Podman destination."
        };

    public static DestinationTypeUi WebAssembly =>
        new()
        {
            Id = DestinationTypes.WebAssembly,
            Name = "WebAssembly",
            Icon = "/logos/webassembly.png",
            Description = "Deploy the resource to the configured WebAssembly runtime destination."
        };

    public static List<DestinationTypeUi> All()
    {
        return
        [
            DockerContainer,
            PodmanContainer,
            WebAssembly
        ];
    }
}
