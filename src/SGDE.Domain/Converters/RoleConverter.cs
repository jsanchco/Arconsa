namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public class RoleConverter
    {
        public static RoleViewModel Convert(Role role)
        {
            if (role == null)
                return null;

            var roleViewModel = new RoleViewModel
            {
                id = role.Id,
                addedDate = role.AddedDate,
                modifiedDate = role.ModifiedDate,
                iPAddress = role.IPAddress,

                name = role.Name
            };

            return roleViewModel;
        }

        public static List<RoleViewModel> ConvertList(IEnumerable<Role> roles)
        {
            return roles?.Select(role =>
            {
                var model = new RoleViewModel
                {
                    id = role.Id,
                    addedDate = role.AddedDate,
                    modifiedDate = role.ModifiedDate,
                    iPAddress = role.IPAddress,

                    name = role.Name
                };
                return model;
            })
                .ToList();
        }
    }
}
