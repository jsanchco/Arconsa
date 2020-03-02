namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class DailySigning : BaseEntity
    {
        public DateTime StartHour { get; set; }
        public DateTime? EndHour { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
