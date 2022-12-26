namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class DetailInvoice : BaseEntity
    {
        public string ServicesPerformed { get; set; }
        public double Units { get; set; }
        public double UnitsAccumulated { get; set; }
        public double PriceUnity { get; set; }
        public string NameUnit { get; set; }
        public double Iva { get; set; }
        public double TaxBase => Math.Round((double)Units * (double)PriceUnity, 4);
        public double TotalIva => Math.Round(((double)Units * (double)PriceUnity) * (Iva), 4);
        public double Total => Math.Round(TaxBase + TotalIva, 4);

        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
