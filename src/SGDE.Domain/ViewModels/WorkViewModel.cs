namespace SGDE.Domain.ViewModels
{
    #region

    using System;

    #endregion

    public class WorkViewModel : BaseEntityViewModel
    {
        public string name { get; set; }
        public string address { get; set; }
        public string estimatedDuration { get; set; }
        public string worksToRealize { get; set; }
        public int numberPersonsRequested { get; set; }
        public bool open { get; set; }
        public DateTime? openDate { get; set; }
        public DateTime? closeDate { get; set; }

        public int clientId { get; set; }
        public string clientName { get; set; }
    }
}
