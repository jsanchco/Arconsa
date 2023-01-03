using System;

namespace SGDE.Domain.Entities
{
    public class InvoicePaymentHistory : BaseEntity
    {
        public DateTime DatePayment { get; set; }
        public double Amount { get; set; }
        public string Observations { get; set; }

        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
