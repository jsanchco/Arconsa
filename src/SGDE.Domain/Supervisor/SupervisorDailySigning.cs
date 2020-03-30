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
    using SGDE.Domain.Helpers;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<DailySigningViewModel> GetAllDailySigning(int skip = 0, int take = 0, int userId = 0)
        {
            var queryResult = _dailySigningRepository.GetAll(skip, take, userId);
            return new QueryResult<DailySigningViewModel>
            {
                Data = DailySigningConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
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

                StartHour = DateTime.ParseExact(newDailySigningViewModel.startHour, "dd/MM/yyyy HH:mm", null),

                EndHour = string.IsNullOrEmpty(newDailySigningViewModel.endHour)
                ? null :
                (DateTime?)DateTime.ParseExact(newDailySigningViewModel.endHour, "dd/MM/yyyy HH:mm", null),

                UserHiringId = newDailySigningViewModel.userHiringId,
                HourTypeId = newDailySigningViewModel.hourTypeId
            };

            if (!_dailySigningRepository.ValidateDalilySigning(dailySigning))
                throw new Exception("El fichaje está mal configurado");

            _dailySigningRepository.Add(dailySigning);

            return GetDailySigningById(dailySigning.Id);
        }

        public bool UpdateDailySigning(DailySigningViewModel dailySigningViewModel)
        {
            if (dailySigningViewModel.id == null)
                return false;

            var dailySigning = _dailySigningRepository.GetById((int)dailySigningViewModel.id);

            if (dailySigning == null) return false;

            dailySigning.ModifiedDate = DateTime.Now;
            dailySigning.IPAddress = dailySigningViewModel.iPAddress;

            dailySigning.StartHour = DateTime.ParseExact(dailySigningViewModel.startHour, "dd/MM/yyyy HH:mm", null);

            dailySigning.EndHour = string.IsNullOrEmpty(dailySigningViewModel.endHour)
                ? null
                : (DateTime?)DateTime.ParseExact(dailySigningViewModel.endHour, "dd/MM/yyyy HH:mm", null);

            dailySigning.UserHiringId = dailySigningViewModel.userHiringId;
            dailySigning.HourTypeId = dailySigningViewModel.hourTypeId;

            if (!_dailySigningRepository.ValidateDalilySigning(dailySigning))
                throw new Exception("El fichaje está mal configurado");

            return _dailySigningRepository.Update(dailySigning);
        }

        public bool DeleteDailySigning(int id)
        {
            return _dailySigningRepository.Delete(id);
        }

        public bool MassiveSigning(MassiveSigningQueryViewModel massiveSigningQueryViewModel)
        {
            var result = true;

            var actualDay = DateTime.ParseExact($"{massiveSigningQueryViewModel.startSigning}", "dd/MM/yyyy", null);
            var endDay = DateTime.ParseExact($"{massiveSigningQueryViewModel.endSigning}", "dd/MM/yyyy", null);
            while (actualDay <= endDay)
            {
                if ((actualDay.DayOfWeek == DayOfWeek.Saturday) || (actualDay.DayOfWeek == DayOfWeek.Sunday))
                {
                    actualDay = actualDay.AddDays(1);
                    continue;
                }

                var dailySigning = new DailySigning
                {
                    AddedDate = DateTime.Now,
                    ModifiedDate = null,
                    IPAddress = null,

                    StartHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, 8, 0, 0),
                    EndHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, 14, 0, 0),

                    UserHiringId = massiveSigningQueryViewModel.userHiringId,
                    HourTypeId = 1
                };
                if (_dailySigningRepository.ValidateDalilySigning(dailySigning))
                {
                    _dailySigningRepository.Add(dailySigning);
                }
                else
                {
                    result = false;
                }

                dailySigning = new DailySigning
                {
                    AddedDate = DateTime.Now,
                    ModifiedDate = null,
                    IPAddress = null,

                    StartHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, 15, 0, 0),
                    EndHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, 18, 0, 0),

                    UserHiringId = massiveSigningQueryViewModel.userHiringId,
                    HourTypeId = 1
                };
                if (_dailySigningRepository.ValidateDalilySigning(dailySigning))
                {
                    _dailySigningRepository.Add(dailySigning);
                }
                else
                {
                    result = false;
                }

                actualDay = actualDay.AddDays(1);
            }

            return result;
        }
    }
}
