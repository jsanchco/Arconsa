using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class UserProfessionConfiguration
    {
        public UserProfessionConfiguration(EntityTypeBuilder<UserProfession> entity)
        {
            entity.ToTable("UserProfession");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => x.UserId).HasName("IFK_User_UserProfession");
            entity.HasOne(u => u.User).WithMany(a => a.UserProfessions).HasForeignKey(a => a.UserId).HasConstraintName("FK__UserProfession__UserId");

            entity.HasIndex(x => x.ProfessionId).HasName("IFK_Profession_UserProfession");
            entity.HasOne(u => u.Profession).WithMany(a => a.UserProfessions).HasForeignKey(a => a.ProfessionId).HasConstraintName("FK__UserProfession__ProfessionId");
        }
    }
}
