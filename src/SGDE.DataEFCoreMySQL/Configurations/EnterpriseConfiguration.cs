using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreMySQL.Configurations
{
    public class EnterpriseConfiguration
    {
        public EnterpriseConfiguration(EntityTypeBuilder<Enterprise> entity)
        {
            entity.ToTable("Enterprise");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
