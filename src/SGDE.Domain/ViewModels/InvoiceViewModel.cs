namespace SGDE.Domain.ViewModels
{
    public class InvoiceViewModel
    {
        public string invoiceNumber { get; set; }
        public string fileName { get; set; }
        public string typeFile { get; set; }
        public string typeInvoiceId { get; set; }
        public byte[] file { get; set; }
    }
}
