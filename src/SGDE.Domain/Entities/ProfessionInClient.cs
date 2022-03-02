namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class ProfessionInClient : BaseEntity
    {
        public double PriceHourSaleOrdinary { get; set; }
        public double PriceHourSaleExtra { get; set; }
        public double PriceHourSaleFestive { get; set; }
        public double PriceHourSaleNocturnal { get; set; }
        public double PriceDailySale { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
