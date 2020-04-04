namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class DetailInvoice : BaseEntity
    {
        public string ServicesPerformed { get; set; }
        public Decimal Units { get; set; }
        public Decimal PriceUnity { get; set; }

        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
