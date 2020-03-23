namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Converters;
    using Entities;
    using ViewModels;

    #endregion

    public partial class Supervisor
    {
        public List<SettingViewModel> GetAllSetting()
        {
            return SettingConverter.ConvertList(_settingRepository.GetAll());
        }

        public SettingViewModel GetSettingById(int id)
        {
            var settingViewModel = SettingConverter.Convert(_settingRepository.GetById(id));

            return settingViewModel;
        }

        public SettingViewModel GetSettingByName(string name)
        {
            var settingViewModel = SettingConverter.Convert(_settingRepository.GetByName(name));

            return settingViewModel;
        }

        public SettingViewModel AddSetting(SettingViewModel newSettingViewModel)
        {
            var setting = new Setting
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newSettingViewModel.iPAddress,

                Name = newSettingViewModel.name,
                Data = newSettingViewModel.data
            };

            _settingRepository.Add(setting);
            return newSettingViewModel;
        }

        public bool UpdateSetting(SettingViewModel settingViewModel)
        {
            if (settingViewModel.id == null)
                return false;

            var setting = _settingRepository.GetById((int)settingViewModel.id);

            if (setting == null) return false;

            setting.ModifiedDate = DateTime.Now;
            setting.IPAddress = settingViewModel.iPAddress;

            setting.Name = settingViewModel.name;
            setting.Data = settingViewModel.data;

            return _settingRepository.Update(setting);
        }

        public bool DeleteSetting(int id)
        {
            return _settingRepository.Delete(id);
        }
    }
 }
