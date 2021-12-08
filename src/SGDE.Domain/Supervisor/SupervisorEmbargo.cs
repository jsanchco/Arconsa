using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<EmbargoViewModel> GetAllEmbargo(int skip = 0, int take = 0, int userId = 0)
        {
            var queryResult = _embargoRepository.GetAll(skip, take, userId);
            return new QueryResult<EmbargoViewModel>
            {
                Data = EmbargoConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public EmbargoViewModel GetEmbargoById(int id)
        {
            var embargoViewModel = EmbargoConverter.Convert(_embargoRepository.GetById(id));

            return embargoViewModel;
        }

        public EmbargoViewModel AddEmbargo(EmbargoViewModel newEmbargo)
        {
            var embargo = new Embargo
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newEmbargo.iPAddress,

                Identifier = newEmbargo.identifier,
                IssuingEntity = newEmbargo.issuingEntity,
                AccountNumber = newEmbargo.accountNumber,
                NotificationDate = newEmbargo.notificationDate?.ToLocalTime(),
                StartDate = newEmbargo.startDate?.ToLocalTime(),
                EndDate = newEmbargo.endDate?.ToLocalTime(),
                Observations = newEmbargo.observations,
                Total = newEmbargo.total,
                Paid = newEmbargo.paid,
                UserId = newEmbargo.userId
            };

            _embargoRepository.Add(embargo);

            return GetEmbargoById(embargo.Id);
        }

        public bool UpdateEmbargo(EmbargoViewModel embargoViewModel)
        {
            if (embargoViewModel.id == null)
                return false;

            var embargo = _embargoRepository.GetById((int)embargoViewModel.id);

            if (embargo == null) return false;

            embargo.ModifiedDate = DateTime.Now;
            embargo.IPAddress = embargoViewModel.iPAddress;

            embargo.Identifier = embargoViewModel.identifier;
            embargo.IssuingEntity = embargoViewModel.issuingEntity;
            embargo.AccountNumber = embargoViewModel.accountNumber;
            embargo.NotificationDate = embargoViewModel.notificationDate?.ToLocalTime();
            embargo.StartDate = embargoViewModel.startDate?.ToLocalTime();
            embargo.EndDate = embargoViewModel.endDate?.ToLocalTime();
            embargo.Observations = embargoViewModel.observations;
            embargo.Total = embargoViewModel.total;
            embargo.Paid = embargoViewModel.paid;
            embargo.UserId = embargoViewModel.userId;

            return _embargoRepository.Update(embargo);
        }

        public bool DeleteEmbargo(int id)
        {
            return _embargoRepository.Delete(id);
        }
    }
}
