namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class InvoiceViewModel : BaseEntityViewModel
    {
        public int invoiceNumber { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public Decimal taxBase { get; set; }
        public Decimal iva { get; set; }
        public Decimal total { get; set; }
        public Decimal retentions { get; set; }

        public int workId { get; set; }
        public string workName { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
    }
}
