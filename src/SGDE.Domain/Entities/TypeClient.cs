namespace SGDE.Domain.Entities
{
    #region

    using System.Collections.Generic;

    #endregion

    public class TypeClient : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Work> Works { get; set; } = new HashSet<Work>();
    }
}
