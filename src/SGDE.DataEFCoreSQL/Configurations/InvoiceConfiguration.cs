namespace SGDE.DataEFCoreSQL.Configurations
{
    #region Using

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class InvoiceConfiguration
    {
        public InvoiceConfiguration(EntityTypeBuilder<Invoice> entity)
        {
            entity.ToTable("Invoice");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Name).IsRequired();
            entity.Ignore(x => x.State);
            entity.Ignore(x => x.IvaTaxBase);
            entity.Ignore(x => x.Total);

            entity.HasIndex(x => x.WorkId).HasName("IFK_Work_Invoice");
            entity.HasOne(u => u.Work).WithMany(a => a.Invoices).HasForeignKey(a => a.WorkId).HasConstraintName("FK__Invoice__WorkId");

            entity.HasIndex(x => x.InvoiceToCancelId).HasName("IFK_Invoice_Invoice");
            entity.HasOne(u => u.InvoiceToCancel).WithMany(a => a.Invoices).HasForeignKey(a => a.InvoiceToCancelId).HasConstraintName("FK__Invoice__InvoiceToCancelId");
        }
    }
}
