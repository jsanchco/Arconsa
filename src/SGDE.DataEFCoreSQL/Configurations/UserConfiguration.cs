namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.AddedDate).IsRequired();
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.BirthDate).IsRequired(false);
            entity.Ignore(x => x.Token);

            //entity.HasIndex(x => x.ProfessionId).HasName("IFK_Profession_User");
            //entity.HasOne(u => u.Profession).WithMany(a => a.Users).HasForeignKey(a => a.ProfessionId).HasConstraintName("FK__User__ProfessionId");

            entity.HasIndex(x => x.RoleId).HasName("IFK_Role_User");
            entity.HasOne(u => u.Role).WithMany(a => a.Users).HasForeignKey(a => a.RoleId).HasConstraintName("FK__User__RoleId");

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_User");
            entity.HasOne(u => u.Work).WithMany(a => a.WorkResponsibles).HasForeignKey(a => a.WorkId).HasConstraintName("FK__User__WorkId").OnDelete(DeleteBehavior.SetNull);
        }
    }
}
