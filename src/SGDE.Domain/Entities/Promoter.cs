namespace SGDE.Domain.Entities
{
    #region

    using System.Collections.Generic;

    #endregion

    public class Promoter : BaseEntity
    {
        public string Name { get; set; }
        public string Cif { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual ICollection<User> PromoterResponsibles { get; set; } = new HashSet<User>();

        public virtual ICollection<Client> Clients { get; set; } = new HashSet<Client>();
    }
}
