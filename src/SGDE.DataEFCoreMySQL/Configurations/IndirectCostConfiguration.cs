namespace SGDE.DataEFCoreMySQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class IndirectCostConfiguration
    {
        public IndirectCostConfiguration(EntityTypeBuilder<IndirectCost> entity)
        {
            entity.ToTable("IndirectCost");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            //entity.Ignore(x => x.Year);
            //entity.Ignore(x => x.Month);
            //entity.Ignore(x => x.Key);
        }
    }
}
