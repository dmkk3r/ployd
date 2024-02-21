using System.ComponentModel.DataAnnotations;
using Ployd.Core.Models.Deployments;

namespace Ployd.Store.Entities;

public class DeploymentTargetEntity {
    [Key] public Guid Id { get; set; }
    public string? Name { get; set; }
    public DeploymentTargetType DeploymentTargetType { get; set; }

    public ICollection<DeploymentEntity> Deployments { get; set; }
}