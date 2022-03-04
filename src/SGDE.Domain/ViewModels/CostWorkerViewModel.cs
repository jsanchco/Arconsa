namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class CostWorkerViewModel : BaseEntityViewModel
    {
        public double priceHourOrdinary { get; set; }
        public double priceHourExtra { get; set; }
        public double priceHourFestive { get; set; }
        public double priceHourNocturnal { get; set; }
        public string priceHourOrdinaryS { get; set; }
        public string priceHourExtraS { get; set; }
        public string priceHourFestiveS { get; set; }
        public string priceHourNocturnalS { get; set; }
        public double priceDaily { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string observations { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public int? professionId { get; set; }
        public string professionName { get; set; } 
    }
}
