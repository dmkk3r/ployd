namespace Ployd.Core.Models.Deployments.Results;

public record DeploymentError(string Message, Exception? Exception = null) {
    public static DeploymentError FromException(Exception ex) => new DeploymentError(ex.Message, ex);
}