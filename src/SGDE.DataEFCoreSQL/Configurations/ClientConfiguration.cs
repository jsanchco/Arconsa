namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class ClientConfiguration
    {
        public ClientConfiguration(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("Client");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.PromoterId).IsRequired(false);

            entity.HasIndex(x => x.PromoterId).HasName("IFK_Promoter_Client");
            entity.HasOne(u => u.Promoter).WithMany(a => a.Clients).HasForeignKey(a => a.PromoterId).HasConstraintName("FK__Client__PromoterId");
        }
    }
}
