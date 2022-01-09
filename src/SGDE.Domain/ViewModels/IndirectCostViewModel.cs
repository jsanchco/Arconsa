using System;

namespace SGDE.Domain.ViewModels
{
    public class IndirectCostViewModel : BaseEntityViewModel
    {
        public DateTime date { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        //public string key => $"{year}/{date.Month:00}";
        public string key { get; set; }
        public string accountNumber { get; set; }
        public double amount { get; set; }
        public string description { get; set; }
    }
}
