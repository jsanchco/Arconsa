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
            CheckAdd(newWorkBudgetViewModel);

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
                TypeFile = newWorkBudgetViewModel.typeFile,
                File = newWorkBudgetViewModel.file
            };

            UpdateFieldsInBudget(newWorkBudgetViewModel, workBudget);

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
            workBudget.Name = workBudgetViewModel.name;
            workBudget.TypeFile = workBudgetViewModel.typeFile;
            workBudget.File = workBudgetViewModel.file;

            return _workBudgetRepository.Update(workBudget);
        }

        public bool DeleteWorkBudget(int id)
        {
            CheckDelete(id);

            return _workBudgetRepository.Delete(id);
        }

        public List<WorkBudgetViewModel> GetAllWorkBudgetLite(int workId = 0)
        {
            return WorkBudgetConverter.ConvertListLite(_workBudgetRepository.GetAllLite(workId));
        }

        #region Auxiliary methods

        private void UpdateFieldsInBudget(WorkBudgetViewModel workBudgetViewModel, WorkBudget workBudget)
        {
            if (workBudgetViewModel.type == "Version X")
            {
                var workBudgets = GetAllWorkBudget(workBudgetViewModel.workId);
                workBudgets = workBudgets.Where(x => x.type == "Version X").ToList();

                workBudget.Name = $"{workBudgetViewModel.reference}_V{workBudgets.Count + 1}";
                workBudget.NameInWork = $"Version {workBudgets.Count + 1}";
            }

            if (workBudgetViewModel.type == "Definitivo Version")
            {
                var workBudgets = GetAllWorkBudget(workBudgetViewModel.workId);

                workBudget.Name = $"{workBudgets.Last().name}.D";
                workBudget.NameInWork = $"Presupuesto Definitivo";
            }

            if (workBudgetViewModel.type == "Complementario Version X")
            {
                var workBudgets = GetAllWorkBudget(workBudgetViewModel.workId).OrderBy(x => x.id);
                var lastBudget = workBudgets.Last();
                if (lastBudget.name.EndsWith(".D"))
                {
                    workBudget.Name = $"{workBudgets.Last().name}.D.C1V1";
                    workBudget.NameInWork = $"Complementario 1 Version 1";
                }
                else
                {

                }

            }
        }

        private void CheckAdd(WorkBudgetViewModel workBudget)
        {
            var workBudgets = GetAllWorkBudget(workBudget.workId);
            if (workBudgets?.Count > 0 &&
                workBudgets[workBudgets.Count - 1].date > workBudget.date)
            {
                throw new Exception("Hay presupuestos con fecha mayor");
            }

            if (workBudget.type == "Complementario Version X")
            {
                if (!workBudgets.Any(x => x.type == "Definitivo"))
                {
                    throw new Exception("No puede haber un Complementario sin tener un Presupuesto Definitivo");
                }
            }

            if (workBudget.type == "Complementario Definitivo")
            {
                if (!workBudgets.Any(x => x.type == "Complementario Version X"))
                {
                    throw new Exception("No puede haber un Complementario Definitivo sin tener una Version del Complementario");
                }
            }

            if (workBudget.type == "Definitivo Version")
            {
                if (workBudgets.Any(x => x.type == "Definitivo Version"))
                {
                    throw new Exception("No puede haber mas de un Presupuesto Definitivo");
                }
            }

            if (workBudget.type == "Version X")
            {
                if (workBudgets.Any(x => x.type == "Definitivo"))
                {
                    throw new Exception("No puede haber mas de una Version de un Presupuesto teniendo un Presupuesto Definitivo");
                }
            }

            return;            
        }

        private void CheckDelete(int id)
        {
            var workBudget = GetWorkBudgetById(id);
            var workBudgets = GetAllWorkBudget(workBudget.workId);

            if (workBudgets.Count() == 0)
                return;

            if (workBudget.id != workBudgets.Last().id)
            {
                throw new Exception("No se puede borrar este Presupuesto, debes primero eliminar el(los) últimos");
            }
        }

        #endregion
    }
}
