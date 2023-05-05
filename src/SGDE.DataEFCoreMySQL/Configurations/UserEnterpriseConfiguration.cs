using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreMySQL.Configurations
{
    public class UserEnterpriseConfiguration
    {
        public UserEnterpriseConfiguration(EntityTypeBuilder<UserEnterprise> entity)
        {
            entity.ToTable("UserEnterprise");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => x.UserId).HasName("IFK_User_UserEnterprise");
            entity.HasOne(u => u.User).WithMany(a => a.UsersEnterprises).HasForeignKey(a => a.UserId).HasConstraintName("FK__UserEnterprise__UserId");

            entity.HasIndex(x => x.EnterpriseId).HasName("IFK_Enterprise_UserEnterprise");
            entity.HasOne(u => u.Enterprise).WithMany(a => a.UsersEnterprises).HasForeignKey(a => a.EnterpriseId).HasConstraintName("FK__UserEnterprise__EnterpriseId");
        }
    }
}
