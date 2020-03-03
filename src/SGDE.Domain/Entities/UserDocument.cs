namespace SGDE.Domain.Entities
{
    public class UserDocument : BaseEntity
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public byte[] File { get; set; }
        public string TypeFile { get; set; }

        public int TypeDocumentId { get; set; }
        public virtual TypeDocument TypeDocument { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
