namespace SGDE.Domain.Converters
{
    #region Using

    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    #endregion

    public class WorkBudgetDataConverter
    {
        public static WorkBudgetDataViewModel Convert(WorkBudgetData workBudgetData)
        {
            if (workBudgetData == null)
                return null;

            var workBudgetDataViewModel = new WorkBudgetDataViewModel
            {
                id = workBudgetData.Id,
                addedDate = workBudgetData.AddedDate,
                modifiedDate = workBudgetData.ModifiedDate,
                iPAddress = workBudgetData.IPAddress,

                description = workBudgetData.Description,
                reference = workBudgetData.Reference,

                workId = workBudgetData.WorkId,
                workName = workBudgetData.Work.Name,
                total = workBudgetData.WorkBudgets.Where(x => x.Type == "Definitivo").Sum(x => x.TotalContract)
            };

            return workBudgetDataViewModel;
        }

        public static List<WorkBudgetDataViewModel> ConvertList(IEnumerable<WorkBudgetData> workBudgetDatas)
        {
            return workBudgetDatas?.Select(workBudgetData =>
            {
                var model = new WorkBudgetDataViewModel
                {
                    id = workBudgetData.Id,
                    addedDate = workBudgetData.AddedDate,
                    modifiedDate = workBudgetData.ModifiedDate,
                    iPAddress = workBudgetData.IPAddress,

                    description = workBudgetData.Description,
                    reference = workBudgetData.Reference,

                    workId = workBudgetData.WorkId,
                    workName = workBudgetData.Work.Name,
                    total = workBudgetData.WorkBudgets.Where(x => x.Type == "Definitivo").Sum(x => x.TotalContract)
                };
                return model;
            })
                .ToList();
        }
    }
}