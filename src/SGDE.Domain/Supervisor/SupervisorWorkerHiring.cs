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
    using Domain.Helpers;
    using System.Linq;

    #endregion

    public partial class Supervisor
    {
        public QueryResult<WorkerHiringViewModel> GetAllWorkerHiring(int skip = 0, int take = 0, string filter = null, int workId = 0)
        {
            var workersInUsers = _userRepository.GetUsersByRole(new List<int> { 3 });
            if (!string.IsNullOrEmpty(filter))
            {
                workersInUsers = workersInUsers
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Dni?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Email?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Observations?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.PhoneNumber?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Surname.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Username?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Role.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(string.Join(',', x.UserProfessions?.Select(y => y.Profession.Name.ToLower()))).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Work?.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Client?.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            var listWorkerHiringViewModel = new List<WorkerHiringViewModel>();
            foreach (var worker in workersInUsers)
            {
                var state = worker.WorkId == workId ? 0 : worker.WorkId == null ? 1 : 2;
                var workerHiringViewModel = new WorkerHiringViewModel
                {
                    id = worker.Id,
                    name = $"{worker.Name} {worker.Surname}",
                    dni = worker.Dni,
                    //professionName = worker.UserProfessions.FirstOrDefault()?.Profession.Name,
                    workId = worker.WorkId,
                    workName = worker.Work?.Name,
                    state = state,
                    dateStart = state != 1 ? _userHiringRepository.GetByWorkAndStartDateNull(workId)?.StartDate.ToString("MM/dd/yyyy") : null
                    //dateStart = state != 1 ? GetAllUserHiring(0, workId).Data?.Find(x => x.endDate == null)?.startDate : null
                };
                if (state == 0)
                {
                    //var userHiring = GetAllUserHiring(worker.Id, 0).Data?.Find(x => x.endDate == null);
                    var userHiring = _userHiringRepository.GetByWorkerAndEndDateNull(worker.Id);
                    if (userHiring?.ProfessionId != null)
                    {
                        workerHiringViewModel.professionId = (int)userHiring.ProfessionId;
                        workerHiringViewModel.professionName = userHiring.Profession.Name;
                    }
                    else
                    {
                        if (worker.UserProfessions != null)
                        {
                            workerHiringViewModel.professionId = worker.UserProfessions.FirstOrDefault().ProfessionId;
                            workerHiringViewModel.professionName = worker.UserProfessions.FirstOrDefault().Profession.Name;
                        }
                    }                   
                }
                else
                {
                    if (worker.UserProfessions != null)
                    {
                        workerHiringViewModel.professionId = worker.UserProfessions.FirstOrDefault().ProfessionId;
                        workerHiringViewModel.professionName = worker.UserProfessions.FirstOrDefault().Profession.Name;
                    }
                }

                listWorkerHiringViewModel.Add(workerHiringViewModel);
            }

            listWorkerHiringViewModel = listWorkerHiringViewModel.OrderBy(x => x.state).ToList();
            var count = listWorkerHiringViewModel.Count;
            if (take == 0)
                take = count;
            return new QueryResult<WorkerHiringViewModel>
            {
                Data = listWorkerHiringViewModel.Skip(skip).Take(take).ToList(),
                Count = count
            };
        }
    }
}
