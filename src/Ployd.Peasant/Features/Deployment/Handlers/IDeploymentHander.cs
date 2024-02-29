using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Models.Deployments.Results;

namespace Ployd.Peasant.Features.Deployment.Handlers;

public interface IDeploymentHander {
    bool CanHandle(CreateDeploymentRequest request);
    Task<(IDeploymentResult?, DeploymentError?)> Handle(CreateDeploymentRequest request, IDeploymentResult? parameter = null);
}