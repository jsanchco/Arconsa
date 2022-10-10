using System;

namespace SGDE.Domain.Entities
{
    public class WorkHistory : BaseEntity
    {
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public string Type { get; set; }
        public string TypeFile { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }
    }
}
