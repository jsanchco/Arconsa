using System;

namespace SGDE.Domain.Entities
{
    public class DetailEmbargo : BaseEntity
    {
        public DateTime DatePay { get; set; } 
        public string Observations { get; set; }
        public double Amount { get; set; }

        public int EmbargoId { get; set; }
        public virtual Embargo Embargo { get; set; }
    }
}
