namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class CostWorkerConfiguration
    {
        public CostWorkerConfiguration(EntityTypeBuilder<CostWorker> entity)
        {
            entity.ToTable("CostWorker");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.PriceHourExtra).HasColumnType("decimal(18,2)");
            entity.Property(x => x.PriceHourFestive).HasColumnType("decimal(18,2)");
            entity.Property(x => x.PriceHourOrdinary).HasColumnType("decimal(18,2)");

            entity.HasIndex(x => x.UserId).HasName("IFK_User_CostClient");
            entity.HasOne(u => u.User).WithMany(a => a.CostWorkers).HasForeignKey(a => a.UserId).HasConstraintName("FK__CostWorker__UserId");

            entity.HasIndex(x => x.ProfessionId).HasName("IFK_Profession_CostWorker");
            entity.HasOne(u => u.Profession).WithMany(a => a.CostWorkers).HasForeignKey(a => a.ProfessionId).HasConstraintName("FK__CostWorker__ProfessionId");
        }
    }
}
