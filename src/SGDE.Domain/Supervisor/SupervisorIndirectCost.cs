using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<IndirectCostViewModel> GetAllIndirectCost(int skip = 0, int take = 0, string filter = null)
        {
            var queryResult = _indirectCostRepository.GetAll(skip, take, filter);
            return new QueryResult<IndirectCostViewModel>
            {
                Data = IndirectCostConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public IndirectCostViewModel GetIndirectCostById(int id)
        {
            var indirectCostViewModel = IndirectCostConverter.Convert(_indirectCostRepository.GetById(id));

            return indirectCostViewModel;
        }

        public IndirectCostViewModel AddIndirectCost(IndirectCostViewModel newIndirectCostViewModel)
        {
            var indirectCost = new IndirectCost
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newIndirectCostViewModel.iPAddress,

                Date = new DateTime(newIndirectCostViewModel.year, newIndirectCostViewModel.month, 1),
                Description = newIndirectCostViewModel.description,
                AccountNumber = newIndirectCostViewModel.accountNumber,
                Amount = newIndirectCostViewModel.amount
            };

            _indirectCostRepository.Add(indirectCost);
            return newIndirectCostViewModel;
        }

        public bool UpdateIndirectCost(IndirectCostViewModel indirectCostViewModel)
        {
            if (indirectCostViewModel.id == null)
                return false;

            var indirectCost = _indirectCostRepository.GetById((int)indirectCostViewModel.id);

            if (indirectCost == null) return false;

            indirectCost.ModifiedDate = DateTime.Now;
            indirectCost.IPAddress = indirectCostViewModel.iPAddress;

            indirectCost.Date = new DateTime(indirectCostViewModel.year, indirectCostViewModel.month, 1);
            indirectCost.Description = indirectCostViewModel.description;
            indirectCost.AccountNumber = indirectCostViewModel.accountNumber;
            indirectCost.Amount = indirectCostViewModel.amount;

            return _indirectCostRepository.Update(indirectCost);
        }

        public bool DeleteIndirectCost(int id)
        {
            return _indirectCostRepository.Delete(id);
        }

        public bool AddIndirectCosts(IndirectCostCopyDataViewModel indirectCostCopyDataViewModel)
        {
            var date = new DateTime(
                indirectCostCopyDataViewModel.YearOld,
                indirectCostCopyDataViewModel.MonthOld,
                1);

            var indirectCosts = _indirectCostRepository.GetAllInDate(date);
            if (indirectCosts.Count == 0)
                throw new Exception("No existen costos para los datos seleccionados");

            var result = true;
            foreach (var indirectCost in indirectCosts)
            {
                var newIndirectCost = new IndirectCost
                {
                    Date = new DateTime(
                    indirectCostCopyDataViewModel.YearNew,
                    indirectCostCopyDataViewModel.MonthNew,
                    1),
                    AccountNumber = indirectCost.AccountNumber,
                    Amount = indirectCost.Amount,
                    Description = indirectCost.Description                    
                };
                var resultAdd = _indirectCostRepository.Add(newIndirectCost);
                if (resultAdd == null && result)
                    result = false;
            }

            return result;
        }
    }
}
