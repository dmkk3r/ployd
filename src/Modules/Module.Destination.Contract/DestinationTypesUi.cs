namespace Module.Destination.Contract;

public static class DestinationTypesUi
{
    public static DestinationTypeUi DockerEngine =>
        new()
        {
            Id = DestinationTypes.DockerEngine,
            Name = "Docker Engine",
            Icon = "/logos/docker.png",
            Description = "Deploy the resource to the configured Docker destination.",
            Group = "Engine"
        };

    public static DestinationTypeUi Podman =>
        new()
        {
            Id = DestinationTypes.Podman,
            Name = "Podman",
            Icon = "/logos/podman.png",
            Description = "Deploy the resource to the configured Podman destination.",
            Group = "Engine"
        };

    public static DestinationTypeUi Wasmtime =>
        new()
        {
            Id = DestinationTypes.Wasmtime,
            Name = "Wasmtime",
            Icon = "/logos/bytecodealliance.png",
            Description = "Deploy the resource to the configured Wasmtime destination.",
            Group = "Runtime"
        };

    public static List<DestinationTypeUi> All()
    {
        return
        [
            DockerEngine,
            Podman,
            Wasmtime
        ];
    }

    public static Dictionary<string, List<DestinationTypeUi>> AllGrouped()
    {
        return new Dictionary<string, List<DestinationTypeUi>>
        {
            {
                "Engine", [
                    DockerEngine,
                    Podman
                ]
            },
            {
                "Runtime", [
                    Wasmtime
                ]
            }
        };
    }
}
