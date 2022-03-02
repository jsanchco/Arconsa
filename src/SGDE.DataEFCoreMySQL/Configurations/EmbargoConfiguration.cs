using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreMySQL.Configurations
{
    public class EmbargoConfiguration
    {
        public EmbargoConfiguration(EntityTypeBuilder<Embargo> entity)
        {
            entity.ToTable("Embargo");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.UserId).IsRequired();

            entity.HasIndex(x => x.UserId).HasName("IFK_User_Embargo");
            entity.HasOne(u => u.User).WithMany(a => a.Embargos).HasForeignKey(a => a.UserId).HasConstraintName("FK__Embargo__UserId");
        }
    }
}
