namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class CostWorkerViewModel : BaseEntityViewModel
    {
        public Decimal priceHourOrdinary { get; set; }
        public Decimal priceHourExtra { get; set; }
        public Decimal priceHourFestive { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string observations { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public int? professionId { get; set; }
        public string professionName { get; set; } 
    }
}
