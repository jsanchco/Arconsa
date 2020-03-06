namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class DailySigningViewModel : BaseEntityViewModel
    {
        public DateTime startHour { get; set; }
        public DateTime? endHour { get; set; }
        
        public int userHiringId { get; set; }
        public string userHiringName { get; set; }
    }
}
