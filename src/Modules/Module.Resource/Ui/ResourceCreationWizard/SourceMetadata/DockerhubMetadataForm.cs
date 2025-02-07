namespace Module.Resource.Ui.ResourceCreationWizard.SourceMetadata;

public class DockerhubMetadataForm : MetadataForm
{
    public string ImageName { get; set; }
    public string ImageTag { get; set; }
    public override string Type => nameof(DockerhubMetadataForm);
}
