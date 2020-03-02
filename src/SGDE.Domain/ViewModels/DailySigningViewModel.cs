namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class DailySigningViewModel : BaseEntityViewModel
    {
        public DateTime startHour { get; set; }
        public DateTime? endHour { get; set; }

        public int workId { get; set; }
        public string workName { get; set; }

        public int userId { get; set; }
        public string userName { get; set; }
    }
}
