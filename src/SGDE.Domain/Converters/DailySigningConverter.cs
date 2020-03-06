namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class DailySigningConverter
    {
        public static DailySigningViewModel Convert(DailySigning dailySigning)
        {
            if (dailySigning == null)
                return null;

            var dailySigningViewModel = new DailySigningViewModel
            {
                id = dailySigning.Id,
                addedDate = dailySigning.AddedDate,
                modifiedDate = dailySigning.ModifiedDate,
                iPAddress = dailySigning.IPAddress,

                startHour = dailySigning.StartHour,
                endHour = dailySigning.EndHour,
                userHiringId = dailySigning.UserHiringId,
                userHiringName = $"{dailySigning.UserHiring.Work.Name} {dailySigning.UserHiring.StartDate.ToShortDateString()}"
            };

            return dailySigningViewModel;
        }

        public static List<DailySigningViewModel> ConvertList(IEnumerable<DailySigning> dailySignings)
        {
            return dailySignings?.Select(dailySigning =>
            {
                var model = new DailySigningViewModel
                {
                    id = dailySigning.Id,
                    addedDate = dailySigning.AddedDate,
                    modifiedDate = dailySigning.ModifiedDate,
                    iPAddress = dailySigning.IPAddress,

                    startHour = dailySigning.StartHour,
                    endHour = dailySigning.EndHour,
                    userHiringId = dailySigning.UserHiringId,
                    userHiringName = $"{dailySigning.UserHiring.Work.Name} {dailySigning.UserHiring.StartDate.ToShortDateString()}"
                };
                return model;
            })
                .ToList();
        }
    }
}
