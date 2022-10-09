using System;

namespace SGDE.Domain.ViewModels
{
    public class CompanyDataViewModel : BaseEntityViewModel
    {
        public string reference { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public DateTime? dateExpiration { get; set; }
        public bool alarm => dateExpiration != null && DateTime.Now >= dateExpiration; 
        public string observations { get; set; }
        public string typeFile { get; set; }
        public byte[] file { get; set; }
        public string fileName { get; set; }
        public bool hasFile => file != null;
    }
}
