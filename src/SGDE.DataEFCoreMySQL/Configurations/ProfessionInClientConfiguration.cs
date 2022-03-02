namespace SGDE.DataEFCoreMySQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class ProfessionInClientConfiguration
    {
        public ProfessionInClientConfiguration(EntityTypeBuilder<ProfessionInClient> entity)
        {
            entity.ToTable("ProfessionInClient");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.ProfessionId).IsRequired();
            entity.Property(x => x.ClientId).IsRequired();

            entity.HasIndex(x => x.ClientId).HasName("IFK_Client_ProfessionInClient");
            entity.HasOne(u => u.Client).WithMany(a => a.ProfessionInClients).HasForeignKey(a => a.ClientId).HasConstraintName("FK__ProfessionInClient__ClientId");

            entity.HasIndex(x => x.ProfessionId).HasName("IFK_Profession_ProfessionInClient");
            entity.HasOne(u => u.Profession).WithMany(a => a.ProfessionInClients).HasForeignKey(a => a.ProfessionId).HasConstraintName("FK__ProfessionInClient__ProfessionId");
        }
    }
}
