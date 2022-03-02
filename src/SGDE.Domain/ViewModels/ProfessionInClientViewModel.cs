namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class ProfessionInClientViewModel : BaseEntityViewModel
    {
        public double priceHourSaleOrdinary { get; set; }
        public double priceHourSaleExtra { get; set; }
        public double priceHourSaleFestive { get; set; }
        public double priceHourSaleNocturnal { get; set; }
        public double priceDailySale { get; set; }

        public int clientId { get; set; }
        public string clientName { get; set; }

        public int professionId { get; set; }
        public string professionName { get; set; }
    }
}
