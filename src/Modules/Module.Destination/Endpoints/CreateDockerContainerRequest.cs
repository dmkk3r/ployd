namespace Module.Destination.Endpoints;

public class CreateDockerContainerRequest
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required string Tag { get; set; }
    public IDictionary<string, string>? Environment { get; set; }
    public required Guid DestinationId { get; set; }
}
