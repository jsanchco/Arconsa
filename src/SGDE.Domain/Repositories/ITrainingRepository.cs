namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface ITrainingRepository
    {
        List<Training> GetAll(int userId);
        Training GetById(int id);
        Training Add(Training newTraining);
        bool Update(Training training);
        bool Delete(int id);
    }
}
