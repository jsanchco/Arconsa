namespace SGDE.DataEFCoreMySQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class TrainingConfiguration
    {
        public TrainingConfiguration(EntityTypeBuilder<Training> entity)
        {
            entity.ToTable("Training");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired();

            entity.HasIndex(x => x.UserId).HasName("IFK_User_Training");
            entity.HasOne(u => u.User).WithMany(a => a.Trainings).HasForeignKey(a => a.UserId).HasConstraintName("FK__Training__UserId");
        }
    }
}