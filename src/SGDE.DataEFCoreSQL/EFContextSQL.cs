// ReSharper disable InconsistentNaming
namespace SGDE.DataEFCoreSQL
{
    #region Using

    using Configurations;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;

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
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<DetailInvoice> DetailInvoice { get; set; }
        public virtual DbSet<UserProfession> UserProfession { get; set; }
        public virtual DbSet<Embargo> Embargo { get; set; }
        public virtual DbSet<DetailEmbargo> DetailEmbargo { get; set; }
        public virtual DbSet<SSHiring> SSHiring { get; set; }
        public virtual DbSet<WorkCost> WorkCost { get; set; }
        public virtual DbSet<WorkBudgetData> WorkBudgetData { get; set; }
        public virtual DbSet<WorkBudget> WorkBudget { get; set; }
        public virtual DbSet<IndirectCost> IndirectCost { get; set; }
        public virtual DbSet<Advance> Advance { get; set; }
        public virtual DbSet<Library> Library { get; set; }
        public virtual DbSet<CompanyData> CompanyData { get; set; }
        public virtual DbSet<WorkHistory> WorkHistory { get; set; }
        public virtual DbSet<WorkStatusHistory> WorkStatusHistory { get; set; }
        public virtual DbSet<InvoicePaymentHistory> InvoicePaymentHistory { get; set; }
        public virtual DbSet<Enterprise> Enterprise { get; set; }

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
            new InvoiceConfiguration(modelBuilder.Entity<Invoice>());
            new DetailInvoiceConfiguration(modelBuilder.Entity<DetailInvoice>());
            new UserProfessionConfiguration(modelBuilder.Entity<UserProfession>());
            new EmbargoConfiguration(modelBuilder.Entity<Embargo>());
            new DetailEmbargoConfiguration(modelBuilder.Entity<DetailEmbargo>());
            new SSHiringConfiguration(modelBuilder.Entity<SSHiring>());
            new WorkCostConfiguration(modelBuilder.Entity<WorkCost>());
            new WorkBudgetDataConfiguration(modelBuilder.Entity<WorkBudgetData>());
            new WorkBudgetConfiguration(modelBuilder.Entity<WorkBudget>());
            new IndirectCostConfiguration(modelBuilder.Entity<IndirectCost>());
            new AdvanceConfiguration(modelBuilder.Entity<Advance>());
            new LibraryConfiguration(modelBuilder.Entity<Library>());
            new CompanyDataConfiguration(modelBuilder.Entity<CompanyData>());
            new WorkHistoryConfiguration(modelBuilder.Entity<WorkHistory>());
            new WorkStatusHistoryConfiguration(modelBuilder.Entity<WorkStatusHistory>());
            new InvoicePaymentHistoryConfiguration(modelBuilder.Entity<InvoicePaymentHistory>());
            new EnterpriseConfiguration(modelBuilder.Entity<Enterprise>());
            new UserEnterpriseConfiguration(modelBuilder.Entity<UserEnterprise>());
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
