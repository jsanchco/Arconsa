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
    }
}
