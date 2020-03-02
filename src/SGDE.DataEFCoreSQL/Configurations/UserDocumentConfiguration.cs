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
        }
    }
}
