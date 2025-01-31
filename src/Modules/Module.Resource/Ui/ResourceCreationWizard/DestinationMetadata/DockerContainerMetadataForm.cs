namespace Module.Resource.Ui.ResourceCreationWizard.DestinationMetadata;

public class DockerContainerMetadataForm
{
    public string Name { get; set; }
    public IDictionary<string, string>? Environment { get; set; }
}
