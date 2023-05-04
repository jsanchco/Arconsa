using System.Collections.Generic;

namespace SGDE.Domain.Entities
{
    public class Enterprise : BaseEntity
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Address { get; set; }

        public virtual ICollection<UserEnterprise> UsersEnterprises { get; set; } = new HashSet<UserEnterprise>();
    }
}
