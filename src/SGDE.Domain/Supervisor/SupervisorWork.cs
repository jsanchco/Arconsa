namespace SGDE.Domain.Supervisor
{
    #region Using

    using Converters;
    using Domain.Helpers;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<WorkViewModel> GetAllWork(int skip = 0, int take = 0, string filter = null, int clientId = 0, bool showCloseWorks = true)
        {
            var queryResult = _workRepository.GetAll(skip, take, filter, clientId, showCloseWorks);
            return new QueryResult<WorkViewModel>
            {
                Data = WorkConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public WorkViewModel GetWorkById(int id)
        {
            var workViewModel = WorkConverter.Convert(_workRepository.GetById(id));

            return workViewModel;
        }

        // 0 = all, 1 = asset, 2 = no asset
        public List<UserViewModel> GetUsersByWork(int workId, int state = 0)
        {
            var listUserViewModel = new List<UserViewModel>();
            var userHirings = _workRepository.GetById(workId).UserHirings;

            if (state == 0)
            {
                foreach (var userHiring in userHirings)
                {
                    AddUserToList(listUserViewModel, UserConverter.Convert(userHiring.User));
                }
            }

            if (state == 1)
            {
                foreach (var userHiring in userHirings)
                {
                    if (userHiring.EndDate == null)
                        AddUserToList(listUserViewModel, UserConverter.Convert(userHiring.User));
                }
            }

            if (state == 2)
            {
                foreach (var userHiring in userHirings)
                {
                    if (userHiring.EndDate != null)
                        AddUserToList(listUserViewModel, UserConverter.Convert(userHiring.User));
                }
            }

            return listUserViewModel;
        }

        public WorkViewModel AddWork(WorkViewModel newWorkViewModel)
        {
            var work = new Work
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newWorkViewModel.iPAddress,

                Name = newWorkViewModel.name,
                Address = newWorkViewModel.address,
                EstimatedDuration = newWorkViewModel.estimatedDuration,
                WorksToRealize = newWorkViewModel.worksToRealize,
                Open = newWorkViewModel.open,
                InvoiceToOrigin = newWorkViewModel.invoiceToOrigin,
                PercentageRetention = newWorkViewModel.percentageRetention,
                PercentageIVA = newWorkViewModel.percentageIVA,

                OpenDate = newWorkViewModel.openDate == null ? DateTime.Now : DateTime.Parse(newWorkViewModel.openDate),
                CloseDate = string.IsNullOrEmpty(newWorkViewModel.closeDate)
                    ? null :
                    (DateTime?)DateTime.Parse(newWorkViewModel.closeDate),

                ClientId = newWorkViewModel.clientId,
                PassiveSubject = newWorkViewModel.passiveSubject
            };

            _workRepository.Add(work);
            return newWorkViewModel;
        }

        public bool UpdateWork(WorkViewModel workViewModel)
        {
            if (workViewModel.id == null)
                return false;

            var work = _workRepository.GetById((int)workViewModel.id);

            if (work == null) return false;

            work.ModifiedDate = DateTime.Now;
            work.IPAddress = workViewModel.iPAddress;

            work.Name = workViewModel.name;
            work.Address = workViewModel.address;
            work.EstimatedDuration = workViewModel.estimatedDuration;
            work.WorksToRealize = workViewModel.worksToRealize;
            work.InvoiceToOrigin = workViewModel.invoiceToOrigin;
            work.PercentageRetention = workViewModel.percentageRetention;
            work.PercentageIVA = workViewModel.percentageIVA;

            work.OpenDate = DateTime.ParseExact(workViewModel.openDate, "dd/MM/yyyy", null);
            work.CloseDate = string.IsNullOrEmpty(workViewModel.closeDate)
                    ? null
                    : (DateTime?)DateTime.ParseExact(workViewModel.closeDate, "dd/MM/yyyy", null);
            work.Open = string.IsNullOrEmpty(workViewModel.closeDate);

            if (work.OpenDate > work.CloseDate)
            {
                throw new Exception("Fechas inconsistentes");
            }

            work.ClientId = workViewModel.clientId;
            work.PassiveSubject = workViewModel.passiveSubject;

            return _workRepository.Update(work);
        }

        public bool UpdateDatesWork(WorkViewModel workViewModel)
        {
            if (workViewModel.id == null)
                return false;

            var work = _workRepository.GetById((int)workViewModel.id);

            if (work == null) return false;

            work.ModifiedDate = DateTime.Now;
            work.IPAddress = workViewModel.iPAddress;

            work.Name = workViewModel.name;
            work.Address = workViewModel.address;
            work.EstimatedDuration = workViewModel.estimatedDuration;
            work.WorksToRealize = workViewModel.worksToRealize;
            work.Open = workViewModel.open;

            if (workViewModel.open)
            {
                work.OpenDate = DateTime.Now;
                work.CloseDate = null;
            }
            else
            {
                work.OpenDate = DateTime.Parse(workViewModel.openDate);
                work.CloseDate = DateTime.Now;
            }

            work.ClientId = workViewModel.clientId;

            return _workRepository.Update(work);
        }

        public bool DeleteWork(int id)
        {
            return _workRepository.Delete(id);
        }

        public List<WorkViewModel> GetAllWorkLite(string filter = null, int clientId = 0)
        {
            return WorkConverter.ConvertListLite(_workRepository.GetAllLite(filter, clientId));
        }

        public WorkClosePageViewModel GetWorkClosePage(int workId)
        {
            var work = _workRepository.GetById(workId);
            if (work == null)
                throw new Exception($"Work [{workId}] NOT found");

            var authorizeCancelWorkers = GetHistoryByWorkId(workId);

            var result = new WorkClosePageViewModel
            {
                workId = work.Id,
                workName = work.Name,
                workAddress = work.Address,
                clientId = work.Client.Id,
                clientName = work.Client.Name,
                openDate = work.OpenDate.ToString("dd/MM/yyyy"),
                closeDate = work.CloseDate?.ToString("dd/MM/yyyy"),
                workBudgetsName = String.Join(", ", work.WorkBudgets
                    .Where(x => x.Type == "Definitivo" || x.Type == "Modificado")
                    .Select(x => x.Name)),
                workBudgetsSumFormat = String.Join(" + ", work.WorkBudgets
                    .Where(x => x.Type == "Definitivo" || x.Type == "Modificado")
                    .Select(x=> x.TotalContract)) + " = " + work.WorkBudgets
                    .Where(x => x.Type == "Definitivo" || x.Type == "Modificado")
                    .Sum(x => x.TotalContract) + "€",
                workBudgetsSum = work.WorkBudgets
                    .Where(x => x.Type == "Definitivo" || x.Type == "Modificado")
                    .Sum(x => x.TotalContract),
                invoicesSum = Math.Round(work.Invoices.Sum(x => x.TaxBase), 2),
                workCostsSum = Math.Round(work.WorkCosts.Sum(x => x.TaxBase), 2),
                authorizeCancelWorkersCostsSum = Math.Round(authorizeCancelWorkers.Data.Sum(x => x.priceTotal), 2),
                authorizeCancelWorkersCostsSalesSum = Math.Round(authorizeCancelWorkers.Data.Sum(x => x.priceTotalSale), 2),
                indirectCostsSum = Math.Round(CalculateIndirectCostsByWork(workId), 2)
            };

            return result;
        }

        #region Auxiliary Methods

        private void AddUserToList(List<UserViewModel> listUserViewModel, UserViewModel userViewModel)
        {
            if (!listUserViewModel.Contains(userViewModel))
                listUserViewModel.Add(userViewModel);
        }

        #endregion
    }
}
