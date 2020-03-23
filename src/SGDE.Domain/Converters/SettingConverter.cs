namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class SettingConverter
    {
        public static SettingViewModel Convert(Setting setting)
        {
            if (setting == null)
                return null;

            var settingViewModel = new SettingViewModel
            {
                id = setting.Id,
                addedDate = setting.AddedDate,
                modifiedDate = setting.ModifiedDate,
                iPAddress = setting.IPAddress,

                name = setting.Name,
                data = setting.Data
            };

            return settingViewModel;
        }

        public static List<SettingViewModel> ConvertList(IEnumerable<Setting> settings)
        {
            return settings?.Select(setting =>
            {
                var model = new SettingViewModel
                {
                    id = setting.Id,
                    addedDate = setting.AddedDate,
                    modifiedDate = setting.ModifiedDate,
                    iPAddress = setting.IPAddress,

                    name = setting.Name,
                    data = setting.Data
                };
                return model;
            })
                .ToList();
        }
    }
}
