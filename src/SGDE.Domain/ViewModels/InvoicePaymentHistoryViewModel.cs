using System;

namespace SGDE.Domain.ViewModels
{
    public class InvoicePaymentHistoryViewModel : BaseEntityViewModel
    {
        public DateTime datePayment { get; set; }
        public double amount { get; set; }
        public string observations { get; set; }

        public int invoiceId { get; set; }
    }
}
