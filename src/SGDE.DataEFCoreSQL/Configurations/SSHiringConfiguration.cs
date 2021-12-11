using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class SSHiringConfiguration
    {
        public SSHiringConfiguration(EntityTypeBuilder<SSHiring> entity)
        {
            entity.ToTable("SSHiring");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.UserId).IsRequired();

            entity.HasIndex(x => x.UserId).HasName("IFK_User_SSHiring");
            entity.HasOne(u => u.User).WithMany(a => a.SSHirings).HasForeignKey(a => a.UserId).HasConstraintName("FK__SSHiring__UserId");
        }
    }
}
