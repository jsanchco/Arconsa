namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class WorkStatusHistoryConverter
    {
        public static WorkStatusHistoryViewModel Convert(WorkStatusHistory workStatusHistory)
        {
            if (workStatusHistory == null)
                return null;

            var workStatusHistoryViewModel = new WorkStatusHistoryViewModel
            {
                id = workStatusHistory.Id,
                addedDate = workStatusHistory.AddedDate,
                modifiedDate = workStatusHistory.ModifiedDate,
                iPAddress = workStatusHistory.IPAddress,

                value = workStatusHistory.Value,
                observations = workStatusHistory.Observations,
                dateChange = workStatusHistory.DateChange,
                workId = workStatusHistory.WorkId,
                workName = workStatusHistory.Work.Name
            };

            return workStatusHistoryViewModel;
        }

        public static List<WorkStatusHistoryViewModel> ConvertList(IEnumerable<WorkStatusHistory> workStatusHistories)
        {
            return workStatusHistories?.Select(workStatusHistory =>
            {
                var model = new WorkStatusHistoryViewModel
                {
                    id = workStatusHistory.Id,
                    addedDate = workStatusHistory.AddedDate,
                    modifiedDate = workStatusHistory.ModifiedDate,
                    iPAddress = workStatusHistory.IPAddress,

                    value = workStatusHistory.Value,
                    observations = workStatusHistory.Observations,
                    dateChange = workStatusHistory.DateChange,
                    workId = workStatusHistory.WorkId,
                    workName = workStatusHistory.Work.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
