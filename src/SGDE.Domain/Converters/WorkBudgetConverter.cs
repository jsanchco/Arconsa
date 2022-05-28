using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public class WorkBudgetConverter
    {
        public static WorkBudgetViewModel Convert(WorkBudget workBudget)
        {
            if (workBudget == null)
                return null;

            var workBudgetViewModel = new WorkBudgetViewModel
            {
                id = workBudget.Id,
                addedDate = workBudget.AddedDate,
                modifiedDate = workBudget.ModifiedDate,
                iPAddress = workBudget.IPAddress,

                date = workBudget.Date,
                name = workBudget.Name,
                reference = workBudget.Reference,
                type = workBudget.Type,
                typeFile = workBudget.TypeFile,
                file = workBudget.File,
                nameInWork = workBudget.NameInWork,
                totalContract = workBudget.TotalContract,
                workBudgetDataId = workBudget.WorkBudgetDataId,
                
                workId = workBudget.WorkId,
                workName = workBudget.Work.Name
            };

            return workBudgetViewModel;
        }

        public static List<WorkBudgetViewModel> ConvertList(IEnumerable<WorkBudget> workBudgets)
        {
            return workBudgets?.Select(workBudget =>
            {
                var model = new WorkBudgetViewModel
                {
                    id = workBudget.Id,
                    addedDate = workBudget.AddedDate,
                    modifiedDate = workBudget.ModifiedDate,
                    iPAddress = workBudget.IPAddress,

                    date = workBudget.Date,
                    name = workBudget.Name,
                    reference = workBudget.Reference,
                    type = workBudget.Type,
                    typeFile = workBudget.TypeFile,
                    file = workBudget.File,
                    nameInWork = workBudget.NameInWork,
                    totalContract = workBudget.TotalContract,
                    workBudgetDataId = workBudget.WorkBudgetDataId,

                    workId = workBudget.WorkId,
                    workName = workBudget.Work.Name
                };
                return model;
            })
                .ToList();
        }

        public static List<WorkBudgetViewModel> ConvertListLite(IEnumerable<WorkBudget> workBudgets)
        {
            return workBudgets?.Select(workBudget =>
            {
                var model = new WorkBudgetViewModel
                {
                    id = workBudget.Id,
                    name = workBudget.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
