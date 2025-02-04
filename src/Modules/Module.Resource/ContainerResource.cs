namespace Module.Resource;

public class ContainerResource : Resource
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required string Tag { get; set; }
    public IDictionary<string, string>? Environment { get; set; }
}
