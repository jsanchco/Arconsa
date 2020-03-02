namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class UserHiring : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int BuilderId { get; set; }
        public virtual Client Builder { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
