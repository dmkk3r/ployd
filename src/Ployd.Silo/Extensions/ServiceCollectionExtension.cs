using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ployd.Silo.Extensions;

public static class ServiceCollectionExtension {
    public static IServiceCollection AddSqlite(
        this IServiceCollection services, IConfiguration configuration) {
        var connectionString = new SqliteConnectionStringBuilder()
        {
            Mode = SqliteOpenMode.ReadWriteCreate,
            DataSource = "PloydStore.db"
        }.ToString();

        services.AddDbContext<PloydStoreContext>(options => {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}