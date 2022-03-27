namespace SGDE.Domain.ViewModels
{
    public class ReportResultsByWorkViewModel
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public int workId { get; set; }
        public string workName { get; set; }
        public double costWorkers { get; set; }
        public double costProviders { get; set; }
        public double total { get; set; }
    }
}
