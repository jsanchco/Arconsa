using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class WorkStatusHistoryConfiguration
    {
        public WorkStatusHistoryConfiguration(EntityTypeBuilder<WorkStatusHistory> entity)
        {
            entity.ToTable("WorkStatusHistory");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.WorkId).IsRequired();

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_WorkStatusHistory");
            entity.HasOne(u => u.Work).WithMany(a => a.WorkStatusHistories).HasForeignKey(a => a.WorkId).HasConstraintName("FK__WorkStatusHistory__WorkId");
        }
    }
}
