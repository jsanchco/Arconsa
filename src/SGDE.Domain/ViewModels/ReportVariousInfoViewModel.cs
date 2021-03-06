﻿namespace SGDE.Domain.ViewModels
{
    public class ReportVariousInfoViewModel
    {
        public string clientName { get; set; }
        public string workName { get; set; }
        public string workerName { get; set; }
        public int totalWorkers { get; set; }
        public double totalHoursOrdinary { get; set; }
        public double totalHoursFestive { get; set; }
        public double totalHoursExtraordinary { get; set; }
        public double priceTotalHoursOrdinary { get; set; }
        public double priceTotalHoursFestive { get; set; }
        public double priceTotalHoursExtraordinary { get; set; }
        public double priceTotalHoursSaleOrdinary { get; set; }
        public double priceTotalHoursSaleFestive { get; set; }
        public double priceTotalHoursSaleExtraordinary { get; set; }
    }
}
