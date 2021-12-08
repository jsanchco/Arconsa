using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class DetailEmbargoConfiguration
    {
        public DetailEmbargoConfiguration(EntityTypeBuilder<DetailEmbargo> entity)
        {
            entity.ToTable("DetailEmbargo");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.EmbargoId).IsRequired();
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");

            entity.HasIndex(x => x.EmbargoId).HasName("IFK_Embargo_DetailEmbargo");
            entity.HasOne(u => u.Embargo).WithMany(a => a.DetailEmbargos).HasForeignKey(a => a.EmbargoId).HasConstraintName("FK__DetailEmbargo__EmbargoId");
        }
    }
}
