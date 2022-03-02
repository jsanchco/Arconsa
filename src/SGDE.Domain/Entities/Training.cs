namespace SGDE.Domain.Entities
{
    #region

    using System;

    #endregion

    public class Training : BaseEntity
    {
        public string Name { get; set; }
        public double Hours { get; set; }
        public string Center { get; set; }
        public string Address { get; set; }
        public byte[] File { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
