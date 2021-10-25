namespace SGDE.Domain.Entities
{
    public class UserProfession : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
