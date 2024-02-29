using Microsoft.EntityFrameworkCore;
using Ployd.Silo.Entities;

namespace Ployd.Silo;

public class PloydStoreContext(DbContextOptions<PloydStoreContext> options) : DbContext(options) {
    public DbSet<DeploymentEntity> Deployments { get; set; }
    public DbSet<DeploymentSourceEntity> DeploymentSources { get; set; }
    public DbSet<DeploymentTargetEntity> DeploymentTargets { get; set; }
}