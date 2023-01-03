using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDE.Domain.Entities;

namespace SGDE.DataEFCoreSQL.Configurations
{
    public class InvoicePaymentHistoryConfiguration
    {
        public InvoicePaymentHistoryConfiguration(EntityTypeBuilder<InvoicePaymentHistory> entity)
        {
            entity.ToTable("InvoicePaymentHistory");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.InvoiceId).IsRequired();

            entity.HasIndex(x => x.InvoiceId).HasName("IFK_Invoice_InvoicePaymentHistory");
            entity.HasOne(u => u.Invoice).WithMany(a => a.InvoicePaymentsHistory).HasForeignKey(a => a.InvoiceId).HasConstraintName("FK__InvoicePaymentHistory__InvoiceId");
        }
    }
}
