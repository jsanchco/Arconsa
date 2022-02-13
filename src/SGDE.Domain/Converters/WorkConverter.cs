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
                totalContract = (double)work.WorkBudgets.FirstOrDefault(x => x.Type == "Definitivo")?.TotalContract,
                percentageRetention = (double)work.PercentageRetention,
                percentageIVA = (double)work.PercentageIVA,

                openDate = work.OpenDate.ToString("dd/MM/yyyy"),
                closeDate = work.CloseDate?.ToString("dd/MM/yyyy"),

                passiveSubject = work.PassiveSubject,

                clientId = work.ClientId,
                clientName = work.Client.Name,
                workBudgets = work.WorkBudgets
                    .Where(x => x.Type == "Definitivo" || x.Type == "Complementario X")
                    .Select(x => (name: x.NameInWork, value: x.TotalContract)).ToList()
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
                    totalContract = (double)work.WorkBudgets.FirstOrDefault(x => x.Type == "Definitivo")?.TotalContract,
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

        public static List<WorkViewModel> ConvertListLite(IEnumerable<Work> works)
        {
            return works?.Select(work =>
            {
                var model = new WorkViewModel
                {
                    id = work.Id,
                    name = work.Name
                };
                return model;
            })
                .ToList();
        }

        private static List<(string, double)> GetWorkBudgets(ICollection<WorkBudget> workBudgets)
        {
            return workBudgets.Select(x => (x.NameInWork, x.TotalContract)).ToList();
        }
    }
}
