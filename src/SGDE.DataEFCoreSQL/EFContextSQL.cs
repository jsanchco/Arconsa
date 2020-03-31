// ReSharper disable InconsistentNaming
namespace SGDE.DataEFCoreSQL
{
    #region Using

    using System.Threading;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Configurations;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    #endregion

    public class EFContextSQL : DbContext
    {
        #region Members

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Profession> Profession { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Promoter> Promoter { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Training> Training { get; set; }
        public virtual DbSet<UserHiring> UserHiring { get; set; }
        public virtual DbSet<Work> Work { get; set; }
        public virtual DbSet<UserDocument> UserDocument { get; set; }
        public virtual DbSet<TypeDocument> TypeDocument { get; set; }
        public virtual DbSet<TypeClient> TypeClient { get; set; }
        public virtual DbSet<DailySigning> DailySigning { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<ProfessionInClient> ProfessionInClient { get; set; }
        public virtual DbSet<HourType> HourType { get; set; }
        public virtual DbSet<CostWorker> CostWorker { get; set; }

        public static long InstanceCount;

        #endregion

        public EFContextSQL(DbContextOptions options) : base(options) => Interlocked.Increment(ref InstanceCount);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserConfiguration(modelBuilder.Entity<User>());
            new ProfessionConfiguration(modelBuilder.Entity<Profession>());
            new PromoterConfiguration(modelBuilder.Entity<Promoter>());
            new ClientConfiguration(modelBuilder.Entity<Client>());
            new RoleConfiguration(modelBuilder.Entity<Role>());
            new TrainingConfiguration(modelBuilder.Entity<Training>());
            new UserHiringConfiguration(modelBuilder.Entity<UserHiring>());
            new WorkConfiguration(modelBuilder.Entity<Work>());
            new UserDocumentConfiguration(modelBuilder.Entity<UserDocument>());
            new TypeDocumentConfiguration(modelBuilder.Entity<TypeDocument>());
            new TypeClientConfiguration(modelBuilder.Entity<TypeClient>());
            new DailySigningConfiguration(modelBuilder.Entity<DailySigning>());
            new SettingConfiguration(modelBuilder.Entity<Setting>());
            new ProfessionInClientConfiguration(modelBuilder.Entity<ProfessionInClient>());
            new HourTypeConfiguration(modelBuilder.Entity<HourType>());
            new CostWorkerConfiguration(modelBuilder.Entity<CostWorker>());
        }

        //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EFContextSQL>
        //{
        //    public EFContextSQL CreateDbContext(string[] args)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile(@Directory.GetCurrentDirectory() + "/../SGDE.API/appsettings.json")
        //            .Build();
        //        var builder = new DbContextOptionsBuilder<EFContextSQL>();
        //        var connectionString = configuration.GetConnectionString("SGDEContextSQL");
        //        builder.UseSqlServer(connectionString);
        //        return new EFContextSQL(builder.Options);
        //    }
        //}
    }
}
