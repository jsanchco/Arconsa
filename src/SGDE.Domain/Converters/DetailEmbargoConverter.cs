using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public class DetailEmbargoConverter
    {
        public static DetailEmbargoViewModel Convert(DetailEmbargo detailEmbargo)
        {
            if (detailEmbargo == null)
                return null;

            var detailEmbargoViewModel = new DetailEmbargoViewModel
            {
                id = detailEmbargo.Id,
                addedDate = detailEmbargo.AddedDate,
                modifiedDate = detailEmbargo.ModifiedDate,
                iPAddress = detailEmbargo.IPAddress,

                datePay = detailEmbargo.DatePay,
                amount = detailEmbargo.Amount,
                observations = detailEmbargo.Observations,
                embargoId = detailEmbargo.EmbargoId
            };

            return detailEmbargoViewModel;
        }

        public static List<DetailEmbargoViewModel> ConvertList(IEnumerable<DetailEmbargo> detailEmbargos)
        {
            return detailEmbargos?.Select(detailEmbargo =>
            {
                var model = new DetailEmbargoViewModel
                {
                    id = detailEmbargo.Id,
                    addedDate = detailEmbargo.AddedDate,
                    modifiedDate = detailEmbargo.ModifiedDate,
                    iPAddress = detailEmbargo.IPAddress,

                    datePay = detailEmbargo.DatePay,
                    amount = detailEmbargo.Amount,
                    observations = detailEmbargo.Observations,
                    embargoId = detailEmbargo.EmbargoId
                };
                return model;
            })
                .ToList();
        }
    }
}
