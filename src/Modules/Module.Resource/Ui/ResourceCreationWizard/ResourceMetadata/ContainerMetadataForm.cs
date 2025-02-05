namespace Module.Resource.Ui.ResourceCreationWizard.ResourceMetadata;

public class ContainerMetadataForm : MetadataForm
{
    public string Name { get; set; }
    public override string Type => nameof(ContainerMetadataForm);
}
