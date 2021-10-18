// ReSharper disable InconsistentNaming
namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class UserViewModel : BaseEntityViewModel
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string username { get; set; }
        public string fullname { 
            get 
            {
                return $"{name} {surname}";
            }
        }
        public string dni { get; set; }
        public string securitySocialNumber { get; set; }
        public string birthDate { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string observations { get; set; }
        public string accountNumber { get; set; }
        public byte[] photo { get; set; }
        public int state { get; set; }
        public string stateDescription { get; set; }
        public string token { get; set; }
        public int? professionId { get; set; }
        public string professionName { get; set; }
        public List<int> professions { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public int? workId { get; set; }
        public string workName { get; set; }
        public int? clientId { get; set; }
        public string clientName { get; set; }
    }
}
