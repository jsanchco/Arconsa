namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class SettingConfiguration
    {
        public SettingConfiguration(EntityTypeBuilder<Setting> entity)
        {
            entity.ToTable("Setting");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Data).IsRequired();
        }
    }
}
