using System;

namespace SGDE.Domain.Entities
{
    public class WorkStatusHistory : BaseEntity
    {
        public DateTime DateChange { get; set; }
        public string Value { get; set; }
        public string Observations { get; set; }


        public int WorkId{ get; set; }
        public virtual Work Work { get; set; }
    }
}
