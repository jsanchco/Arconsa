namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
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

                StartHour = DateTime.Parse(newDailySigningViewModel.startHour),

                EndHour = string.IsNullOrEmpty(newDailySigningViewModel.endHour)
                ? null :
                (DateTime?)DateTime.Parse(newDailySigningViewModel.endHour),

                UserHiringId = newDailySigningViewModel.userHiringId
            };

            if (!_dailySigningRepository.ValidateDalilySigning(dailySigning))
                throw new Exception("El fichaje está mal configurado");

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

            dailySigning.StartHour = DateTime.Parse(dailySigningViewModel.startHour);

            dailySigning.EndHour = string.IsNullOrEmpty(dailySigningViewModel.endHour)
                ? null
                : (DateTime?)DateTime.Parse(dailySigningViewModel.endHour);

            dailySigning.UserHiringId = dailySigningViewModel.userHiringId;

            if (!_dailySigningRepository.ValidateDalilySigning(dailySigning))
                throw new Exception("El fichaje está mal configurado");

            return _dailySigningRepository.Update(dailySigning);
        }

        public bool DeleteDailySigning(int id)
        {
            return _dailySigningRepository.Delete(id);
        }
    }
}
