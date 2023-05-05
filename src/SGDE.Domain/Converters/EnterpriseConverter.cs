using SGDE.Domain.Entities;
using SGDE.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.Domain.Converters
{
    public static class EnterpriseConverter
    {
        public static EnterpriseViewModel Convert(Enterprise enterprise)
        {
            if (enterprise == null)
                return null;

            var enterpriseViewModel = new EnterpriseViewModel
            {
                id = enterprise.Id,
                addedDate = enterprise.AddedDate,
                modifiedDate = enterprise.ModifiedDate,
                iPAddress = enterprise.IPAddress,

                name = enterprise.Name,
                alias = enterprise.Alias,
                cif = enterprise.CIF,
                address = enterprise.Address,
                phoneNumber = enterprise.PhoneNumber
            };

            return enterpriseViewModel;
        }

        public static List<EnterpriseViewModel> ConvertList(IEnumerable<Enterprise> enterprises)
        {
            return enterprises?.Select(enterprise =>
            {
                var model = new EnterpriseViewModel
                {
                    id = enterprise.Id,
                    addedDate = enterprise.AddedDate,
                    modifiedDate = enterprise.ModifiedDate,
                    iPAddress = enterprise.IPAddress,

                    name = enterprise.Name,
                    alias = enterprise.Alias,
                    cif = enterprise.CIF,
                    address = enterprise.Address,
                    phoneNumber = enterprise.PhoneNumber
                };
                return model;
            })
                .ToList();
        }
    }
}
