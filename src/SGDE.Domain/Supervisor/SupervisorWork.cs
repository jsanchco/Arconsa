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
        public List<WorkViewModel> GetAllWork()
        {
            return WorkConverter.ConvertList(_workRepository.GetAll());
        }

        public WorkViewModel GetWorkById(int id)
        {
            var workViewModel = WorkConverter.Convert(_workRepository.GetById(id));

            return workViewModel;
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
                OpenDate = newWorkViewModel.openDate == null ? DateTime.Now : (DateTime)newWorkViewModel.openDate,
                CloseDate = newWorkViewModel.closeDate,
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
                work.OpenDate = (DateTime)workViewModel.openDate;
                work.CloseDate = DateTime.Now;
            }

            work.ClientId = workViewModel.clientId;

            return _workRepository.Update(work);
        }

        public bool DeleteWork(int id)
        {
            return _workRepository.Delete(id);
        }
    }
}
