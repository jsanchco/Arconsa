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
                if (startHour == null || endHour == null)
                    return null;

                var dtStartHour = DateTime.ParseExact(startHour, "MM/dd/yyyy HH:mm", null);
                var dtEndHour = DateTime.ParseExact(endHour, "MM/dd/yyyy HH:mm", null);

                return ((DateTime)dtEndHour - dtStartHour).TotalHours;
            }
        }

        public string startHour { get; set; }
        public string endHour { get; set; }
        public int userHiringId { get; set; }
        public string userHiringName { get; set; }
        public int? hourTypeId { get; set; }
        public string hourTypeName { get; set; }
        public int professionId { get; set; }
        public string professionName { get; set; }
    }
}
