namespace SGDE.Domain.Supervisor
{
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
                    Data = null,
                    Count = 0
                };
            }

            var historyHiringViewModel = new HistoryHiringViewModel
            {
                userHiringId = dailySignings[0].UserHiring.Id,
                dtStartDate = (DateTime)dailySignings[0].StartHour,
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
                        dtStartDate = (DateTime)dailySigning.StartHour,
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
                    historyHiringViewModel.dtEndDate = dailySigning.EndHour;
                }
            }
            if (historyHiringViewModel.inWork == true)
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
                dtStartDate = (DateTime)dailySignings[0].StartHour,
                userId = dailySignings[0].UserHiring.UserId,
                userName = $"{dailySignings[0].UserHiring.User.Name} {dailySignings[0].UserHiring.User.Surname}",
                clientId = dailySignings[0].UserHiring.Work.Client.Id,
                clientName = dailySignings[0].UserHiring.Work.Client.Name,
                professionId = dailySignings[0].UserHiring.Profession.Id,
                professionName = dailySignings[0].UserHiring.Profession.Name,
                inWork = dailySignings[0].UserHiring.InWork
            };

            foreach (var dailySigning in dailySignings)
            {
                if (dailySigning.UserHiring.UserId != historyHiringViewModel.userId)
                {
                    listHistoryHiringViewModel.Add(historyHiringViewModel);

                    historyHiringViewModel = new HistoryHiringViewModel
                    {
                        userHiringId = dailySigning.UserHiring.Id,
                        dtStartDate = (DateTime)dailySigning.StartHour,
                        userId = dailySigning.UserHiring.UserId,
                        userName = $"{dailySigning.UserHiring.User.Name} {dailySigning.UserHiring.User.Surname}",
                        clientId = dailySigning.UserHiring.Work.Client.Id,
                        clientName = dailySigning.UserHiring.Work.Client.Name,
                        professionId = dailySigning.UserHiring.Profession.Id,
                        professionName = dailySigning.UserHiring.Profession.Name,
                        inWork = dailySigning.UserHiring.InWork
                    };
                }
                else
                {
                    historyHiringViewModel.dtEndDate = dailySigning.EndHour;
                }
            }
            if (historyHiringViewModel.inWork == true)
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

            userHiring.InWork = historyHiringViewModel.inWork;
            if (historyHiringViewModel.inWork == true)
            {
                var usersHirings = _userHiringRepository.GetByUserAndInWork(userHiring.UserId, historyHiringViewModel.inWork);
                foreach (var hiring in usersHirings)
                {
                    if (hiring.Id == historyHiringViewModel.userHiringId)
                        continue;

                    if (hiring.InWork == false)
                        continue;

                    hiring.InWork = false;
                    _userHiringRepository.Update(hiring);
                }
            }

            return _userHiringRepository.Update(userHiring);
        }
    }
}
