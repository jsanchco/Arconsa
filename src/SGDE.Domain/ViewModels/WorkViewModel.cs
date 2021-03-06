﻿namespace SGDE.Domain.ViewModels
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
        public int? numberPersonsRequested { get; set; }
        public bool open { get; set; }
        public string openDate { get; set; }
        public string closeDate { get; set; }
        public bool passiveSubject { get; set; }
        public bool invoiceToOrigin { get; set; }
        public double totalContract { get; set; }
        public double percentageRetention { get; set; }

        public int clientId { get; set; }
        public string clientName { get; set; }
    }
}
