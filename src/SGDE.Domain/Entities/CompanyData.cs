using System;

namespace SGDE.Domain.Entities
{
    public class CompanyData : BaseEntity
    {
        public string Reference { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateWarning { get; set; }
        public DateTime? DateExpiration { get; set; }
        public string Observations { get; set; }
        public string TypeFile { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }

        public int? EnterpriseId { get; set; }
        public virtual Enterprise Enterprise { get; set; }
    }
}
