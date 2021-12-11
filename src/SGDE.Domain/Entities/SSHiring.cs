using System;

namespace SGDE.Domain.Entities
{
    public class SSHiring : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Observations { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
