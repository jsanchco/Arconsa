using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkReportViewModel
    {
        public string status { get; set; }
        public string workBudgetName { get; set; }
        public string workType { get; set; }
        public int workId { get; set; }
        public string workName { get; set; }
        public int? clientId { get; set; }
        public string clientName { get; set; }
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
        public double? invoicePaidSum { get; set; }
    }
}
