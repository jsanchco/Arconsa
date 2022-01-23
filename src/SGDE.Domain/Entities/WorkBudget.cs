using System;
using System.Collections.Generic;

namespace SGDE.Domain.Entities
{
    public class WorkBudget : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string NameInWork { get; set; }
        public string TypeFile { get; set; }
        public byte[] File { get; set; }
        public double TotalContract { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
