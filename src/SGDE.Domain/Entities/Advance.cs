using System;

namespace SGDE.Domain.Entities
{
    public class Advance : BaseEntity
    {
        public DateTime ConcessionDate { get; set; }
        public DateTime? PayDate { get; set; }
        public double Amount { get; set; }
        public bool Paid => PayDate != null;

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
