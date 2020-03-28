namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface IHourTypeRepository
    {
        List<HourType> GetAll();
        HourType GetById(int id);
        HourType Add(HourType newHourType);
        bool Update(HourType hourType);
        bool Delete(int id);
    }
}
