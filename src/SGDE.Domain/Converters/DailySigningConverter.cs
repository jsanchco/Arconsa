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

                startHour = dailySigning.StartHour?.ToString("MM/dd/yyyy HH:mm"),
                endHour = dailySigning.EndHour?.ToString("MM/dd/yyyy HH:mm"),
                userHiringId = dailySigning.UserHiringId,
                userHiringName = $"{dailySigning.UserHiring.Work.Name} {dailySigning.UserHiring.StartDate.ToShortDateString()}",
                hourTypeId = dailySigning.HourTypeId,
                hourTypeName = dailySigning.HourType?.Name,
                professionId = dailySigning.ProfessionId,
                professionName = dailySigning.Profession.Name
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

                    startHour = dailySigning.StartHour?.ToString("MM/dd/yyyy HH:mm"),
                    endHour = dailySigning.EndHour?.ToString("MM/dd/yyyy HH:mm"),
                    userHiringId = dailySigning.UserHiringId,
                    userHiringName = $"{dailySigning.UserHiring.Work.Name} {dailySigning.UserHiring.StartDate.ToShortDateString()}",
                    hourTypeId = dailySigning.HourTypeId,
                    hourTypeName = dailySigning.HourType?.Name,
                    professionId = dailySigning.ProfessionId,
                    professionName = dailySigning.Profession.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
