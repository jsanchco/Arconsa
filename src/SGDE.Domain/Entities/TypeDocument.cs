namespace SGDE.Domain.Entities
{
    #region

    using System.Collections.Generic;

    #endregion

    public class TypeDocument : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserDocument> UserDocuments { get; set; } = new HashSet<UserDocument>();
    }
}
