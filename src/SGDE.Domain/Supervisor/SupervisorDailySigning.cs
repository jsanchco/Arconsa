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
            if (!ValidateDataMassiveSigning(massiveSigningQueryViewModel.data))
                throw new Exception("Algunos periodos no están bien configurados");

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

                foreach (var item in massiveSigningQueryViewModel.data)
                {
                    var hourStart = Convert.ToInt32(item.startHour.Substring(0, 2));
                    var minuteStart = Convert.ToInt32(item.startHour.Substring(3, 2));
                    var hourEnd = Convert.ToInt32(item.endHour.Substring(0, 2));
                    var minuteEnd = Convert.ToInt32(item.endHour.Substring(3, 2));

                    var dailySigning = new DailySigning
                    {
                        AddedDate = DateTime.Now,
                        ModifiedDate = null,
                        IPAddress = null,

                        StartHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, hourStart, minuteStart, 0),
                        EndHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, hourEnd, minuteEnd, 0),

                        UserHiringId = massiveSigningQueryViewModel.userHiringId,
                        HourTypeId = item.hourTypeId
                    };

                    if (_dailySigningRepository.ValidateDalilySigning(dailySigning))
                    {
                        _dailySigningRepository.Add(dailySigning);
                    }
                    else
                    {
                        result = false;
                    }
                }
                actualDay = actualDay.AddDays(1);
            }

            return result;
        }

        public bool ValidateDataMassiveSigning(List<PeriodByHoursViewModel> data)
        {
            foreach(var item in data)
            {
                foreach (var itemCompare in data)
                {
                    if (item == itemCompare)
                        continue;

                    var numItemStart = Convert.ToInt32(item.startHour.Replace(":", ""));
                    var numItemEnd = Convert.ToInt32(item.endHour.Replace(":", ""));
                    var numItemCompareStart = Convert.ToInt32(itemCompare.startHour.Replace(":", ""));
                    var numItemCompareEnd = Convert.ToInt32(itemCompare.endHour.Replace(":", ""));

                    if (numItemCompareStart > numItemStart && numItemCompareStart < numItemEnd)
                        return false;
                    if (numItemCompareEnd > numItemStart && numItemCompareEnd < numItemEnd)
                        return false;
                }
            }

            return true;
        }
    }
}
