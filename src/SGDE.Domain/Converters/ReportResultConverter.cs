namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;
    using System;

    #endregion

    public class ReportResultConverter
    {
        public static ReportResultViewModel Convert(DailySigning dailySigning)
        {
            if (dailySigning == null)
                return null;

            var reportResultViewModel = new ReportResultViewModel
            {
                userName = $"{dailySigning.UserHiring.User.Name} {dailySigning.UserHiring.User.Surname}",
                workName = dailySigning.UserHiring.Work.Name,
                clientName = dailySigning.UserHiring.Work.Client.Name,
                hours = ((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours,
                dateHour = dailySigning.StartHour.ToString("dd/MM/yyyy"),
                priceHour = dailySigning.UserHiring.User.PriceHour,
                priceHourSale = dailySigning.UserHiring.User.PriceHourSale
            };

            return reportResultViewModel;
        }

        public static List<ReportResultViewModel> ConvertList(IEnumerable<DailySigning> dailySignings)
        {
            return dailySignings?.Select(dailySigning =>
            {
                var model = new ReportResultViewModel
                {
                    userName = $"{dailySigning.UserHiring.User.Name} {dailySigning.UserHiring.User.Surname}",
                    workName = dailySigning.UserHiring.Work.Name,
                    clientName = dailySigning.UserHiring.Work.Client.Name,
                    hours = ((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours,
                    dateHour = dailySigning.StartHour.ToString("dd/MM/yyyy"),
                    priceHour = (decimal)((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours * dailySigning.UserHiring.User.PriceHour,
                    priceHourSale = (decimal)((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours * dailySigning.UserHiring.User.PriceHourSale
                };
                return model;
            })
                .ToList();
        }
    }
}
