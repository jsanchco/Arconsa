namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class ReportResultViewModel
    {
        public string clientName { get; set; }
        public string workName { get; set; }
        public string userName { get; set; }
        public string dateHour { get; set; }
        public double hours { get; set; }
        public Decimal priceHour { get; set; }
        public Decimal priceHourSale { get; set; }
    }
}
