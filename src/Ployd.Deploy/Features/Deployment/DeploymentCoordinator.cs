using System.Collections.Concurrent;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Models.Deployments.Results;
using Ployd.Deploy.Features.Deployment.Handlers;

namespace Ployd.Deploy.Features.Deployment;

public class DeploymentCoordinator(IServiceProvider serviceProvider) {
    private readonly ConcurrentStack<IDeploymentHander> _sourcePipelineHanders = [];
    private readonly ConcurrentStack<IDeploymentHander> _targetPipelineHanders = [];

    public async Task Deploy(CreateDeploymentRequest request) {
        if (_sourcePipelineHanders.IsEmpty || _targetPipelineHanders.IsEmpty)
            throw new InvalidOperationException("Deployment pipeline handlers are not set");

        (IDeploymentResult? deploymentResult, DeploymentError? error) result = (null, null);

        foreach (var sourceHandler in _sourcePipelineHanders)
        {
            if (!sourceHandler.CanHandle(request)) continue;

            result = await sourceHandler.Handle(request);

            if (result.error != null)
                throw new InvalidOperationException(result.error.Message);
        }

        foreach (var targetHandler in _targetPipelineHanders)
        {
            if (!targetHandler.CanHandle(request)) continue;

            result = await targetHandler.Handle(request, result.deploymentResult);

            if (result.error != null)
                throw new InvalidOperationException(result.error.Message);
        }
    }

    public DeploymentCoordinator WithSource<T>() where T : IDeploymentHander {
        var sourceHandler = serviceProvider.GetService<T>();

        if (sourceHandler == null)
            throw new InvalidOperationException($"Service {typeof(T).Name} not found");

        _sourcePipelineHanders.Push(sourceHandler);

        return this;
    }

    public DeploymentCoordinator WithTarget<T>() where T : IDeploymentHander {
        var targetHandler = serviceProvider.GetService<T>();

        if (targetHandler == null)
            throw new InvalidOperationException($"Service {typeof(T).Name} not found");

        _targetPipelineHanders.Push(targetHandler);

        return this;
    }
}