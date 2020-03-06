namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class DailySigningConfiguration
    {
        public DailySigningConfiguration(EntityTypeBuilder<DailySigning> entity)
        {
            entity.ToTable("DailySigning");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.StartHour).IsRequired();

            entity.HasIndex(x => x.UserHiringId).HasName("IFK_UserHiring_DailySigning");
            entity.HasOne(u => u.UserHiring).WithMany(a => a.DailysSigning).HasForeignKey(a => a.UserHiringId).HasConstraintName("FK__DailySigning__UserHiringId");
        }
    }
}
