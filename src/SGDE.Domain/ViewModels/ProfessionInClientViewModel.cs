namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class ProfessionInClientViewModel : BaseEntityViewModel
    {
        public Decimal priceHourSale { get; set; }

        public int clientId { get; set; }
        public string clientName { get; set; }

        public int professionId { get; set; }
        public string professionName { get; set; }
    }
}
