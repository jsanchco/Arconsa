namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class WorkConverter
    {
        public static WorkViewModel Convert(Work work)
        {
            if (work == null)
                return null;

            var workViewModel = new WorkViewModel
            {
                id = work.Id,
                addedDate = work.AddedDate,
                modifiedDate = work.ModifiedDate,
                iPAddress = work.IPAddress,

                name = work.Name,
                address = work.Address,
                estimatedDuration = work.EstimatedDuration,
                worksToRealize = work.WorksToRealize,
                numberPersonsRequested = work.UserHirings.Where(x => x.EndDate == null)?.Count(),
                open = work.Open,
                invoiceToOrigin = work.InvoiceToOrigin,
                totalContract = (double)work.TotalContract,
                percentageRetention = (double)work.PercentageRetention,

                openDate = work.OpenDate.ToString("dd/MM/yyyy"),
                closeDate = work.CloseDate?.ToString("dd/MM/yyyy"),

                passiveSubject = work.PassiveSubject,

                clientId = work.ClientId,
                clientName = work.Client.Name
            };

            return workViewModel;
        }

        public static List<WorkViewModel> ConvertList(IEnumerable<Work> works)
        {
            return works?.Select(work =>
            {
                var model = new WorkViewModel
                {
                    id = work.Id,
                    addedDate = work.AddedDate,
                    modifiedDate = work.ModifiedDate,
                    iPAddress = work.IPAddress,

                    name = work.Name,
                    address = work.Address,
                    estimatedDuration = work.EstimatedDuration,
                    worksToRealize = work.WorksToRealize,
                    numberPersonsRequested = work.UserHirings.Where(x => x.EndDate == null).Count(),
                    open = work.Open,
                    invoiceToOrigin = work.InvoiceToOrigin,
                    totalContract = (double)work.TotalContract,
                    percentageRetention = (double)work.PercentageRetention,

                    openDate = work.OpenDate.ToString("dd/MM/yyyy"),
                    closeDate = work.CloseDate?.ToString("dd/MM/yyyy"),

                    passiveSubject = work.PassiveSubject,

                    clientId = work.ClientId,
                    clientName = work.Client.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
