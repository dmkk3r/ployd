﻿using System.ComponentModel.DataAnnotations;
using Ployd.Core.Models.Deployments;

namespace Ployd.Store.Entities;

public class DeploymentSourceEntity {
    [Key] public Guid Id { get; set; }
    public string? Name { get; set; }
    public DeploymentSourceType DeploymentSourceType { get; set; }
    
    public ICollection<DeploymentEntity> Deployments { get; set; }
}