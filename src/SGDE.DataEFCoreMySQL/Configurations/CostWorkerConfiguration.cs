namespace SGDE.DataEFCoreMySQL.Configurations
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

            entity.HasIndex(x => x.UserId).HasName("IFK_User_CostClient");
            entity.HasOne(u => u.User).WithMany(a => a.CostWorkers).HasForeignKey(a => a.UserId).HasConstraintName("FK__CostWorker__UserId");
        }
    }
}