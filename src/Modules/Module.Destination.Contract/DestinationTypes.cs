namespace Module.Destination.Contract;

public static class DestinationTypes
{
    public static Guid DockerEngine => new(DockerEngineId);
    public static Guid Podman => new(PodmanContainerId);
    public static Guid Wasmtime => new(WasmtimeId);

    public const string DockerEngineId = "0a9e9d72-1133-476d-ae39-365377893d56";
    public const string PodmanContainerId = "a9ebfab7-1591-4781-8bf8-896017031d17";
    public const string WasmtimeId = "a4e32068-9657-4aa8-a79e-56d1d385f6de";
}
