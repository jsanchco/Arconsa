using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class WorkBudgetDataConfiguration
    {
        public WorkBudgetDataConfiguration(EntityTypeBuilder<WorkBudgetData> entity)
        {
            entity.ToTable("WorkBudgetData");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.WorkId).IsRequired();

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_WorkBudgetData");
            entity.HasOne(u => u.Work).WithMany(a => a.WorkBudgetDatas).HasForeignKey(a => a.WorkId).HasConstraintName("FK__WorkBudgetData__WorkId");
        }
    }
}
