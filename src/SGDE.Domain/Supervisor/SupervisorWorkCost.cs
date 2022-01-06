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
        public List<WorkCostViewModel> GetAllWorkCost(int workId)
        {
            return WorkCostConverter.ConvertList(_workCostRepository.GetAll(workId));
        }

        public WorkCostViewModel GetWorkCostById(int id)
        {
            var workCostViewModel = WorkCostConverter.Convert(_workCostRepository.GetById(id));

            return workCostViewModel;
        }

        public WorkCostViewModel AddWorkCost(WorkCostViewModel newWorkCostViewModel)
        {
            var workCost = new WorkCost
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newWorkCostViewModel.iPAddress,

                //Date = newWorkCostViewModel.date.ToLocalTime(),
                Date = newWorkCostViewModel.date,
                Description = newWorkCostViewModel.description,
                File = newWorkCostViewModel.file,
                TypeFile = newWorkCostViewModel.typeFile,
                WorkId = newWorkCostViewModel.workId,
                NumberInvoice = newWorkCostViewModel.numberInvoice,
                TaxBase = newWorkCostViewModel.taxBase,
                TypeWorkCost = newWorkCostViewModel.typeWorkCost,
                Provider = newWorkCostViewModel.provider,

                FileName = GetFileName(newWorkCostViewModel)
            };

            _workCostRepository.Add(workCost);
            return newWorkCostViewModel;
        }

        public bool UpdateWorkCost(WorkCostViewModel workCostViewModel)
        {
            if (workCostViewModel.id == null)
                return false;

            var workCost = _workCostRepository.GetById((int)workCostViewModel.id);

            if (workCost == null) return false;

            workCost.ModifiedDate = DateTime.Now;
            workCost.IPAddress = workCostViewModel.iPAddress;

            workCost.Date = workCostViewModel.date;
            workCost.Description = workCostViewModel.description;
            workCost.File = workCostViewModel.file;
            workCost.TypeFile = workCostViewModel.typeFile;
            workCost.WorkId = workCostViewModel.workId;
            workCost.NumberInvoice = workCostViewModel.numberInvoice;
            workCost.TaxBase = workCostViewModel.taxBase;
            workCost.TypeWorkCost = workCostViewModel.typeWorkCost;
            workCost.Provider = workCostViewModel.provider;

            workCost.FileName = GetFileName(workCostViewModel, false);

            return _workCostRepository.Update(workCost);
        }

        public bool DeleteWorkCost(int id)
        {
            return _workCostRepository.Delete(id);
        }

        #region Auxiliary Methods

        private string GetFileName(WorkCostViewModel workCost, bool isAdd = true)
        {
            var workCosts = GetAllWorkCost(workCost.workId);
            workCosts = workCosts.Where(x => x.provider == workCost.provider).ToList();

            return isAdd ?
                $"{workCost.workId}_{workCost.provider}_{workCosts.Count + 1}" :
                $"{workCost.workId}_{workCost.provider}_{workCosts.Count}";
        }

        #endregion
    }
}
