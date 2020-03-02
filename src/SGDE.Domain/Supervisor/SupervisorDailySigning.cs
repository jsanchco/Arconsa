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
        public List<DailySigningViewModel> GetAllDailySigning()
        {
            return DailySigningConverter.ConvertList(_dailySigningRepository.GetAll());
        }

        public DailySigningViewModel GetDailySigningById(int id)
        {
            var dailySigningViewModel = DailySigningConverter.Convert(_dailySigningRepository.GetById(id));

            return dailySigningViewModel;
        }

        public DailySigningViewModel AddDailySigning(DailySigningViewModel newDailySigningViewModel)
        {
            var dailySigning = new DailySigning
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newDailySigningViewModel.iPAddress,

                StartHour = newDailySigningViewModel.startHour,
                EndHour = newDailySigningViewModel.endHour,
                WorkId = newDailySigningViewModel.workId,
                UserId = newDailySigningViewModel.userId
            };

            _dailySigningRepository.Add(dailySigning);
            return newDailySigningViewModel;
        }

        public bool UpdateDailySigning(DailySigningViewModel dailySigningViewModel)
        {
            if (dailySigningViewModel.id == null)
                return false;

            var dailySigning = _dailySigningRepository.GetById((int)dailySigningViewModel.id);

            if (dailySigning == null) return false;

            dailySigning.ModifiedDate = DateTime.Now;
            dailySigning.IPAddress = dailySigningViewModel.iPAddress;

            dailySigning.StartHour = dailySigningViewModel.startHour;
            dailySigning.EndHour = dailySigningViewModel.endHour;
            dailySigning.WorkId = dailySigningViewModel.workId;
            dailySigning.UserId = dailySigningViewModel.userId;

            return _dailySigningRepository.Update(dailySigning);
        }

        public bool DeleteDailySigning(int id)
        {
            return _dailySigningRepository.Delete(id);
        }
    }
}
