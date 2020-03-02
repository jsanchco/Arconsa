namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class UserHiringConfiguration
    {
        public UserHiringConfiguration(EntityTypeBuilder<UserHiring> entity)
        {
            entity.ToTable("UserHiring");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => x.UserId).HasName("IFK_User_UserHiring");
            entity.HasOne(u => u.User).WithMany(a => a.UserHirings).HasForeignKey(a => a.UserId).HasConstraintName("FK__UserHiring__UserId");
        }
    }
}
