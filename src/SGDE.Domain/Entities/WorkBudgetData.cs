using System;
using System.Collections.Generic;

namespace SGDE.Domain.Entities
{
    public class WorkBudgetData : BaseEntity
    {
        public string Description { get; set; }
        public string Reference { get; set; }

        public int WorkId { get; set; }
        public virtual Work Work { get; set; }

        public virtual ICollection<WorkBudget> WorkBudgets { get; set; } = new HashSet<WorkBudget>();
    }
}
