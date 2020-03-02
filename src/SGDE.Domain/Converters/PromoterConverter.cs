namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class PromoterConverter
    {
        public static PromoterViewModel Convert(Promoter promoter)
        {
            if (promoter == null)
                return null;

            var promoterViewModel = new PromoterViewModel
            {
                id = promoter.Id,
                addedDate = promoter.AddedDate,
                modifiedDate = promoter.ModifiedDate,
                iPAddress = promoter.IPAddress,

                name = promoter.Name,
                cif = promoter.Cif,
                address = promoter.Address,
                phoneNumber = promoter.PhoneNumber
            };

            return promoterViewModel;
        }

        public static List<PromoterViewModel> ConvertList(IEnumerable<Promoter> promoters)
        {
            return promoters?.Select(promoter =>
            {
                var model = new PromoterViewModel
                {
                    id = promoter.Id,
                    addedDate = promoter.AddedDate,
                    modifiedDate = promoter.ModifiedDate,
                    iPAddress = promoter.IPAddress,

                    name = promoter.Name,
                    cif = promoter.Cif,
                    address = promoter.Address,
                    phoneNumber = promoter.PhoneNumber
                };
                return model;
            })
                .ToList();
        }
    }
}
