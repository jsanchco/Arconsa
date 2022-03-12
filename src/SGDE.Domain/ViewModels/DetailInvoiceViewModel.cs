namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class DetailInvoiceViewModel : BaseEntityViewModel
    {
        public int invoiceId { get; set; }
        public string servicesPerformed { get; set; }
        public double units { get; set; }
        public double unitsAccumulated { get; set; }
        public double unitsTotal => units + unitsAccumulated;
        public string nameUnit { get; set; }
        public double priceUnity { get; set; }
        public double iva { get; set; }
        public double amountUnits => Math.Round(units * priceUnity, 2);
        public double amountAccumulated => Math.Round(unitsAccumulated * priceUnity, 2);
        public double amountTotal => Math.Round((unitsTotal * priceUnity) * (1 + iva), 2);
    }
}
