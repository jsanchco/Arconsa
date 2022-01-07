using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public List<WorkBudgetViewModel> GetAllWorkBudget(int workId)
        {
            return WorkBudgetConverter.ConvertList(_workBudgetRepository.GetAll(workId));
        }

        public WorkBudgetViewModel GetWorkBudgetById(int id)
        {
            var workBudgetViewModel = WorkBudgetConverter.Convert(_workBudgetRepository.GetById(id));

            return workBudgetViewModel;
        }

        public WorkBudgetViewModel AddWorkBudget(WorkBudgetViewModel newWorkBudgetViewModel)
        {
            var workBudget = new WorkBudget
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newWorkBudgetViewModel.iPAddress,

                Date = newWorkBudgetViewModel.date,
                WorkId = newWorkBudgetViewModel.workId,
                Reference = newWorkBudgetViewModel.reference,
                TotalContract = newWorkBudgetViewModel.totalContract,
                Type = newWorkBudgetViewModel.type,

                Name = GetNameBudget(newWorkBudgetViewModel)
            };

            _workBudgetRepository.Add(workBudget);
            return newWorkBudgetViewModel;
        }

        public bool UpdateWorkBudget(WorkBudgetViewModel workBudgetViewModel)
        {
            if (workBudgetViewModel.id == null)
                return false;

            var workBudget = _workBudgetRepository.GetById((int)workBudgetViewModel.id);

            if (workBudget == null) return false;

            workBudget.ModifiedDate = DateTime.Now;
            workBudget.IPAddress = workBudgetViewModel.iPAddress;

            workBudget.Date = workBudgetViewModel.date;
            workBudget.WorkId = workBudgetViewModel.workId;
            workBudget.Reference = workBudgetViewModel.reference;
            workBudget.TotalContract = workBudgetViewModel.totalContract;
            workBudget.Type = workBudgetViewModel.type;

            workBudget.Name = GetNameBudget(workBudgetViewModel, false);

            return _workBudgetRepository.Update(workBudget);
        }

        public bool DeleteWorkBudget(int id)
        {
            return _workBudgetRepository.Delete(id);
        }

        #region Auxiliary methods

        private string GetNameBudget(WorkBudgetViewModel workBudget, bool isAdd = true)
        {
            if (workBudget.type == "Version X")
            {
                var workBudgets = GetAllWorkBudget(workBudget.workId);
                workBudgets = workBudgets.Where(x => x.type == "Version X").ToList();

                return isAdd ?
                    $"{workBudget.reference}_V{workBudgets.Count + 1}" :
                    $"{workBudget.reference}_V{workBudgets.Count}";
            }

            if (workBudget.type == "Definitivo")
            {
                return $"{workBudget.reference}_D";
            }

            if (workBudget.type == "Anexo X")
            {
                var workBudgets = GetAllWorkBudget(workBudget.workId);
                workBudgets = workBudgets.Where(x => x.type == "Anexo X").ToList();

                return isAdd ?
                    $"{workBudget.reference}_A{workBudgets.Count + 1}" :
                    $"{workBudget.reference}_A{workBudgets.Count}";
            }

            return null;
        }

        #endregion
    }
}
