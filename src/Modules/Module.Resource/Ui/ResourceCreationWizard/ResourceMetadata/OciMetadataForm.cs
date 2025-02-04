namespace Module.Resource.Ui.ResourceCreationWizard.ResourceMetadata;

public class OciMetadataForm : MetadataForm
{
    public string Image { get; set; }
    public string Tag { get; set; }
    public override string Type => nameof(OciMetadataForm);
}
