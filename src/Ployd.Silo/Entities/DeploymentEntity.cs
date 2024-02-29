using System.ComponentModel.DataAnnotations;
using Ployd.Core.Models.Deployments;

namespace Ployd.Silo.Entities;

public class DeploymentEntity {
    [Key] public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Endpoint { get; set; }
    public DeploymentStatus Status { get; set; }

    public Guid DeploymentSourceId { get; set; }
    public DeploymentSourceEntity DeploymentSource { get; set; }

    public ICollection<DeploymentTargetEntity> DeploymentTargets { get; set; }
}