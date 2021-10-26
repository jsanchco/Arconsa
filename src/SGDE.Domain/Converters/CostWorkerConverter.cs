namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class CostWorkerConverter
    {
        public static CostWorkerViewModel Convert(CostWorker costWorker)
        {
            if (costWorker == null)
                return null;

            var costWorkerViewModel = new CostWorkerViewModel
            {
                id = costWorker.Id,
                addedDate = costWorker.AddedDate,
                modifiedDate = costWorker.ModifiedDate,
                iPAddress = costWorker.IPAddress,

                startDate = costWorker.StartDate.ToString("MM/dd/yyyy"),
                endDate = costWorker.EndDate?.ToString("MM/dd/yyyy"),
                priceHourOrdinary = costWorker.PriceHourOrdinary,
                priceHourExtra = costWorker.PriceHourExtra,
                priceHourFestive = costWorker.PriceHourFestive,
                observations = costWorker.Observations,
                userId = costWorker.UserId,
                userName = costWorker.User.Name,
                professionId = costWorker.ProfessionId,
                professionName = costWorker.Profession.Name
            };

            return costWorkerViewModel;
        }

        public static List<CostWorkerViewModel> ConvertList(IEnumerable<CostWorker> costWorkers)
        {
            return costWorkers?.Select(costWorker =>
            {
                var model = new CostWorkerViewModel
                {
                    id = costWorker.Id,
                    addedDate = costWorker.AddedDate,
                    modifiedDate = costWorker.ModifiedDate,
                    iPAddress = costWorker.IPAddress,

                    startDate = costWorker.StartDate.ToString("MM/dd/yyyy"),
                    endDate = costWorker.EndDate?.ToString("MM/dd/yyyy"),
                    priceHourOrdinary = costWorker.PriceHourOrdinary,
                    priceHourExtra = costWorker.PriceHourExtra,
                    priceHourFestive = costWorker.PriceHourFestive,
                    observations = costWorker.Observations,
                    userId = costWorker.UserId,
                    userName = costWorker.User.Name,
                    professionId = costWorker.ProfessionId,
                    professionName = costWorker.Profession.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
