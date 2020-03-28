namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class ProfessionInClient : BaseEntity
    {
        public Decimal PriceHourOrdinary { get; set; }
        public Decimal PriceHourExtra { get; set; }
        public Decimal PriceHourFestive { get; set; }
        public Decimal PriceHourSaleOrdinary { get; set; }
        public Decimal PriceHourSaleExtra { get; set; }
        public Decimal PriceHourSaleFestive { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
