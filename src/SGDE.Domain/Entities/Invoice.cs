namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class Invoice : BaseEntity
    {
        public int InvoiceNumber { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Decimal TaxBase { get; set; }
        public Decimal Iva { get; set; }
        public Decimal Total { get; set; }
        public Decimal Retentions { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }
    }
}
