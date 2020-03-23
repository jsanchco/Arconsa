namespace SGDE.Domain.Repositories
{
    #region Using

    using SGDE.Domain.Entities;
    using System.Collections.Generic;

    #endregion

    public interface ISettingRepository
    {
        List<Setting> GetAll();
        Setting GetById(int id);
        Setting GetByName(string id);
        Setting Add(Setting newSetting);
        bool Update(Setting setting);
        bool Delete(int id);
    }
}
