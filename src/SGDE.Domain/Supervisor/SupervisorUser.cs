// ReSharper disable InconsistentNaming
namespace SGDE.Domain.Supervisor
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Converters;
    using Entities;
    using ViewModels;
    using Domain.Helpers;

    #endregion

    public partial class Supervisor
    {
        public async Task<UserViewModel> Authenticate(string username, string password, CancellationToken ct = default(CancellationToken))
        {
            var userViewModel = UserConverter.Convert(await _userRepository.Authenticate(username, password, ct));
            if (userViewModel == null)
                return null;

            return userViewModel;
        }

        public async Task<List<UserViewModel>> GetAllUserAsync(CancellationToken ct = default(CancellationToken))
        {
            return UserConverter.ConvertList(await _userRepository.GetAllAsync(ct));
        }

        public async Task<UserViewModel> GetUserByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            var userViewModel = UserConverter.Convert(await _userRepository.GetByIdAsync(id, ct));
            return userViewModel;
        }

        public async Task<UserViewModel> AddUserAsync(UserViewModel newUserViewModel, CancellationToken ct = default(CancellationToken))
        {
            var user = new User
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newUserViewModel.iPAddress,

                Name = newUserViewModel.name,
                Surname = newUserViewModel.surname,
                Username = newUserViewModel.username,
                BirthDate = string.IsNullOrEmpty(newUserViewModel.birthDate)
                    ? null
                    : (DateTime?)DateTime.Parse(newUserViewModel.birthDate),
                Email = newUserViewModel.email,
                Password = "123456",
                ProfessionId = newUserViewModel.professionId
            };

            await _userRepository.AddAsync(user, ct);

            return newUserViewModel;
        }

        public async Task<bool> UpdateUserAsync(UserViewModel userViewModel, CancellationToken ct = default(CancellationToken))
        {
            if (userViewModel.id == null)
                return false;

            var user = await _userRepository.GetByIdAsync((int)userViewModel.id, ct);

            if (user == null) return false;

            user.ModifiedDate = DateTime.Now;
            user.IPAddress = userViewModel.iPAddress;

            user.Name = userViewModel.name;
            user.Surname = userViewModel.surname;
            user.Username = userViewModel.username;
            user.BirthDate = string.IsNullOrEmpty(userViewModel.birthDate)
                ? null
                : (DateTime?)DateTime.Parse(userViewModel.birthDate);
            user.Email = userViewModel.email;            
            user.ProfessionId = userViewModel.professionId;

            return await _userRepository.UpdateAsync(user, ct);
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return await _userRepository.DeleteAsync(id, ct);
        }

        public QueryResult<UserViewModel> GetAllUsers(int skip = 0, int take = 0, string filter = null, List<int> roles = null)
        {
            var queryResult = _userRepository.GetAll(skip, take, filter, roles);
            return new QueryResult<UserViewModel>
            {
                Data = UserConverter.ConvertList(queryResult.Data),
                Count = queryResult.Count
            };
        }

        public UserViewModel GetUserById(int id)
        {
            var userViewModel = UserConverter.Convert(_userRepository.GetById(id));
            return userViewModel;
        }

        public UserViewModel AddUser(UserViewModel newUserViewModel)
        {
            var user = new User
            {
                AddedDate = DateTime.Now,
                ModifiedDate = null,
                IPAddress = newUserViewModel.iPAddress,

                Name = newUserViewModel.name,
                Surname = newUserViewModel.surname,
                Username = newUserViewModel.username,
                Dni = newUserViewModel.dni,
                SecuritySocialNumber = newUserViewModel.securitySocialNumber,
                BirthDate = string.IsNullOrEmpty(newUserViewModel.birthDate)
                    ? null
                    : (DateTime?)DateTime.ParseExact(newUserViewModel.birthDate, "dd/MM/yyyy", null),
                Email = newUserViewModel.email,
                Address = newUserViewModel.address,
                PhoneNumber = newUserViewModel.phoneNumber,
                PriceHour = newUserViewModel.priceHour,
                PriceHourSale = newUserViewModel.priceHourSale,
                Observations = newUserViewModel.observations,
                AccountNumber = newUserViewModel.accountNumber,
                Photo = newUserViewModel.photo,
                RoleId = newUserViewModel.roleId,
                ProfessionId = newUserViewModel.professionId,
                WorkId = newUserViewModel.workId,
                ClientId = newUserViewModel.clientId,
                Password = "123456"
            };

            _userRepository.Add(user);

            return newUserViewModel;
        }

        public bool UpdateUser(UserViewModel userViewModel)
        {
            if (userViewModel.id == null)
                return false;

            var user = _userRepository.GetById((int)userViewModel.id);

            if (user == null) return false;

            user.ModifiedDate = DateTime.Now;
            user.IPAddress = userViewModel.iPAddress;

            user.Name = userViewModel.name;
            user.Surname = userViewModel.surname;
            user.Username = userViewModel.username;
            user.Dni = userViewModel.dni;
            user.SecuritySocialNumber = userViewModel.securitySocialNumber;
            user.BirthDate = string.IsNullOrEmpty(userViewModel.birthDate)
                ? null
                : (DateTime?)DateTime.ParseExact(userViewModel.birthDate, "dd/MM/yyyy", null);
            user.Email = userViewModel.email;
            user.Address = userViewModel.address;
            user.PhoneNumber = userViewModel.phoneNumber;
            user.PriceHour = userViewModel.priceHour;
            user.PriceHourSale = userViewModel.priceHourSale;
            user.Observations = userViewModel.observations;
            user.AccountNumber = userViewModel.accountNumber;
            user.Photo = userViewModel.photo;
            user.RoleId = userViewModel.roleId;
            user.ProfessionId = userViewModel.professionId;
            user.WorkId = userViewModel.workId;
            user.ClientId = userViewModel.clientId;

            return _userRepository.Update(user);
        }

        public bool UpdatePassword(UserViewModel userViewModel)
        {
            if (userViewModel.id == null)
                return false;

            var user = _userRepository.GetById((int)userViewModel.id);

            if (user == null) return false;

            if (user.Password != userViewModel.password)
                throw new Exception("La contraseña introducida no coincide con la guardada");

            if (user.Password == userViewModel.newPassword)
                throw new Exception("La contraseña introducida no puede coincidir con la guardada");

            user.Password = userViewModel.newPassword;

            return _userRepository.Update(user);
        }

        public bool RestorePassword(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null) return false;

            user.Password = "123456";

            return _userRepository.Update(user);
        }

        public bool UpdateUserPhoto(int userId, byte[] photo)
        {
            var user = _userRepository.GetById(userId);

            if (user == null) return false;

            user.ModifiedDate = DateTime.Now;
            user.Photo = photo;

            return _userRepository.Update(user);
        }

        public bool DeleteUser(int id)
        {
            return _userRepository.Delete(id);
        }
    }
}
