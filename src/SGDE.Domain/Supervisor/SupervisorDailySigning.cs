namespace SGDE.Domain.Supervisor
{
    #region Using

    using Converters;
    using Entities;
    using SGDE.Domain.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

                StartHour = newDailySigningViewModel.startHour?.ToLocalTime(),
                EndHour = newDailySigningViewModel.endHour?.ToLocalTime(),

                UserHiringId = newDailySigningViewModel.userHiringId,
                ProfessionId = newDailySigningViewModel.professionId,
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

            dailySigning.StartHour = dailySigningViewModel.startHour?.ToLocalTime();
            dailySigning.EndHour = dailySigningViewModel.endHour?.ToLocalTime();

            dailySigning.UserHiringId = dailySigningViewModel.userHiringId;
            dailySigning.ProfessionId = dailySigningViewModel.professionId;
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
            foreach (var transform in massiveSigningQueryViewModel.data)
            {
                transform.startHour = transform.startHour?.ToLocalTime();
                transform.endHour = transform.endHour?.ToLocalTime();
            }

            massiveSigningQueryViewModel.data = massiveSigningQueryViewModel.data.OrderBy(x => x.startHour).ToList();
            if (!ValidateDataMassiveSigning(massiveSigningQueryViewModel.data))
                throw new Exception("Algunos periodos no están bien configurados");

            var result = true;
            foreach (var item in massiveSigningQueryViewModel.data)
            {
                DailySigning dailySigning;
                if (item.hourTypeId == 5)
                {
                    dailySigning = new DailySigning
                    {
                        AddedDate = DateTime.Now,
                        ModifiedDate = null,
                        IPAddress = null,

                        StartHour = item.startHour,
                        EndHour = item.endHour,
                        UserHiringId = massiveSigningQueryViewModel.userHiringId,
                        ProfessionId = massiveSigningQueryViewModel.professionId,
                        HourTypeId = item.hourTypeId
                    };
                }
                else
                {
                    var hourStart = item.startHour.Value.Hour;
                    var minuteStart = item.startHour.Value.Minute;
                    var hourEnd = item.endHour.Value.Hour;
                    var minuteEnd = item.endHour.Value.Minute;

                    dailySigning = new DailySigning
                    {
                        AddedDate = DateTime.Now,
                        ModifiedDate = null,
                        IPAddress = null,

                        StartHour = new DateTime(item.startHour.Value.Year, item.startHour.Value.Month, item.startHour.Value.Day, hourStart, minuteStart, 0),
                        EndHour = new DateTime(item.endHour.Value.Year, item.endHour.Value.Month, item.endHour.Value.Day, hourEnd, minuteEnd, 0),

                        UserHiringId = massiveSigningQueryViewModel.userHiringId,
                        ProfessionId = massiveSigningQueryViewModel.professionId,
                        HourTypeId = item.hourTypeId
                    };
                }

                if (_dailySigningRepository.ValidateDalilySigning(dailySigning))
                {
                    _dailySigningRepository.Add(dailySigning);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public (List<DailySigningViewModel> dailiesSigningsViewModel, bool fails) ViewMassiveSigning(MassiveSigningQueryViewModel massiveSigningQueryViewModel)
        {
            var result = new List<DailySigning>();
            var fail = false;

            var actualDay = DateTime.ParseExact($"{massiveSigningQueryViewModel.startSigning}", "dd/MM/yyyy", null);
            var endDay = DateTime.ParseExact($"{massiveSigningQueryViewModel.endSigning}", "dd/MM/yyyy", null);
            while (actualDay <= endDay)
            {
                if ((actualDay.DayOfWeek == DayOfWeek.Saturday) && (!massiveSigningQueryViewModel.includeSaturdays))
                {
                    actualDay = actualDay.AddDays(1);
                    continue;
                }

                if ((actualDay.DayOfWeek == DayOfWeek.Sunday) && (!massiveSigningQueryViewModel.includeSundays))
                {
                    actualDay = actualDay.AddDays(1);
                    continue;
                }

                foreach (var item in massiveSigningQueryViewModel.data)
                {
                    DailySigning dailySigning;
                    if (item.hourTypeId == 5)
                    {
                        dailySigning = new DailySigning
                        {
                            AddedDate = DateTime.Now,
                            ModifiedDate = null,
                            IPAddress = null,

                            StartHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day),
                            EndHour = null,
                            UserHiringId = massiveSigningQueryViewModel.userHiringId,
                            ProfessionId = massiveSigningQueryViewModel.professionId,
                            HourTypeId = item.hourTypeId
                        };
                    }
                    else
                    {
                        var hourStart = item.startHour.Value.Hour;
                        var minuteStart = item.startHour.Value.Minute;
                        var hourEnd = item.endHour.Value.Hour;
                        var minuteEnd = item.endHour.Value.Minute;

                        dailySigning = new DailySigning
                        {
                            AddedDate = DateTime.Now,
                            ModifiedDate = null,
                            IPAddress = null,

                            StartHour = new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, hourStart, minuteStart, 0),
                            EndHour = hourStart <= hourEnd ?
                                new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, hourEnd, minuteEnd, 0) :
                                new DateTime(actualDay.Year, actualDay.Month, actualDay.Day, hourEnd, minuteEnd, 0).AddDays(1),

                            UserHiringId = massiveSigningQueryViewModel.userHiringId,
                            ProfessionId = massiveSigningQueryViewModel.professionId,
                            HourTypeId = item.hourTypeId
                        };
                    }

                    if (_dailySigningRepository.ValidateDalilySigning(dailySigning))
                    {
                        result.Add(dailySigning);
                    }
                    else
                    {
                        fail = true;
                    }
                }
                actualDay = actualDay.AddDays(1);
            }

            return (DailySigningConverter.ConvertList(result), fail);
        }

        public bool ValidateDataMassiveSigning(List<PeriodByHoursViewModel> data)
        {
            for (var i=0; i<data.Count; i++)
            {
                var item = data[i];
                for (var j=0; j<data.Count; j++)
                {
                    var itemCompare = data[j];
                    if (item == itemCompare)
                        continue;

                    if (!item.endHour.HasValue)
                        item.endHour = new DateTime(item.startHour.Value.Year, item.startHour.Value.Month, item.startHour.Value.Day, 23, 59, 0);
                    if (!itemCompare.endHour.HasValue)
                        itemCompare.endHour = new DateTime(itemCompare.startHour.Value.Year, itemCompare.startHour.Value.Month, itemCompare.startHour.Value.Day, 23, 59, 0);

                    if (itemCompare.startHour >= item.startHour && itemCompare.startHour <= item.endHour ||
                        itemCompare.endHour <= item.startHour && itemCompare.endHour >= item.endHour)
                        return false;
                }
            }

            return true;
        }
    }
}
