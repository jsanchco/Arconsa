namespace SGDE.Domain.Entities
{   
    #region Using

    using System.Collections.Generic;
    using System;

    #endregion

    public class Work : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string EstimatedDuration { get; set; }
        public string WorksToRealize { get; set; }
        public int NumberPersonsRequested { get; set; }
        public bool Open { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<User> WorkResponsibles { get; set; } = new HashSet<User>();
        public virtual ICollection<UserHiring> UserHirings { get; set; } = new HashSet<UserHiring>();
        public virtual ICollection<DailySigning> DailySignings { get; set; } = new HashSet<DailySigning>();
    }
}
