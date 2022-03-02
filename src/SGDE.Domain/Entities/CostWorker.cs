namespace SGDE.Domain.Entities
{
    #region Using

    using System;

    #endregion

    public class CostWorker : BaseEntity
    {
        public double PriceHourOrdinary { get; set; }
        public double PriceHourExtra { get; set; }
        public double PriceHourFestive { get; set; }
        public double PriceHourNocturnal { get; set; }
        public double PriceDaily { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Observations { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int? ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
