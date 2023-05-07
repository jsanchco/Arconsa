using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class LibraryConfiguration
    {
        public LibraryConfiguration(EntityTypeBuilder<Library> entity)
        {
            entity.ToTable("Library");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => x.EnterpriseId).HasName("IFK_Enterprise_Library");
            entity.HasOne(u => u.Enterprise).WithMany(a => a.Libraries).HasForeignKey(a => a.EnterpriseId).HasConstraintName("FK__Library__EnerpriseId");
        }
    }
}
