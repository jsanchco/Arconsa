namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Converters;
    using Entities;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public List<UserHiringViewModel> GetAllUserHiring(int userId = 0, int workId = 0)
        {
            return UserHiringConverter.ConvertList(_userHiringRepository.GetAll(userId, workId));
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
                ProfessionId = newUserHiringViewModel.professionId
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

            return _userHiringRepository.Update(userHiring);
        }

        public bool DeleteUserHiring(int id)
        {
            return _userHiringRepository.Delete(id);
        }

        public bool AssignWorkers(WorkersInWorkViewModel workersInWorkViewModel)
        {
            return _userHiringRepository.AssignWorkers(workersInWorkViewModel.listUserId, workersInWorkViewModel.workId);
        }
    }
}
