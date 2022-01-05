using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class WorkCostConfiguration
    {
        public WorkCostConfiguration(EntityTypeBuilder<WorkCost> entity)
        {
            entity.ToTable("WorkCost");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.WorkId).IsRequired();

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_WorkCost");
            entity.HasOne(u => u.Work).WithMany(a => a.WorkCosts).HasForeignKey(a => a.WorkId).HasConstraintName("FK__WorkCost__WorkId");
        }
    }
}
