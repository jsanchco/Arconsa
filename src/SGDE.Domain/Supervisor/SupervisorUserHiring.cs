﻿namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using Converters;
    using Entities;
    using ViewModels;
    using Domain.Helpers;
    using System.Collections.Generic;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<UserHiringViewModel> GetAllUserHiring(int skip = 0, int take = 0, string filter = null, int userId = 0, int workId = 0)
        {
            var queryResult = _userHiringRepository.GetAll(skip, take, filter, userId, workId);
            return new QueryResult<UserHiringViewModel>
            {
                Data = UserHiringConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public List<UserHiringViewModel> GetAllUserHiringByWorkId(int workId = 0, bool actualWorking = false)
        {
            return UserHiringConverter.ConvertList(_userHiringRepository.GetAllByWorkId(workId, actualWorking));
        }

        public UserHiringViewModel GetUserHiringById(int id)
        {
            var userHiringViewModel = UserHiringConverter.Convert(_userHiringRepository.GetById(id));

            return userHiringViewModel;
        }

        public UserHiringViewModel AddUserHiring(UserHiringViewModel newUserHiringViewModel)
        {
            var userHiring = new UserHiring
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newUserHiringViewModel.iPAddress,

                StartDate = DateTime.Parse(newUserHiringViewModel.startDate),
                EndDate = string.IsNullOrEmpty(newUserHiringViewModel.endDate)
                    ? null
                    : (DateTime?)DateTime.Parse(newUserHiringViewModel.endDate),

                WorkId = newUserHiringViewModel.workId,
                UserId = newUserHiringViewModel.userId,
                ProfessionId = newUserHiringViewModel.professionId,
                InWork = newUserHiringViewModel.inWork
            };

            _userHiringRepository.Add(userHiring);
            return newUserHiringViewModel;
        }

        public bool UpdateUserHiring(UserHiringViewModel userHiringViewModel)
        {
            if (userHiringViewModel.id == null)
                return false;

            var userHiring = _userHiringRepository.GetById((int)userHiringViewModel.id);

            if (userHiring == null) return false;

            userHiring.ModifiedDate = DateTime.Now;
            userHiring.IPAddress = userHiringViewModel.iPAddress;

            userHiring.StartDate = DateTime.ParseExact(userHiringViewModel.startDate, "dd/MM/yyyy", null);
            userHiring.EndDate = string.IsNullOrEmpty(userHiringViewModel.endDate)
                    ? null
                    : (DateTime?)DateTime.ParseExact(userHiringViewModel.endDate, "dd/MM/yyyy", null);

            userHiring.WorkId = userHiringViewModel.workId;
            userHiring.UserId = userHiringViewModel.userId;
            userHiring.ProfessionId = userHiringViewModel.professionId;
            userHiring.InWork = userHiringViewModel.inWork;

            return _userHiringRepository.Update(userHiring);
        }

        public bool DeleteUserHiring(int id)
        {
            return _userHiringRepository.Delete(id);
        }

        public bool IsProfessionInClient(int? professionId, int workId = 0, int clientId = 0)
        {
            return _userHiringRepository.IsProfessionInClient(professionId, workId, clientId);
        }

        public bool AssignWorkers(WorkersInWorkViewModel workersInWorkViewModel)
        {
            return _userHiringRepository.AssignWorkers(workersInWorkViewModel.listUserId, workersInWorkViewModel.workId);
        }
    }
}
