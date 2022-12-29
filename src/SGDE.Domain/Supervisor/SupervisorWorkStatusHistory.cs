using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public List<WorkStatusHistoryViewModel> GetAllWorkStatusHistory(int workStatusHistoryId)
        {
            return WorkStatusHistoryConverter.ConvertList(_workStatusHistoryRepository.GetAll(workStatusHistoryId));
        }

        public List<WorkStatusHistoryViewModel> GetAllWorkStatusHistoryBetweenDates(DateTime startDate, DateTime endDate)
        {
            return WorkStatusHistoryConverter.ConvertList(_workStatusHistoryRepository.GetAllBetweenDates(startDate, endDate));
        }

        public WorkStatusHistoryViewModel GetWorkStatusHistoryById(int id)
        {
            var workStatusHistoryViewModel = WorkStatusHistoryConverter.Convert(_workStatusHistoryRepository.GetById(id));

            return workStatusHistoryViewModel;
        }

        public WorkStatusHistoryViewModel AddWorkStatusHistory(WorkStatusHistoryViewModel newWorkStatusHistoryViewModel)
        {
            var workStatusHistory = new WorkStatusHistory
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newWorkStatusHistoryViewModel.iPAddress,

                Value = newWorkStatusHistoryViewModel.value,
                Observations = newWorkStatusHistoryViewModel.observations,
                DateChange = newWorkStatusHistoryViewModel.dateChange,
                WorkId = newWorkStatusHistoryViewModel.workId
            };

            _workStatusHistoryRepository.Add(workStatusHistory);
            return newWorkStatusHistoryViewModel;
        }

        public bool UpdateWorkStatusHistory(WorkStatusHistoryViewModel workStatusHistoryViewModel)
        {
            if (workStatusHistoryViewModel.id == null)
                return false;

            var workStatusHistory = _workStatusHistoryRepository.GetById((int)workStatusHistoryViewModel.id);

            if (workStatusHistory == null) return false;

            workStatusHistory.ModifiedDate = DateTime.Now;
            workStatusHistory.IPAddress = workStatusHistoryViewModel.iPAddress;

            workStatusHistory.DateChange = workStatusHistoryViewModel.dateChange;
            workStatusHistory.Value = workStatusHistoryViewModel.value;
            workStatusHistory.Observations = workStatusHistoryViewModel.observations;
            workStatusHistory.WorkId = workStatusHistoryViewModel.workId;

            return _workStatusHistoryRepository.Update(workStatusHistory);
        }

        public bool DeleteWorkStatusHistory(int id)
        {
            return _workStatusHistoryRepository.Delete(id);
        }
    }
}
