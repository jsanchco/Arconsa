namespace SGDE.Domain.Supervisor
{
    º#region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;
    using Domain.Helpers;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<WorkViewModel> GetAllWork(int skip = 0, int take = 0, string filter = null, int clientId = 0)
        {
            return WorkConverter.ConvertList(_workRepository.GetAll(skip, take, filter, clientId));
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
                foreach(var userHiring in userHirings)
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
                NumberPersonsRequested = newWorkViewModel.numberPersonsRequested,
                Open = newWorkViewModel.open,

                OpenDate = newWorkViewModel.openDate == null ? DateTime.Now : DateTime.Parse(newWorkViewModel.openDate),
                CloseDate = string.IsNullOrEmpty(newWorkViewModel.closeDate)
                    ? null :
                    (DateTime?)DateTime.Parse(newWorkViewModel.closeDate),

                ClientId = newWorkViewModel.clientId
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
            work.NumberPersonsRequested = workViewModel.numberPersonsRequested;
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

        private void AddUserToList(List<UserViewModel> listUserViewModel, UserViewModel userViewModel)
        {
            if (!listUserViewModel.Contains(userViewModel))
                listUserViewModel.Add(userViewModel);
        }
    }
}
