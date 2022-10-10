using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<WorkHistoryViewModel> GetAllWorkHistory(int workId, int skip = 0, int take = 0, string filter = null)
        {
            var queryResult = _workHistoryRepository.GetAll(workId, skip, take, filter);
            return new QueryResult<WorkHistoryViewModel>
            {
                Data = WorkHistoryConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public WorkHistoryViewModel GetWorkHistoryById(int id)
        {
            var workHistoryViewModel = WorkHistoryConverter.Convert(_workHistoryRepository.GetById(id));

            return workHistoryViewModel;
        }

        public WorkHistoryViewModel AddWorkHistory(WorkHistoryViewModel newWorkHistoryViewModel)
        {
            var workHistory = new WorkHistory
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newWorkHistoryViewModel.iPAddress,

                Reference = newWorkHistoryViewModel.reference,
                Date = newWorkHistoryViewModel.date,
                WorkId = newWorkHistoryViewModel.workId,
                Description = newWorkHistoryViewModel.description,
                Observations = newWorkHistoryViewModel.observations,
                Type = newWorkHistoryViewModel.type,
                TypeFile = newWorkHistoryViewModel.typeFile,
                FileName = newWorkHistoryViewModel.fileName,
                File = newWorkHistoryViewModel.file
            };

            _workHistoryRepository.Add(workHistory);
            return newWorkHistoryViewModel;
        }

        public bool UpdateWorkHistory(WorkHistoryViewModel workHistoryViewModel)
        {
            if (workHistoryViewModel.id == null)
                return false;

            var workHistory = _workHistoryRepository.GetById((int)workHistoryViewModel.id);

            if (workHistory == null) return false;

            workHistory.ModifiedDate = DateTime.Now;
            workHistory.IPAddress = workHistoryViewModel.iPAddress;

            workHistory.Reference = workHistoryViewModel.reference;
            workHistory.Date = workHistoryViewModel.date;
            workHistory.WorkId = workHistoryViewModel.workId;
            workHistory.Description = workHistoryViewModel.description;
            workHistory.Observations = workHistoryViewModel.observations;
            workHistory.Type = workHistoryViewModel.type;
            workHistory.TypeFile = workHistoryViewModel.typeFile;
            workHistory.FileName = workHistoryViewModel.fileName;
            workHistory.File = workHistoryViewModel.file;

            return _workHistoryRepository.Update(workHistory);
        }

        public bool DeleteWorkHistory(int id)
        {
            return _workHistoryRepository.Delete(id);
        }
    }
}
