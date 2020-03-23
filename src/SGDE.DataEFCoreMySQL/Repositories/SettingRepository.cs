namespace SGDE.DataEFCoreMySQL.Repositories
{
    #region Using

    using System.Collections.Generic;
    using Domain.Entities;
    using System.Linq;
    using SGDE.Domain.Repositories;

    #endregion

    public class SettingRepository : ISettingRepository
    {
        private readonly EFContextMySQL _context;

        public SettingRepository(EFContextMySQL context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private bool SettingExists(int id)
        {
            return GetById(id) != null;
        }

        public List<Setting> GetAll()
        {
            return _context.Setting
                .ToList();
        }

        public Setting GetById(int id)
        {
            return _context.Setting
                .FirstOrDefault(x => x.Id == id);
        }

        public Setting GetByName(string name)
        {
            return _context.Setting
                .FirstOrDefault(x => x.Name == name);
        }

        public Setting Add(Setting newSetting)
        {
            var findSetting = _context.Setting.FirstOrDefault(x => x.Name == newSetting.Name);
            if (findSetting == null)
            {
                _context.Setting.Add(newSetting);
            }
            else
            {
                findSetting.Data = newSetting.Data;
                _context.Setting.Update(findSetting);
            }

            _context.SaveChanges();
            return newSetting;
        }

        public bool Update(Setting setting)
        {
            if (!SettingExists(setting.Id))
                return false;

            _context.Setting.Update(setting);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (!SettingExists(id))
                return false;

            var toRemove = _context.Setting.Find(id);
            _context.Setting.Remove(toRemove);
            _context.SaveChanges();
            return true;

        }
    }
}
