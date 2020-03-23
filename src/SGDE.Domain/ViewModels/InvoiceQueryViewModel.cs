namespace SGDE.Domain.ViewModels
{
    public class InvoiceQueryViewModel
    {
        public string invoiceNumber { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string issueDate { get; set; }
        public int typeInvoice { get; set; }        
        public int? clientId { get; set; }
        public int? workId { get; set; }
        public int? workerId { get; set; }
    }
}
