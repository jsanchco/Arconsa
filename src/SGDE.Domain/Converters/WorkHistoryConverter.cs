using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public static class WorkHistoryConverter
    {
        public static WorkHistoryViewModel Convert(WorkHistory workHistory)
        {
            if (workHistory == null)
                return null;

            var workHistoryViewModel = new WorkHistoryViewModel
            {
                id = workHistory.Id,
                addedDate = workHistory.AddedDate,
                modifiedDate = workHistory.ModifiedDate,
                iPAddress = workHistory.IPAddress,

                reference = workHistory.Reference,
                description = workHistory.Description,
                date = workHistory.Date,
                observations = workHistory.Observations,
                type = workHistory.Type,
                workId = workHistory.WorkId,
                typeFile = workHistory.TypeFile,
                file = workHistory.File,
                fileName = workHistory.FileName
            };

            return workHistoryViewModel;
        }

        public static List<WorkHistoryViewModel> ConvertList(IEnumerable<WorkHistory> workHistories)
        {
            return workHistories?.Select(workHistory =>
            {
                var model = new WorkHistoryViewModel
                {
                    id = workHistory.Id,
                    addedDate = workHistory.AddedDate,
                    modifiedDate = workHistory.ModifiedDate,
                    iPAddress = workHistory.IPAddress,

                    reference = workHistory.Reference,
                    description = workHistory.Description,
                    date = workHistory.Date,
                    observations = workHistory.Observations,
                    type = workHistory.Type,
                    workId = workHistory.WorkId,
                    typeFile = workHistory.TypeFile,
                    file = workHistory.File,
                    fileName = workHistory.FileName
                };
                return model;
            })
                .ToList();
        }
    }
}
