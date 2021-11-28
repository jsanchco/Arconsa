namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;
    using System.Globalization;

    #endregion

    public class DailySigningViewModel  : BaseEntityViewModel
    {
        public double? totalHours
        {
            get
            {
                if (!startHour.HasValue || !endHour.HasValue)
                    return null;

                return (endHour.Value - startHour.Value).TotalHours;
            }
        }

        public DateTime? startHour { get; set; }
        public DateTime? endHour { get; set; }
        public int userHiringId { get; set; }
        public string userHiringName { get; set; }
        public int? hourTypeId { get; set; }
        public string hourTypeName { get; set; }
        public int professionId { get; set; }
        public string professionName { get; set; }
    }
}
