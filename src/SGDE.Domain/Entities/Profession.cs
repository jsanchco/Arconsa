namespace SGDE.Domain.Entities
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class Profession : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserProfession> UserProfessions { get; set; } = new HashSet<UserProfession>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        public virtual ICollection<ProfessionInClient> ProfessionInClients { get; set; } = new HashSet<ProfessionInClient>();
        public virtual ICollection<UserHiring> UserHirings { get; set; } = new HashSet<UserHiring>();
        public virtual ICollection<CostWorker> CostWorkers { get; set; } = new HashSet<CostWorker>();
        public virtual ICollection<DailySigning> DailySignings { get; set; } = new HashSet<DailySigning>();
        //public virtual ICollection<CostWorker> CostWorker { get; set; } = new HashSet<DailySigning>();
    }
}
