using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class CompanyDataConfiguration
    {
        public CompanyDataConfiguration(EntityTypeBuilder<CompanyData> entity)
        {
            entity.ToTable("CompanyData");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => x.EnterpriseId).HasName("IFK_Enterprise_CompanyData");
            entity.HasOne(u => u.Enterprise).WithMany(a => a.CompanyDatas).HasForeignKey(a => a.EnterpriseId).HasConstraintName("FK__CompanyData__EnerpriseId");
        }
    }
}
