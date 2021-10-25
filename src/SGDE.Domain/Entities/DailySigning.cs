namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class DailySigning : BaseEntity
    {
        public DateTime StartHour { get; set; }
        public DateTime? EndHour { get; set; }

        public int UserHiringId { get; set; }
        public virtual UserHiring UserHiring { get; set; }

        public int? HourTypeId { get; set; }
        public virtual HourType HourType { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
