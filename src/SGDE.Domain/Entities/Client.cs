namespace SGDE.Domain.Entities
{
    #region
    
    using System.Collections.Generic;

    #endregion

    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string Cif { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public int? PromoterId { get; set; } 
        public virtual Promoter Promoter { get; set; }

        public int? TypeClientId { get; set; }
        public virtual TypeClient TypeClient { get; set; }

        public virtual ICollection<User> ClientResponsibles { get; set; } = new HashSet<User>();
    }
}
