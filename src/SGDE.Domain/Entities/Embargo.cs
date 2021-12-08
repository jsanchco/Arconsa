using System;
using System.Collections.Generic;

namespace SGDE.Domain.Entities
{
    public class Embargo : BaseEntity
    {
        public string Identifier { get; set; }
        public string IssuingEntity { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? NotificationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Observations { get; set; }
        public Decimal Total { get; set; }        
        public bool Paid { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<DetailEmbargo> DetailEmbargos { get; set; } = new HashSet<DetailEmbargo>();
    }
}
