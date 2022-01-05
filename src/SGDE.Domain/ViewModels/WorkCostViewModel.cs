using System;

namespace SGDE.Domain.ViewModels
{
    public class WorkCostViewModel : BaseEntityViewModel
    {
        public string typeWorkCost { get; set; }
        public DateTime date { get; set; }
        public string numberInvoice { get; set; }
        public string provider { get; set; }
        public double taxBase { get; set; }
        public string description { get; set; }
        public string fileName { get; set; }
        public string typeFile { get; set; }
        public byte[] file { get; set; }
        public int workId { get; set; }
        public string workName { get; set; }
        public bool hasFile => file != null;
    }
}
