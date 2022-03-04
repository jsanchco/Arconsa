namespace SGDE.Domain.Supervisor
{
    using SGDE.Domain.Converters;
    using SGDE.Domain.Entities;
    #region Using

    using SGDE.Domain.Helpers;
    using SGDE.Domain.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<HistoryHiringViewModel> GetHistoryByUserId(int userId, int skip = 0, int take = 0)
        {
            var listHistoryHiringViewModel = new List<HistoryHiringViewModel>();
            var dailySignings = _dailySigningRepository.GetHistoryByUserId(userId);

            if (dailySignings == null || dailySignings.Count == 0)
            {
                return new QueryResult<HistoryHiringViewModel>
                {
                    Data = new List<HistoryHiringViewModel>(),
                    Count = 0
                };
            }

            var historyHiringViewModel = new HistoryHiringViewModel
            {
                userHiringId = dailySignings[0].UserHiring.Id,
                userId = dailySignings[0].UserHiring.UserId,
                dtStartDate = (DateTime)dailySignings[0].StartHour,
                dtEndDate = dailySignings[0].EndHour,
                workId = dailySignings[0].UserHiring.WorkId,
                workName = dailySignings[0].UserHiring.Work.Name,
                clientId = dailySignings[0].UserHiring.Work.Client.Id,
                clientName = dailySignings[0].UserHiring.Work.Client.Name,
                professionId = dailySignings[0].UserHiring.Profession.Id,
                professionName = dailySignings[0].UserHiring.Profession.Name,
                inWork = dailySignings[0].UserHiring.InWork
            };

            foreach (var dailySigning in dailySignings)
            {
                if (dailySigning.UserHiring.WorkId != historyHiringViewModel.workId)
                {
                    listHistoryHiringViewModel.Add(historyHiringViewModel);

                    historyHiringViewModel = new HistoryHiringViewModel
                    {
                        userHiringId = dailySigning.UserHiring.Id,
                        userId = dailySigning.UserHiring.UserId,
                        dtStartDate = (DateTime)dailySigning.StartHour,
                        dtEndDate = dailySigning.EndHour,
                        workId = dailySigning.UserHiring.WorkId,
                        workName = dailySigning.UserHiring.Work.Name,
                        clientId = dailySigning.UserHiring.Work.Client.Id,
                        clientName = dailySigning.UserHiring.Work.Client.Name,
                        professionId = dailySigning.UserHiring.Profession.Id,
                        professionName = dailySigning.UserHiring.Profession.Name,
                        inWork = dailySigning.UserHiring.InWork
                    };
                }
                else
                {
                    historyHiringViewModel.dtEndDate = dailySigning.HourTypeId != 5 ?
                        historyHiringViewModel.dtEndDate = dailySigning.EndHour :
                        historyHiringViewModel.dtEndDate = dailySigning.StartHour;
                }
            }
            if (historyHiringViewModel.inWork == true && !historyHiringViewModel.dtEndDate.HasValue)
                historyHiringViewModel.dtEndDate = null;

            listHistoryHiringViewModel.Add(historyHiringViewModel);

            listHistoryHiringViewModel = listHistoryHiringViewModel.OrderByDescending(x => x.dtStartDate).ToList();
            var count = listHistoryHiringViewModel.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<HistoryHiringViewModel>
                {
                    Data = listHistoryHiringViewModel.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<HistoryHiringViewModel>
                {
                    Data = listHistoryHiringViewModel.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public QueryResult<HistoryHiringViewModel> GetHistoryByWorkId(int workId, int skip = 0, int take = 0)
        {
            var listHistoryHiringViewModel = new List<HistoryHiringViewModel>();
            var dailySignings = _dailySigningRepository.GetHistoryByWorkId(workId);

            if (dailySignings == null || dailySignings.Count == 0)
            {
                return new QueryResult<HistoryHiringViewModel>
                {
                    Data = null,
                    Count = 0
                };
            }

            var historyHiringViewModel = new HistoryHiringViewModel
            {
                userHiringId = dailySignings[0].UserHiring.Id,
                userId = dailySignings[0].UserHiring.UserId,
                dtStartDate = (DateTime)dailySignings[0].StartHour,
                dtEndDate = dailySignings[0].EndHour,
                userName = $"{dailySignings[0].UserHiring.User.Name} {dailySignings[0].UserHiring.User.Surname}",
                clientId = dailySignings[0].UserHiring.Work.Client.Id,
                clientName = dailySignings[0].UserHiring.Work.Client.Name,
                professionId = dailySignings[0].UserHiring.Profession.Id,
                professionName = dailySignings[0].UserHiring.Profession.Name,
                workId = dailySignings[0].UserHiring.Work.Id,
                workName = dailySignings[0].UserHiring.Work.Name,
                inWork = !dailySignings[0].UserHiring.EndDate.HasValue,

                //priceTotal = dailySignings[0].HourTypeId != 5 ?
                //        ((DateTime)dailySignings[0].EndHour - (DateTime)dailySignings[0].StartHour).TotalHours * (double)ReportResultConverter.GetPriceHourCost(dailySignings[0].UserHiring.User, dailySignings[0].ProfessionId, (DateTime)dailySignings[0].StartHour, dailySignings[0].HourTypeId) :
                //        (double)ReportResultConverter.GetPriceHourCost(dailySignings[0].UserHiring.User, dailySignings[0].ProfessionId, (DateTime)dailySignings[0].StartHour, dailySignings[0].HourTypeId),
                //priceTotalSale = dailySignings[0].HourTypeId != 5 ?
                //        ((DateTime)dailySignings[0].EndHour - (DateTime)dailySignings[0].StartHour).TotalHours * (double)ReportResultConverter.GetPriceHourSale(dailySignings[0].UserHiring.Work.Client, dailySignings[0].HourTypeId, dailySignings[0].ProfessionId) :
                //        (double)ReportResultConverter.GetPriceHourSale(dailySignings[0].UserHiring.Work.Client, dailySignings[0].HourTypeId, dailySignings[0].ProfessionId)
            };

            //PrintDailySignings(dailySignings);

            foreach (var dailySigning in dailySignings)
            {
                if (dailySigning.UserHiring.UserId != historyHiringViewModel.userId)
                {
                    listHistoryHiringViewModel.Add(historyHiringViewModel);

                    historyHiringViewModel = new HistoryHiringViewModel
                    {
                        userHiringId = dailySigning.UserHiring.Id,
                        dtStartDate = (DateTime)dailySigning.StartHour,
                        dtEndDate = dailySigning.EndHour,
                        userId = dailySigning.UserHiring.UserId,
                        userName = $"{dailySigning.UserHiring.User.Name} {dailySigning.UserHiring.User.Surname}",
                        clientId = dailySigning.UserHiring.Work.Client.Id,
                        clientName = dailySigning.UserHiring.Work.Client.Name,

                        professionId = dailySigning.UserHiring.Profession == null ?
                            0 :
                            dailySigning.UserHiring.Profession.Id,

                        professionName = dailySigning.UserHiring.Profession?.Name,
                        workId = dailySigning.UserHiring.Work.Id,
                        workName = dailySigning.UserHiring.Work.Name,
                        inWork = !dailySigning.UserHiring.EndDate.HasValue,

                        priceTotal = dailySigning.HourTypeId != 5 ?
                                ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * (double)ReportResultConverter.GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId) :
                                (double)ReportResultConverter.GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId),
                        priceTotalSale = dailySigning.HourTypeId != 5 ?
                                ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * (double)ReportResultConverter.GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.ProfessionId) :
                                (double)ReportResultConverter.GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.ProfessionId)
                    };
                }
                else
                {
                    historyHiringViewModel.dtEndDate = dailySigning.HourTypeId != 5 ?
                        historyHiringViewModel.dtEndDate = dailySigning.EndHour :
                        historyHiringViewModel.dtEndDate = dailySigning.StartHour;

                    historyHiringViewModel.priceTotal += dailySigning.HourTypeId != 5 ?
                        ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * (double)ReportResultConverter.GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId) :
                        (double)ReportResultConverter.GetPriceHourCost(dailySigning.UserHiring.User, dailySigning.ProfessionId, (DateTime)dailySigning.StartHour, dailySigning.HourTypeId);

                    historyHiringViewModel.priceTotalSale += dailySigning.HourTypeId != 5 ?
                        ((DateTime)dailySigning.EndHour - (DateTime)dailySigning.StartHour).TotalHours * (double)ReportResultConverter.GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.ProfessionId) :
                        (double)ReportResultConverter.GetPriceHourSale(dailySigning.UserHiring.Work.Client, dailySigning.HourTypeId, dailySigning.ProfessionId);
                }
            }
            if (historyHiringViewModel.inWork == true && !historyHiringViewModel.dtEndDate.HasValue)
                historyHiringViewModel.dtEndDate = null;

            listHistoryHiringViewModel.Add(historyHiringViewModel);

            listHistoryHiringViewModel = listHistoryHiringViewModel.OrderByDescending(x => x.dtStartDate).ToList();
            var count = listHistoryHiringViewModel.Count;
            return (skip != 0 || take != 0)
                ? new QueryResult<HistoryHiringViewModel>
                {
                    Data = listHistoryHiringViewModel.Skip(skip).Take(take).ToList(),
                    Count = count
                }
                : new QueryResult<HistoryHiringViewModel>
                {
                    Data = listHistoryHiringViewModel.Skip(0).Take(count).ToList(),
                    Count = count
                };
        }

        public bool UpdateHistoryInWork(HistoryHiringViewModel historyHiringViewModel)
        {
            var userHiring = _userHiringRepository.GetById(historyHiringViewModel.userHiringId);
            if (userHiring == null) return false;

            userHiring.ProfessionId = historyHiringViewModel.professionId;
            userHiring.StartDate = historyHiringViewModel.dtStartDate;
            userHiring.EndDate = historyHiringViewModel.dtEndDate;
            userHiring.InWork = !historyHiringViewModel.dtEndDate.HasValue;

            return _userHiringRepository.Update(userHiring);
        }

        private void PrintDailySignings(List<DailySigning> dailySignings)
        {
            foreach (var dailySigning in dailySignings)
            {
                if (dailySigning.EndHour.HasValue)
                    System.Diagnostics.Debug.WriteLine($"{dailySigning.UserHiring.User.Name};{dailySigning.UserHiring.Id};{dailySigning.StartHour.Value:dd/MM/yyyy HHH:mm};{dailySigning.EndHour.Value:dd/MM/yyyy HHH:mm};");
                else
                    System.Diagnostics.Debug.WriteLine($"{dailySigning.UserHiring.User.Name};{dailySigning.UserHiring.Id};{dailySigning.StartHour.Value:dd/MM/yyyy HHH:mm};NULL");
            }
        }
    }
}
