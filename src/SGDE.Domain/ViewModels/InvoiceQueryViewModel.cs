namespace SGDE.Domain.ViewModels
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class InvoiceQueryViewModel
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string issueDate { get; set; }
        public int typeInvoice { get; set; }        
        public int? clientId { get; set; }
        public int? workId { get; set; }
        public int? workerId { get; set; }

        public List<DetailInvoiceViewModel> detailInvoice { get; set; }

        public InvoiceQueryViewModel() => detailInvoice = new List<DetailInvoiceViewModel>();
    }
}
