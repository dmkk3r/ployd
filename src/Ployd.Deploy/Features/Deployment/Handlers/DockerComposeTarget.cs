using Ployd.Core.Models.Deployments;
using Ployd.Core.Models.Deployments.Parameter;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Models.Deployments.Results;

namespace Ployd.Deploy.Features.Deployment.Handlers;

public class DockerComposeTarget : IDeploymentHander {
    public bool CanHandle(CreateDeploymentRequest request) {
        return request is { DeploymentTarget: DeploymentTarget.DockerCompose, TargetParameter: DockerComposeParameter };
    }

    public Task<(IDeploymentResult?, DeploymentError?)> Handle(CreateDeploymentRequest request, IDeploymentResult? handledResult = null) {
        throw new NotImplementedException();
    }
}