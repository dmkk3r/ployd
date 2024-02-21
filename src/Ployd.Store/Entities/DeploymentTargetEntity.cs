using System.ComponentModel.DataAnnotations;
using Ployd.Core.Models.Deployments;

namespace Ployd.Store.Entities;

public class DeploymentTargetEntity {
    [Key] public Guid Id { get; set; }
    public string? Name { get; set; }
    public DeploymentTarget DeploymentTarget { get; set; }

    public ICollection<DeploymentEntity> Deployments { get; set; }
}