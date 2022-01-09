using System;

namespace SGDE.Domain.Entities
{
    public class IndirectCost : BaseEntity
    {
        public DateTime Date { get; set; }
        //public int Year => Date.Year;
        //public int Month => Date.Month;
        public string Key => $"{Date.Year}/{Date.Month:00}";
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
