using System;

namespace SGDE.Domain.Entities
{
    public class Library : BaseEntity
    {
        public string Reference { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Edition { get; set; }
        public bool Active { get; set; } = true;
        public string TypeFile { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
    }
}
