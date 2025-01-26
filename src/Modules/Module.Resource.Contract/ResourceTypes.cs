namespace Module.Resource.Contract;

public static class ResourceTypes
{
    public static Guid Dockerfile => new("c34e57a7-184f-4159-ae32-b44e6792ae56");
    public static Guid DockerCompose => new("45053268-345c-4c05-8822-08ab32c5660a");
    public static Guid PodmanCompose => new("7da57274-27ba-4e2f-9b7b-39bcb05d9ad1");
    public static Guid OciImage => new("913df7b7-1fb1-4386-870d-bd2d3ebe94eb");
    public static Guid WebAssembly => new("487d433c-bfc1-4d21-8836-d79c047e8c00");
}
