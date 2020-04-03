namespace SGDE.Domain.ViewModels
{
    public class InvoiceResponseViewModel
    {
        public string invoiceNumber { get; set; }
        public string fileName { get; set; }
        public string typeFile { get; set; }
        public string typeInvoiceId { get; set; }
        public byte[] file { get; set; }
        public InvoiceViewModel data { get; set; }
    }
}
