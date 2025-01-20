namespace Module.Destination.Endpoints.Docker;

public class CreateDockerDestinationRequest
{
    public required string Name { get; set; }
    public required Uri Uri { get; set; }
}
