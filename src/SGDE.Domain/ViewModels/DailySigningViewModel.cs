namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;
    using System.Globalization;

    #endregion

    public class DailySigningViewModel : BaseEntityViewModel
    {
        public double? totalHours
        {
            get
            {
                if (endHour == null)
                    return null;

                var dtStartHour = DateTime.Parse(startHour);
                var dtEndHour = DateTime.Parse(endHour);

                return ((DateTime)dtEndHour - dtStartHour).TotalHours;
            }
        }

        public string startHour { get; set; }
        public string endHour { get; set; }
        public int userHiringId { get; set; }
        public string userHiringName { get; set; }
    }
}
