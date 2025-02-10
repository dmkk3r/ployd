namespace Module.Resource.Ui.ResourceCreationWizard.SourceMetadata;

public class GhcrMetadataForm : MetadataForm
{
    public string ImageName { get; set; }
    public string ImageTag { get; set; }
    public override string Type => nameof(GhcrMetadataForm);
}
