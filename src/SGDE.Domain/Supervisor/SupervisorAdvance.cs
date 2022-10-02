using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<AdvanceViewModel> GetAllAdvance(int skip = 0, int take = 0, int userId = 0)
        {
            var queryResult = _advanceRepository.GetAll(skip, take, userId);
            return new QueryResult<AdvanceViewModel>
            {
                Data = AdvanceConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public AdvanceViewModel GetAdvanceById(int id)
        {
            var advanceViewModel = AdvanceConverter.Convert(_advanceRepository.GetById(id));

            return advanceViewModel;
        }

        public AdvanceViewModel AddAdvance(AdvanceViewModel newAdvanceViewModel)
        {
            var advance = new Advance
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newAdvanceViewModel.iPAddress,

                ConcessionDate = newAdvanceViewModel.concessionDate,
                Amount = newAdvanceViewModel.amount,
                Observations = newAdvanceViewModel.observations,
                PayDate = newAdvanceViewModel.payDate,
                UserId = newAdvanceViewModel.userId
            };

            _advanceRepository.Add(advance);
            return newAdvanceViewModel;
        }

        public bool UpdateAdvance(AdvanceViewModel advanceViewModel)
        {
            if (advanceViewModel.id == null)
                return false;

            var advance = _advanceRepository.GetById((int)advanceViewModel.id);

            if (advance == null) return false;

            advance.ModifiedDate = DateTime.Now;
            advance.IPAddress = advanceViewModel.iPAddress;

            advance.ConcessionDate = advanceViewModel.concessionDate;
            advance.Amount = advanceViewModel.amount;
            advance.Observations = advanceViewModel.observations;
            advance.PayDate = advanceViewModel.payDate;
            advance.UserId = advanceViewModel.userId;

            return _advanceRepository.Update(advance);
        }

        public bool DeleteAdvance(int id)
        {
            return _advanceRepository.Delete(id);
        }
    }
}
