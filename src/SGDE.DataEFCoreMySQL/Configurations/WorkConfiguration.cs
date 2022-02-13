namespace SGDE.DataEFCoreMySQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class WorkConfiguration
    {
        public WorkConfiguration(EntityTypeBuilder<Work> entity)
        {
            entity.ToTable("Work");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.PercentageRetention).HasColumnType("decimal(18,2)");
            //entity.Property(x => x.TotalContract).HasColumnType("decimal(18,2)");

            entity.HasIndex(x => x.ClientId).HasName("IFK_Client_Work");
            entity.HasOne(u => u.Client).WithMany(a => a.Works).HasForeignKey(a => a.ClientId).HasConstraintName("FK__Work__ClientId");
        }
    }
}
