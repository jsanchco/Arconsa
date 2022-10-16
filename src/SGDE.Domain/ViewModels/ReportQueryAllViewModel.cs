using System;

namespace SGDE.Domain.ViewModels
{
    public class ReportQueryAllViewModel
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool workers { get; set; }
        public bool works { get; set; }
        public bool clients { get; set; }
        public bool showCeros { get; set; }
        public string filter { get; set; }
    }
}
