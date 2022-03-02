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
                professionName = dailySigning.Profession.Name,
                professionId = dailySigning.ProfessionId,
                workName = dailySigning.UserHiring.Work.Name,
                clientName = dailySigning.UserHiring.Work.Client.Name,
                hours = ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours,
                dateHour = dailySigning.StartHour?.ToString("dd/MM/yyyy"),
                priceHour = ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId),
                priceHourSale = ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * GetPriceHourSale(dailySigning.UserHiring.Work.Client, (int)dailySigning.HourTypeId, (int)dailySigning.UserHiring.ProfessionId)
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
                    professionName = dailySigning.Profession.Name,
                    professionId = dailySigning.ProfessionId,
                    hourTypeId = dailySigning.HourTypeId,
                    hourTypeName = dailySigning.HourType?.Name,
                    workName = dailySigning.UserHiring.Work.Name,
                    clientName = dailySigning.UserHiring.Work.Client.Name,
                    hours = dailySigning.HourTypeId != 5 ? ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours : 0,
                    dateHour = dailySigning.StartHour?.ToString("dd/MM/yyyy"),

                    priceHour = dailySigning.HourTypeId != 5 ? 
                        ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId) :
                        GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId),

                    priceHourSale = dailySigning.HourTypeId != 5 ? 
                        ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.ProfessionId) :
                        GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.ProfessionId)
                };
                return model;
            })
                .ToList();
        }

        public static double GetPriceHourCost(
            User user,
            int professionId,
            DateTime date, 
            int? hourType)
        {
            if (hourType == null)
                return 0;

            var dateResetHour = new DateTime(date.Year, date.Month, date.Day);
            var costWorker = user.CostWorkers.FirstOrDefault(x => (x.ProfessionId == professionId) && 
                                                                  ((x.EndDate == null && x.StartDate <= dateResetHour) || 
                                                                   (x.EndDate != null && x.StartDate <= dateResetHour && x.EndDate >= dateResetHour)));
            if (costWorker == null)
                return 0;

            switch (hourType)
            {
                case 1:
                    return costWorker.PriceHourOrdinary;
                case 2:
                    return costWorker.PriceHourExtra;
                case 3:
                    return costWorker.PriceHourFestive;
                case 4:
                    return costWorker.PriceHourNocturnal;
                case 5:
                    return costWorker.PriceDaily;

                default:
                    return 0;
            }
        }

        public static double GetPriceHourSale(
            Client client, 
            int? type, 
            int? professionId)
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
                case 4:
                    return professionInClient.PriceHourSaleNocturnal;
                case 5:
                    return professionInClient.PriceDailySale;

                default:
                    return 0;
            }
        }
    }
}
