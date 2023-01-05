namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class InvoiceViewModel : BaseEntityViewModel
    {
        public int invoiceNumber { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime? payDate { get; set; }
        public int expirationDays { get; set; }
        public DateTime? expirationDate { get; set; }
        public double taxBase { get; set; }
        public double ivaTaxBase { get; set; }
        public double total { get; set; }
        public bool iva { get; set; } 
        public double ivaValue { get; set; } 
        public int typeInvoice { get; set; }  // 1 = por horas, 2 = custom
        public double retentions { get; set; }       
        public double totalPayment { get; set; }
        public double remaining => total - totalPayment;
        public bool isPaid => totalPayment >= total;

        public int? workId { get; set; }
        public string workName { get; set; }
        public int? clientId { get; set; }
        public string clientName { get; set; }
        public int? userId { get; set; }
        public string userName { get; set; }
        public int? invoiceToCancelId { get; set; }
        public string invoiceToCancelName { get; set; }
        public int? workBudgetId { get; set; }
        public string workBudgetName { get; set; }

        public List<DetailInvoiceViewModel> detailInvoice { get; set; }
    }
}
