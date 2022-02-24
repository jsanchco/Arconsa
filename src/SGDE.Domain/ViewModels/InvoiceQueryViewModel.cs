namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class InvoiceQueryViewModel
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime? payDate { get; set; }
        public int typeInvoice { get; set; }        
        public int? clientId { get; set; }
        public int? workId { get; set; }
        public int? workerId { get; set; }

        public List<DetailInvoiceViewModel> detailInvoice { get; set; }

        public InvoiceQueryViewModel() => detailInvoice = new List<DetailInvoiceViewModel>();
    }
}
