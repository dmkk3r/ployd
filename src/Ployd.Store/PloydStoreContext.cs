using Microsoft.EntityFrameworkCore;
using Ployd.Store.Entities;

namespace Ployd.Store;

public class PloydStoreContext(DbContextOptions<PloydStoreContext> options) : DbContext(options) {
    public DbSet<DeploymentEntity> Deployments { get; set; }
    public DbSet<DeploymentSourceEntity> DeploymentSources { get; set; }
    public DbSet<DeploymentTargetEntity> DeploymentTargets { get; set; }
}