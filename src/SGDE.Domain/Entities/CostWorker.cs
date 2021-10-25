namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class CostWorker : BaseEntity
    {
        public Decimal PriceHourOrdinary { get; set; }
        public Decimal PriceHourExtra { get; set; }
        public Decimal PriceHourFestive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Observations { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int? ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
