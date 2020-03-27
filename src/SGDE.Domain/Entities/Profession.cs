namespace SGDE.Domain.Entities
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class Profession : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users{ get; set; } = new HashSet<User>();
        public virtual ICollection<ProfessionInClient> ProfessionInClients { get; set; } = new HashSet<ProfessionInClient>();
        public virtual ICollection<UserHiring> UserHirings { get; set; } = new HashSet<UserHiring>();
    }
}
