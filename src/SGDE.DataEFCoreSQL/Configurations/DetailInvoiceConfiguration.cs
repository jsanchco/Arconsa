namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class DetailInvoiceConfiguration
    {
        public DetailInvoiceConfiguration(EntityTypeBuilder<DetailInvoice> entity)
        {
            entity.ToTable("DetailInvoice");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Ignore(x => x.Total);

            entity.HasIndex(x => x.InvoiceId).HasName("IFK_Invoice_DetailInvoice");
            entity.HasOne(u => u.Invoice).WithMany(a => a.DetailsInvoice).HasForeignKey(a => a.InvoiceId).HasConstraintName("FK__DetailInvoice__InvoiceId");
        }
    }
}
