namespace Module.Destination.Contract;

public class DockerDestination : Contract.Destination
{
    public string Name { get; set; }
    public Uri Uri { get; set; }
}
