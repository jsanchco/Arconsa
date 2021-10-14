namespace SGDE.Domain.Converters
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

                startDate = userHiring.StartDate.ToString("MM/dd/yyyy"),
                endDate = userHiring.EndDate?.ToString("MM/dd/yyyy"),
                inWork = userHiring.InWork,
                userId = userHiring.UserId,
                userName = $"{userHiring.User.Name} {userHiring.User.Surname}",
                clientName = userHiring.Work.Client.Name,
                workId = userHiring.WorkId,
                workName = userHiring.Work.Name,
                professionId = userHiring.ProfessionId,
                professionName = userHiring.Profession?.Name
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

                    startDate = userHiring.StartDate.ToString("MM/dd/yyyy"),
                    endDate = userHiring.EndDate?.ToString("MM/dd/yyyy"),
                    inWork = userHiring.InWork,
                    userId = userHiring.UserId,
                    userName = $"{userHiring.User.Name} {userHiring.User.Surname}",
                    clientName = userHiring.Work.Client.Name,
                    workId = userHiring.WorkId,
                    workName = userHiring.Work.Name,
                    professionId = userHiring.ProfessionId,
                    professionName = userHiring.Profession?.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
