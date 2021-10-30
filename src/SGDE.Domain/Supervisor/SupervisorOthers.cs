namespace SGDE.Domain.Supervisor
{
    #region Using

    using SGDE.Domain.Entities;

    #endregion

    public partial class Supervisor
    {
        public Client GetClient(int clientId)
        {
            return _clientRepository.GetById(clientId);
        }

        public Work GetWork(int workId)
        {
            return _workRepository.GetById(workId);
        }

        public User GetWorker(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public void Update()
        {
            //var costWorkers = _costWorkerRepository.GetAll().Data;
            //foreach (var costWorker in costWorkers)
            //{
            //    if (costWorker.ProfessionId != costWorker.User.ProfessionId)
            //    {
            //        if (costWorker.User.ProfessionId.HasValue)
            //        {
            //            costWorker.ProfessionId = costWorker.User.ProfessionId.Value;
            //            _costWorkerRepository.Update(costWorker);
            //        }
            //    }
            //}
            //var dailySignings = _dailySigningRepository.GetAll().Data;
            //var cont = 1;
            //foreach (var dailySigning in dailySignings)
            //{
            //    if (dailySigning.ProfessionId != dailySigning.UserHiring.User.Profession.Id)
            //    {
            //        dailySigning.ProfessionId = dailySigning.UserHiring.User.Profession.Id;
            //        _dailySigningRepository.Update(dailySigning);
            //    }
            //    System.Diagnostics.Debug.WriteLine($"[{cont}]dailySigning [{dailySigning.Id}] de {dailySignings.Count}");
            //    cont++;
            //}
        }
    }
}
