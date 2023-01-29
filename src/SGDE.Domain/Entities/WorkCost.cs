using System;

namespace SGDE.Domain.Entities
{
    public class WorkCost : BaseEntity
    {
        public string TypeWorkCost { get; set; }
        public DateTime Date { get; set; }
        public string NumberInvoice { get; set; }
        public string Provider { get; set; }
        public double TaxBase { get; set; }
        public double Iva { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string TypeFile { get; set; }
        public byte[] File { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }
    }
}
