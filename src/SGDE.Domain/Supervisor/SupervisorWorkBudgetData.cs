using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public List<WorkBudgetDataViewModel> GetAllWorkBudgetData(int workId)
        {
            return WorkBudgetDataConverter.ConvertList(_workBudgetDataRepository.GetAll(workId));
        }

        public WorkBudgetDataViewModel GetWorkBudgetDataById(int id)
        {
            var workBudgetDataViewModel = WorkBudgetDataConverter.Convert(_workBudgetDataRepository.GetById(id));

            return workBudgetDataViewModel;
        }

        public WorkBudgetDataViewModel AddWorkBudgetData(WorkBudgetDataViewModel newWorkBudgetDataViewModel)
        {
            var workBudgetData = new WorkBudgetData
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newWorkBudgetDataViewModel.iPAddress,

                Description = newWorkBudgetDataViewModel.description,
                WorkId = newWorkBudgetDataViewModel.workId,
                Reference = newWorkBudgetDataViewModel.reference,
            };

            _workBudgetDataRepository.Add(workBudgetData);
            return newWorkBudgetDataViewModel;
        }

        public bool UpdateWorkBudgetData(WorkBudgetDataViewModel workBudgetDataViewModel)
        {
            if (workBudgetDataViewModel.id == null)
                return false;

            var workBudgetData = _workBudgetDataRepository.GetById((int)workBudgetDataViewModel.id);

            if (workBudgetData == null) return false;

            workBudgetData.ModifiedDate = DateTime.Now;
            workBudgetData.IPAddress = workBudgetDataViewModel.iPAddress;

            workBudgetData.Description = workBudgetDataViewModel.description;
            workBudgetData.WorkId = workBudgetDataViewModel.workId;
            workBudgetData.Reference = workBudgetDataViewModel.reference;

            return _workBudgetDataRepository.Update(workBudgetData);
        }

        public bool DeleteWorkBudgetData(int id)
        {      
            return _workBudgetDataRepository.Delete(id);
        }
    }
}
