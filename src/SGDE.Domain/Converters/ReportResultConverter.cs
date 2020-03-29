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
                professionName = dailySigning.UserHiring.User.Profession?.Name,
                professionId = dailySigning.UserHiring.User.Profession?.Id,
                workName = dailySigning.UserHiring.Work.Name,
                clientName = dailySigning.UserHiring.Work.Client.Name,
                hours = ((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours,
                dateHour = dailySigning.StartHour.ToString("dd/MM/yyyy"),
                priceHour = (decimal)((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours * GetPriceHourCost(dailySigning.UserHiring.Work.Client, (int)dailySigning.HourTypeId, (int)dailySigning.UserHiring.ProfessionId),
                priceHourSale = (decimal)((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours * GetPriceHourSale(dailySigning.UserHiring.Work.Client, (int)dailySigning.HourTypeId, (int)dailySigning.UserHiring.ProfessionId)
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
                    professionName = dailySigning.UserHiring.Profession?.Name,
                    professionId = dailySigning.UserHiring.ProfessionId,
                    hourTypeId = dailySigning.HourTypeId,
                    hourTypeName = dailySigning.HourType?.Name,
                    workName = dailySigning.UserHiring.Work.Name,
                    clientName = dailySigning.UserHiring.Work.Client.Name,
                    hours = ((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours,
                    dateHour = dailySigning.StartHour.ToString("dd/MM/yyyy"),
                    priceHour = (decimal)((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours * GetPriceHourCost(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.UserHiring.ProfessionId),
                    priceHourSale = (decimal)((DateTime)dailySigning.EndHour - dailySigning.StartHour).TotalHours * GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.UserHiring.ProfessionId)
                };
                return model;
            })
                .ToList();
        }

        private static decimal GetPriceHourCost(Client client, int? type, int? professionId)
        {
            if (type == null || professionId == null)
                return 0;

            var professionInClient = client.ProfessionInClients.FirstOrDefault(x => x.ProfessionId == professionId);
            if (professionInClient == null)
                return 0;

            switch (type)
            {
                case 1:
                    return professionInClient.PriceHourOrdinary;
                case 2:
                    return professionInClient.PriceHourExtra;
                case 3:
                    return professionInClient.PriceHourFestive;

                default:
                    return 0;
            }
        }

        private static decimal GetPriceHourSale(Client client, int? type, int? professionId)
        {
            if (type == null || professionId == null)
                return 0;

            var professionInClient = client.ProfessionInClients.FirstOrDefault(x => x.ProfessionId == professionId);
            if (professionInClient == null)
                return 0;

            switch (type)
            {
                case 1:
                    return professionInClient.PriceHourSaleOrdinary;
                case 2:
                    return professionInClient.PriceHourSaleExtra;
                case 3:
                    return professionInClient.PriceHourSaleFestive;

                default:
                    return 0;
            }
        }
    }
}
