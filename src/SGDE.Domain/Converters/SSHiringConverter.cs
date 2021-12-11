using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public class SSHiringConverter
    {
        public static SSHiringViewModel Convert(SSHiring sSHiring)
        {
            if (sSHiring == null)
                return null;

            var sSHiringViewModel = new SSHiringViewModel
            {
                id = sSHiring.Id,
                addedDate = sSHiring.AddedDate,
                modifiedDate = sSHiring.ModifiedDate,
                iPAddress = sSHiring.IPAddress,

                startDate = sSHiring.StartDate,
                endDate = sSHiring.EndDate,
                observations = sSHiring.Observations,
                userId = sSHiring.UserId
            };

            return sSHiringViewModel;
        }

        public static List<SSHiringViewModel> ConvertList(IEnumerable<SSHiring> sSHirings)
        {
            return sSHirings?.Select(sSHiring =>
            {
                var model = new SSHiringViewModel
                {
                    id = sSHiring.Id,
                    addedDate = sSHiring.AddedDate,
                    modifiedDate = sSHiring.ModifiedDate,
                    iPAddress = sSHiring.IPAddress,

                    startDate = sSHiring.StartDate,
                    endDate = sSHiring.EndDate,
                    observations = sSHiring.Observations,
                    userId = sSHiring.UserId
                };
                return model;
            })
                .ToList();
        }
    }
}
