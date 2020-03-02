namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class TypeDocumentConfiguration
    {
        public TypeDocumentConfiguration(EntityTypeBuilder<TypeDocument> entity)
        {
            entity.ToTable("TypeDocument");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired();
        }
    }
}
