namespace Module.Destination.Contract;

public static class DestinationTypesUi
{
    public static DestinationTypeUi Docker =>
        new()
        {
            Id = DestinationTypes.Docker,
            Name = "Docker",
            Icon = "/logos/docker.png",
            Description = "Deploy the resource to the configured Docker destination."
        };

    public static DestinationTypeUi Podman =>
        new()
        {
            Id = DestinationTypes.Podman,
            Name = "Podman",
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
            Docker,
            Podman,
            WebAssembly
        ];
    }
}
