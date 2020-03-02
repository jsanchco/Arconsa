namespace SGDE.API.Configurations
{
    #region Using

    using Domain.DbInfo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using DataEFCoreSQL;
    using DataEFCoreMySQL;
    using SGDE.Domain.Helpers;

    #endregion

    public static class ConfigureConnections
    {
        public static IServiceCollection AddConnectionProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var infrastructureSection = configuration.GetSection("Infrastructure");
            services.Configure<InfrastructureAppSettings>(infrastructureSection);
            var infrastructure = infrastructureSection.Get<InfrastructureAppSettings>();

            switch(infrastructure.Type) {
                case "SQL":
                    services.AddDbContextPool<EFContextSQL>(options => options.UseSqlServer(infrastructure.ConnectionString));
                    services.AddSingleton(new DbInfo(infrastructure.ConnectionString));
                    break;

                case "MySQL":
                    services.AddDbContextPool<EFContextMySQL>(options => options.UseMySql(infrastructure.ConnectionString));
                    services.AddSingleton(new DbInfo(infrastructure.ConnectionString));
                    break;

                default:
                    services.AddDbContextPool<EFContextSQL>(options => options.UseSqlServer(infrastructure.ConnectionString));
                    services.AddSingleton(new DbInfo(infrastructure.ConnectionString));
                    break;
            }

            return services;
        }
    }
}