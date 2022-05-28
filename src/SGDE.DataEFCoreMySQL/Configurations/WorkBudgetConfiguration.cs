using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreMySQL.Configurations
{
    public class WorkBudgetConfiguration
    {
        public WorkBudgetConfiguration(EntityTypeBuilder<WorkBudget> entity)
        {
            entity.ToTable("WorkBudget");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.WorkId).IsRequired();

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_WorkBudget");
            entity.HasOne(u => u.Work).WithMany(a => a.WorkBudgets).HasForeignKey(a => a.WorkId).HasConstraintName("FK__WorkBudget__WorkId");

            entity.HasIndex(x => x.WorkBudgetDataId).HasName("IFK_WorkBudgetData_WorkBudget");
            entity.HasOne(u => u.WorkBudgetData).WithMany(a => a.WorkBudgets).HasForeignKey(a => a.WorkBudgetDataId).HasConstraintName("FK__WorkBudget__WorkBudgetDataId").OnDelete(DeleteBehavior.SetNull);
        }
    }
}
