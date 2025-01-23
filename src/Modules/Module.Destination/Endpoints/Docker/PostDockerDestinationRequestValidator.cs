using FluentValidation;

namespace Module.Destination.Endpoints.Docker;

public class PostDockerDestinationRequestValidator : AbstractValidator<PostDockerDestinationRequest>
{
    public PostDockerDestinationRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;

        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Endpoint)
            .NotEmpty();
        RuleFor(x => x.Endpoint)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.Endpoint));
    }
}
