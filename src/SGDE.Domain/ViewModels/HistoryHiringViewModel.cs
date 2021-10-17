namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class HistoryHiringViewModel
    {
        public int userHiringId { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public int workId { get; set; }
        public string workName { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public DateTime dtStartDate { get; set; }
        public string startDate => dtStartDate.ToString("MM/dd/yyyy");
        public DateTime? dtEndDate { get; set; }
        public string endDate => dtEndDate?.ToString("MM/dd/yyyy");
        public int professionId { get; set; }
        public string professionName { get; set; }
        public bool inWork { get; set; }
        public string status => inWork ? "En Obra" : "Fuera de Obra";
    }
}
