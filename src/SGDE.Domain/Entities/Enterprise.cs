using System.Collections.Generic;

namespace SGDE.Domain.Entities
{
    public class Enterprise : BaseEntity
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string CIF { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<UserEnterprise> UsersEnterprises { get; set; } = new HashSet<UserEnterprise>();
        public virtual ICollection<CompanyData> CompanyDatas { get; set; } = new HashSet<CompanyData>();
        public virtual ICollection<IndirectCost> IndirectCosts{ get; set; } = new HashSet<IndirectCost>();
    }
}
