namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class HourTypeConverter
    {
        public static HourTypeViewModel Convert(HourType hourType)
        {
            if (hourType == null)
                return null;

            var hourTypeViewModel = new HourTypeViewModel
            {
                id = hourType.Id,
                addedDate = hourType.AddedDate,
                modifiedDate = hourType.ModifiedDate,
                iPAddress = hourType.IPAddress,

                name = hourType.Name
            };

            return hourTypeViewModel;
        }

        public static List<HourTypeViewModel> ConvertList(IEnumerable<HourType> hourTypes)
        {
            return hourTypes?.Select(hourType =>
            {
                var model = new HourTypeViewModel
                {
                    id = hourType.Id,
                    addedDate = hourType.AddedDate,
                    modifiedDate = hourType.ModifiedDate,
                    iPAddress = hourType.IPAddress,

                    name = hourType.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
