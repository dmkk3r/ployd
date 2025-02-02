namespace Module.Resource.Contract;

public static class ResourceTypes
{
    public static Guid Container => new(ContainerId);
    public static Guid DockerCompose => new(DockerComposeId);
    public static Guid PodmanCompose => new(PodmanComposeId);
    public static Guid WebAssembly => new(WebAssemblyId);

    public const string DockerComposeId = "45053268-345c-4c05-8822-08ab32c5660a";
    public const string PodmanComposeId = "7da57274-27ba-4e2f-9b7b-39bcb05d9ad1";
    public const string ContainerId = "913df7b7-1fb1-4386-870d-bd2d3ebe94eb";
    public const string WebAssemblyId = "487d433c-bfc1-4d21-8836-d79c047e8c00";
}
