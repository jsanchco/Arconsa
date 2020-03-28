namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class UserDocumentConfiguration
    {
        public UserDocumentConfiguration(EntityTypeBuilder<UserDocument> entity)
        {
            entity.ToTable("UserDocument");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.TypeDocumentId).IsRequired();
            entity.Property(x => x.UserId).IsRequired();

            entity.HasIndex(x => x.TypeDocumentId).HasName("IFK_TypeDocument_UserDocument");
            entity.HasOne(u => u.TypeDocument).WithMany(a => a.UserDocuments).HasForeignKey(a => a.TypeDocumentId).HasConstraintName("FK__UserDocument__TypeDocumentId");
        }
    }
}
