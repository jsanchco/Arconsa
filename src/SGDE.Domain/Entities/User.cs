﻿namespace SGDE.Domain.Entities
{
    #region Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Dni { get; set; }
        public string SecuritySocialNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }       
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Observations { get; set; }
        public string AccountNumber { get; set; }
        public byte[] Photo { get; set; }
        public string Token { get; set; }

        //public int? ProfessionId { get; set; }
        //public virtual Profession Profession { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int? WorkId { get; set; }
        public virtual Work Work { get; set; }

        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }

        public int? EnterpriseId { get; set; }
        public virtual Enterprise Enterprise { get; set; }

        public virtual ICollection<UserProfession> UserProfessions { get; set; } = new HashSet<UserProfession>();
        public virtual ICollection<Training> Trainings { get; set; } = new HashSet<Training>();
        public virtual ICollection<User> ContactPersons { get; set; } = new HashSet<User>();
        public virtual ICollection<UserHiring> UserHirings { get; set; } = new HashSet<UserHiring>();
        public virtual ICollection<DailySigning> DailySignings { get; set; } = new HashSet<DailySigning>();
        public virtual ICollection<CostWorker> CostWorkers { get; set; } = new HashSet<CostWorker>();
        public virtual ICollection<Embargo> Embargos { get; set; } = new HashSet<Embargo>();
        public virtual ICollection<SSHiring> SSHirings { get; set; } = new HashSet<SSHiring>();
        public virtual ICollection<Advance> Advances { get; set; } = new HashSet<Advance>();
        public virtual ICollection<UserEnterprise> UsersEnterprises { get; set; } = new HashSet<UserEnterprise>();
    }
}
