namespace Module.Resource.Ui.ResourceCreationWizard.DestinationMetadata;

public class DockerContainerMetadataForm : MetadataForm
{
    public string Name { get; set; }
    public IDictionary<string, string>? Environment { get; set; }
    public override string Type => nameof(DockerContainerMetadataForm);
}
