using System;

namespace SGDE.Domain.ViewModels
{
    public class TracingViewModel
    {
        public int workBudgetId { get; set; }
        public string situation { get; set; }
        public string workBudgetName { get; set; }
        public string workBudgetType { get; set; }
        public string workBudgetCode { get; set; }
        public int workId { get; set; }
        public string workName { get; set; }
        public string workStatus { get; set; }
        public int? clientId { get; set; }
        public string clientName { get; set; }
        public string clientEmail { get; set; }
        public DateTime dateSendWorkBudget { get; set; }
        public DateTime? dateAcceptanceWorkBudget { get; set; }
        public DateTime? dateOpenWork { get; set; }
        public string dateOpenWorkFormat
        {
            get
            {
                return dateOpenWork.HasValue ? dateOpenWork.Value.ToString("dd/MM/yyyy") : "PENDIENTE";
            }
        }
        public DateTime? dateCloseWork { get; set; }
        public string dateCloseWorkFormat
        {
            get
            {
                return dateCloseWork.HasValue ? dateCloseWork.Value.ToString("dd/MM/yyyy") : "PENDIENTE";
            }
        }
        public double? workBudgetTotalContract { get; set; }
        public double? invoiceSum { get; set; }
        public double? invoiceTotalPaymentSum { get; set; }
        public string datesSendInvoices { get; set; }
    }
}
