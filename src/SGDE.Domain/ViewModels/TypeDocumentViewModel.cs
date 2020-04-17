namespace SGDE.Domain.ViewModels
{
    public class TypeDocumentViewModel : BaseEntityViewModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public bool isRequired { get; set; }
    }
}
