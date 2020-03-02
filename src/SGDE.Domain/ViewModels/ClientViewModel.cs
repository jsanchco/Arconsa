namespace SGDE.Domain.ViewModels
{
    public class ClientViewModel : BaseEntityViewModel
    {
        public string name { get; set; }
        public string cif { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public int? typeClientId { get; set; }
        public string typeClientName { get; set; }
    }
}
