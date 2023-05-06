namespace SGDE.Domain.ViewModels
{
    public class ClientViewModel : BaseEntityViewModel
    {
        public string idClient { get; set; }
        public string name { get; set; }
        public string cif { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string wayToPay { get; set; }
        public int expirationDays { get; set; }
        public string accountNumber { get; set; }
        public int? typeClientId { get; set; }
        public string typeClientName { get; set; }
        public string email { get; set; }
        public string emailInvoice { get; set; }
        public bool active { get; set; }
        public int ? enterpriseId { get; set; }
    }
}
