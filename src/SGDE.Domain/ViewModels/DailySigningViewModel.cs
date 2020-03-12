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

                var dtStartHour = DateTime.ParseExact(startHour, "MM/dd/yyyy hh:mm", null);
                var dtEndHour = DateTime.ParseExact(endHour, "MM/dd/yyyy hh:mm", null);

                return ((DateTime)dtEndHour - dtStartHour).TotalHours;
            }
        }

        public string startHour { get; set; }
        public string endHour { get; set; }
        public int userHiringId { get; set; }
        public string userHiringName { get; set; }
    }
}
