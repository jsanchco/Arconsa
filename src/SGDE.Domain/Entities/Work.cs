namespace SGDE.Domain.Entities
{   
    #region Using

    using System.Collections.Generic;
    using System;

    #endregion

    public class Work : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string EstimatedDuration { get; set; }
        public string WorksToRealize { get; set; }
        public bool Open { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public bool PassiveSubject { get; set; }
        public bool InvoiceToOrigin { get; set; }
        public double TotalContract { get; set; }
        public double PercentageRetention { get; set; }
        public double PercentageIVA { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<User> WorkResponsibles { get; set; } = new HashSet<User>();
        public virtual ICollection<UserHiring> UserHirings { get; set; } = new HashSet<UserHiring>();
        public virtual ICollection<DailySigning> DailySignings { get; set; } = new HashSet<DailySigning>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
        public virtual ICollection<WorkCost> WorkCosts { get; set; } = new HashSet<WorkCost>();
        public virtual ICollection<WorkBudgetData> WorkBudgetDatas { get; set; } = new HashSet<WorkBudgetData>();
        public virtual ICollection<WorkBudget> WorkBudgets { get; set; } = new HashSet<WorkBudget>();
    }
}
