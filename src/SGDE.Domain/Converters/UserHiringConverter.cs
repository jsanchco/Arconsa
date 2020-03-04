﻿namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class UserHiringConverter
    {
        public static UserHiringViewModel Convert(UserHiring userHiring)
        {
            if (userHiring == null)
                return null;

            var userHiringViewModel = new UserHiringViewModel
            {
                id = userHiring.Id,
                addedDate = userHiring.AddedDate,
                modifiedDate = userHiring.ModifiedDate,
                iPAddress = userHiring.IPAddress,

                startDate = userHiring.StartDate,
                endDate = userHiring.EndDate,
                userId = userHiring.UserId,
                userName = $"{userHiring.User.Name} {userHiring.User.Surname}",
                workId = userHiring.WorkId,
                workName = userHiring.Work.Name
            };

            return userHiringViewModel;
        }

        public static List<UserHiringViewModel> ConvertList(IEnumerable<UserHiring> userHirings)
        {
            return userHirings?.Select(userHiring =>
            {
                var model = new UserHiringViewModel
                {
                    id = userHiring.Id,
                    addedDate = userHiring.AddedDate,
                    modifiedDate = userHiring.ModifiedDate,
                    iPAddress = userHiring.IPAddress,

                    startDate = userHiring.StartDate,
                    endDate = userHiring.EndDate,
                    userId = userHiring.UserId,
                    userName = $"{userHiring.User.Name} {userHiring.User.Surname}",
                    workId = userHiring.WorkId,
                    workName = userHiring.Work.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
