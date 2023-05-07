using System;

namespace SGDE.Domain.ViewModels
{
    public class LibraryViewModel : BaseEntityViewModel
    {
        public int? enterpriseId { get; set; }
        public string reference { get; set; }
        public string department { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public string edition { get; set; }
        public bool active { get; set; }
        public string typeFile { get; set; }
        public byte[] file { get; set; }
        public string fileName { get; set; }
        public bool hasFile => file != null;
    }
}
