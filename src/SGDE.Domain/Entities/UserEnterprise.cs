namespace SGDE.Domain.Entities
{
    public class UserEnterprise : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int EnterpriseId { get; set; }
        public virtual Enterprise Enterprise { get; set; }
    }
}
