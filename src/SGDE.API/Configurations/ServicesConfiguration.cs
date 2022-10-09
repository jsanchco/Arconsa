namespace SGDE.API.Configurations
{
    #region Using

    using Domain.Repositories;
    using Domain.Supervisor;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Domain.Helpers;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    #endregion

    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var infrastructureSection = configuration.GetSection("Infrastructure");
            services.Configure<InfrastructureAppSettings>(infrastructureSection);
            var infrastructure = infrastructureSection.Get<InfrastructureAppSettings>();

            switch (infrastructure.Type)
            {
                case "SQL":
                    services
                        .AddScoped<IUserRepository, DataEFCoreSQL.Repositories.UserRepository>()
                        .AddScoped<IProfessionRepository, DataEFCoreSQL.Repositories.ProfessionRepository>()
                        .AddScoped<IPromoterRepository, DataEFCoreSQL.Repositories.PromoterRepository>()
                        .AddScoped<IRoleRepository, DataEFCoreSQL.Repositories.RoleRepository>()
                        .AddScoped<ITrainingRepository, DataEFCoreSQL.Repositories.TrainingRepository>()
                        .AddScoped<IUserHiringRepository, DataEFCoreSQL.Repositories.UserHiringRepository>()
                        .AddScoped<IWorkRepository, DataEFCoreSQL.Repositories.WorkRepository>()
                        .AddScoped<IClientRepository, DataEFCoreSQL.Repositories.ClientRepository>()
                        .AddScoped<IUserDocumentRepository, DataEFCoreSQL.Repositories.UserDocumentRepository>()
                        .AddScoped<ITypeDocumentRepository, DataEFCoreSQL.Repositories.TypeDocumentRepository>()
                        .AddScoped<ITypeClientRepository, DataEFCoreSQL.Repositories.TypeClientRepository>()
                        .AddScoped<IDailySigningRepository, DataEFCoreSQL.Repositories.DailySigningRepository>()
                        .AddScoped<ISettingRepository, DataEFCoreSQL.Repositories.SettingRepository>()
                        .AddScoped<IProfessionInClientRepository, DataEFCoreSQL.Repositories.ProfessionInClientRepository>()
                        .AddScoped<IHourTypeRepository, DataEFCoreSQL.Repositories.HourTypeRepository>()
                        .AddScoped<ICostWorkerRepository, DataEFCoreSQL.Repositories.CostWorkerRepository>()
                        .AddScoped<IInvoiceRepository, DataEFCoreSQL.Repositories.InvoiceRepository>()
                        .AddScoped<IDetailInvoiceRepository, DataEFCoreSQL.Repositories.DetailInvoiceRepository>()
                        .AddScoped<IUserProfessionRepository, DataEFCoreSQL.Repositories.UserProfessionRepository>()
                        .AddScoped<IEmbargoRepository, DataEFCoreSQL.Repositories.EmbargoRepository>()
                        .AddScoped<IDetailEmbargoRepository, DataEFCoreSQL.Repositories.DetailEmbargoRepository>()
                        .AddScoped<ISSHiringRepository, DataEFCoreSQL.Repositories.SSHiringRepository>()
                        .AddScoped<IWorkCostRepository, DataEFCoreSQL.Repositories.WorkCostRepository>()
                        .AddScoped<IWorkBudgetDataRepository, DataEFCoreSQL.Repositories.WorkBudgetDataRepository>()
                        .AddScoped<IWorkBudgetRepository, DataEFCoreSQL.Repositories.WorkBudgetRepository>()
                        .AddScoped<IIndirectCostRepository, DataEFCoreSQL.Repositories.IndirectCostRepository>()
                        .AddScoped<IAdvanceRepository, DataEFCoreSQL.Repositories.AdvanceRepository>()
                        .AddScoped<ILibraryRepository, DataEFCoreSQL.Repositories.LibraryRepository>()
                        .AddScoped<ICompanyDataRepository, DataEFCoreSQL.Repositories.CompanyDataRepository>();
                    break;
                case "MySQL":
                    services
                        .AddScoped<IUserRepository, DataEFCoreMySQL.Repositories.UserRepository>()
                        .AddScoped<IProfessionRepository, DataEFCoreMySQL.Repositories.ProfessionRepository>()
                        .AddScoped<IPromoterRepository, DataEFCoreMySQL.Repositories.PromoterRepository>()
                        .AddScoped<IRoleRepository, DataEFCoreMySQL.Repositories.RoleRepository>()
                        .AddScoped<ITrainingRepository, DataEFCoreMySQL.Repositories.TrainingRepository>()
                        .AddScoped<IUserHiringRepository, DataEFCoreMySQL.Repositories.UserHiringRepository>()
                        .AddScoped<IWorkRepository, DataEFCoreMySQL.Repositories.WorkRepository>()
                        .AddScoped<IClientRepository, DataEFCoreMySQL.Repositories.ClientRepository>()
                        .AddScoped<IUserDocumentRepository, DataEFCoreMySQL.Repositories.UserDocumentRepository>()
                        .AddScoped<ITypeDocumentRepository, DataEFCoreMySQL.Repositories.TypeDocumentRepository>()
                        .AddScoped<ITypeClientRepository, DataEFCoreMySQL.Repositories.TypeClientRepository>()
                        .AddScoped<IDailySigningRepository, DataEFCoreMySQL.Repositories.DailySigningRepository>()
                        .AddScoped<ISettingRepository, DataEFCoreMySQL.Repositories.SettingRepository>()
                        .AddScoped<IProfessionInClientRepository, DataEFCoreMySQL.Repositories.ProfessionInClientRepository>()
                        .AddScoped<IHourTypeRepository, DataEFCoreMySQL.Repositories.HourTypeRepository>()
                        .AddScoped<ICostWorkerRepository, DataEFCoreMySQL.Repositories.CostWorkerRepository>()
                        .AddScoped<IInvoiceRepository, DataEFCoreMySQL.Repositories.InvoiceRepository>()
                        .AddScoped<IDetailInvoiceRepository, DataEFCoreMySQL.Repositories.DetailInvoiceRepository>()
                        .AddScoped<IUserProfessionRepository, DataEFCoreMySQL.Repositories.UserProfessionRepository>()
                        .AddScoped<IEmbargoRepository, DataEFCoreMySQL.Repositories.EmbargoRepository>()
                        .AddScoped<IDetailEmbargoRepository, DataEFCoreMySQL.Repositories.DetailEmbargoRepository>()
                        .AddScoped<ISSHiringRepository, DataEFCoreMySQL.Repositories.SSHiringRepository>()
                        .AddScoped<IWorkCostRepository, DataEFCoreMySQL.Repositories.WorkCostRepository>()
                        .AddScoped<IWorkBudgetDataRepository, DataEFCoreMySQL.Repositories.WorkBudgetDataRepository>()
                        .AddScoped<IWorkBudgetRepository, DataEFCoreMySQL.Repositories.WorkBudgetRepository>()
                        .AddScoped<IIndirectCostRepository, DataEFCoreMySQL.Repositories.IndirectCostRepository>()
                        .AddScoped<IAdvanceRepository, DataEFCoreMySQL.Repositories.AdvanceRepository>();
                    break;

                default:
                    services
                        .AddScoped<IProfessionRepository, DataEFCoreMySQL.Repositories.ProfessionRepository>()
                        .AddScoped<IPromoterRepository, DataEFCoreMySQL.Repositories.PromoterRepository>()
                        .AddScoped<IRoleRepository, DataEFCoreMySQL.Repositories.RoleRepository>()
                        .AddScoped<ITrainingRepository, DataEFCoreMySQL.Repositories.TrainingRepository>()
                        .AddScoped<IUserHiringRepository, DataEFCoreMySQL.Repositories.UserHiringRepository>()
                        .AddScoped<IWorkRepository, DataEFCoreMySQL.Repositories.WorkRepository>()
                        .AddScoped<IClientRepository, DataEFCoreMySQL.Repositories.ClientRepository>()
                        .AddScoped<IUserDocumentRepository, DataEFCoreMySQL.Repositories.UserDocumentRepository>()
                        .AddScoped<ITypeDocumentRepository, DataEFCoreMySQL.Repositories.TypeDocumentRepository>()
                        .AddScoped<ITypeClientRepository, DataEFCoreMySQL.Repositories.TypeClientRepository>()
                        .AddScoped<IDailySigningRepository, DataEFCoreMySQL.Repositories.DailySigningRepository>()
                        .AddScoped<ISettingRepository, DataEFCoreMySQL.Repositories.SettingRepository>()
                        .AddScoped<IProfessionInClientRepository, DataEFCoreMySQL.Repositories.ProfessionInClientRepository>()
                        .AddScoped<IHourTypeRepository, DataEFCoreMySQL.Repositories.HourTypeRepository>()
                        .AddScoped<ICostWorkerRepository, DataEFCoreMySQL.Repositories.CostWorkerRepository>()
                        .AddScoped<IInvoiceRepository, DataEFCoreMySQL.Repositories.InvoiceRepository>()
                        .AddScoped<IDetailInvoiceRepository, DataEFCoreMySQL.Repositories.DetailInvoiceRepository>()
                        .AddScoped<IUserProfessionRepository, DataEFCoreSQL.Repositories.UserProfessionRepository>()
                        .AddScoped<IEmbargoRepository, DataEFCoreSQL.Repositories.EmbargoRepository>()
                        .AddScoped<IDetailEmbargoRepository, DataEFCoreSQL.Repositories.DetailEmbargoRepository>()
                        .AddScoped<ISSHiringRepository, DataEFCoreSQL.Repositories.SSHiringRepository>()
                        .AddScoped<IWorkCostRepository, DataEFCoreSQL.Repositories.WorkCostRepository>()
                        .AddScoped<IWorkBudgetDataRepository, DataEFCoreSQL.Repositories.WorkBudgetDataRepository>()
                        .AddScoped<IWorkBudgetRepository, DataEFCoreSQL.Repositories.WorkBudgetRepository>()
                        .AddScoped<IIndirectCostRepository, DataEFCoreSQL.Repositories.IndirectCostRepository>()
                        .AddScoped<IAdvanceRepository, DataEFCoreSQL.Repositories.AdvanceRepository>()
                        .AddScoped<ILibraryRepository, DataEFCoreSQL.Repositories.LibraryRepository>()
                        .AddScoped<ICompanyDataRepository, DataEFCoreSQL.Repositories.CompanyDataRepository>();
                    break;
            }

            return services;
        }

        public static IServiceCollection ConfigureSupervisor(this IServiceCollection services)
        {
            services.AddScoped<ISupervisor, Supervisor>();

            return services;
        }

        public static IServiceCollection AddMiddleware(this IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(options => 
                options.SerializerSettings.ReferenceLoopHandling = new ReferenceLoopHandling());

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            services.Configure<JwtAppSettings>(jwtSection);

            // configure jwt authentication
            var jwtAppSettings = jwtSection.Get<JwtAppSettings>();
            var key = Encoding.ASCII.GetBytes(jwtAppSettings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .Build());
            });
    }
}