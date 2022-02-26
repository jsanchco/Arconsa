using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class AdvanceConfiguration
    {
        public AdvanceConfiguration(EntityTypeBuilder<Advance> entity)
        {
            entity.ToTable("Advance");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Ignore(x => x.Paid);

            entity.HasIndex(x => x.UserId).HasName("IFK_User_Advance");
            entity.HasOne(u => u.User).WithMany(a => a.Advances).HasForeignKey(a => a.UserId).HasConstraintName("FK__Advance__UserId");
        }
    }
}
