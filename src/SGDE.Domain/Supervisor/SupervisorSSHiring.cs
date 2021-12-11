﻿using SGDE.Domain.Converters;
using SGDE.Domain.Entities;
using SGDE.Domain.Helpers;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.Domain.Supervisor
{
    public partial class Supervisor
    {
        public QueryResult<SSHiringViewModel> GetAllSSHiring(int skip = 0, int take = 0, int userId = 0)
        {
            var queryResult = _sSHiringRepository.GetAll(skip, take, userId);
            return new QueryResult<SSHiringViewModel>
            {
                Data = SSHiringConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public SSHiringViewModel GetSSHiringById(int id)
        {
            var ssHiringViewModel = SSHiringConverter.Convert(_sSHiringRepository.GetById(id));

            return ssHiringViewModel;
        }

        public SSHiringViewModel AddSSHiring(SSHiringViewModel newSSHiringViewModel)
        {
            var sSHiring = new SSHiring
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newSSHiringViewModel.iPAddress,

                StartDate = newSSHiringViewModel.startDate.ToLocalTime(),
                EndDate = newSSHiringViewModel.endDate?.ToLocalTime(),
                Observations = newSSHiringViewModel.observations,
                UserId = newSSHiringViewModel.userId
            };

            _sSHiringRepository.Add(sSHiring);

            return GetSSHiringById(sSHiring.Id);
        }

        public bool UpdateSSHiring(SSHiringViewModel sSHiringViewModel)
        {
            if (sSHiringViewModel.id == null)
                return false;

            var sSHiring = _sSHiringRepository.GetById((int)sSHiringViewModel.id);

            if (sSHiring == null) return false;

            sSHiring.ModifiedDate = DateTime.Now;
            sSHiring.IPAddress = sSHiringViewModel.iPAddress;

            sSHiring.StartDate = sSHiringViewModel.startDate.ToLocalTime();
            sSHiring.EndDate = sSHiringViewModel.endDate?.ToLocalTime();
            sSHiring.Observations = sSHiringViewModel.observations;
            sSHiring.UserId = sSHiringViewModel.userId;

            return _sSHiringRepository.Update(sSHiring);
        }

        public bool DeleteSSHiring(int id)
        {
            return _sSHiringRepository.Delete(id);
        }
    }
}
