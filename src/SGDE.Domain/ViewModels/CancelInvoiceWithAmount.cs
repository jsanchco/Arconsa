namespace SGDE.Domain.ViewModels
{
    public class CancelInvoiceWithAmount
    {
        public int invoiceId { get; set; }
        public double amount { get; set; } 
        public double iva { get; set; }
        public string description { get; set; }
    }
}
