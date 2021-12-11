namespace SGDE.Domain.Converters
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using ViewModels;

    #endregion

    public static class UserConverter
    {
        public static UserViewModel Convert(User user)
        {
            if (user == null)
                return null;

            var userViewModel = new UserViewModel
            {
                id = user.Id,
                addedDate = user.AddedDate,                                           
                modifiedDate = user.ModifiedDate,
                iPAddress = user.IPAddress,
                name = user.Name,
                surname = user.Surname,
                username = user.Username,
                dni = user.Dni,
                securitySocialNumber = user.SecuritySocialNumber,
                birthDate = user.BirthDate?.ToString("MM/dd/yyyy"),
                email = user.Email,
                address = user.Address,
                phoneNumber = user.PhoneNumber,
                observations = user.Observations,
                accountNumber = user.AccountNumber,
                photo = user.Photo,
                professions = string.Join(',', user.UserProfessions?.Select(x => x.Profession?.Name)),
                userProfessions = user.UserProfessions?.Select(x => x.ProfessionId).ToList(),
                roleId = user.RoleId,
                roleName = user.Role.Name,
                workId = user.WorkId,
                workName = user.Work?.Name,
                clientId = user.ClientId,
                clientName = user.Client?.Name,
                hasEmbargosPending = user.Embargos.Any(x => x.Paid == false)
            };

            return userViewModel;
        }

        public static List<UserViewModel> ConvertList(IEnumerable<User> users)
        {
            return users?.Select(user =>
                {
                    var model = new UserViewModel
                    {
                        id = user.Id,
                        addedDate = user.AddedDate,
                        modifiedDate = user.ModifiedDate,
                        iPAddress = user.IPAddress,
                        name = user.Name,
                        surname = user.Surname,
                        username = user.Username,
                        dni = user.Dni,
                        securitySocialNumber = user.SecuritySocialNumber,
                        birthDate = user.BirthDate?.ToString("MM/dd/yyyy"),
                        email = user.Email,
                        address = user.Address,
                        phoneNumber = user.PhoneNumber,
                        observations = user.Observations,
                        accountNumber = user.AccountNumber,
                        photo = user.Photo,
                        professions = string.Join(',', user.UserProfessions?.Select(x => x.Profession?.Name)),
                        userProfessions = user.UserProfessions?.Select(x => x.ProfessionId).ToList(),
                        roleId = user.RoleId,
                        roleName = user.Role.Name,
                        workId = user.WorkId,
                        workName = user.Work?.Name,
                        clientId = user.ClientId,
                        clientName = user.Client?.Name,
                        hasEmbargosPending = user.Embargos.Any(x => x.Paid == false)
                    };
                    return model;
                })
                .ToList();
        }
    }
}
