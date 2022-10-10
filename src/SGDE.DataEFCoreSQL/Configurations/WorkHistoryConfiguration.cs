using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class WorkHistoryConfiguration
    {
        public WorkHistoryConfiguration(EntityTypeBuilder<WorkHistory> entity)
        {
            entity.ToTable("WorkHistory");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.WorkId).IsRequired();

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_WorkHistory");
            entity.HasOne(u => u.Work).WithMany(a => a.WorkHistories).HasForeignKey(a => a.WorkId).HasConstraintName("FK__Work__WorkId");
        }
    }
}
