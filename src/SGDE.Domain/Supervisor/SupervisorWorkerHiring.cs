namespace SGDE.Domain.Supervisor
{
    #region Using

    using Domain.Helpers;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public WorkerHiringViewModel GetWorkerHiring(WorkerHiringViewModel workerHiringViewModel)
        {
            workerHiringViewModel.professionName = GetProfessionById(workerHiringViewModel.professionId).name;

            return workerHiringViewModel;
        }

        public QueryResult<WorkerHiringViewModel> GetAllWorkerHiring(int skip = 0, int take = 0, int enterpriseId = 0, string filter = null, int workId = 0)
        {
            #region Old code
            //var workersInUsers = _userRepository.GetUsersByRole(new List<int> { 3 });
            //if (!string.IsNullOrEmpty(filter))
            //{
            //    workersInUsers = workersInUsers
            //        .Where(x =>
            //            Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Dni?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Email?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Observations?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.PhoneNumber?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Surname?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Username?.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Role.Name.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(string.Join(',', x.UserProfessions?.Select(y => y.Profession.Name.ToLower()))).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Work?.Name.ToLower()).Contains(filter) ||
            //            Searcher.RemoveAccentsWithNormalization(x.Client?.Name.ToLower()).Contains(filter))
            //        .ToList();
            //}

            //var listWorkerHiringViewModel = new List<WorkerHiringViewModel>();
            //foreach (var worker in workersInUsers)
            //{
            //    var state = worker.WorkId == workId ? 0 : worker.WorkId == null ? 1 : 2;
            //    var workerHiringViewModel = new WorkerHiringViewModel
            //    {
            //        id = worker.Id,
            //        name = $"{worker.Name} {worker.Surname}",
            //        dni = worker.Dni,
            //        //professionName = worker.UserProfessions.FirstOrDefault()?.Profession.Name,
            //        workId = worker.WorkId,
            //        workName = worker.Work?.Name,
            //        state = state,
            //        dateStart = state != 1 ? _userHiringRepository.GetByWorkAndStartDateNull(workId)?.StartDate.ToString("MM/dd/yyyy") : null
            //        //dateStart = state != 1 ? GetAllUserHiring(0, workId).Data?.Find(x => x.endDate == null)?.startDate : null
            //    };
            //    if (state == 0)
            //    {
            //        //var userHiring = GetAllUserHiring(worker.Id, 0).Data?.Find(x => x.endDate == null);
            //        var userHiring = _userHiringRepository.GetByWorkerAndEndDateNull(worker.Id);
            //        if (userHiring?.ProfessionId != null)
            //        {
            //            workerHiringViewModel.professionId = (int)userHiring.ProfessionId;
            //            workerHiringViewModel.professionName = userHiring.Profession.Name;
            //        }
            //        else
            //        {
            //            if (worker.UserProfessions != null)
            //            {
            //                workerHiringViewModel.professionId = worker.UserProfessions.FirstOrDefault().ProfessionId;
            //                workerHiringViewModel.professionName = worker.UserProfessions.FirstOrDefault().Profession.Name;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (worker.UserProfessions != null)
            //        {
            //            workerHiringViewModel.professionId = worker.UserProfessions.FirstOrDefault().ProfessionId;
            //            workerHiringViewModel.professionName = worker.UserProfessions.FirstOrDefault().Profession.Name;
            //        }
            //    }

            //    listWorkerHiringViewModel.Add(workerHiringViewModel);
            //}

            //listWorkerHiringViewModel = listWorkerHiringViewModel.OrderBy(x => x.state).ToList();
            //var count = listWorkerHiringViewModel.Count;
            //if (take == 0)
            //    take = count;
            //return new QueryResult<WorkerHiringViewModel>
            //{
            //    Data = listWorkerHiringViewModel.Skip(skip).Take(take).ToList(),
            //    Count = count
            //};
            #endregion

            var workersWithSS = _userRepository.GetWorkersWithSS(enterpriseId);
            if (!string.IsNullOrEmpty(filter))
            {
                workersWithSS = workersWithSS
                    .Where(x =>
                        Searcher.RemoveAccentsWithNormalization(x.Address?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Dni?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Email?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Name.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Observations?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.PhoneNumber?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Surname?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Username?.ToLower()).Contains(filter) ||
                        Searcher.RemoveAccentsWithNormalization(x.Work?.Name.ToLower()).Contains(filter))
                    .ToList();
            }

            var listWorkerHiringViewModel = new List<WorkerHiringViewModel>();
            foreach (var worker in workersWithSS)
            {
                var workerHiringViewModel = new WorkerHiringViewModel
                {
                    id = worker.Id,
                    name = $"{worker.Name} {worker.Surname}",
                    dni = worker.Dni,
                    workId = worker.WorkId,
                    workName = worker.Work?.Name,
                    state = worker.UserHirings.Any(x => x.WorkId == workId) ? 0 : 1
                };

                listWorkerHiringViewModel.Add(workerHiringViewModel);
            }

            listWorkerHiringViewModel = listWorkerHiringViewModel.OrderByDescending(x => x.state).ToList();
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
