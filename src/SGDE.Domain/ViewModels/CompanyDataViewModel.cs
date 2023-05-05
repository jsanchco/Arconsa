using System;

namespace SGDE.Domain.ViewModels
{
    public class CompanyDataViewModel : BaseEntityViewModel
    {
        public string reference { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public DateTime? dateWarning { get; set; }
        public DateTime? dateExpiration { get; set; }
        public bool alarm => dateWarning != null && DateTime.Now >= dateWarning; 
        public string observations { get; set; }
        public string typeFile { get; set; }
        public byte[] file { get; set; }
        public string fileName { get; set; }
        public int? enterpriseId { get; set; }
        public bool hasFile => file != null;
    }
}
