namespace SGDE.Domain.ViewModels
{
    public class UserDocumentViewModel : BaseEntityViewModel
    {
        public string fileName { get; set; }
        public string description { get; set; }
        public string observations { get; set; }
        public byte[] file { get; set; }
        public string typeFile { get; set; }
        public int typeDocumentId { get; set; }
        public string typeDocumentName { get; set; }
        public int userId { get; set; }
        public string userUserName { get; set; }
    }
}
