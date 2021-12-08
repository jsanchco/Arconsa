using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public class EmbargoConverter
    {
        public static EmbargoViewModel Convert(Embargo embargo)
        {
            if (embargo == null)
                return null;

            var embargoViewModel = new EmbargoViewModel
            {                
                id = embargo.Id,
                addedDate = embargo.AddedDate,
                modifiedDate = embargo.ModifiedDate,
                iPAddress = embargo.IPAddress,

                identifier = embargo.Identifier,
                issuingEntity = embargo.IssuingEntity,
                accountNumber = embargo.AccountNumber,
                notificationDate = embargo.NotificationDate,
                startDate = embargo.StartDate,
                endDate = embargo.EndDate,
                observations = embargo.Observations,
                total = embargo.Total,
                remaining = embargo.Total - embargo.DetailEmbargos.Sum(x => x.Amount),
                paid = embargo.Paid,
                userId = embargo.UserId
            };

            return embargoViewModel;
        }

        public static List<EmbargoViewModel> ConvertList(IEnumerable<Embargo> embargos)
        {
            return embargos?.Select(embargo =>
            {
                var model = new EmbargoViewModel
                {
                    id = embargo.Id,
                    addedDate = embargo.AddedDate,
                    modifiedDate = embargo.ModifiedDate,
                    iPAddress = embargo.IPAddress,

                    identifier = embargo.Identifier,
                    issuingEntity = embargo.IssuingEntity,
                    accountNumber = embargo.AccountNumber,
                    notificationDate = embargo.NotificationDate,
                    startDate = embargo.StartDate,
                    endDate = embargo.EndDate,                    
                    total = embargo.Total,
                    remaining = embargo.Total - embargo.DetailEmbargos.Sum(x => x.Amount),
                    observations = embargo.Observations,
                    paid = embargo.Paid,
                    userId = embargo.UserId
                };
                return model;
            })
                .ToList();
        }
    }
}
