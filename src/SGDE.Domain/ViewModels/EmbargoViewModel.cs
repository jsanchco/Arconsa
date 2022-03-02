using System;

namespace SGDE.Domain.ViewModels
{
    public class EmbargoViewModel : BaseEntityViewModel
    {
        public string identifier { get; set; }
        public string issuingEntity { get; set; }
        public string accountNumber { get; set; }
        public DateTime? notificationDate { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string observations { get; set; }
        public double total { get; set; }
        public double? remaining { get; set; }
        public bool paid { get; set; }
        public int userId { get; set; }
    }
}
