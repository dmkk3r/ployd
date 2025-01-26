namespace Module.Destination.Contract;

public static class DestinationTypesUi
{
    public static DestinationTypeUi Docker =>
        new() { Id = DestinationTypes.Docker, Name = "Docker", Icon = "/logos/docker.png" };

    public static DestinationTypeUi Podman =>
        new() { Id = DestinationTypes.Podman, Name = "Podman", Icon = "/logos/podman.png" };

    public static DestinationTypeUi WebAssembly =>
        new() { Id = DestinationTypes.WebAssembly, Name = "WebAssembly", Icon = "/logos/webassembly.png" };

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
