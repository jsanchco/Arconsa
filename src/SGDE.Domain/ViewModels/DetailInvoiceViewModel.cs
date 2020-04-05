namespace SGDE.Domain.ViewModels
{
    public class DetailInvoiceViewModel : BaseEntityViewModel
    {
        public int invoiceId { get; set; }
        public string servicesPerformed { get; set; }
        public double units { get; set; }
        public string nameUnit { get; set; }
        public double priceUnity { get; set; }
        public double total => units * priceUnity;
    }
}
