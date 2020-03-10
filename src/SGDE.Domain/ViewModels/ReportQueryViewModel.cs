namespace SGDE.Domain.ViewModels
{
    public class ReportQueryViewModel
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? workerId { get; set; }
        public int? workId { get; set; }
        public int? clientId { get; set; }
    }
}
