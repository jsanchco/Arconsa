namespace SGDE.Domain.Entities
{
    #region Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class UserHiring : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<DailySigning> DailysSigning { get; set; } = new HashSet<DailySigning>();
    }
}
