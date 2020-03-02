namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Converters;
    using Entities;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public List<UserHiringViewModel> GetAllUserHiring()
        {
            return UserHiringConverter.ConvertList(_userHiringRepository.GetAll());
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

                StartDate = newUserHiringViewModel.startDate,
                EndDate = newUserHiringViewModel.endDate,
                BuilderId = newUserHiringViewModel.builderId,
                UserId = newUserHiringViewModel.userId
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

            userHiring.StartDate = userHiringViewModel.startDate;
            userHiring.EndDate = userHiringViewModel.endDate;
            userHiring.BuilderId = userHiringViewModel.builderId;
            userHiring.UserId = userHiringViewModel.userId;

            return _userHiringRepository.Update(userHiring);
        }

        public bool DeleteUserHiring(int id)
        {
            return _userHiringRepository.Delete(id);
        }
    }
}
