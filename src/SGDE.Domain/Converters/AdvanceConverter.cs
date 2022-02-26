using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public static class AdvanceConverter
    {
        public static AdvanceViewModel Convert(Advance advance)
        {
            if (advance == null)
                return null;

            var advanceViewModel = new AdvanceViewModel
            {
                id = advance.Id,
                addedDate = advance.AddedDate,
                modifiedDate = advance.ModifiedDate,
                iPAddress = advance.IPAddress,

                concessionDate = advance.ConcessionDate,
                amount = advance.Amount,
                payDate = advance.PayDate,
                userId = advance.UserId,
                userName = $"{ advance.User.Name} { advance.User.Surname}"
            };

            return advanceViewModel;
        }

        public static List<AdvanceViewModel> ConvertList(IEnumerable<Advance> advances)
        {
            return advances?.Select(advance =>
            {
                var model = new AdvanceViewModel
                {
                    id = advance.Id,
                    addedDate = advance.AddedDate,
                    modifiedDate = advance.ModifiedDate,
                    iPAddress = advance.IPAddress,

                    concessionDate = advance.ConcessionDate,
                    amount = advance.Amount,
                    payDate = advance.PayDate,
                    userId = advance.UserId,
                    userName = $"{ advance.User.Name} { advance.User.Surname}"
                };
                return model;
            })
                .ToList();
        }
    }
}
